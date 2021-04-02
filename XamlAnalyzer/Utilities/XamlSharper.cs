using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using XamlAnalyzer.Model;

namespace XamlAnalyzer.Utilities
{
    public static class XamlSharper
    {
        /// <summary>
        /// Remove Class name ,
        /// Remove DataContext,
        /// add xmlns
        /// from Xaml
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns></returns>
        public static XamlFileModel Sharp(XamlFileModel xaml)
        {
            try
            {

                xaml.SharpedContent = Normalize(xaml.Content);
                //remove class 
                var className = GetClassName(xaml.SharpedContent);
                xaml.ClassName = className;
                if (!string.IsNullOrEmpty(className))
                {
                    xaml.SharpedContent = xaml.SharpedContent.Replace(className, string.Empty, StringComparison.CurrentCultureIgnoreCase);
                }

                //remove datacontexts
                var dataContexts = GetDataContexts(xaml.SharpedContent);
                foreach (var s in dataContexts)
                {
                    xaml.SharpedContent = xaml.SharpedContent.Replace(s, string.Empty, StringComparison.CurrentCultureIgnoreCase);
                }

                //get namespaces
                foreach (var n in GetNamespaces(xaml.SharpedContent))
                {
                    xaml.Namespaces.Add(n);
                }

                //get staticResources
                foreach (var s in GetAllStaticResourcesName(xaml.SharpedContent))
                {
                    xaml.RequiredStaticResources.Add(s);
                }

                //get DynamicResources
                foreach (var d in GetAllDynamicResourcesName(xaml.SharpedContent))
                {
                    xaml.RequiredDynamicResources.Add(d);
                }

                //get Resources
                foreach(var r in GetTagsResources(xaml.SharpedContent))
                {
                    xaml.Resources.Add(r);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }



            return xaml;
        }
        /// <summary>
        /// remove /r/n /t
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns></returns>
        static string Normalize(string xaml)
        {
            //xaml = xaml.Replace("\r", string.Empty);
            //xaml = xaml.Replace("\n", string.Empty);
            //xaml = xaml.Replace("\t", string.Empty);
            //return xaml.Trim();

            //xaml = Regex.Replace(xaml, "\\r\\n|\\r|\\t", "");
            //xaml = Regex.Replace(xaml, @"\\""", @"""");
            return xaml;
        }

        public static IEnumerable<string> GetDataContexts(string xaml)
        {
            List<string> list = new List<string>();

            Regex regex = new Regex(XamlParseRegexes.GetDataContext, RegexOptions.Multiline);
            var matches = regex.Matches(xaml);

            foreach (Match m in matches)
            {
                list.Add(m.Groups[0].Value.ToString());
            }

            return list;
        }
        public static string GetClassName(string xaml)
        {
            Regex regex = new Regex(XamlParseRegexes.GetClassName, RegexOptions.IgnoreCase);
            var match = regex.Match(xaml);

            return match.Groups[0].Value.ToString();//return full match
        }
        public static IEnumerable<NamespaceModel> GetNamespaces(string xaml)
        {
            List<NamespaceModel> namespaces = new System.Collections.Generic.List<NamespaceModel>();

            Regex regex = new Regex(XamlParseRegexes.GetNamespaces, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(xaml);

            foreach (Match m in matches)
            {
                NamespaceModel namespaceModel = new NamespaceModel();
                try
                {
                    namespaceModel.Namespace = m.Groups[2].Value.ToString();
                    namespaceModel.Prefix = m.Groups[1].Value.ToString();
                }
                catch (Exception ex)
                {
                }

                if (!string.IsNullOrEmpty(namespaceModel.Namespace))
                {
                    namespaces.Add(namespaceModel);
                }
            }


            return namespaces;
        }
        public static IEnumerable<StaticResourceModel> GetAllStaticResourcesName(string xaml)
        {
            List<StaticResourceModel> statics = new List<StaticResourceModel>();

            Regex regex = new Regex(XamlParseRegexes.GetRequiredStaticResources, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(xaml);

            foreach (Match m in matches)
            {
                var srm = new StaticResourceModel()
                {
                    Key = m.Groups[4].Value,
                    PropertyName = m.Groups[2].Value
                };
                if (!statics.Contains(srm))
                {
                    statics.Add(srm);
                }
            }

            return statics;
        }
        public static IEnumerable<DynamicResourceModel> GetAllDynamicResourcesName(string xaml)
        {
            List<DynamicResourceModel> dynamic = new List<DynamicResourceModel>();

            Regex regex = new Regex(XamlParseRegexes.GetRequiredDynamicResources, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(xaml);

            foreach (Match m in matches)
            {
                var drm = new DynamicResourceModel()
                {
                    Key = m.Groups[4].Value,
                    PropertyName = m.Groups[2].Value
                };
                if (!dynamic.Contains(drm))
                {
                    dynamic.Add(drm);
                }
            }

            return dynamic;
        }
        public static IEnumerable<TagResourcesModel> GetTagsResources(string xaml)
        {
            List<TagResourcesModel> resources = new List<TagResourcesModel>();
            Regex regex = new Regex(XamlParseRegexes.GetTagsResources, RegexOptions.IgnoreCase);
            //https://stackoverflow.com/questions/17003799/what-are-regular-expression-balancing-groups
            MatchCollection matches = regex.Matches(xaml);

            foreach(Match m in matches)
            {
                TagResourcesModel resourcesModel = new TagResourcesModel();
                resourcesModel.Content = m.Groups[3].Value;
                resourcesModel.TagName = m.Groups[2].Value;

                resources.Add(resourcesModel);
            }

            return resources;
        }

        /// <summary>
        /// Add 
        /// &lt;ResourceDic...&gt;
        ///     &lt;Res....Mergeddic...&gt;
        ///     &lt;/Res....Mergeddic...&gt;
        /// &lt;/ResourceDic...&gt;
        /// to root of the xaml and return it.
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns></returns>
        public static string AddResourceTagToRootTag(string xaml)
        {
            string result = "";



            return result;
        }
    }
}
