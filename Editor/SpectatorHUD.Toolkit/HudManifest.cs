#if UNITY_EDITOR
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Toolkit
{
    public class HudManifest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("prefab_asset")]
        public string PrefabAsset { get; set; }
    }
}
#endif