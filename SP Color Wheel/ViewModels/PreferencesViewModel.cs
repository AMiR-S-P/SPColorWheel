using SP_Color_Wheel.Interfaces;
using SP_Color_Wheel.UserControls.Preferences;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.ViewModels
{
    public class PreferencesViewModel
    {
        public ObservableCollection<IPreferenceControl> Preferences { get; set; } = new ObservableCollection<IPreferenceControl>();

        public PreferencesViewModel()
        {
            Preferences.Add(new ColorWheelPreferenceViewModel() { Title = "Color Wheel" });
            Preferences.Add(new XamlPreferenceViewModel() { Title = "Xaml" });

        }
    }
}
