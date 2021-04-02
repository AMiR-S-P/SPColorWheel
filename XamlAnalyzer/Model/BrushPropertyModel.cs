using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XamlAnalyzer.Model
{
    public class BrushPropertyModel : PropertyModel
    {
        private BrushPropertyModel color;

        //for solidColorBrush
        public BrushPropertyModel Color { get => color; set { color = value; OnPropertyChanged(); } }

        //for GradientBrush
        public BrushPropertyModel(DependencyObject owner) : base(owner)
        {
        }

    }
}
