using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Toolkit
{
    // ReSharper disable once InconsistentNaming
    public class HudPackageSO : ScriptableObject
    {
        public GameObject hudObject;
        public HudManifestSO hudManifest;
        public HudConfigSO hudConfig;
    }
}
