using SP_Color_Wheel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.ViewModels
{
    public class XamlPreferenceViewModel : BaseViewModel, IPreferenceControl
    {
        private string title;

        public string Title { get => title; set { title = value; OnPropertyChanged(); } }
    }
}
