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
    public class PropertyModel : BaseModel, ICloneable
    {
        private PropertyInfo property;
        private object _value;
        private DependencyObject owner;
        private DependencyProperty dependencyProperty;

        public PropertyInfo Property
        {
            get => property; set
            {
                property = value; OnPropertyChanged();
                Value = property?.GetValue(Owner);
                PopulateDependencyProperty();
            }
        }
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
        public DependencyProperty DependencyProperty
        {
            get => dependencyProperty;
            set
            {
                dependencyProperty = value;
                OnPropertyChanged();
            }
        }
        public DependencyObject Owner
        {
            get => owner; private set
            {
                owner = value;
                OnPropertyChanged();
            }
        }
        public PropertyModel(DependencyObject owner)
        {
            Owner = owner;
        }

        void PopulateDependencyProperty()
        {
            //(Owner as FrameworkElement).GetType().get
        }

        public void SetOwnerProperty(object? value)
        {
            Owner.SetValue(DependencyProperty, value);
            Value = value;
        }
        public void SetProperty(object? value)
        {
            Property.SetValue(Owner, value);
            Value = value;
        }


        public object Clone()
        {
            PropertyModel copy = new PropertyModel(Owner);
            copy.Property = Property;
            copy.Value = Property.GetValue(copy.Owner);
            copy.DependencyProperty = DependencyProperty;
            return copy;
        }

    }
}
