using SP_Color_Wheel.EventArguments;
using SP_Color_Wheel.Extensions;
using SP_Color_Wheel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace SP_Color_Wheel.UserControls.Common
{
    /// <summary>
    /// Interaction logic for VerticalColorIndicator.xaml
    /// </summary>
    public partial class VerticalColorIndicator : Border, INotifyPropertyChanged
    {
        RenderTargetBitmap renderTargetBitmap;
        CroppedBitmap croppedBitmap;
        Int32Rect int32Rect;
        bool isAlphaMode = false;
        double lastIndicatorPosition;
        private double? alphaIndicatorPosition;
       

        public double? AlphaIndicatorPosition
        {
            get => alphaIndicatorPosition;
            set
            {
                alphaIndicatorPosition = value;
                if (value != null)
                {
                    if (alphaIndicatorPosition < -subGrid.ActualHeight / 2)
                    {
                        alphaIndicatorPosition = -subGrid.ActualHeight / 2;
                    }
                    if (alphaIndicatorPosition > subGrid.ActualHeight / 2)
                    {
                        alphaIndicatorPosition = subGrid.ActualHeight / 2;
                    }
                    indicator.Margin = new Thickness(0, (double)alphaIndicatorPosition, 0/*-indicator.ActualWidth / 2*/, (double)-alphaIndicatorPosition);
                    CurrentAlpha = (byte)((indicator.Margin.Top + subGrid.ActualHeight / 2) * 255 / subGrid.ActualHeight);

                    GetAlpha(new Point(subGrid.ActualWidth / 2, indicator.Margin.Top + subGrid.ActualHeight / 2));
                }
            }
        }


        public byte CurrentAlpha { get; set; } = 255;
        public Color CurrentColor
        {
            get { return (Color)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        #region IsChainedProperty
        public bool IsChained
        {
            get { return (bool)GetValue(IsChainedProperty); }
            set { SetValue(IsChainedProperty, value); }
        }

        public static readonly DependencyProperty IsChainedProperty =
            DependencyProperty.Register("IsChained", typeof(bool), typeof(VerticalColorIndicator), new PropertyMetadata(true, IsChainedChangedCallback));

        private static void IsChainedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VerticalColorIndicator)d)?.OnIsChainedChanged((bool)e.NewValue);
            //if ((bool)e.NewValue)
            //{
            //    ((VerticalColorIndicator)d).chainImage.Source = new BitmapImage(new Uri("/SP_Color_Wheel;component/Resources/Icons/Link.png", UriKind.Relative));
            //}
            //else
            //{
            //    ((VerticalColorIndicator)d).chainImage.Source = new BitmapImage(new Uri("/SP_Color_Wheel;component/Resources/Images/brokenLink.png", UriKind.Relative));
            //}
        }

        public event EventHandler<IsChainedChanged> IsChainedChanged;
        public void OnIsChainedChanged(bool value)
        {
            IsChainedChanged?.Invoke(this, new IsChainedChanged(value));
        }
        #endregion

        #region IsLockedPrpoperty
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        public event EventHandler<IsLockedChanged> IsLockedChanged;
        void OnIsLockedChanged(bool value)
        {
            IsLockedChanged?.Invoke(this, new IsLockedChanged(value));
        }

        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(VerticalColorIndicator), new PropertyMetadata(false, IsLockedChangedCallback));

        private static void IsLockedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VerticalColorIndicator)?.OnIsLockedChanged((bool)e.NewValue);
        }
        #endregion

        #region IndicatorRegionProperty
        //public double IndicatorPosition
        //{
        //    get { return indicatorPosition; }
        //    set
        //    {
        //        indicatorPosition = value;
        //        if (indicatorPosition < -subGrid.ActualHeight / 2)
        //        {
        //            indicatorPosition = -subGrid.ActualHeight / 2;
        //        }
        //        if (indicatorPosition > subGrid.ActualHeight / 2)
        //        {
        //            indicatorPosition = subGrid.ActualHeight / 2;
        //        }
        //        indicator.Margin = new Thickness(0, indicatorPosition, 0 /*-val.indicator.ActualWidth / 2*/, -indicatorPosition);

        //        Task.Run(async () =>
        //       {
        //           await subGrid.Dispatcher.InvokeAsync(async () =>
        //           {
        //               GetColor(new Point(subGrid.ActualWidth / 2, indicator.Margin.Top + subGrid.ActualHeight / 2)); // + ActualHeight because verticalAl... of Thumb is Center
        //           });
        //       });
        //        // 
        //        OnPropertyChanged();
        //    }
        //}
        public double IndicatorPosition
        {
            get { return (double)GetValue(IndicatorPositionProperty); }
            set { SetValue(IndicatorPositionProperty, value); }
        }

        public static readonly DependencyProperty IndicatorPositionProperty =
            DependencyProperty.RegisterAttached("IndicatorPosition", typeof(double), typeof(VerticalColorIndicator), new PropertyMetadata(0.0, IndicatorPositionChanged));



        private static  void IndicatorPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var indicatorPosition = (double)e.NewValue;
            var val = d as VerticalColorIndicator;
            if (indicatorPosition < -val.subGrid.ActualHeight / 2)
            {
                indicatorPosition = -val.subGrid.ActualHeight / 2;
            }
            if (indicatorPosition > val.subGrid.ActualHeight / 2)
            {
                indicatorPosition = val.subGrid.ActualHeight / 2;
            }
            val.indicator.Margin = new Thickness(0, indicatorPosition, 0 /*-val.indicator.ActualWidth / 2*/, -indicatorPosition);
            val.GetColor(new Point(val.subGrid.ActualWidth / 2, val.indicator.Margin.Top + val.subGrid.ActualHeight / 2)).ConfigureAwait(false); // + ActualHeight because verticalAl... of Thumb is Center
            //   
        }
        #endregion


        public static readonly DependencyProperty CurrentColorProperty =
                DependencyProperty.Register("CurrentColor", typeof(Color), typeof(VerticalColorIndicator), new PropertyMetadata(Colors.Transparent));


        #region Brushpropertty
        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brush", typeof(Brush), typeof(VerticalColorIndicator), new PropertyMetadata(Brushes.Transparent, BackgroundChangedCallback));


        private static void BackgroundChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var val = d as VerticalColorIndicator;

            val.subGrid.Background = e.NewValue as Brush;

            val.renderTargetBitmap = null;

            //await val.GetColor(new Point(val.ActualWidth / 2, val.indicator.Margin.Top + val.ActualHeight / 2));
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public VerticalColorIndicator()
        {
            Initialized += VerticalColorIndicator_Initialized;


            InitializeComponent();
            SizeChanged += VerticalColorIndicator_SizeChanged;
            indicator.SizeChanged += Indicator_SizeChanged;
            Loaded += VerticalColorIndicator_Loaded;
        }

        private void VerticalColorIndicator_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void VerticalColorIndicator_Initialized(object sender, EventArgs e)
        {
        }

        private void Indicator_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }



        private void VerticalColorIndicator_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderTargetBitmap = null;
            //indicator.Margin = new Thickness(0, indicator.Margin.Top, -indicator.Width / 2, indicator.Margin.Bottom);
        }

        private void indicator_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (!isAlphaMode)
            {
                IndicatorPosition = indicator.Margin.Top + e.VerticalChange;
            }
            else
            {
                AlphaIndicatorPosition = indicator.Margin.Top + e.VerticalChange;
            }
        }

        private void indicator_KeyDown(object sender, KeyEventArgs e)
        {
            if (indicator.IsFocused)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        {
                            e.Handled = true;
                            if (isAlphaMode)
                            {
                                AlphaIndicatorPosition = -subGrid.ActualHeight / 2;
                            }
                            else
                            {
                                IndicatorPosition = -subGrid.ActualHeight / 2;
                            }
                            break;
                        }
                    case Key.Right:
                        {
                            e.Handled = true;
                            if (isAlphaMode)
                            {
                                AlphaIndicatorPosition = subGrid.ActualHeight / 2;
                            }
                            else
                            {
                                IndicatorPosition = subGrid.ActualHeight / 2;
                            }
                            break;
                        }

                    case Key.Up:
                        {
                            e.Handled = true;
                            if (isAlphaMode)
                            {
                                AlphaIndicatorPosition--;
                            }
                            else
                            {
                                IndicatorPosition--;
                            }
                            break;
                        }
                    case Key.Down:
                        {
                            e.Handled = true;
                            if (isAlphaMode)
                            {
                                AlphaIndicatorPosition++;
                            }
                            else
                            {
                                IndicatorPosition++;
                            }
                            break;
                        }
                    case Key.NumPad0:
                    case Key.D0:
                        {
                            e.Handled = true;
                            if (isAlphaMode)
                            {
                                AlphaIndicatorPosition = 0;
                            }
                            else
                            {
                                IndicatorPosition = 0;
                            }
                            break;
                        }
                }
            }
        }

        void GetAlpha(Point position)
        {
            try
            {
                if (renderTargetBitmap == null)
                {
                    subGrid.UpdateLayout();
                    renderTargetBitmap = new RenderTargetBitmap((int)subGrid.ActualWidth, (int)subGrid.ActualHeight, 96, 96, PixelFormats.Default);
                    renderTargetBitmap.Render(subGrid);

                    croppedBitmap = new CroppedBitmap();
                    croppedBitmap.BeginInit();
                    croppedBitmap.Source = renderTargetBitmap;
                    croppedBitmap.EndInit();

                    int32Rect = new Int32Rect
                    {
                        Width = 1,
                        Height = 1
                    };
                }
                var pixels = new byte[4];

                int32Rect.X = (int)4/*(int)position.X - 5*/;
                int32Rect.Y = (int)position.Y;

                if (AlphaIndicatorPosition < -subGrid.ActualHeight / 2 + 0.6)
                {
                    CurrentColor = Color.FromArgb(0, 0, 0, 0);
                }
                else if (AlphaIndicatorPosition >= subGrid.ActualHeight / 2 - 0.05)
                {
                    CurrentColor = Color.FromArgb(255, 0, 0, 0);
                }
                else
                {

                    renderTargetBitmap.CopyPixels(int32Rect, pixels, 4, 0);


                    CurrentColor = System.Windows.Media.Color.FromArgb(CurrentAlpha, pixels[2], pixels[1], pixels[0]);

                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    CurrentColor = Colors.Transparent;
                });

                //Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
            //return Task.CompletedTask;
        }
        async Task GetColor(Point position)
        {
            try
            {
                if (renderTargetBitmap == null)
                {
                    subGrid.UpdateLayout();
                    renderTargetBitmap = new RenderTargetBitmap((int)subGrid.ActualWidth, (int)subGrid.ActualHeight, 96, 96, PixelFormats.Default);
                    renderTargetBitmap.Render(subGrid);

                    croppedBitmap = new CroppedBitmap();
                    croppedBitmap.BeginInit();
                    croppedBitmap.Source = renderTargetBitmap;
                    croppedBitmap.EndInit();

                    int32Rect = new Int32Rect
                    {
                        Width = 1,
                        Height = 1
                    };

                    renderTargetBitmap.Freeze();

                    //using (var file = File.Create(System.IO.Path.Combine(Environment.CurrentDirectory, "bb.png")))
                    //{
                    //    var encoder = new JpegBitmapEncoder();
                    //    BitmapFrame bitmapFrame = BitmapFrame.Create(renderTargetBitmap);
                    //    encoder.Frames.Add(bitmapFrame);

                    //    encoder.Save(file);
                    //}
                }
                var pixels = new byte[4];

                int32Rect.X = (int)4/*(int)position.X - 5*/;
                int32Rect.Y = (int)position.Y;

                if (IndicatorPosition < -subGrid.ActualHeight / 2 + 0.6)
                {
                    CurrentColor = Color.FromArgb(CurrentAlpha, 255, 255, 255);
                }
                else if (IndicatorPosition >= subGrid.ActualHeight / 2 - 0.05)
                {
                    CurrentColor = Color.FromArgb(CurrentAlpha, 0, 0, 0);
                }
                else
                {
                    //croppedBitmap.CopyPixels(int32Rect, pixels, 4, 0);
                    await renderTargetBitmap.CopyPixelsAsync(int32Rect, pixels, 4, 0);
                    CurrentColor = System.Windows.Media.Color.FromArgb(CurrentAlpha, pixels[2], pixels[1], pixels[0]);
                }

            }
            catch (Exception ex)
            {
                CurrentColor = Colors.Transparent;

                //Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
            //return Task.CompletedTask;
        }

        private void indicator_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            Cursor = Cursors.None;
        }
        private void indicator_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Cursor = Cursors.Arrow;

        }
        private void indicator_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed && !indicator.IsDragging)
            //{
            //    this.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 1, MouseButton.Left) { RoutedEvent = UIElement.MouseLeftButtonDownEvent, Source = indicator });
            //}
        }

        void SetAlphaModeBrush()
        {
            renderTargetBitmap = null;


            var linearGradientBrush = new LinearGradientBrush();
            GradientStop g1 = new GradientStop(Color.FromArgb(0, 0, 0, 0), 0);
            GradientStop g2 = new GradientStop(Color.FromArgb(255, 0, 0, 0), 1);
            linearGradientBrush.GradientStops.Add(g1);
            linearGradientBrush.GradientStops.Add(g2);
            subGrid.Background = linearGradientBrush;
        }
        void SetNormalBrush()
        {
            subGrid.Background = Brush;

            renderTargetBitmap = null;
        }
        internal protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                isAlphaMode = true;

                SetAlphaModeBrush();
                if (IsLoaded  )
                {
                    lastIndicatorPosition = IndicatorPosition;
                    if (alphaIndicatorPosition == null)
                    {
                        AlphaIndicatorPosition = subGrid.ActualHeight / 2;
                    }
                    else
                    {
                        AlphaIndicatorPosition = alphaIndicatorPosition;
                    }
                }
            }
        }
        internal protected void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                isAlphaMode = false;

                SetNormalBrush();
                if (IsLoaded)
                {
                    IndicatorPosition = lastIndicatorPosition;

                    VerticalColorIndicator.IndicatorPositionChanged(this, new DependencyPropertyChangedEventArgs(VerticalColorIndicator.IndicatorPositionProperty, IndicatorPosition, lastIndicatorPosition));
                }
            }
        }
    }
}
