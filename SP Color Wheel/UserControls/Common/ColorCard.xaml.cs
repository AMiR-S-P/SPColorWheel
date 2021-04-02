using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SP_Color_Wheel.UserControls.Common
{
    /// <summary>
    /// Interaction logic for ColorCard.xaml
    /// </summary>
    public partial class ColorCard : Border, INotifyPropertyChanged
    {
        private bool isLocked;
        private bool isLockable;
        private string cardName;
        object tempDataContext;
        string tempColor;

        public static readonly DependencyProperty CurrentColorProperty =
            DependencyProperty.Register("CurrentColor", typeof(Color), typeof(ColorCard), new PropertyMetadata(Colors.Transparent/*,CurrentColorChanged*/));

        private static void CurrentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var val = d as ColorCard;
            //val.ColorCode = e.NewValue.ToString();
        }

        public static readonly DependencyProperty ShowBackgroundProperty =
            DependencyProperty.Register("ShowBackground", typeof(bool), typeof(ColorCard), new PropertyMetadata(false, ShowBackgroundChangedCallback));


        public Color CurrentColor
        {
            get { return (Color)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }
        public string ColorCode
        {
            get => CurrentColor.ToString();
            private set
            {
                CurrentColor = (Color)ColorConverter.ConvertFromString(value?.ToString());
                OnPropertyChanged();
            }
        }
        public string CardName { get => cardName; set { cardName = value; OnPropertyChanged(); } }
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                isLocked = value;
                OnPropertyChanged();
                if (value)
                {
                    tempDataContext = DataContext;
                    tempColor = ColorCode;
                    DataContext = null;
                    ColorCode = tempColor;
                }
                else
                {
                    DataContext = tempDataContext;
                }
            }
        }
        public bool IsLockable
        {
            get => isLockable;
            set
            {
                isLockable = value;
                OnPropertyChanged();
                if (!value)
                {
                    lockBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lockBtn.Visibility = Visibility.Visible;
                }
            }
        }
        public bool ShowBackground
        {
            get { return (bool)GetValue(ShowBackgroundProperty); }
            set { SetValue(ShowBackgroundProperty, value); }
        }


        private static void ShowBackgroundChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var val = d as ColorCard;
            if ((bool)e.NewValue == true)
            {
                val.backgroundImage.Visibility = Visibility.Visible;
            }
            else
            {
                val.backgroundImage.Visibility = Visibility.Hidden;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertName));
        }
        public ColorCard()
        {
            InitializeComponent();
            IsLockable = false;

            showBackgroundMI.SetBinding(MenuItem.IsCheckedProperty, new Binding("ShowBackground") { Source = this });
        }
    }
}
