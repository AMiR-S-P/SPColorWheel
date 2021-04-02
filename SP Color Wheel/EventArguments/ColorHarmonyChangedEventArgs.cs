using SP_Color_Wheel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.EventArguments
{
    public class ColorHarmonyChangedEventArgs : EventArgs
    {
        public PointerHarmonyType NewValue { get; set; }

        public ColorHarmonyChangedEventArgs(PointerHarmonyType newValue)
        {
            NewValue = newValue;
        }
    }
}
