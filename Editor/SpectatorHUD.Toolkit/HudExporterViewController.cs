using System;
using System.IO;
using System.Net.Mime;
using SpectatorHUD;
using SpectatorHUD.Toolkit;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
public class HudExporterViewController : EditorWindow
{
    private TextField _displayName;
    private TextField _displayAuthor;
    private TextField _displayVersion;
    private TextField _displayDescription;
    private ObjectField _hudPackage;
    private Button _exportButton;
    
    [MenuItem("SpectatorHUD/Export HUD")]
    public static void ShowExample()
    {
        var wnd = GetWindow<HudExporterViewController>();
        wnd.titleContent = new GUIContent("HUD Export Wizard");
    }

    public void CreateGUI()
    {
        #region UI Setup
        
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = Resources.Load<VisualTreeAsset>("Views/HudExporterView");
        VisualElement uxml = visualTree.Instantiate();
        root.Add(uxml);

        #endregion

        _displayName = root.Q<TextField>("displayName");
        _displayAuthor = root.Q<TextField>("displayAuthor");
        _displayVersion = root.Q<TextField>("displayVersion");
        _displayDescription = root.Q<TextField>("displayDescription");
        
        var contentBox = root.Q<GroupBox>("content");
        _hudPackage = new ObjectField("HUD Package")
        {
            objectType = typeof(HudPackageSO)
        };
        _hudPackage.RegisterCallback<ChangeEvent<Object>>(OnHudPackageChanged);
        contentBox.Insert(1 ,_hudPackage);

        _exportButton = root.Q<Button>("exportButton");
        _exportButton.clicked += ExportHud;
    }

    private void OnHudPackageChanged(ChangeEvent<Object> changeEvent)
    {
        var hudPackage = changeEvent.newValue as HudPackageSO;

        if (!hudPackage)
        {
            _displayName.value = "";
            _displayAuthor.value = "";
            _displayVersion.value = "";
            _displayDescription.value = "";
            return;
        }
        
        HudManifestSO hudManifest = hudPackage.hudManifest;
        
        _displayName.value = hudManifest.hudName;
        _displayAuthor.value = hudManifest.author;
        _displayVersion.value = hudManifest.version.ToString();
        _displayDescription.value = hudManifest.description;
    }

    private void ExportHud()
    {
        _exportButton.focusable = false;

        var hudPackage = _hudPackage.value as HudPackageSO;
        HudManifestSO hudManifest = hudPackage!.hudManifest;
        string hudName = hudManifest.hudName;

        string filePath =
            EditorUtility.SaveFilePanel("Export HUD", "", hudName + ".hud", "hud");

        HudConfigSO hudConfig = hudPackage.hudConfig;
        GameObject hudObject = hudPackage.hudObject;

        string manifestPath = AssetDatabase.GetAssetPath(hudManifest);
        string configPath = AssetDatabase.GetAssetPath(hudConfig);
        string prefabPath = AssetDatabase.GetAssetPath(hudObject);

        var bundle = new AssetBundleBuild
        {
            assetBundleName = Path.GetFileName(filePath),
            assetNames = new[] { manifestPath, configPath, prefabPath }
        };

        Directory.CreateDirectory(Path.Combine(Application.dataPath, "_SH Build"));

        BuildPipeline.BuildAssetBundles("Assets/_SH Build", new[] { bundle }, BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);
        
        File.Copy(Path.Combine(Application.dataPath, "_SH Build", $"{hudName.ToLower()}.hud"), filePath, true);
        
        EditorUtility.RevealInFinder(filePath);
        
        Close();
    }
}