using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Counters
{
    public class AmmoReserveCounter : MonoBehaviour
    {
        public int value;
        public Reserve reserve = Reserve.Medium;

        public bool useCustomFormatting;
        public string format = "{0}";

        public enum Reserve
        {
            Small,
            Medium,
            Heavy
        }
    }
}
