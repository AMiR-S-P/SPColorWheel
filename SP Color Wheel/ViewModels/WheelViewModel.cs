using Newtonsoft.Json;
using SP_Color_Wheel.Enums;
using SP_Color_Wheel.EventArguments;
using SP_Color_Wheel.Helper;
using SP_Color_Wheel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SP_Color_Wheel.ViewModels
{

    [JsonObject(MemberSerialization.OptIn)]
    public class WheelViewModel : BaseViewModel, IColorSelector
    {
        private string selectorName;
        private Color mainColor;
        private Color color2;
        private Color color3;
        private Color color4;
        private Color color5;
        private Tint mainColorTint = new Tint();
        private Tone mainColorTone = new Tone();
        private Shade mainColorShade = new Shade();


        private Color rawMainColor;
        private Color rawColor2;
        private Color rawColor3;
        private Color rawColor4;
        private Color rawColor5;




        public string SelectorName { get => selectorName; set { selectorName = value; OnPropertyChanged(); } }
        public Color MainColor
        {
            get => mainColor; set
            {
                mainColor = value;
                MainColorTint.MainTint = value;
                MainColorTone.MainTone = value;
                MainColorShade.MainShade = value;
                OnPropertyChanged();
            }
        }
        public Color Color2 { get => color2; set { color2 = value; OnPropertyChanged(); } }
        public Color Color3 { get => color3; set { color3 = value; OnPropertyChanged(); } }
        public Color Color4 { get => color4; set { color4 = value; OnPropertyChanged(); } }
        public Color Color5 { get => color5; set { color5 = value; OnPropertyChanged(); } }
        public Tint MainColorTint { get => mainColorTint; set { mainColorTint = value; OnPropertyChanged(); } }
        public Tone MainColorTone { get => mainColorTone; set { mainColorTone = value; OnPropertyChanged(); } }
        public Shade MainColorShade { get => mainColorShade; set { mainColorShade = value; OnPropertyChanged(); } }
        public IColorProperties Properties { get; set; }


        public Color RawMainColor { get => rawMainColor; set { rawMainColor = value; OnPropertyChanged(); } }
        public Color RawColor2 { get => rawColor2; set { rawColor2 = value; OnPropertyChanged(); } }
        public Color RawColor3 { get => rawColor3; set { rawColor3 = value; OnPropertyChanged(); } }
        public Color RawColor4 { get => rawColor4; set { rawColor4 = value; OnPropertyChanged(); } }
        public Color RawColor5 { get => rawColor5; set { rawColor5 = value; OnPropertyChanged(); } }



        #region ColorCircle
        private bool hasRed = true;
        private bool hasBlue = true;
        private bool hasGreen = true;

        public bool HasRed
        {
            get => hasRed; set
            {
                hasRed = value;
                //SP_Color_Wheel.Properties.Settings.Default.HasRed = value;
                //SP_Color_Wheel.Properties.Settings.Default.Save();
                OnPropertyChanged();
                OnColorIncludedChanged(value, hasGreen, hasBlue);
            }
        }
        public bool HasGreen
        {
            get => hasGreen; set
            {
                hasGreen = value;
                //SP_Color_Wheel.Properties.Settings.Default.HasGreen = value;
                //SP_Color_Wheel.Properties.Settings.Default.Save();
                OnPropertyChanged();
                OnColorIncludedChanged(hasRed, value, hasBlue);
            }
        }
        public bool HasBlue
        {
            get => hasBlue; set
            {
                hasBlue = value;
                //SP_Color_Wheel.Properties.Settings.Default.HasBlue = value;
                //SP_Color_Wheel.Properties.Settings.Default.Save();
                OnPropertyChanged();
                OnColorIncludedChanged(hasRed, hasGreen, value);
            }
        }

        public event EventHandler<ColorIncludedChanged> ColorIncludedChanged;

        public void OnColorIncludedChanged(bool hasRed, bool hasGreen, bool hasBlue)
        {
            ColorIncludedChanged?.Invoke(this, new EventArguments.ColorIncludedChanged(hasRed, hasGreen, hasBlue));
        }

        #endregion

        #region PointersContainer

        private PointerHarmonyType harmonyType;

        public PointerHarmonyType HarmonyType
        {
            get => harmonyType; set
            {
                harmonyType = value;
                //SP_Color_Wheel.Properties.Settings.Default.ColorHarmony = (int)value;
                //SP_Color_Wheel.Properties.Settings.Default.Save();
                OnPropertyChanged();   
                OnColorHarmonyChanged(value);

            }
        }

        public event EventHandler<ColorHarmonyChangedEventArgs> ColorHarmonyChanged;

        public void OnColorHarmonyChanged(PointerHarmonyType type) => ColorHarmonyChanged?.Invoke(this, new ColorHarmonyChangedEventArgs(type));


        #region AnalogousProperties
        private double angle;
        private bool isWheelLoaded;

        public double Angle
        {
            get => angle; set
            {
                angle = value;
                //SP_Color_Wheel.Properties.Settings.Default.Angle = value;
                //SP_Color_Wheel.Properties.Settings.Default.Save();
                OnAngleChanged(value);
                OnPropertyChanged();
            }
        }


        public event EventHandler<AngleChangedEventArgs> AngleChanged;
        public void OnAngleChanged(double newAngle)
        {
            AngleChanged?.Invoke(this, new AngleChangedEventArgs(newAngle));
        }
        #endregion

        #endregion

        public bool IsWheelLoaded
        {
            get => isWheelLoaded; set
            {
                isWheelLoaded = value;
                if (value)
                {
                    LoadSettings();
                }
                OnPropertyChanged();
            }
        }
        public WheelViewModel()
        {
            SelectorName = "Wheel";
            
        }

        public override void LoadSettings()
        {
            HarmonyType = (PointerHarmonyType)SP_Color_Wheel.Properties.Settings.Default.ColorHarmony;
            
            Angle = SP_Color_Wheel.Properties.Settings.Default.Angle;

            HasBlue = SP_Color_Wheel.Properties.Settings.Default.HasBlue;
            HasRed = SP_Color_Wheel.Properties.Settings.Default.HasRed;
            HasGreen = SP_Color_Wheel.Properties.Settings.Default.HasGreen;
            base.LoadSettings();
        }

        public override void SaveSettings()
        {
            SP_Color_Wheel.Properties.Settings.Default.HasBlue = HasBlue;
            SP_Color_Wheel.Properties.Settings.Default.HasGreen = HasGreen;
            SP_Color_Wheel.Properties.Settings.Default.HasRed = HasRed;

            SP_Color_Wheel.Properties.Settings.Default.ColorHarmony = (int)HarmonyType;

            SP_Color_Wheel.Properties.Settings.Default.Angle = Angle;


            SP_Color_Wheel.Properties.Settings.Default.Save();
            base.SaveSettings();
        }

        //void ISettingsHelper.LoadSettings()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
