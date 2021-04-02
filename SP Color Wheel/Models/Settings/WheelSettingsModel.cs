using SP_Color_Wheel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.Models.Settings
{
    public class WheelSettingsModel
    {
        public bool HasRed { get; set; }
        public bool HasGreen { get; set; }
        public bool HasBlue { get; set; }

        public PointerHarmonyType HarmonyType { set; get; }

        public double Angle { get; set; }
    }
}
