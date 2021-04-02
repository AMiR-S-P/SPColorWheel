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
    public class ToneToNextToneConverter : IValueConverter
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

                var mean = (blue+red+green)/3;

                var redFactor =   (255-mean)/ factor; /* ((255 - red) / factor) == 0 ? 25 : (255 - red) / factor;*/
                var greenFactor = (255-mean)/ factor;/* ((255 - green) / factor) == 0 ? 25 : (255 - green) / factor;*/
                var blueFactor =  (255-mean)/ factor; /* ((255 - blue) / factor) == 0 ? 25 : (255 - blue) / factor;*/

                double newRed, newGreen, newBlue;
                if (red >= mean)
                {
                    newRed = Math.Ceiling(red - redFactor);
                    if(newRed<mean)
                    {
                        newRed = mean;
                    }
                }
                else
                {
                    newRed = Math.Ceiling(red + redFactor);
                    if (newRed > mean)
                    {
                        newRed = mean;
                    }
                }

                if (green >= mean)
                {
                    newGreen = Math.Ceiling(green - greenFactor);
                    if (newGreen < mean)
                    {
                        newGreen = mean;
                    }
                }
                else
                {
                    newGreen = Math.Ceiling(green + greenFactor);
                    if (newGreen > mean)
                    {
                        newGreen = mean;
                    }
                }
                if (blue >= mean)
                {
                    newBlue = Math.Ceiling(blue - blueFactor);
                    if (newBlue <mean)
                    {
                        newBlue = mean;
                    }
                }
                else
                {
                    newBlue = Math.Ceiling(blue + blueFactor);
                    if (newBlue > mean)
                    {
                        newBlue = mean;
                    }
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
