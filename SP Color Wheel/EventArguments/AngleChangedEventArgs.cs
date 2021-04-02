using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.EventArguments
{
    public class AngleChangedEventArgs:EventArgs
    {
        public double  NewAngle { get; set; }

        public AngleChangedEventArgs(double newAngle)
        {
            NewAngle = newAngle;
        }
    }
}
