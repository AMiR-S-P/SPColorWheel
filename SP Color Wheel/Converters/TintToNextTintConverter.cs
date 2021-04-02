using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SP_Color_Wheel.Converters
{
    public class TintToNextTintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var color = value.ToString();
                var factor = parameter == null ? 30 :byte.Parse(parameter.ToString());

                double alpha = System.Convert.ToByte(color.Substring(1, 2), 16);
                double red = System.Convert.ToByte(color.Substring(3, 2), 16);
                double green = System.Convert.ToByte(color.Substring(5, 2), 16);
                double blue = System.Convert.ToByte(color.Substring(7, 2), 16);


                var redFactor = (255 - red) / factor;
                var greenFactor = (255 - green) / factor;
                var blueFactor = (255 - blue) / factor;


                double newRed =Math.Ceiling(red + redFactor);
                double newGreen = Math.Ceiling(green + greenFactor);
                double newBlue = Math.Ceiling(blue + blueFactor);

                if (newRed > 255)
                {
                    newRed = 255;
                }
                if (newGreen > 255)
                {
                    newGreen = 255;
                }
                if (newBlue > 255)
                {
                    newBlue = 255;
                }

                return Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
            }
            return Colors.Transparent ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
