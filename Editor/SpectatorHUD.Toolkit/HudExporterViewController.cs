using SpectatorHUD;
using SpectatorHUD.Toolkit;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


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
}