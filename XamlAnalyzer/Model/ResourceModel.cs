using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class StaticResourceModel : BaseModel
    {
        public string PropertyName { get; set; }
        public string Key { get; set; }
        public bool IsLoaded { get; set; }

        public override string ToString()
        {
            return PropertyName + "=\"{StaticResource " + Key + "}\"";
        }

        public override bool Equals(object obj)
        {
            var o = obj as StaticResourceModel;
            return o.Key == Key && PropertyName == o.PropertyName;
        }
    }
    public class DynamicResourceModel : BaseModel
    {
        public string PropertyName { get; set; }
        public string Key { get; set; }
        public bool IsLoaded { get; set; }

        public override string ToString()
        {
            return PropertyName + "=\"{DynamicResource " + Key + "}\"";
        }
        public override bool Equals(object obj)
        {
            var o = obj as DynamicResourceModel;
            return o.Key == Key && PropertyName == o.PropertyName;
        }
    }
}
