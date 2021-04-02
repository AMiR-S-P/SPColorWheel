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

namespace SP_Color_Wheel.UserControls.Wheel
{
    /// <summary>
    /// Interaction logic for ColorCircle.xaml
    /// </summary>
    public partial class ColorCircle : Grid, INotifyPropertyChanged
    {

        private RenderTargetBitmap renderTargetBitmap;
        private CroppedBitmap croppedBitmap;
        Int32Rect int32Rect = new Int32Rect();

        public double ColorBarWidth { get; set; } // to set max width,height of mainpointer 
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public ColorCircle()
        {
            InitializeComponent();
            DataContextChanged += ColorCircle_DataContextChanged;
        }

        private void ColorCircle_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is WheelViewModel)
            {
                var val = e.NewValue as WheelViewModel;
                val.ColorIncludedChanged += Val_ColorIncludedChanged;
            }
        }

        private async void Val_ColorIncludedChanged(object sender, EventArguments.ColorIncludedChanged e)
        {
            renderTargetBitmap = null;
            await DrawColorCircle(e.HaseRed, e.HasGreen, e.HasBlue);
        }

        protected override async void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            await DrawColorCircle((DataContext as WheelViewModel).HasRed, (DataContext as WheelViewModel).HasGreen, (DataContext as WheelViewModel).HasBlue);
            await RenderBackground();
        }
        public Task DrawColorCircle(bool hasRed, bool hasGreen, bool hasBlue)
        {
            Children.Clear();
            double red = hasRed == false ? 0 : 255;
            double green = 0;
            double blue = 0;


            double colorStep = 255.0 / 255.0;
            Geometry data = Geometry.Parse("M0,0 V1 L1,0 v1 L0,0Z");

            ColorBarWidth = Math.Min(ActualWidth, ActualHeight);
            for (double j = 0; j < 765; j += 1)
            {
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                path.HorizontalAlignment = HorizontalAlignment.Center;
                path.VerticalAlignment = VerticalAlignment.Center;
                path.Height = ColorBarWidth / 50;
                path.Width = ColorBarWidth;
                path.Stretch = Stretch.Fill;
                path.Data = data;
                //path.Margin =new Thickness(1);

                if (j < 255 && j >= 0)
                {
                    if (hasRed)
                    {
                        red = 255;
                    }
                    if (hasGreen)
                    {
                        green = 0;
                    }
                    if (hasBlue)
                    {
                        blue += colorStep;
                    }
                }
                else if (j >= 255 && j < 510)
                {
                    if (hasRed)
                    {
                        red -= colorStep;
                    }
                    if (hasGreen)
                    {
                        green = 0;
                    }
                    if (hasBlue)
                    {
                        blue = 255;
                    }
                }
                else if (j >= 510)
                {
                    if (hasRed)
                    {
                        red = 0;
                    }
                    if (hasGreen)
                    {
                        green += colorStep;
                    }
                    if (hasBlue)
                    {
                        blue = 255;
                    }
                }
                LinearGradientBrush linear = new LinearGradientBrush();
                RenderOptions.SetEdgeMode(linear, EdgeMode.Aliased);

                linear.EndPoint = new Point(1, 0);
                linear.StartPoint = new Point(0, 1);

                GradientStop start = new GradientStop();
                GradientStop end = new GradientStop();

                start.Offset = 0;
                end.Offset = 1;

                start.Color = Color.FromArgb(255, (byte)(red), (byte)(green), (byte)(blue));
                end.Color = Color.FromArgb(255, (byte)(255 - red), (byte)(255 - green), (byte)(255 - blue));


                linear.GradientStops.Add(start);
                linear.GradientStops.Add(end);

                path.LayoutTransform = new RotateTransform(j * 0.2352941176470588 - 180.0, path.ActualWidth / 2, path.ActualHeight / 2);
                path.Name = $"{start.Color}_{end.Color}_{(path.LayoutTransform as RotateTransform).Angle}".Replace("#", "").Replace("-", "N").Replace(".", "P");
                path.ToolTip = path.Name;

                path.Fill = linear;
                Children.Add(path);
            }

            return Task.CompletedTask;
        }

        public Task RenderBackground()
        {
            try
            {
                renderTargetBitmap = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Default);

                this.UpdateLayout();

                renderTargetBitmap.Render(this);

                croppedBitmap = new CroppedBitmap();
                croppedBitmap.BeginInit();
                croppedBitmap.Source = renderTargetBitmap;
                croppedBitmap.EndInit();

                renderTargetBitmap.Freeze();
                //using (var file = File.Create(System.IO.Path.Combine(Environment.CurrentDirectory, "aa.png")))
                //{
                //    var encoder = new JpegBitmapEncoder();
                //    BitmapFrame bitmapFrame = BitmapFrame.Create(renderTargetBitmap);
                //    encoder.Frames.Add(bitmapFrame);

                //    encoder.Save(file);
                //}
            }

            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
            return Task.CompletedTask;
        }

        public async Task<Color> GetColor(Point point)
        {
            try
            {
                if (renderTargetBitmap == null)
                {
                    await RenderBackground();
                }
                int32Rect.Width = 1;
                int32Rect.Height = 1;

                var pixel = new byte[4];

                int32Rect.X = (int)point.X;
                int32Rect.Y = (int)point.Y;

                //croppedBitmap.CopyPixels(int32Rect, pixel, 4, 0);
                await renderTargetBitmap.CopyPixelsAsync(int32Rect, pixel, 4, 0);

                Color color = Color.FromArgb(255, pixel[2], pixel[1], pixel[0]);
                return color;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
            return Color.FromArgb(0, 0, 0, 0);
        }
    }
}
