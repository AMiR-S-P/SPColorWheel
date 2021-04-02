using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.EventArguments
{
    public class IsLockedChanged:EventArgs
    {
        public bool IsLocked { get; private set; }
        public IsLockedChanged(bool value)
        {
            IsLocked = value;
        }
    }
}
