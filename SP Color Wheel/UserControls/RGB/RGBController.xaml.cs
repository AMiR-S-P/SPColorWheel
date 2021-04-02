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
    /// Interaction logic for RGBController.xaml
    /// </summary>
    public partial class RGBController : UserControl,INotifyPropertyChanged
    {
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(RGBController), new PropertyMetadata(Color.FromArgb(255,0,0,0), ColorChangedCallback));

        private static void ColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var val = d as RGBController;
            var color = (Color)e.NewValue;
            val.Alpha = color.A;
            val.Red = color.R;
            val.Green = color.G;
            val.Blue = color.B;
        }

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register("Red", typeof(byte), typeof(RGBController), new PropertyMetadata((byte)0, RedPropertyChanged));

        private static void RedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var controller = d as RGBController;
            (controller).SetValue(RGBController.ColorProperty, Color.FromArgb(controller.Color.A, (byte)e.NewValue, controller.Color.G, controller.Color.B));
        }

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register("Green", typeof(byte), typeof(RGBController), new PropertyMetadata((byte)0, GreenPropertyChanged));

        private static void GreenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var controller = d as RGBController;
            (controller).SetValue(RGBController.ColorProperty, Color.FromArgb(controller.Color.A, controller.Color.R, (byte)e.NewValue, controller.Color.B));
        }

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register("Blue", typeof(byte), typeof(RGBController), new PropertyMetadata((byte)0, BluePropertyChanged));

        private static void BluePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var controller = d as RGBController;
            (controller).SetValue(RGBController.ColorProperty, Color.FromArgb(controller.Color.A, controller.Color.R, controller.Color.G, (byte)e.NewValue));
        }

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(byte), typeof(RGBController), new PropertyMetadata((byte)255, AlphaPropertyChanged));


        private static void AlphaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
                var controller = d as RGBController;
                (controller).SetValue(RGBController.ColorProperty, Color.FromArgb((byte)e.NewValue, controller.Color.R, controller.Color.G, controller.Color.B));
            
        }



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RGBController), new PropertyMetadata(""));



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RGBController()
        {
            InitializeComponent();
        }

    }
}
