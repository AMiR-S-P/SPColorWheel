using SP_Color_Wheel.CustomControls;
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

namespace SP_Color_Wheel.UserControls.Wheel
{
    /// <summary>
    /// Interaction logic for MainPointer.xaml
    /// </summary>
    public partial class MainPointer : BasePointer
    {


        public double MaxH
        {
            get { return (double)GetValue(MaxHProperty); }
            set { SetValue(MaxHProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxH.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxHProperty =
            DependencyProperty.Register("MaxH", typeof(double), typeof(MainPointer), new PropertyMetadata(10.0));




        public double MaxW
        {
            get { return (double)GetValue(MaxWProperty); }
            set { SetValue(MaxWProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxW.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxWProperty =
            DependencyProperty.Register("MaxW", typeof(double), typeof(MainPointer), new PropertyMetadata(10.0));



        public MainPointer()
        {
            InitializeComponent();
        }

        private async void BasePointer_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double left = Margin.Left + e.HorizontalChange;
            double top = Margin.Top + e.VerticalChange;


                await (Parent as PointersContainer).SetMainPointerPosisitionAndGetColor(new Point(left, top));
            
        }

        private void BasePointer_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            Cursor = Cursors.None;
        }

        private void BasePointer_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}
