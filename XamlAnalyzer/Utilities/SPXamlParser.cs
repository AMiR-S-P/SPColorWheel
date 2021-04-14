using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using XamlAnalyzer.Model;

namespace XamlAnalyzer.Utilities
{
    public class SPXamlParser : DependencyObject
    {
        public readonly string XMLNS = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        AssemblyLoadContext assemblyLoad = new AssemblyLoadContext("XamlEditor", true);


        public static readonly DependencyProperty XamlProperty = DependencyProperty.Register(
            nameof(Xaml),
            typeof(string),
            typeof(SPXamlParser),
            new PropertyMetadata(""));


        public string Xaml { get => (string)GetValue(XamlProperty); private set => SetValue(XamlProperty, value); }
        public XmlDocument Document { get; private set; }

        public ObservableCollection<ResourceFileModel> ImportedResources { get; set; } = new ObservableCollection<ResourceFileModel>();
        public ObservableCollection<AssemblyFileModel> ImportedAssemblies { set; get; } = new ObservableCollection<AssemblyFileModel>();



        public event EventHandler XamlChanged;
        void OnXamlChanged()
        {
            XamlChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// Parse xaml with XmlDocument
        /// </summary>
        /// <param name="xaml">xaml to parse</param>
        public SPXamlParser(string xaml)
        {
            Document = new XmlDocument();
            XmlSchema xmlSchema = new XmlSchema();
            xmlSchema.Namespaces.Add("xmlns", XMLNS);
            Document.Schemas.Add(xmlSchema);
            Task.Run(async () =>
            {
                await LoadXaml(xaml);
            });
            InitEvents();
        }

        /// <summary>
        /// Call LoadXaml(string xaml) after init 
        /// </summary>
        public SPXamlParser()
        {
            InitEvents();
        }

        void InitEvents()
        {
            ImportedResources.CollectionChanged += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(Xaml))
                {
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    {
                        //remove from document then put it in xamlfile.Content
                        await RemoveResourceFromXaml((e.OldItems[0] as ResourceFileModel));
                    }
                    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    {
                        await AddResourceToXaml((e.NewItems[0] as ResourceFileModel));
                    }
                    await FormatXaml();
                }
            };

            ImportedAssemblies.CollectionChanged += async (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    //var Loadedassembly = assemblyLoad.LoadFromAssemblyPath((e.NewItems[0] as AssemblyFileModel).Path);
                    //foreach (var c in Loadedassembly.GetTypes())
                    //{
                    //    (e.NewItems[0] as AssemblyFileModel).ClassNames.Add(c.FullName);
                    //}
                    await ReloadAssembelies();
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    //assemblyLoad.Assemblies.
                    //assemblyLoad.Unload();
                    await ReloadAssembelies();
                }

            };
        }
        public async Task ResetXamlParser()
        {
            try
            {
                await LoadXaml("");
                ImportedResources.Clear();
                ImportedAssemblies.Clear();
                assemblyLoad.Unload();
            }
            catch (Exception ex)
            {

            }
        }
        public async Task LoadXaml(string xaml)
        {
            try
            {
                Document.LoadXml(xaml);
                ((XmlElement)Document.ChildNodes[0]).SetAttribute("xmlns", XMLNS);
                Xaml = Document.InnerXml;
                await FormatXaml();
            }
            catch (Exception ex)
            {

            }
        }
        public void ReloadXaml()
        {
            try
            {
                Document.LoadXml(Xaml);
            }
            catch (Exception ex)
            {

            }
        }
        public Task ReloadAssembelies()
        {
            assemblyLoad = new AssemblyLoadContext("XamlEditor", true);
            foreach (var assembly in ImportedAssemblies)
            {
                var Loadedassembly = assemblyLoad.LoadFromAssemblyPath(assembly.Path);
                foreach (var c in Loadedassembly.GetTypes())
                {
                    assembly.ClassNames.Add(c.FullName);
                }
            }
            return Task.CompletedTask;
        }
        public async Task FormatXaml()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var xmlW = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true, Async = true, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
                {
                    try
                    {
                        Document.LoadXml(Xaml);
                        //((XmlElement)Document.ChildNodes[0]).SetAttribute("xmlns", XMLNS);
                        Xaml = Document.InnerXml;
                        //LoadXaml(Xaml);

                        //ms.WriteAsync(Encoding.UTF8.GetBytes(Xaml), 0, xaml.Length);
                        Document.WriteContentTo(xmlW);
                        await xmlW.FlushAsync();
                        ms.Position = 0;

                        var formatedXaml = await new StreamReader(ms).ReadToEndAsync();
                        Xaml = formatedXaml;
                        //LoadXaml(formatedXaml);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public async Task RemoveResourceFromXaml(ResourceFileModel resource)
        {
            await RemoveResourceDictionaryFromRootMerged(resource.Tag);
            resource.IsUsing = false;
        }
        public async Task AddResourceToXaml(ResourceFileModel resource)
        {
            if (!string.IsNullOrEmpty(Xaml))
            {
                ////manage Resources tag
                ReloadXaml();
                if (!await RootElementContainResourceTag())
                {
                    await AddResourceTagToRoot();
                }
                if (!await RootElementResourceContainResourceDictionary())
                {
                    await AddResourceDictionaryTagToRootResource();
                }
                if (!await RootElementResourceContainMergedDictionary())
                {
                    await AddMergedDictionaryTagToRootResourceDictionary();
                }

                await AddContentToMergedDictionaryTagOfRootResourceDictionary(resource.Tag);
                resource.IsUsing = true;
            }
        }
        public async Task AddResourceFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (FileStream file = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = new byte[file.Length];
                await file.ReadAsync(bytes);
                ResourceFileModel resourceFileModel = new ResourceFileModel()
                {
                    Content = Encoding.UTF8.GetString(bytes),
                    FilePath = path,
                    Name = path.Substring(path.LastIndexOf('\\') + 1)
                };
                var resource = ImportedResources.Where(x => x.Source == resourceFileModel.Source).FirstOrDefault();
                if (resource != null)
                {
                    ImportedResources.Remove(resource);
                }

                SPXamlParser parser = new SPXamlParser(resourceFileModel.Content);
                foreach (XmlNode n in parser.Document.ChildNodes[0].ChildNodes)
                {
                    XmlAttribute key = null;
                    if (n.Attributes != null)
                    {
                        foreach (XmlAttribute a in n.Attributes)
                        {
                            if (a.Name.Contains(":Key"))
                            {
                                key = a;
                                break;
                            }
                        }
                    }
                    if (key == null)
                    {
                        resourceFileModel.Resources.Add(n.Name);
                    }
                    else //contain key
                    {
                        resourceFileModel.Resources.Add(key.Value);
                    }
                }

                ImportedResources.Add(resourceFileModel);
            }

        }
        public Task RemoveAssembly(AssemblyFileModel assembly)
        {
            ImportedAssemblies.Remove(assembly);

            return Task.CompletedTask;
        }
        public Task AddAssemblyFile(AssemblyFileModel assembly)
        {
            ImportedAssemblies.Add(assembly);

            return Task.CompletedTask;
        }

        public async Task<bool> RootElementContainResourceTag()
        {
            return await Task.Run(() =>
            {
                foreach (XmlNode n in Document.ChildNodes[0]?.ChildNodes)
                {
                    if (n.Name == Document.ChildNodes[0].Name + ".Resources")
                    {
                        return true;
                    }
                }
                return false;
            });
        }
        public async Task<bool> RootElementContainResourceAttribute()
        {
            return await Task.Run(() =>
            {
                foreach (XmlNode n in Document.ChildNodes[0]?.ChildNodes)
                {
                    if (n.Name == Document.ChildNodes[0].Name + ".Resources")
                    {
                        return true;
                    }
                }
                return false;
            });
        }
        public async Task<bool> RootElementResourceContainMergedDictionary()
        {
            return await Task.Run(() =>
            {
                foreach (XmlNode n in Document.ChildNodes[0]?.ChildNodes)
                {
                    if (n.Name == Document.ChildNodes[0].Name + ".Resources")
                    {
                        foreach (XmlNode r in n.ChildNodes)
                        {
                            if (r.Name == "ResourceDictionary")
                            {
                                foreach (XmlNode m in r.ChildNodes)
                                {
                                    if (m.Name == "ResourceDictionary.MergedDictionaries")
                                    {
                                        return true;
                                    }
                                }
                                return false;
                            }
                        }
                    }
                }
                return false;
            });
        }
        public async Task<bool> RootElementResourceContainResourceDictionary()
        {
            return await Task.Run(() =>
            {
                foreach (XmlNode n in Document.ChildNodes[0]?.ChildNodes)
                {
                    if (n.Name == Document.ChildNodes[0].Name + ".Resources")
                    {
                        foreach (XmlNode r in n.ChildNodes)
                        {
                            if (r.Name == "ResourceDictionary")
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            });
        }

        public async Task RemoveResourceDictionaryFromRootMerged(string tag)
        {

            foreach (XmlNode node in Document.ChildNodes[0].ChildNodes)
            {
                if (node.Name == Document.ChildNodes[0].Name + ".Resources")
                {
                    foreach (XmlNode rd in node.ChildNodes)
                    {
                        if (rd.Name == "ResourceDictionary")
                        {
                            foreach (XmlNode md in rd.ChildNodes)
                            {
                                if (md.Name == "ResourceDictionary.MergedDictionaries")
                                {
                                    XmlNode tagToRemove = null;
                                    foreach (XmlNode t in md.ChildNodes)
                                    {
                                        var x = t.OuterXml.Replace(" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"", "");
                                        if (x == tag)
                                        {
                                            tagToRemove = t;
                                            if (tagToRemove != null)
                                            {
                                                md.RemoveChild(tagToRemove);
                                            }
                                            //break;
                                        }
                                    }

                                    await LoadXaml(Document.InnerXml);
                                 }
                            }
                        }
                    }
                }
            }
           await LoadXaml(Document.InnerXml);
         }

        public async Task<string> GetRootResourceAttribute()
        {
            return await Task.Run(() =>
            {
                return Document.ChildNodes[0]?.Attributes["Resources"].Value;
            });
        }
        public async Task<string> GetRootResourceTag()
        {
            return await Task.Run(() =>
            {
                if (Document?.ChildNodes.Count > 0)
                {
                    foreach (XmlNode node in Document.ChildNodes[0]?.ChildNodes)
                    {
                        if (node.Name == Document.ChildNodes[0].Name + ".Resources")
                        {
                            return node.InnerXml;
                        }
                    }
                }
                return "";
            });
        }

        public async Task AddResourceDictionaryTagToRootResource()
        {

            try
            {
                string rootName = Document.ChildNodes[0].Name;
                string resourceTag = Document.ChildNodes[0].Name + ".Resources";
                XmlElement node = null;
                foreach (XmlNode n in Document.ChildNodes[0].ChildNodes)
                {
                    if (n.Name == resourceTag)
                    {
                        node = (XmlElement)n;
                        break;
                    }
                }
                var resourceDic = Document.CreateElement($"ResourceDictionary", XMLNS);
                resourceDic.InnerText = Environment.NewLine;

                var mergedDic = Document.CreateElement($"ResourceDictionary.MergedDictionaries", XMLNS);
                mergedDic.InnerText = Environment.NewLine;

                resourceDic.PrependChild(mergedDic);
                node?.PrependChild(resourceDic);

            }
            catch (Exception ex)
            {
            }
            await LoadXaml(Document.InnerXml);
 
        }
        public async Task AddMergedDictionaryTagToRootResourceDictionary()
        {
            try
            {
                string rootName = Document.ChildNodes[0].Name;
                string resourceTag = Document.ChildNodes[0].Name + ".Resources";
                XmlElement node = null;
                foreach (XmlNode n in Document.ChildNodes[0].ChildNodes)
                {
                    if (n.Name == resourceTag)
                    {
                        foreach (XmlNode r in n.ChildNodes)
                        {
                            if (r.Name == "ResourceDictionary")
                            {

                                node = (XmlElement)r;
                            }
                        }
                    }
                }

                var mergedDic = Document.CreateElement($"ResourceDictionary.MergedDictionaries", XMLNS);
                mergedDic.InnerText = Environment.NewLine;

                node?.PrependChild(mergedDic);

            }
            catch (Exception ex)
            {
            }
            await LoadXaml(Document.InnerXml);
 
        }
        public async Task AddContentToMergedDictionaryTagOfRootResourceDictionary(string content)
        {

            try
            {
                string rootName = Document.ChildNodes[0].Name;
                string resourceTag = Document.ChildNodes[0].Name + ".Resources";
                string merged = "ResourceDictionary.MergedDictionaries";
                XmlElement node = null;
                XmlNode mergedNode = null;
                foreach (XmlNode n in Document.ChildNodes[0].ChildNodes)
                {
                    if (n.Name == resourceTag)
                    {
                        node = (XmlElement)n;
                        break;
                    }
                }

                foreach (XmlNode n in node?.ChildNodes[0].ChildNodes)
                {
                    if (n.Name == merged)
                    {
                        mergedNode = n;
                        break;
                    }
                }


                mergedNode.InnerXml += content + Environment.NewLine;
            }
            catch (Exception ex)
            {
            }
            await LoadXaml(Document.InnerXml);
         }
        public async Task AddResourceTagToRoot()
        {
            try
            {
                var node = (XmlElement)Document.ChildNodes[0];
                var newNode = Document.CreateElement($"{Document.ChildNodes[0].Name}.Resources", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                newNode.InnerText = Environment.NewLine;

                node.PrependChild(newNode);
            }
            catch (Exception ex)
            {
            }
            await LoadXaml(Document.InnerXml);
 
        }
    }
}
