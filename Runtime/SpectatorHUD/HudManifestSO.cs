using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD
{
    // ReSharper disable once InconsistentNaming
    public class HudManifestSO : ScriptableObject
    {
        [InspectorName("Name")]
        public string hudName = "Cool hud";
        public string author = "Me";
        public ushort version = 1;
        [TextArea(minLines: 3, maxLines: 8)]
        public string description = "A cool hud.";
    }
}
