using SP_Color_Wheel.Interfaces;
using SP_Color_Wheel.UserControls;
using SP_Color_Wheel.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SP_Color_Wheel.Converters
{
    public class ColorSelectorToColorPropertisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as IColorSelector;
            var name = val?.SelectorName.ToLower();
            switch (name)
            {
                case "wheel":
                    {
                        //var dc = (MainWindowVM)((value as Wheel).DataContext);
                        //return new WheelProperties(dc);
                        break;
                    }
                case "rgb":
                    {
                        //return new RgbProperties()/* { DataContext = (MainWindowVM)((value as Wheel).DataContext) }*/;
                        break;
                    }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
