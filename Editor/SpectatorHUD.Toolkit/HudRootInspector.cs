#if UNITY_EDITOR
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Toolkit
{
    [CustomEditor(typeof(HudRoot))]
    public class HudRootInspector : Editor
    {
        private string _hudAssetPath;
        private string _manifestPath;
        private HudManifest _manifest;
        
        private TextField _nameInput;
        private TextField _versionInput;
        private TextField _authorInput;
        private TextField _prefabAssetInput;

        private void OnEnable() => EditorApplication.update += Update;
        private void OnDisable() => EditorApplication.update -= Update;
        

        public override VisualElement CreateInspectorGUI()
        {
            Object parentObj = PrefabUtility.GetCorrespondingObjectFromSource(target);
            _hudAssetPath = parentObj != null
                ? AssetDatabase.GetAssetPath(parentObj)
                : AssetDatabase.GetAssetPath(target);
            Debug.Log(_hudAssetPath);
            _manifestPath = Path.Combine(Path.GetDirectoryName(_hudAssetPath)!, "manifest.json");
            Debug.Log(_manifestPath);
            string json = File.ReadAllText(_manifestPath);
            _manifest = JsonConvert.DeserializeObject<HudManifest>(json);
            
            var inspector = new VisualElement();
            var layout = new GroupBox();

            _nameInput = new TextField("Name")
            {
                value = _manifest.Name
            };
            _versionInput = new TextField("Version")
            {
                value = _manifest.Version
            };
            _authorInput = new TextField("Author")
            {
                value = _manifest.Author
            };

            _prefabAssetInput = new TextField("Prefab Asset");
            _prefabAssetInput.SetEnabled(false);

            var updateManifestButton = new Button
            {
                text = "Update Manifest"
            };
            updateManifestButton.clicked += UpdateManifest;

            var compileButton = new Button
            {
                text = "Build HUD"
            };
            compileButton.clicked += BuildHUD;

            layout.Add(_nameInput);
            layout.Add(_versionInput);
            layout.Add(_authorInput);
            layout.Add(_prefabAssetInput);
            layout.Add(updateManifestButton);
            layout.Add(compileButton);

            inspector.Add(layout);
            return inspector;
        }

        private void Update()
        {
            if (_prefabAssetInput != null) _prefabAssetInput.value = Path.GetFileName(_hudAssetPath);
        }

        private void UpdateManifest()
        {
            _manifest.Name = _nameInput.value;
            _manifest.Version = _versionInput.value;
            _manifest.Author = _authorInput.value;
            _manifest.PrefabAsset = _prefabAssetInput.value;
            
            string json = JsonConvert.SerializeObject(_manifest);
            Debug.Log(json);
            File.WriteAllText(_manifestPath, json);
        }

        private void BuildHUD()
        {
            var hudBundle = new AssetBundleBuild
            {
                assetBundleName = _manifest.Name + ".hud",
                assetNames = new[] { _hudAssetPath, _manifestPath }
            };

            const string localOutputPath = "SpectatorHUD/Build Output";
            string outputPath = Path.Combine(Application.dataPath, localOutputPath);

            Directory.CreateDirectory(outputPath);

            BuildPipeline.BuildAssetBundles(Path.Combine("Assets", localOutputPath), new[] { hudBundle },
                BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            
            EditorUtility.RevealInFinder(outputPath);
        }
    }
}
#endif