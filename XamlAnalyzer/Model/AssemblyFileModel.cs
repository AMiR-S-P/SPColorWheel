using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class AssemblyFileModel:BaseModel
    {
        public string Name { get; set; }
        public string  Path { get; set; }

        public ObservableCollection<string> ClassNames { get; set; } = new ObservableCollection<string>();
    }
}
