using SP_Color_Wheel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SP_Color_Wheel.UserControls.Properties.Wheel
{
    /// <summary>
    /// Interaction logic for WheelProperties.xaml
    /// </summary>
    public partial class WheelProperties : UserControl,IColorProperties
    {
        public WheelProperties()
        {
            InitializeComponent();
        }

    }
}
