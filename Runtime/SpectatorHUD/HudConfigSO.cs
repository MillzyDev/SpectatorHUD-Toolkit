using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD
{
    // ReSharper disable once InconsistentNaming
    public class HudConfigSO : ScriptableObject
    {
        public CombinedAmmoCounterConfig combinedAmmoCounterConfiguration;
        public HealthCounterConfig healthCounterDisplayMode;

        // Config for if the ammo count is combined
        public enum CombinedAmmoCounterConfig : ushort
        {
            CombineAlways = 1, // Always combine the ammo counts of the weapons in both hands
            CombineIfOfSameType = 2, // Combine the ammo counts only if both weapons use the same ammo type. Otherwise use dominant hand.
        }
        
        public enum HealthCounterConfig
        {
            Value, // Value with modifers applied
            RawValue, // Value without the modifiers applied (no rounding)
            Percentage, // Your health as a percentage
            GlobalPercentage // Your health as a percentage, relative to 10hp.
        }
    }
}
