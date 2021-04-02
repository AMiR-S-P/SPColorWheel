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
    public class Shade : INotifyPropertyChanged
    {
        private Color mainShade = Colors.Transparent;
        private Color shade2;
        private Color shade3;
        private Color shade4;
        private Color shade5;
        private Color shade6;

        public Color MainShade { get => mainShade; set { mainShade = value; Task.Run(async () => { await CalculateShade(); }); OnPropertyChanged(); } }
        public Color Shade2 { get => shade2; set { shade2 = value; OnPropertyChanged(); } }
        public Color Shade3 { get => shade3; set { shade3 = value; OnPropertyChanged(); } }
        public Color Shade4 { get => shade4; set { shade4 = value; OnPropertyChanged(); } }
        public Color Shade5 { get => shade5; set { shade5 = value; OnPropertyChanged(); } }
        public Color Shade6 { get => shade6; set { shade6 = value; OnPropertyChanged(); } }


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        Task CalculateShade()
        {
            var color = MainShade.ToString();
            var factor = 5;

            double alpha = System.Convert.ToByte(color.Substring(1, 2), 16);
            double red = System.Convert.ToByte(color.Substring(3, 2), 16);
            double green = System.Convert.ToByte(color.Substring(5, 2), 16);
            double blue = System.Convert.ToByte(color.Substring(7, 2), 16);

            var redFactor = red / factor;
            var greenFactor = green / factor;
            var blueFactor = blue / factor;

            for (int i = 0; i < 5; i++)
            {
                double newRed = Math.Ceiling(red - redFactor * i);
                double newGreen = Math.Ceiling(green - greenFactor * i);
                double newBlue = Math.Ceiling(blue - blueFactor * i);

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

                if (i == 0)
                {
                    Shade2 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 1)
                {
                    Shade3 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 2)
                {
                    Shade4 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 3)
                {
                    Shade5 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
                else if (i == 4)
                {
                    Shade6 = Color.FromArgb((byte)alpha, (byte)newRed, (byte)newGreen, (byte)newBlue);
                }
            }

            return Task.CompletedTask;


        }
    }
}
