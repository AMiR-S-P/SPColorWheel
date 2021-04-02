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

namespace SP_Color_Wheel.UserControls.Common
{
    /// <summary>
    /// Interaction logic for HorizontalColorSlider.xaml
    /// </summary>
    public partial class HorizontalColorSlider : Border
    {

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }



        public static readonly DependencyProperty ColorProperty =
    DependencyProperty.Register("Color", typeof(Brush), typeof(HorizontalColorSlider), new PropertyMetadata(Brushes.Green));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(HorizontalColorSlider), new PropertyMetadata(0, ValueChangedCallback));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(HorizontalColorSlider), new PropertyMetadata(0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(HorizontalColorSlider), new PropertyMetadata(100));

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var val = d as HorizontalColorSlider;
            int newVal = (int)e.NewValue < 0 ? 0 : (int)e.NewValue;
            val.bar.Width = ((double.IsNaN(val.barContainer.Width) ? val.barContainer.ActualWidth : val.barContainer.Width) * newVal) / val.Maximum;

        }
        public HorizontalColorSlider()
        {
            InitializeComponent();
        }



        private void barContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(bar);
            var x = e.GetPosition(barContainer).X;
            double value = ((Maximum * x) / ((double.IsNaN(barContainer.Width) ? barContainer.ActualWidth : barContainer.Width)));
            Value = (int)value;
            if (value > Maximum )
            {
                Value = Maximum;
            }
            bar.Width = x;
        }

        private void barContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == bar)
            {
                var x = e.GetPosition(barContainer).X<0?0: e.GetPosition(barContainer).X;
                double value = ((Maximum * x) / ((double.IsNaN(barContainer.Width) ? barContainer.ActualWidth : barContainer.Width)));
                Value = (int)value;
                if (value > Maximum )
                {
                    Value = Maximum;
                }
                bar.Width = x;
            }
        }
        private void BarContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bar.Width = (e.NewSize.Width * Value) / Maximum;
        }

        private void BarContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (barContainer.IsFocused)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        {
                            if (Value > Minimum)
                            {
                                Value--;
                            }
                            e.Handled = true;
                            break;
                        }
                    case Key.Right:
                        {
                            if (Value < Maximum)
                            {
                                Value++;
                            }
                            e.Handled = true;
                            break;
                        }
                    case Key.Up:
                        {
                            Value = Maximum;
                            e.Handled = true;
                            break;
                        }
                    case Key.Down:
                        {
                            Value = Minimum;
                            e.Handled = true;
                            break;
                        }
                }
            }
        }

        private void barContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture(null);
        }
    }
}
