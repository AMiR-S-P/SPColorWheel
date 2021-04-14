using SP_Color_Wheel.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for PointersContainer.xaml
    /// </summary>
    public partial class PointersContainer : Grid
    {


        public delegate Task PointerSetterMethod(Point point);
        public PointerSetterMethod PointerPositionSetter { set; get; }



        public ColorCircle ColorCircle
        {
            get { return (ColorCircle)GetValue(ColorCircleProperty); }
            set { SetValue(ColorCircleProperty, value); }
        }

        public static readonly DependencyProperty ColorCircleProperty =
            DependencyProperty.Register("ColorCircle", typeof(ColorCircle), typeof(PointersContainer), new PropertyMetadata(null));



        #region AnalogousProperties    
        //public double AnalogousAngle
        //{
        //    get
        //    {
        //        return (double)GetValue(AnalogousAngleProperty);
        //    }
        //    set
        //    {
        //        SetValue(AnalogousAngleProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty AnalogousAngleProperty =
        //    DependencyProperty.Register("AnalogousAngle", typeof(double), typeof(PointersContainer), new PropertyMetadata(25.0, AnalogousAngleChanged), ValidateValue);

        //private static void AnalogousAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var val = d as PointersContainer;
        //    val.PointerPositionSetter(new Point(val.MainPointer.Margin.Left, val.MainPointer.Margin.Top));
        //}

        private static bool ValidateValue(object value)
        {
            var val = double.Parse(value.ToString());

            return val > 0 && val < 91;
        }
        #endregion

        public PointersContainer()
        {

            InitializeComponent();

            DataContextChanged += PointersContainer_DataContextChanged;
            MouseLeftButtonUp += PointersContainer_MouseLeftButtonUp ;
        }

        private async void PointersContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await SetMainPointerPosisitionAndGetColor(new Point(e.GetPosition(this).X, e.GetPosition(this).Y));
        }

      


        private void PointersContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is WheelViewModel)
            {
                var val = e.NewValue as WheelViewModel;
                val.ColorHarmonyChanged += Val_ColorHarmonyChanged;
                val.AngleChanged += Val_AngleChanged;


            }
            if (e.OldValue is WheelViewModel)
            {
                var val = e.OldValue as WheelViewModel;
                val.ColorHarmonyChanged -= Val_ColorHarmonyChanged;
                val.AngleChanged -= Val_AngleChanged;
            }
        }

        private void Val_AngleChanged(object sender, EventArguments.AngleChangedEventArgs e)
        {
            PointerPositionSetter(new Point(MainPointer.Margin.Left, MainPointer.Margin.Top));
        }

        private async void Val_ColorHarmonyChanged(object sender, EventArguments.ColorHarmonyChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case Enums.PointerHarmonyType.Single:
                    {
                        PointerPositionSetter = SetSinglePointer;
                        Pointer2.Visibility = Visibility.Hidden;
                        Pointer3.Visibility = Visibility.Hidden;
                        Pointer4.Visibility = Visibility.Hidden;
                        Pointer5.Visibility = Visibility.Hidden;
                        break;
                    }
                case Enums.PointerHarmonyType.Monochromatic:
                    {
                        PointerPositionSetter = SetMonochromaticPointer;
                        Pointer2.Visibility = Visibility.Visible;
                        Pointer3.Visibility = Visibility.Visible;
                        Pointer4.Visibility = Visibility.Visible;
                        Pointer5.Visibility = Visibility.Visible;
                        break;
                    }
                case Enums.PointerHarmonyType.Analogous:
                    {
                        PointerPositionSetter = SetAnalogousPointer;
                        Pointer2.Visibility = Visibility.Visible;
                        Pointer3.Visibility = Visibility.Visible;
                        Pointer4.Visibility = Visibility.Visible;
                        Pointer5.Visibility = Visibility.Visible;
                        break;
                    }
                case Enums.PointerHarmonyType.Triadic:
                    {
                        PointerPositionSetter = SetTriadicPointer;
                        Pointer2.Visibility = Visibility.Visible;
                        Pointer3.Visibility = Visibility.Visible;
                        Pointer4.Visibility = Visibility.Visible;
                        Pointer5.Visibility = Visibility.Visible;
                        break;
                    }
                case Enums.PointerHarmonyType.Square:
                    {
                        PointerPositionSetter = SetSquarePointer;
                        Pointer2.Visibility = Visibility.Visible;
                        Pointer3.Visibility = Visibility.Visible;
                        Pointer4.Visibility = Visibility.Visible;
                        Pointer5.Visibility = Visibility.Visible;
                        break;
                    }
                case Enums.PointerHarmonyType.Complementary:
                    {
                        PointerPositionSetter = SetComplementaryPointer;
                        Pointer2.Visibility = Visibility.Visible;
                        Pointer3.Visibility = Visibility.Visible;
                        Pointer4.Visibility = Visibility.Visible;
                        Pointer5.Visibility = Visibility.Visible;
                        break;
                    }
            }
            var point = new Point();
            if (MainPointer.Margin.Left <= 10 && MainPointer.Margin.Top <= 10)
            {
                point = new Point(ActualWidth / 2, ActualHeight / 2);
            }
            else
            {
                point = new Point(MainPointer.Margin.Left, MainPointer.Margin.Top);
            }
            await SetMainPointerPosisitionAndGetColor(point);
        }



        public async Task SetMainPointerPosisitionAndGetColor(Point point)
        {
            try
            {
                //double distance = 0;//Math.Sqrt(Math.Pow((this.ActualWidth-point.X),2)+ Math.Pow((this.ActualHeight - point.Y), 2));

                MainPointer.Margin = new Thickness(point.X, point.Y, -point.X, -point.Y);

                await PointerPositionSetter(point);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
        }



        async Task SetSinglePointer(Point point)
        {
            //this.MainPointer.SetPositionAndGetColor(Wheel.ColorCircle, point, z => Wheel.RawMainColor = z);
            (DataContext as WheelViewModel).RawMainColor = await ColorCircle.GetColor(point);
            (DataContext as WheelViewModel).RawColor2 = await ColorCircle.GetColor(point);
            (DataContext as WheelViewModel).RawColor3 = await ColorCircle.GetColor(point);
            (DataContext as WheelViewModel).RawColor4 = await ColorCircle.GetColor(point);
            (DataContext as WheelViewModel).RawColor5 = await ColorCircle.GetColor(point);
        }
        async Task SetComplementaryPointer(Point point)
        {
            (DataContext as WheelViewModel).RawMainColor = await ColorCircle.GetColor(point);

            Point p = new Point(this.ActualWidth - MainPointer.Margin.Left, this.ActualHeight - MainPointer.Margin.Top);
            (DataContext as WheelViewModel).RawColor2 = await ColorCircle.GetColor(p);
            Pointer2.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            double xStep = ((Pointer2.Margin.Left) - MainPointer.Margin.Left) / 4.0;
            double yStep = ((Pointer2.Margin.Top) - MainPointer.Margin.Top) / 4;

            p = new Point(this.ActualWidth - MainPointer.Margin.Left - xStep * 2, this.ActualHeight - MainPointer.Margin.Top - yStep * 2);
            (DataContext as WheelViewModel).RawColor3 = await ColorCircle.GetColor(p);
            Pointer3.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(MainPointer.Margin.Left + xStep * 3, MainPointer.Margin.Top + yStep * 3);
            (DataContext as WheelViewModel).RawColor4 = await ColorCircle.GetColor(p);
            Pointer4.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(MainPointer.Margin.Left + xStep * 1, MainPointer.Margin.Top + yStep * 1);
            (DataContext as WheelViewModel).RawColor5 = await ColorCircle.GetColor(p);
            Pointer5.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

        }
        async Task SetAnalogousPointer(Point point)
        {
            (DataContext as WheelViewModel).RawMainColor = await ColorCircle.GetColor(point);

            double distance = Math.Sqrt(Math.Pow(point.X - (ActualWidth / 2), 2) + Math.Pow(point.Y - (ActualHeight / 2), 2));
            double rad = Math.Atan2((point.Y - (ActualHeight / 2)), (point.X - (ActualWidth / 2)));
            double radianAngle = (DataContext as WheelViewModel).Angle * Math.PI / 180;

            Point p = new Point(Math.Cos(rad - 2 * radianAngle) * distance + ActualWidth / 2, Math.Sin(rad - 2 * radianAngle) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor2 = await ColorCircle.GetColor(p);
            Pointer2.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(Math.Cos(rad - radianAngle) * distance + ActualWidth / 2, Math.Sin(rad - radianAngle) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor3 = await ColorCircle.GetColor(p);
            Pointer3.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(Math.Cos(rad + radianAngle) * distance + ActualWidth / 2, Math.Sin(rad + radianAngle) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor4 = await ColorCircle.GetColor(p);
            Pointer4.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(Math.Cos(rad + 2 * radianAngle) * distance + ActualWidth / 2, Math.Sin(rad + 2 * radianAngle) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor5 = await ColorCircle.GetColor(p);
            Pointer5.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);
        }
        async Task SetSquarePointer(Point point)
        {
            (DataContext as WheelViewModel).RawMainColor = await ColorCircle.GetColor(point);

            Point p = new Point(this.ActualWidth - MainPointer.Margin.Left, this.ActualHeight - MainPointer.Margin.Top);
            (DataContext as WheelViewModel).RawColor3 = await ColorCircle.GetColor(p);
            Pointer3.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor5 = await ColorCircle.GetColor(p); //center
            Pointer5.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);


            double rad = Math.Atan2(point.Y - Pointer3.Margin.Top, point.X - Pointer3.Margin.Left);//in radian PI

            rad += Math.PI / 2;
            double distance = Math.Sqrt(Math.Pow(point.X - (ActualWidth / 2), 2) + Math.Pow(point.Y - (ActualHeight / 2), 2));

            p = new Point(Math.Cos(rad) * distance + ActualWidth / 2, Math.Sin(rad) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor2 = await ColorCircle.GetColor(p);
            Pointer2.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(this.ActualWidth - Pointer2.Margin.Left, this.ActualHeight - Pointer2.Margin.Top);
            (DataContext as WheelViewModel).RawColor4 = await ColorCircle.GetColor(p);
            Pointer4.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

        }
        async Task SetTriadicPointer(Point point)
        {
            (DataContext as WheelViewModel).RawMainColor = await ColorCircle.GetColor(point);

            double distance = Math.Sqrt(Math.Pow(point.X - (ActualWidth / 2), 2) + Math.Pow(point.Y - (ActualHeight / 2), 2));
            double rad = Math.Atan2((point.Y - (ActualHeight / 2)), (point.X - (ActualWidth / 2)));
            double radianAngle = 120 * Math.PI / 180;

            Point p = new Point(Math.Cos(rad + radianAngle) * distance + ActualWidth / 2, Math.Sin(rad + radianAngle) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor2 = await ColorCircle.GetColor(p);
            Pointer2.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(Math.Cos(rad + radianAngle) * distance / 2 + ActualWidth / 2, Math.Sin(rad + radianAngle) * distance / 2 + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor3 = await ColorCircle.GetColor(p);
            Pointer3.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(Math.Cos(rad - radianAngle) * distance + ActualWidth / 2, Math.Sin(rad - radianAngle) * distance + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor4 = await ColorCircle.GetColor(p);
            Pointer4.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(Math.Cos(rad - radianAngle) * distance / 2 + ActualWidth / 2, Math.Sin(rad - radianAngle) * distance / 2 + ActualHeight / 2);
            (DataContext as WheelViewModel).RawColor5 = await ColorCircle.GetColor(p);
            Pointer5.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);
        }
        async Task SetMonochromaticPointer(Point point)
        {
            (DataContext as WheelViewModel).RawMainColor = await ColorCircle.GetColor(point);

            double xStep = (ActualWidth / 2 - MainPointer.Margin.Left) / 5;
            double yStep = (ActualHeight / 2 - MainPointer.Margin.Top) / 5;

            Point p = new Point(point.X + xStep, point.Y + yStep);
            (DataContext as WheelViewModel).RawColor2 = await ColorCircle.GetColor(p);
            Pointer2.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(point.X + xStep * 2, point.Y + yStep * 2);
            (DataContext as WheelViewModel).RawColor3 = await ColorCircle.GetColor(p);
            Pointer3.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(point.X + xStep * 3, point.Y + yStep * 3);
            (DataContext as WheelViewModel).RawColor4 = await ColorCircle.GetColor(p);
            Pointer4.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);

            p = new Point(point.X + xStep * 4, point.Y + yStep * 4);
            (DataContext as WheelViewModel).RawColor5 = await ColorCircle.GetColor(p);
            Pointer5.Margin = new Thickness(p.X, p.Y, -p.X, -p.Y);
            //this.MainPointer.SetPositionAndGetColor(Wheel.ColorCircle, point, z => Wheel.RawMainColor = z);

            //double xStep = (ActualWidth / 2 - MainPointer.Margin.Left) / 5;
            //double yStep = (ActualHeight / 2 - MainPointer.Margin.Top) / 5;

            //this.Pointer2.SetPositionAndGetColor(Wheel.ColorCircle, new Point(point.X + xStep, point.Y + yStep), z => Wheel.RawColor2 = z);
            //this.Pointer3.SetPositionAndGetColor(Wheel.ColorCircle, new Point(point.X + xStep * 2, point.Y + yStep * 2), z => Wheel.RawColor3 = z);
            //this.Pointer4.SetPositionAndGetColor(Wheel.ColorCircle, new Point(point.X + xStep * 3, point.Y + yStep * 3), z => Wheel.RawColor4 = z);
            //this.Pointer5.SetPositionAndGetColor(Wheel.ColorCircle, new Point(point.X + xStep * 4, point.Y + yStep * 4), z => Wheel.RawColor5 = z);
        }
    }
}
