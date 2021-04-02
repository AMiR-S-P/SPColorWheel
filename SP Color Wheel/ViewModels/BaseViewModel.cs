using SP_Color_Wheel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged,ISettingsHelper
    {
        private bool isBusy;

        public bool IsBusy { get => isBusy; set { isBusy = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;



        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void LoadSettings()
        {
        }
        public virtual void SaveSettings()
        {

        }
    }
}
