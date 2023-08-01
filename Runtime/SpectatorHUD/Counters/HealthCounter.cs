using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Counters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HealthCounter : MonoBehaviour
    {
        public float value;
        public float multiplier = 10f;
        [Range(0, 5)]
        public int decimalPrecision;

        public bool useCustomFormatting = false;
        public string format = "{0.f}";
    }
}
