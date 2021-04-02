using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SP_Color_Wheel.Helper
{
    public class Tone : INotifyPropertyChanged
    {

        private Color mainTone = Colors.Transparent;
        private Color tone2;
        private Color tone3;
        private Color tone4;
        private Color tone5;
        private Color tone6;

        public Color MainTone { get => mainTone; set { mainTone = value;Task.Run(async () => { await CalculateTone(); }); OnPropertyChanged(); } }
        public Color Tone2 { get => tone2; set { tone2 = value; OnPropertyChanged(); } }
        public Color Tone3 { get => tone3; set { tone3 = value; OnPropertyChanged(); } }
        public Color Tone4 { get => tone4; set { tone4 = value; OnPropertyChanged(); } }
        public Color Tone5 { get => tone5; set { tone5 = value; OnPropertyChanged(); } }
        public Color Tone6 { get => tone6; set { tone6 = value; OnPropertyChanged(); } }


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        Task CalculateTone()
        {
            var color = MainTone.ToString();
            var factor = 5;

            double alpha = System.Convert.ToByte(color.Substring(1, 2), 16);
            double red = System.Convert.ToByte(color.Substring(3, 2), 16);
            double green = System.Convert.ToByte(color.Substring(5, 2), 16);
            double blue = System.Convert.ToByte(color.Substring(7, 2), 16);

            var mean = (blue + red + green) / 3;

            var redFactor = (255 - mean) / factor;
            var greenFactor = (255 - mean) / factor;
            var blueFactor = (255 - mean) / factor;

            for (int i = 0; i < 5; i++)
            {
                double newRed, newGreen, newBlue;
                if (red >= mean)
                {
                    newRed = Math.Ceiling(red - redFactor * i);
                    if (newRed < mean)
                    {
                        newRed = mean;
                    }
                }
                else
                {
                    newRed = Math.Ceiling(red + redFactor * i);
                    if (newRed > mean)
                    {
                        newRed = mean;
                    }
                }

                if (green >= mean)
                {
                    newGreen = Math.Ceiling(green - greenFactor * i);
                    if (newGreen < mean)
                    {
                        newGreen = mean;
                    }
                }
                else
                {
                    newGreen = Math.Ceiling(green + greenFactor * i);
                    if (newGreen > mean)
                    {
                        newGreen = mean;
                    }
                }
                if (blue >= mean)
                {
                    newBlue = Math.Ceiling(blue - blueFactor * i);
                    if (newBlue < mean)
                    {
                        newBlue = mean;
                    }
                }
                else
                {
                    newBlue = Math.Ceiling(blue + blueFactor * i);
                    if (newBlue > mean)
                    {
                        newBlue = mean;
                    }
                }

                if (i == 0)
                {
                    Tone2 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 1)
                {
                    Tone3 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 2)
                {
                    Tone4 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 3)
                {
                    Tone5 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i ==4)
                {
                    Tone6 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
            }

            return Task.CompletedTask;
        }
    }
}
