using SP_Color_Wheel.Interfaces;
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
    public class Tint : INotifyPropertyChanged
    {
        private Color mainTint = Colors.Transparent;
        private Color tint2;
        private Color tint3;
        private Color tint4;
        private Color tint5;
        private Color tint6;

        public Color MainTint { get => mainTint; set { mainTint = value; Task.Run(async () => { await CalculateTint(); }); OnPropertyChanged(); } }
        public Color Tint2 { get => tint2; set { tint2 = value; OnPropertyChanged(); } }
        public Color Tint3 { get => tint3; set { tint3 = value; OnPropertyChanged(); } }
        public Color Tint4 { get => tint4; set { tint4 = value; OnPropertyChanged(); } }
        public Color Tint5 { get => tint5; set { tint5 = value; OnPropertyChanged(); } }

        public Color Tint6 { get => tint6; set { tint6 = value; OnPropertyChanged(); } }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        Task CalculateTint()
        {
            var color = MainTint.ToString();

            var factor = 5;

            double alpha = System.Convert.ToByte(color.Substring(1, 2), 16);
            double red = System.Convert.ToByte(color.Substring(3, 2), 16);
            double green = System.Convert.ToByte(color.Substring(5, 2), 16);
            double blue = System.Convert.ToByte(color.Substring(7, 2), 16);

            var redFactor = (255 - red) / factor;
            var greenFactor = (255 - green) / factor;
            var blueFactor = (255 - blue) / factor;
            for (int i = 0; i < 5; i++)
            {

                double newRed = Math.Ceiling(red + redFactor * i);
                double newGreen = Math.Ceiling(green + greenFactor * i);
                double newBlue = Math.Ceiling(blue + blueFactor * i);

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

                if (i == 0)
                {
                    Tint2 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 1)
                {
                    Tint3 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 2)
                {
                    Tint4 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 3)
                {
                    Tint5 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 4)
                {
                    Tint6 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
            }


            return Task.CompletedTask;

        }
    }
}
