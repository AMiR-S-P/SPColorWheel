using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class NamespaceModel:BaseModel
    {
        public string Namespace { get; set; }
        public string Prefix { get; set; }

        public override string ToString()
        {
            string p = string.IsNullOrEmpty(Prefix) ? string.Empty : ":" + Prefix;
            return $"xmlns{p}=\"{Namespace}\"";
        }

    }
}
