using System.Globalization;
using UnityEngine;

namespace SpectatorHUD.Counters
{
    public class HealthCounter : HealthCounterBase
    {
        protected override void UpdateCounter()
        {
            text.SetText(Value.ToString(CultureInfo.CurrentCulture));
        }
    }
}
