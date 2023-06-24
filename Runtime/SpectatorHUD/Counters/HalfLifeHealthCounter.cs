using System;

namespace SpectatorHUD.Counters
{
    public class HalfLifeHealthCounter : HealthCounterBase
    {
        protected override void UpdateCounter()
        {
            var value = (int)Math.Round((decimal)Value*10, 0);
            text.SetText(value.ToString());
        }
    }
}
