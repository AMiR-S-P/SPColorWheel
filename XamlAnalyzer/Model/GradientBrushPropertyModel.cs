using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XamlAnalyzer.Model
{
    public class GradientBrushPropertyModel : PropertyModel
    {
        public ObservableCollection<GradientStop> GradientStops { set; get; } = new ObservableCollection<GradientStop>();

        public GradientBrushPropertyModel(DependencyObject owner) : base(owner)
        {
        }


    }
}
