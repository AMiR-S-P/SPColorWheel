using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XamlAnalyzer.Utilities
{
    public class BrushToXaml
    {
        public Brush Brush { get; private set; }
        public string Xaml { get; private set; }

        public BrushToXaml(Brush brush)
        {
            Brush = brush;

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
                linear += "<LinearGradientBrush.GradientStops>";
            }

            foreach (var g in brush.GradientStops)
            {
                linear += $"<GradientStop Color=\"{g.Color}\" Offset=\"{g.Offset}\" />";
            }

            if (brush.GradientStops.Any())
            {
                linear += "</LinearGradientBrush.GradientStops>";
            }
            linear += "</LinearGradientBrush>";

            return linear;
        }

        string RadialToXaml()
        {
            string radial = "";
            RadialGradientBrush brush = Brush as RadialGradientBrush;
            radial = $"<RadialGradientBrush GradientOrigin=\"{brush.GradientOrigin}\" RadiusX=\"{brush.RadiusX}\"  RadiusY=\"{brush.RadiusY}\"  Center=\"{brush.Center}\" ColorInterpolationMode=\"{brush.ColorInterpolationMode}\" MappingMode=\"{brush.MappingMode}\" SpreadMethod=\"{brush.SpreadMethod}\" Opacity=\"{brush.Opacity}\">{Environment.NewLine}";
            if (brush.GradientStops.Any())
            {
                radial += "<RadialGradientBrush.GradientStops>";
            }

            foreach (var g in brush.GradientStops)
            {
                radial += $"<GradientStop Color=\"{g.Color}\" Offset=\"{g.Offset}\" />";
            }

            if (brush.GradientStops.Any())
            {
                radial += "</RadialGradientBrush.GradientStops>";
            }
            radial += "</RadialGradientBrush>";

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
