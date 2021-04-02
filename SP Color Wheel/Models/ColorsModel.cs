using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SP_Color_Wheel.Models
{
    class ColorsModel : BaseModel
    {
        private Color mainColor;
        private Color color2;
        private Color color3;
        private Color color4;
        private Color color5;

        public Color MainColor { get => mainColor; set { mainColor = value; OnPropertyChanged(); } }
        public Color Color2 { get => color2; set { color2 = value; OnPropertyChanged(); } }
        public Color Color3 { get => color3; set { color3 = value; OnPropertyChanged(); } }
        public Color Color4 { get => color4; set { color4 = value; OnPropertyChanged(); } }
        public Color Color5 { get => color5; set { color5 = value; OnPropertyChanged(); } }
    }
}
