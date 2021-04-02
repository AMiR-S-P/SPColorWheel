using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class TagModel : BaseModel
    {
        public string TagName { get; set; }
        public string Content { get; set; }
        public string  Attributes { get; set; }

        public ObservableCollection<TagModel> Childs { get; set; }
        public TagModel NextTag { get; set; }
        public TagModel Parent { get; set; }

    }
}
