using SP_Color_Wheel.Helper;
using SP_Color_Wheel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SP_Color_Wheel.UserControls.RGB
{
    /// <summary>
    /// Interaction logic for RGBColors.xaml
    /// </summary>
    public partial class RGBColors : UserControl, INotifyPropertyChanged/*, IColorSelector*/
    {
        //private string selectorName;
        //private Color mainColor;
        //private Color color2;
        //private Color color3;
        //private Color color4;
        //private Color color5;
        //private Tint mainColorTint = new Tint();
        //private Tone mainColorTone = new Tone();
        //private Shade mainColorShade = new Shade();

        //public string SelectorName { get => selectorName; set { selectorName = value; OnPropertyChanged(); } }
        //public Color MainColor
        //{
        //    get => mainColor; set
        //    {
        //        mainColor = value;
        //        MainColorTint.MainTint = value;
        //        MainColorTone.MainTone = value;
        //        MainColorShade.MainShade = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public Color Color2 { get => color2; set { color2 = value; OnPropertyChanged(); } }
        //public Color Color3 { get => color3; set { color3 = value; OnPropertyChanged(); } }
        //public Color Color4 { get => color4; set { color4 = value; OnPropertyChanged(); } }
        //public Color Color5 { get => color5; set { color5 = value; OnPropertyChanged(); } }
        //public Tint MainColorTint { get => mainColorTint; set { mainColorTint = value; OnPropertyChanged(); } }
        //public Tone MainColorTone { get => mainColorTone; set { mainColorTone = value; OnPropertyChanged(); } }
        //public Shade MainColorShade { get => mainColorShade; set { mainColorShade = value; OnPropertyChanged(); } }

        //public IColorProperties Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RGBColors()
        {
            InitializeComponent();
            //SelectorName = "RGB";
        }
    }
}
