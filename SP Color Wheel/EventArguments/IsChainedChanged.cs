using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.EventArguments
{
    public class IsChainedChanged:EventArgs
    {
        public IsChainedChanged(bool isChained)
        {
            IsChained = isChained;
        }

        public bool IsChained { get; set; }
        
    }
}
