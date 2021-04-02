using SP_Color_Wheel.UserControls.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SP_Color_Wheel.Behaviors.Wheel
{
    class LoadedUnloadedBehavior : Behavior<UserControls.Wheel.Wheel>
    {


        KeyEventHandler KeyUpHandler;
        KeyEventHandler KeyDownHandler;


        VerticalColorIndicator mainSlider;
        VerticalColorIndicator color2Slider;
        VerticalColorIndicator color3Slider;
        VerticalColorIndicator color4Slider;
        VerticalColorIndicator color5Slider;

        protected override void OnAttached()
        {
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            base.OnAttached();

        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            base.OnDetaching();
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                InitGradients();
            }
        }

        private void AssociatedObject_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.MainWindow.RemoveHandler(Window.PreviewKeyDownEvent, KeyDownHandler);
            Application.Current.MainWindow.RemoveHandler(Window.PreviewKeyUpEvent, KeyUpHandler);
        }

        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            KeyDownHandler = new KeyEventHandler((ss, ee) =>
            {
                mainSlider.OnKeyDown(ss, ee);
                color2Slider.OnKeyDown(ss, ee);
                color3Slider.OnKeyDown(ss, ee);
                color4Slider.OnKeyDown(ss, ee);
                color5Slider.OnKeyDown(ss, ee);
            });

            KeyUpHandler = new KeyEventHandler((ss, ee) =>
            {
                mainSlider.OnKeyUp(ss, ee);
                color2Slider.OnKeyUp(ss, ee);
                color3Slider.OnKeyUp(ss, ee);
                color4Slider.OnKeyUp(ss, ee);
                color5Slider.OnKeyUp(ss, ee);
            });

            Application.Current.MainWindow.AddHandler(Window.PreviewKeyDownEvent, KeyDownHandler);
            Application.Current.MainWindow.AddHandler(Window.PreviewKeyUpEvent, KeyUpHandler);
        }

        void InitGradients()
        {
            mainSlider = new VerticalColorIndicator()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Width = 35,
                Margin = new System.Windows.Thickness(0, -8, 5, 2),

            };

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            GradientStop offset0 = new GradientStop(Color.FromArgb(255, 0, 0, 0), 1);
            GradientStop offset05 = new GradientStop(Color.FromArgb(255, 127, 127, 127), 0.5);
            GradientStop offset1 = new GradientStop(Color.FromArgb(255, 255, 255, 255), 0);

            BindingOperations.SetBinding(offset05, GradientStop.ColorProperty, new Binding("RawMainColor") { Source = AssociatedObject.DataContext });
            offset05.Offset = 0.5;
            linearGradientBrush.GradientStops.Add(offset0);
            linearGradientBrush.GradientStops.Add(offset05);
            linearGradientBrush.GradientStops.Add(offset1);
            mainSlider.Brush = linearGradientBrush;
            mainSlider.SetBinding(VerticalColorIndicator.CurrentColorProperty, new Binding("MainColor") { Source = AssociatedObject.DataContext, Mode = BindingMode.OneWayToSource });

            mainSlider.IsLockedChanged += (s, e) =>
            {
                if (e.IsLocked)
                {
                    var lastColor = offset05.Color;
                    BindingOperations.ClearBinding(offset05, GradientStop.ColorProperty);
                    offset05.Color = lastColor;
                }
                else
                {
                    BindingOperations.SetBinding(offset05, GradientStop.ColorProperty, new Binding("RawMainColor") { Source = AssociatedObject.DataContext });
                }
            };

            Grid.SetColumn(mainSlider, 1);
            Grid.SetRow(mainSlider, 1);
            AssociatedObject.MainGrid.Children.Add(mainSlider);



            color2Slider = new VerticalColorIndicator()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Width = 35,
                Margin = new System.Windows.Thickness(5, -8, 5, 2),
            };

            LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
            GradientStop offset20 = new GradientStop(Color.FromArgb(255, 0, 0, 0), 1);
            GradientStop offset205 = new GradientStop(Color.FromArgb(255, 127, 127, 127), 0.5);
            GradientStop offset21 = new GradientStop(Color.FromArgb(255, 255, 255, 255), 0);
            linearGradientBrush2.GradientStops.Add(offset20);
            linearGradientBrush2.GradientStops.Add(offset205);
            linearGradientBrush2.GradientStops.Add(offset21);
            BindingOperations.SetBinding(offset205, GradientStop.ColorProperty, new Binding("RawColor2") { Source = AssociatedObject.DataContext });
            color2Slider.Brush = linearGradientBrush2;
            color2Slider.SetBinding(VerticalColorIndicator.CurrentColorProperty, new Binding("Color2") { Source = AssociatedObject.DataContext, Mode = BindingMode.OneWayToSource });
            color2Slider.IsLockedChanged += (s, e) =>
            {
                if (e.IsLocked)
                {
                    var lastColor = offset205.Color;
                    BindingOperations.ClearBinding(offset205, GradientStop.ColorProperty);
                    offset205.Color = lastColor;
                }
                else
                {
                    BindingOperations.SetBinding(offset205, GradientStop.ColorProperty, new Binding("RawColor2") { Source = AssociatedObject.DataContext });
                }
            };
            color2Slider.IsChainedChanged += (s, e) =>
            {
                if (e.IsChained)
                {

                    BindingOperations.SetBinding(color2Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
                }
                else
                {
                    double iP = color2Slider.IndicatorPosition;
                    BindingOperations.ClearBinding(color2Slider, VerticalColorIndicator.IndicatorPositionProperty);
                    color2Slider.IndicatorPosition = iP;
                }
            };

            Grid.SetColumn(color2Slider, 2);
            Grid.SetRow(color2Slider, 1);
            AssociatedObject.MainGrid.Children.Add(color2Slider);



            color3Slider = new VerticalColorIndicator()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Width = 35,
                Margin = new System.Windows.Thickness(5, -8, 5, 2),
            };

            LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush();
            GradientStop offset30 = new GradientStop(Color.FromArgb(255, 0, 0, 0), 1);
            GradientStop offset305 = new GradientStop(Color.FromArgb(255, 127, 127, 127), 0.5);
            GradientStop offset31 = new GradientStop(Color.FromArgb(255, 255, 255, 255), 0);
            linearGradientBrush3.GradientStops.Add(offset30);
            linearGradientBrush3.GradientStops.Add(offset305);
            linearGradientBrush3.GradientStops.Add(offset31);
            BindingOperations.SetBinding(offset305, GradientStop.ColorProperty, new Binding("RawColor3") { Source = AssociatedObject.DataContext });
            color3Slider.Brush = linearGradientBrush3;
            color3Slider.SetBinding(VerticalColorIndicator.CurrentColorProperty, new Binding("Color3") { Source = AssociatedObject.DataContext, Mode = BindingMode.OneWayToSource });
            color3Slider.IsLockedChanged += (s, e) =>
            {
                if (e.IsLocked)
                {
                    var lastColor = offset305.Color;
                    BindingOperations.ClearBinding(offset305, GradientStop.ColorProperty);
                    offset305.Color = lastColor;
                }
                else
                {
                    BindingOperations.SetBinding(offset305, GradientStop.ColorProperty, new Binding("RawColor3") { Source = AssociatedObject.DataContext });
                }
            };
            color3Slider.IsChainedChanged += (s, e) =>
            {
                if (e.IsChained)
                {
                    BindingOperations.SetBinding(color3Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
                }
                else
                {
                    double iP = color3Slider.IndicatorPosition;
                    BindingOperations.ClearBinding(color3Slider, VerticalColorIndicator.IndicatorPositionProperty);
                    color3Slider.IndicatorPosition = iP;
                }
            };
            Grid.SetColumn(color3Slider, 3);
            Grid.SetRow(color3Slider, 1);
            AssociatedObject.MainGrid.Children.Add(color3Slider);



            color4Slider = new VerticalColorIndicator()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Width = 35,
                Margin = new System.Windows.Thickness(5, -8, 5, 2),
            };

            LinearGradientBrush linearGradientBrush4 = new LinearGradientBrush();
            GradientStop offset40 = new GradientStop(Color.FromArgb(255, 0, 0, 0), 1);
            GradientStop offset405 = new GradientStop(Color.FromArgb(255, 127, 127, 127), 0.5);
            GradientStop offset41 = new GradientStop(Color.FromArgb(255, 255, 255, 255), 0);
            linearGradientBrush4.GradientStops.Add(offset40);
            linearGradientBrush4.GradientStops.Add(offset405);
            linearGradientBrush4.GradientStops.Add(offset41);
            BindingOperations.SetBinding(offset405, GradientStop.ColorProperty, new Binding("RawColor4") { Source = AssociatedObject.DataContext });
            color4Slider.Brush = linearGradientBrush4;
            color4Slider.SetBinding(VerticalColorIndicator.CurrentColorProperty, new Binding("Color4") { Source = AssociatedObject.DataContext, Mode = BindingMode.OneWayToSource });
            color4Slider.IsLockedChanged += (s, e) =>
            {
                if (e.IsLocked)
                {
                    var lastColor = offset405.Color;
                    BindingOperations.ClearBinding(offset405, GradientStop.ColorProperty);
                    offset405.Color = lastColor;
                }
                else
                {
                    BindingOperations.SetBinding(offset405, GradientStop.ColorProperty, new Binding("RawColor4") { Source = AssociatedObject.DataContext });
                }
            };
            color4Slider.IsChainedChanged += (s, e) =>
            {
                if (e.IsChained)
                {
                    BindingOperations.SetBinding(color4Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
                }
                else
                {
                    double iP = color4Slider.IndicatorPosition;
                    BindingOperations.ClearBinding(color4Slider, VerticalColorIndicator.IndicatorPositionProperty);
                    color4Slider.IndicatorPosition = iP;
                }
            };
            Grid.SetColumn(color4Slider, 4);
            Grid.SetRow(color4Slider, 1);
            AssociatedObject.MainGrid.Children.Add(color4Slider);



            color5Slider = new VerticalColorIndicator()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Width = 35,
                Margin = new System.Windows.Thickness(5, -8, 5, 2),
            };

            LinearGradientBrush linearGradientBrush5 = new LinearGradientBrush();
            GradientStop offset50 = new GradientStop(Color.FromArgb(255, 0, 0, 0), 1);
            GradientStop offset505 = new GradientStop(Color.FromArgb(255, 127, 127, 127), 0.5);
            GradientStop offset51 = new GradientStop(Color.FromArgb(255, 255, 255, 255), 0);
            linearGradientBrush5.GradientStops.Add(offset50);
            linearGradientBrush5.GradientStops.Add(offset505);
            linearGradientBrush5.GradientStops.Add(offset51);
            BindingOperations.SetBinding(offset505, GradientStop.ColorProperty, new Binding("RawColor5") { Source = AssociatedObject.DataContext });
            color5Slider.Brush = linearGradientBrush5;
            color5Slider.SetBinding(VerticalColorIndicator.CurrentColorProperty, new Binding("Color5") { Source = AssociatedObject.DataContext, Mode = BindingMode.OneWayToSource });
            color5Slider.IsLockedChanged += (s, e) =>
            {
                if (e.IsLocked)
                {
                    var lastColor = offset505.Color;
                    BindingOperations.ClearBinding(offset505, GradientStop.ColorProperty);
                    offset505.Color = lastColor;
                }
                else
                {
                    BindingOperations.SetBinding(offset505, GradientStop.ColorProperty, new Binding("RawColor5") { Source = AssociatedObject.DataContext });
                }
            };
            color5Slider.IsChainedChanged += (s, e) =>
            {
                if (e.IsChained)
                {
                    BindingOperations.SetBinding(color5Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
                }
                else
                {
                    double iP = color5Slider.IndicatorPosition;
                    BindingOperations.ClearBinding(color5Slider, VerticalColorIndicator.IndicatorPositionProperty);
                    color5Slider.IndicatorPosition = iP;
                }
            };
            Grid.SetColumn(color5Slider, 5);
            Grid.SetRow(color5Slider, 1);

            BindingOperations.SetBinding(color2Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(color3Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(color4Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(color5Slider, VerticalColorIndicator.IndicatorPositionProperty, new Binding("IndicatorPosition") { Source = mainSlider, Mode = BindingMode.TwoWay });


            AssociatedObject.MainGrid.Children.Add(color5Slider);



        }
    }
}
