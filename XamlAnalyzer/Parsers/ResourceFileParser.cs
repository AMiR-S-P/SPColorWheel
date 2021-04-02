using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XamlAnalyzer.Model;
using XamlAnalyzer.Utilities;

namespace XamlAnalyzer.Parsers
{
    public class ResourceFileParser
    {
        //public static ResourceDictionaryModel Parse(ResourceFileModel model)
        //{
        //    try
        //    {
        //        ResourceDictionaryModel dictionaryModel = new ResourceDictionaryModel();
        //        Regex regex = new Regex(XamlParseRegexes.GetResourceDictionaryContent);

        //        //content of RD without root tag
        //        Match content = regex.Match(model.Content);

        //        dictionaryModel.Content = content.Groups[2].Value;

        //        foreach (Match m in new Regex(XamlParseRegexes.GetOneLineTags).Matches(content.Groups[2].Value))
        //        {
        //            dictionaryModel.Resources.Add(m.Groups[1].Value);
        //        }
        //        foreach (Match m in new Regex(XamlParseRegexes.GetOpenCloseTags).Matches(content.Groups[2].Value))
        //        {
        //            dictionaryModel.Resources.Add(m.Groups[1].Value);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
        //    }
        //}
    }
}

