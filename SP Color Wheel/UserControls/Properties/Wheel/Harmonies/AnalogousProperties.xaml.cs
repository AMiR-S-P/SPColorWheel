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

namespace SP_Color_Wheel.UserControls.Properties.Wheel.Harmonies
{
    /// <summary>
    /// Interaction logic for AnalogousProperties.xaml
    /// </summary>
    public partial class AnalogousProperties : UserControl
    {
        public AnalogousProperties()
        {
            InitializeComponent();
            DataContextChanged += AnalogousProperties_DataContextChanged;
        }

        private void AnalogousProperties_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
         }

        private void Button_Click(object sender, RoutedEventArgs e)
        
        {

        }
    }
}
