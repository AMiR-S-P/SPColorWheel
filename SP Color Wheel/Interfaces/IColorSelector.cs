using SP_Color_Wheel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SP_Color_Wheel.Interfaces
{
    public interface IColorSelector
    {
        string SelectorName { get; set; }
        Color MainColor { get; set; }
        Color Color2 { get; set; }
        Color Color3 { get; set; }
        Color Color4 { get; set; }
        Color Color5 { get; set; }

        Tint MainColorTint { set; get; }
        Tone MainColorTone { set; get; }
        Shade MainColorShade { set; get; }

        public IColorProperties Properties { get; set; } 
    }
}
