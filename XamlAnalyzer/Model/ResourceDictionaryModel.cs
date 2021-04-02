using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class ResourceDictionaryModel
    {
        public string  Content { get; set; }
        public ObservableCollection<NamespaceModel> Namespaces { get; set; } = new ObservableCollection<NamespaceModel>();

        public ObservableCollection<string> Resources { get; set; } = new ObservableCollection<string>();

    }
}
