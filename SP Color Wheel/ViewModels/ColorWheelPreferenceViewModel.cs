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
    public class ColorWheelPreferenceViewModel : BaseViewModel, IPreferenceControl
    {
        private string title;
        private ObservableCollection<string> imageSources = new ObservableCollection<string>();
        private string selectedImage;

        public string Title { get => title; set { title = value; OnPropertyChanged(); } }
        public ObservableCollection<string> ImageSources { get => imageSources; set => imageSources = value; }
        public string SelectedImage { get => selectedImage; set { selectedImage = value;OnPropertyChanged(); } }
        public ColorWheelPreferenceViewModel()
        {
            ImageSources.Add("Nothing");
            ImageSources.Add("/Resources/Images/lock.png");
            ImageSources.Add("/Resources/Images/link.png"); ImageSources.Add("/Resources/Images/colorfull.png");
            ImageSources.Add("/Resources/Images/lock.png");
            ImageSources.Add("/Resources/Images/link.png"); ImageSources.Add("/Resources/Images/colorfull.png");
            ImageSources.Add("/Resources/Images/lock.png");
            ImageSources.Add("/Resources/Images/link.png"); ImageSources.Add("/Resources/Images/colorfull.png");
            ImageSources.Add("/Resources/Images/lock.png");
            ImageSources.Add("/Resources/Images/link.png");
        }
    }
}
