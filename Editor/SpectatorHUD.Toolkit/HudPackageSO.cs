using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Toolkit
{
    [CreateAssetMenu(menuName = "SpectatorHUD/HUD Package Asset", fileName = "Package")]
    // ReSharper disable once InconsistentNaming
    public class HudPackageSO : ScriptableObject
    {
        public GameObject hudObject;
        public HudManifestSO hudManifest;
        public HudConfigSO hudConfig;
    }
}
