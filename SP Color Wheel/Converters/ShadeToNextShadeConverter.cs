using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SP_Color_Wheel.Converters
{
    public class ShadeToNextShadeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var color = value.ToString();
                var factor = parameter == null ? 30 : double.Parse(parameter.ToString());

                double alpha = System.Convert.ToByte(color.Substring(1, 2), 16);
                double red = System.Convert.ToByte(color.Substring(3, 2), 16);
                double green = System.Convert.ToByte(color.Substring(5, 2), 16);
                double blue = System.Convert.ToByte(color.Substring(7, 2), 16);


                var redFactor = red / factor; /* ((255 - red) / factor) == 0 ? 25 : (255 - red) / factor;*/
                var greenFactor = green / factor;/* ((255 - green) / factor) == 0 ? 25 : (255 - green) / factor;*/
                var blueFactor = blue / factor; /* ((255 - blue) / factor) == 0 ? 25 : (255 - blue) / factor;*/

                double newRed = Math.Ceiling(red - redFactor);
                double newGreen = Math.Ceiling(green - greenFactor);
                double newBlue = Math.Ceiling(blue - blueFactor);

                if (newRed < 0)
                {
                    newRed = 0;
                }
                if (newGreen < 0)
                {
                    newGreen = 0;
                }
                if (newBlue < 0)
                {
                    newBlue = 0;
                }

                return Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
