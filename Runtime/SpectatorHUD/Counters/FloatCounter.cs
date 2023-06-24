using TMPro;
using UnityEngine;

namespace SpectatorHUD.Counters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class FloatCounter : MonoBehaviour
    {
        protected TextMeshProUGUI text;
        
        public float Value { get; set; }

        public void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        public void Update()
        {
            UpdateCounter();
        }

        protected abstract void UpdateCounter();
    }
}