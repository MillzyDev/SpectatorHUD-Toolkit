using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD
{
    public class Hud : MonoBehaviour
    {
        [Tooltip("This is the recommended config for your HUD, users are free to override individual parts of it.")]
        public HudConfigSO hudConfig;
    }
}