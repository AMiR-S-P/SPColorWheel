using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.EventArguments
{
    public class ColorIncludedChanged:EventArgs
    {
        public bool HaseRed { get; set; }
        public bool HasGreen { get; set; }
        public bool HasBlue { get; set; }


        public ColorIncludedChanged(bool haseRed, bool hasGreen, bool hasBlue)
        {
            HaseRed = haseRed;
            HasBlue = hasBlue;
            HasGreen = hasGreen;
        }
    }
}
