#if UNITY_EDITOR
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WebSocketSharp;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Toolkit
{
    public class NewHud : EditorWindow
    {
        [SerializeField]
        private Object defaultTemplate;

        private TextField _nameInput;
        private TextField _authorInput;
        private TextField _versionInput;
        private ObjectField _templateInput;
        private Toggle _addToSceneInput;
        private Button _createButton;
        private Label _errorMessage;

        [MenuItem("SpectatorHUD/New HUD")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<NewHud>();
            window.titleContent = new GUIContent("Create New HUD");
        }

        private void CreateGUI()
        {
            #region Manifest
            var manifest = new GroupBox();
            
            var manifestLabel = new Label("Manifest");
            manifest.Add(manifestLabel);
            
            _nameInput = new TextField("Name*");
            _authorInput = new TextField("Author*");
            _versionInput = new TextField("Version*");
            
            manifest.Add(_nameInput);
            manifest.Add(_versionInput);
            manifest.Add(_authorInput);
            #endregion

            #region Create
            var createSettings = new GroupBox();

            var createSettingsLabel = new Label("Creation Settings");
            
            _templateInput = new ObjectField("Template*")
            {
                objectType = typeof(GameObject),
                allowSceneObjects = false,
                value = defaultTemplate
            };

            _addToSceneInput = new Toggle("Add to scene");

            _createButton = new Button
            {
                text = "Create HUD"
            };
            _createButton.clicked += CreateHUD;

            _errorMessage = new Label
            {
                style =
                {
                    color = new StyleColor(Color.red)
                }
            };

            createSettings.Add(createSettingsLabel);
            createSettings.Add(_templateInput);
            createSettings.Add(_addToSceneInput);
            createSettings.Add(_createButton);
            createSettings.Add(_errorMessage);
            #endregion
            
            rootVisualElement.Add(manifest);
            rootVisualElement.Add(createSettings);
        }

        private void CreateHUD()
        {
            _createButton.focusable = false;
            
            if (_nameInput.value.IsNullOrEmpty() || _authorInput.value.IsNullOrEmpty() ||
                _versionInput.value.IsNullOrEmpty() || _templateInput.value is null)
            {
                _errorMessage.text = "All fields must be filled before creating a HUD.";
                _createButton.focusable = true;
                return;
            }

            string assets = Application.dataPath;
            string hudName = _nameInput.value;
            string fullPath = Path.Combine(assets, "Custom HUDs", hudName);
            _ = Directory.CreateDirectory(fullPath);

            string prefabPath = AssetDatabase.GetAssetPath(_templateInput.value);
            string prefabFilename = Path.GetFileName(prefabPath);
            string newPrefabPath = Path.Combine(fullPath, $"{hudName}.prefab");
            File.Copy(prefabPath, newPrefabPath);
            
            Object clonePrefab = Resources.Load(newPrefabPath);
            clonePrefab.name = hudName;

            var manifest = new HudManifest
            {
                Name = hudName,
                Author = _authorInput.value,
                Version = _versionInput.value,
                PrefabAsset = prefabFilename
            };
            string jsonString = JsonConvert.SerializeObject(manifest);
            File.WriteAllText(Path.Combine(fullPath, "manifest.json"), jsonString);

            if (_addToSceneInput.value)
            {
                Instantiate(clonePrefab);
            }
            
            EditorUtility.RevealInFinder(newPrefabPath);

            Close();
        }
    }
} 
#endif
