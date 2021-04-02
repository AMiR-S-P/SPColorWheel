
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class ExportControlModel : ControlModel
    {
        private bool isSelected;
        private bool isMarked;

        public bool IsSelected { get => isSelected; set { isSelected = value; OnPropertyChanged(); } }
        public bool IsMarked { get => isMarked; set { isMarked = value;OnPropertyChanged(); } }

    }
}
