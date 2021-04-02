using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel currentViewModel;

        public BaseViewModel CurrentViewModel { get => currentViewModel; set { currentViewModel = value;OnPropertyChanged(); } }


    }
}
