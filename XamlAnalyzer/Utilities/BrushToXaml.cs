using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XamlAnalyzer.Utilities
{
    public class BrushToXaml : INotifyPropertyChanged
    {
        private string xaml;
        private object brush;

        public object Brush
        {
            get => brush;
            set
            {
                brush = value;  //Task.Run(async () =>
                //{
                    //if (value != null)
                    //{
                         Convert();
                    //}
                //});
                OnPropertyChanged();
            }
        }
        public string Xaml { get => xaml; private set { xaml = value; OnPropertyChanged(); } }

        public BrushToXaml(Brush brush)
        {
            Brush = brush;

            Task.Run(async () =>
            {
                if (Brush != null)
                {
                     Convert();
                }
            });
        }
        public BrushToXaml()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void Convert()
        {

            if (Brush is SolidColorBrush)
            {
                Xaml = SolidToXaml();
            }
            else if (Brush is LinearGradientBrush)
            {
                Xaml = LinearToXaml();
            }
            else if (Brush is RadialGradientBrush)
            {
                Xaml = RadialToXaml();
            }
            else if (Brush is ImageBrush)
            {
                Xaml = ImageToXaml();
            }
            else
            {
                Xaml = "{x:null}";
            }
            //return Task.CompletedTask;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string SolidToXaml()
        {
            string solid = "";
            var brush = Brush as SolidColorBrush;
            solid = $"<SolidColorBrush Color=\"{brush.Color}\" Opacity=\"{brush.Opacity}\" />";

            return solid;
        }

        string LinearToXaml()
        {
            string linear = "";
            LinearGradientBrush brush = Brush as LinearGradientBrush;
            linear = $"<LinearGradientBrush StartPoint=\"{brush.StartPoint}\" EndPoint=\"{brush.EndPoint}\" ColorInterpolationMode=\"{brush.ColorInterpolationMode}\" MappingMode=\"{brush.MappingMode}\" Opacity=\"{brush.Opacity}\">{Environment.NewLine}";
            if (brush.GradientStops.Any())
            {
                linear += "\t<LinearGradientBrush.GradientStops>";
                linear += Environment.NewLine;
            }

            foreach (var g in brush.GradientStops)
            {
                linear += $"\t\t<GradientStop Color=\"{g.Color}\" Offset=\"{g.Offset}\" />";
                linear += Environment.NewLine;
            }

            if (brush.GradientStops.Any())
            {
                linear += "\t</LinearGradientBrush.GradientStops>";
                linear += Environment.NewLine;
            }
            linear += "</LinearGradientBrush>";
            linear += Environment.NewLine;

            return linear;
        }

        string RadialToXaml()
        {
            string radial = "";
            RadialGradientBrush brush = Brush as RadialGradientBrush;
            radial = $"<RadialGradientBrush GradientOrigin=\"{brush.GradientOrigin}\" RadiusX=\"{brush.RadiusX}\"  RadiusY=\"{brush.RadiusY}\"  Center=\"{brush.Center}\" ColorInterpolationMode=\"{brush.ColorInterpolationMode}\" MappingMode=\"{brush.MappingMode}\" SpreadMethod=\"{brush.SpreadMethod}\" Opacity=\"{brush.Opacity}\">{Environment.NewLine}";
            if (brush.GradientStops.Any())
            {
                radial += "/t<RadialGradientBrush.GradientStops>";
                radial += Environment.NewLine;
            }

            foreach (var g in brush.GradientStops)
            {
                radial += $"/t/t<GradientStop Color=\"{g.Color}\" Offset=\"{g.Offset}\" />";
                radial += Environment.NewLine;
            }

            if (brush.GradientStops.Any())
            {
                radial += "/t</RadialGradientBrush.GradientStops>";
                radial += Environment.NewLine;
            }
            radial += "</RadialGradientBrush>";
            radial += Environment.NewLine;

            return radial;
        }

        string ImageToXaml()
        {
            throw new Exception();
            //string image = "";
            //var brush = Brush as ImageBrush;
            //solid = $"<ImageBrush Color=\"{brush}\" Opacity=\"{brush.Opacity}\" />";

            //return solid;
        }
    }
}
