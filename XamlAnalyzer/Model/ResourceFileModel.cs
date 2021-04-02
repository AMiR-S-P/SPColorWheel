using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XamlAnalyzer.Model
{
    public class ResourceFileModel:BaseModel
    {
        private bool isUsing = false;

        public string FilePath { get; set; }

        public string Content { get; set; }
        public string Name { get; set; }

        public ObservableCollection<string> Resources { get; set; } = new ObservableCollection<string>();

        public bool IsUsing { get => isUsing; set { isUsing = value; OnPropertyChanged(); } }
        public string Source { get { return $"pack://siteoforigin:,,,/Workplace/{Name}"; } }
        public string Tag { get { return $"<ResourceDictionary Source=\"pack://siteoforigin:,,,/Workplace/{Name}\" />"; } }

        //public override bool Equals(object obj)
        //{
        //    var o = obj as ResourceFileModel;
        //    return o.FilePath == FilePath;
        //}

    }
}
