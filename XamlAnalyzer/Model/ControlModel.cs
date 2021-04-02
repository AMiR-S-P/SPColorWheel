using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XamlAnalyzer.Model
{
    public class ControlModel:BaseModel
    {
        public string ClassName { get; set; }
        public string Name { get; set; }
        public ObservableCollection<PropertyModel> BrushProperties { get; set; } = new ObservableCollection<PropertyModel>();
        public ObservableCollection<ControlModel> Children { get; set; } = new ObservableCollection<ControlModel>();
        public DependencyObject Element { get; set; }
    }
}
