using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Toggle = UnityEngine.UIElements.Toggle;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Toolkit
{
    public class HudCreatorViewController : EditorWindow
    {
        private TextField _hudName;
        private TextField _hudAuthor;
        private TextField _hudDescription;
        private Toggle _createConfigToggle;
        private Button _createButton;
        private Label _errorMessage;

        [MenuItem("SpectatorHUD/Create New HUD", priority = 1)]
        public static void ShowExample()
        {
            var wnd = GetWindow<HudCreatorViewController>();
            wnd.titleContent = new GUIContent("HUD Creation Wizard");
        }

        public void CreateGUI()
        {
            #region UI Setup

            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = Resources.Load<VisualTreeAsset>("Views/HudCreatorView");
            VisualElement uxml = visualTree.Instantiate();
            root.Add(uxml);

            #endregion

            // Get all the elements we need to instantiation.
            _hudName = root.Q<TextField>("hudName");
            _hudAuthor = root.Q<TextField>("hudAuthor");
            _hudDescription = root.Q<TextField>("hudDescription");
            _createConfigToggle = root.Q<Toggle>("createConfig");
            _createButton = root.Q<Button>("createButton");
            _errorMessage = root.Q<Label>("errorMessage");

            // register click event
            _createButton.clicked += CreateNewHud;
        }

        private void CreateNewHud()
        {
            _createButton.focusable = false;
            
            if (!ValidateInputFields(out string message))
            {
                _errorMessage.text = message;
                return;
            }
            
            string hudName = _hudName.value;

            // Create folder
            string dataPath = Application.dataPath;
            string folderPath;
            bool validPath;

            do
            {
                folderPath =
                    EditorUtility.SaveFolderPanel("Save HUD assets to directory", dataPath, hudName);

                validPath = folderPath.StartsWith(dataPath);
            } while (!validPath);

            folderPath = folderPath.Remove(0, dataPath.Length + 1);
            AssetDatabase.CreateFolder("Assets/", folderPath);

            // Create manifest
            var hudManifest = CreateInstance<HudManifestSO>();

            string hudAuthor = _hudAuthor.value;
            string hudDescription = _hudDescription.value;

            hudManifest.hudName = hudName;
            hudManifest.author = hudAuthor;
            hudManifest.version = 1;
            hudManifest.description = hudDescription;

            string assetPath = Path.Combine("Assets/", folderPath);

            string manifestPath = Path.Combine(assetPath, "Manifest.asset");
            AssetDatabase.CreateAsset(hudManifest, manifestPath);

            // Create config
            HudConfigSO hudConfig = null;
            string configPath = Path.Combine(assetPath, "Config.asset");
            bool createConfig = _createConfigToggle.value;

            if (createConfig)
            {
                hudConfig = CreateInstance<HudConfigSO>();
                AssetDatabase.CreateAsset(hudConfig, configPath);
            }

            // Create HUD prefab
            var hudGameObject = new GameObject(hudName);
            hudGameObject.AddComponent<RectTransform>();

            var canvas = hudGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

            var canvasScaler = hudGameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;
            canvasScaler.referencePixelsPerUnit = 100f;

            hudGameObject.AddComponent<GraphicRaycaster>();

            var hud = hudGameObject.AddComponent<Hud>();

            if (createConfig)
                hud.hudConfig = hudConfig;

            string hudAssetPath = Path.Combine(assetPath, $"{hudName}.prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(hudGameObject, hudAssetPath,
                InteractionMode.AutomatedAction);

            // Create HUD package asset.
            var hudPackage = CreateInstance<HudPackageSO>();

            hudPackage.hudObject = AssetDatabase.LoadAssetAtPath<GameObject>(hudAssetPath);
            hudPackage.hudManifest = AssetDatabase.LoadAssetAtPath<HudManifestSO>(manifestPath);
            if (createConfig)
                hudPackage.hudConfig = AssetDatabase.LoadAssetAtPath<HudConfigSO>(configPath);

            AssetDatabase.CreateAsset(hudPackage, Path.Combine(assetPath, $"{hudName}.asset"));
        }

        private bool ValidateInputFields(out string message)
        {
            if (_hudName.value.Length == 0)
            {
                message = "HUD Name is empty.";
                return false;
            }

            if (_hudAuthor.value.Length == 0)
            {
                message = "Author is empty.";
            }

            message = "";
            return true;
        }
    }
}
