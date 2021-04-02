using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XamlAnalyzer
{
    public static class XamlParseRegexes
    {
        public static string GetTagsResources = @"((?<word><([\w]+)).Resources>)([\w\W]*?)((?<word><\/[\w]+).Resources>)";
        public static string GetRequiredDynamicResources = @"\s+([\w]+)=""{DynamicResource\s+([^}]*)}""";
        public static string GetRequiredStaticResources = @"\s+([\w]+)=""{StaticResource\s+([^}]*)}""";
        public static string GetNamespaces = @"xmlns(?([:]):(\w+))=""([^\s|\n|\r\n|\t] +)""";
        public static string GetClassName= @"x:Class=""([^""]*)""";
        public static string GetDataContext = @"DataContext[.]*[^""]+""([{]?[.]*[^""]*[}]?)[.]*[^""]+""";
        public static string GetResourceDictionaryContent = @"<ResourceDictionary>([\w\W]*?)</ResourceDictionary>";
        public static string GetMergedDictionarayContent = @"<ResourceDictionary.MergedDictionaries>([\w\W]*?)</ResourceDictionary.MergedDictionaries>";
        public static string GetResourceDictionarySource = @"<ResourceDictionary[\s|\n]*Source=""([\w\W] *?)""/>";
        public static string GetRootTagWithContentsAndAttributes = @"<(?<tag>(?([\w]+[:][\w]+)([\w]+[:][\w]+)|([\w]+)))([^>]*)>([\w\W]*)</(\k<tag>)>";

        public static string GetAllStartTags = @"(<([\w]+)[\s|\n]+([^>]+)[^/]>)+";
        /// <summary>
        /// Only first match
        /// </summary>
        public static string GetRootTag = @"(?:<([\w]+)[\s|\n]+([^>]+)[^/]>)?";

        /// <summary>
        /// Without Root Tag (&lt;ResourceDictionary xmln...)
        /// </summary>
        public static string GetResourceDictionaryFileContent = @"<ResourceDictionary[\s\n]+([^>]*)>([\w\W]+?)</ResourceDictionary>";
        public static string GetOneLineTags = @"(<[^!\/][^>]+/>)";
        public static string GetOpenCloseTags = @"(<[^!\/>]+>[\w\W]*?</[^!\/][^>]+>)";

    }
}
