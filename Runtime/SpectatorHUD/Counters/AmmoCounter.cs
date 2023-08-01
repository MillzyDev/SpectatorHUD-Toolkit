﻿using UnityEngine;

// ReSharper disable once CheckNamespace
namespace SpectatorHUD.Counters
{
    public class AmmoCounter : MonoBehaviour
    {
        public int value;
        public DisplayMode displayMode = DisplayMode.Combined;

        public bool useCustomFormatting;
        public string format = "{0}";

        public enum DisplayMode
        {
            Combined,
            Dominant,
            LeftHand,
            RightHand
        }
    }
}
