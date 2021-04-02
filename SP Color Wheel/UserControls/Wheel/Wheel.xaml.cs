using SP_Color_Wheel.Enums;
using SP_Color_Wheel.Helper;
using SP_Color_Wheel.Interfaces;
using SP_Color_Wheel.UserControls.Common;
using SP_Color_Wheel.ViewModels;
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

namespace SP_Color_Wheel.UserControls.Wheel
{
    /// <summary>
    /// Interaction logic for Wheel.xaml
    /// </summary>
    public partial class Wheel : Border,/* IColorSelector,*/ INotifyPropertyChanged
    {


        public new bool IsLoaded
        {
            get { return (bool)GetValue(IsLoadedProperty); }
            set { SetValue(IsLoadedProperty, value); }
        }

        public static readonly DependencyProperty IsLoadedProperty =
            DependencyProperty.Register("IsLoaded", typeof(bool), typeof(Wheel), new PropertyMetadata(false));



        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public Wheel()
        {

            InitializeComponent();

            Loaded += Wheel_Loaded;
            DataContextChanged += Wheel_DataContextChanged;
            
        }

        private void Wheel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)//save data immediatly 
            {
                (e.OldValue as ViewModels.WheelViewModel).SaveSettings();
            }
        }

        private  void Wheel_Loaded(object sender, RoutedEventArgs e)
        {
            IsLoaded = true;
        }
    }
}
