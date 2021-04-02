using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using XamlAnalyzer.Commands;
using XamlAnalyzer.Model;
using XamlAnalyzer.Services;
using XamlAnalyzer.Utilities;
using XamlAnalyzer.View;

namespace XamlAnalyzer.ViewModel
{
    public partial class XamlEditorViewModel : BaseDependencyViewModel
    {
        private string xamlContent;
        private bool isCommited;
        private FrameworkElement uI;
        private XamlFileModel xaml;
        SPXamlParser xamlParser;
        PropertyModel brush;
        private PropertyModel selectedProperty;
        private object selectedBrush;
        int objectCounter = 0;
        List<string> allNames = new List<string>();
        private Window windowUI;
        private ControlModel uIControl=new ControlModel();
        private bool isExported;

        public ObservableCollection<ControlModel> UIControls { get; set; } = new ObservableCollection<ControlModel>();
        public ControlModel UIControl
        {
            get => uIControl; set { uIControl = value; OnPropertyChanged(); }
        }
        public XamlFileModel XamlFile { get => xaml; set { xaml = value; OnPropertyChanged(); } }
        public SPXamlParser XamlParser
        {
            get => xamlParser; set
            {
                xamlParser = value; OnPropertyChanged();
                if (value != null)
                {
                    Task.Run(async () =>
                    {
                        await Dispatcher.BeginInvoke(new Action(async () =>
                      {
                          await UpdateUI();
                      }));
                    });
                }
                else
                {
                    UI = null;
                }
            }
        }
        public bool IsExported { get => isExported; set { isExported = value; OnPropertyChanged(); } }

        public bool IsCommited { get => isCommited; set { isCommited = value; OnPropertyChanged(); } }
        public FrameworkElement UI { get => uI; set { uI = value; OnPropertyChanged(); } }
        public Window WindowUI { get => windowUI; set { windowUI?.Close(); windowUI = value; OnPropertyChanged(); } }
        public object SelectedBrush
        {
            get => selectedBrush; set
            {
                selectedBrush = value;
                OnPropertyChanged();
                SetBrushColorCommand.OnCanExecuteChanged();
            }
        }
        public PropertyModel Brush { get => brush; set { brush = value; OnPropertyChanged(); } }
        //PropertyModel of UIElement eg Background ,Foreground
        public PropertyModel SelectedProperty
        {
            get => selectedProperty; set
            {
                selectedProperty = value;
                if (selectedProperty?.Value is SolidColorBrush || selectedProperty?.Value is ImageBrush)
                {
                    SelectedBrush = selectedProperty.Value;
                }
                Brush = null;
                SetBackgroundToImageCommand.OnCanExecuteChanged();
                SetBackgroundToLinearCommand.OnCanExecuteChanged();
                SetBackgroundToNoneCommand.OnCanExecuteChanged();
                SetBackgroundToRadialCommand.OnCanExecuteChanged();
                SetBackgroundToSolidCommand.OnCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        public XamlEditorViewModel()
        {
            //XamlFile = xamlFile;
            XamlFile.ContentChanged += async (s, e) =>
            {
                await UpdateUI();
            };
            initCommands();
        }

        public XamlEditorViewModel(XamlFileModel xamlFile)
        {
            //XamlFile = xamlFile;
            initCommands();
        }


        private async Task UpdateUI()
        {
            try
            {
                WindowUI = null;
                UI = null;
                UIControls.Clear();
                objectCounter = 0;
                allNames.Clear();
                //xamlParser.LoadXaml(XamlFile.Content);

                //check resources
                var resources = await xamlParser.GetRootResourceTag();
                foreach (var r in xamlParser.ImportedResources)
                {
                    if (!resources.Contains(r.Source))
                    {
                        r.IsUsing = false;
                    }
                    else
                    {
                        r.IsUsing = true;
                    }
                }

                //check assemblies
                var attributes = xamlParser.Document.ChildNodes[0].Attributes;
                foreach (XmlAttribute a in attributes)
                {
                    try
                    {
                        Regex regex = new Regex(@"xmlns:([^=]+)=""clr-namespace:([^;]+);assembly=([^""]+)""");
                        Match match = regex.Match(a.OuterXml);

                        if (match.Success)
                        {
                            var name = match.Groups[3].Value;
                            //if (ImportedAssemblies.Any(x => x.Name.ToLower() == name.ToLower()))
                            //{
                            //    //Assembly.LoadFile(ImportedAssemblies.FirstOrDefault(x => x.Name == match.Groups[3].Value.ToLower()).Path);
                            //    //assemblyLoad.LoadFromAssemblyPath(ImportedAssemblies.FirstOrDefault(x => x.Name.ToLower() == name.ToLower()).Path); ;
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        await Logger.LogAsync(LogLevel.Error, ex.Message);
                    }
                }

                ParserContext parserContext = new ParserContext();
                parserContext.XamlTypeMapper = new XamlTypeMapper(new string[] { });

                var xamlUI = (FrameworkElement)(XamlReader.Parse(xamlParser.Document.InnerXml, parserContext));

                xamlUI.Loaded += async (s, e) =>
                 {
                     //await GetAllElementNames(UI);
                    UIControl.Children.Add(await FillUIControls(xamlUI));

                 };
                if (xamlUI is Window)
                {
                    WindowUI = (Window)xamlUI;
                    WindowUI.Show();
                }
                else
                {
                    UI = xamlUI;
                }
                Logger.Logs.Clear();
            }
            catch (Exception ex)
            {
                UI = null;
                await Logger.LogAsync(LogLevel.Error, ex.Message);
            }
        }



        async Task<string> GenerateNewName(string preFix)
        {
            objectCounter = 0;
            return await Task.Run(() =>
             {
                 var newName = $"{preFix}{objectCounter++}";
                 while (allNames.Contains(newName))
                 {
                     newName = $"{preFix}{objectCounter++}";
                 }
                 allNames.Add(newName);
                 return newName;
             });
        }
        async Task GetAllElementNames(FrameworkElement element)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is FrameworkElement)
                {
                    if (!string.IsNullOrEmpty((child as FrameworkElement).Name))
                    {
                        if (!allNames.Contains((child as FrameworkElement).Name))
                            allNames.Add((child as FrameworkElement).Name);
                    }
                }

                await GetAllElementNames(child as FrameworkElement);
            }
        }


        async Task<ControlModel> FillUIControls(FrameworkElement element)
        {
            if (element != null)
            {
                ControlModel control = new ControlModel();
                control.Element = element;
                control.ClassName = (element).GetType().ToString().Substring((element).GetType().ToString().LastIndexOf(".") + 1);
                control.Name = (element as FrameworkElement).Name;
                if (!string.IsNullOrEmpty((element as FrameworkElement).Name))
                {
                    //if name not duplicate
                    if (allNames.Count(x => x == (element as FrameworkElement).Name) > 1)
                    {
                        control.Name = (element as FrameworkElement).Name;
                        allNames.Add(control.Name);
                    }
                    else
                    {
                        control.Name = await GenerateNewName(control.ClassName);
                        element.SetValue(FrameworkElement.NameProperty, control.Name);
                    }

                }
                else
                {
                    control.Name = await GenerateNewName(control.ClassName);
                    element.SetValue(FrameworkElement.NameProperty, control.Name);
                }


                var fields = element.GetType().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                foreach (var p in element.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType == typeof(Brush) || x.PropertyType == typeof(Color)).Select(x => x))
                {
                    DependencyProperty dp = fields?.Where(x => x.FieldType == typeof(DependencyProperty) && x.Name == p.Name + "Property").FirstOrDefault()?.GetValue(null) as DependencyProperty;
                    control.BrushProperties.Add(new PropertyModel(element)
                    {
                        Property = p,
                        DependencyProperty = dp,
                        //Value = p.GetValue(c)
                    });
                }
                UIControls.Add(control);

                var cc = VisualTreeHelper.GetChildrenCount(element);
                for (int i = 0; i < cc; i++)
                {
                    var c = VisualTreeHelper.GetChild(element, i);
                    if (c is FrameworkElement)
                    {
                        try
                        {
                            control.Children.Add(await FillUIControls((FrameworkElement)c));
                        }
                        catch (Exception ex) { }
                    }
                }

                return control;
            }
            return null;
        }
        //async Task FillUIControls(FrameworkElement element)
        //{
        //    if (element != null)
        //    {
        //        ControlModel control = new ControlModel();
        //        control.Element = element;
        //        control.ClassName = (element).GetType().ToString().Substring((element).GetType().ToString().LastIndexOf(".") + 1);
        //        control.Name = (element as FrameworkElement).Name;
        //        if (!string.IsNullOrEmpty((element as FrameworkElement).Name))
        //        {
        //            //if name not duplicate
        //            if (allNames.Count(x => x == (element as FrameworkElement).Name) > 1)
        //            {
        //                control.Name = (element as FrameworkElement).Name;
        //                allNames.Add(control.Name);
        //            }
        //            else
        //            {
        //                control.Name = await GenerateNewName(control.ClassName);
        //                element.SetValue(FrameworkElement.NameProperty, control.Name);
        //            }

        //        }
        //        else
        //        {
        //            control.Name = await GenerateNewName(control.ClassName);
        //            element.SetValue(FrameworkElement.NameProperty, control.Name);
        //        }


        //        var fields = element.GetType().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        //        foreach (var p in element.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType == typeof(Brush) || x.PropertyType == typeof(Color)).Select(x => x))
        //        {
        //            DependencyProperty dp = fields?.Where(x => x.FieldType == typeof(DependencyProperty) && x.Name == p.Name + "Property").FirstOrDefault()?.GetValue(null) as DependencyProperty;
        //            control.BrushProperties.Add(new PropertyModel(element)
        //            {
        //                Property = p,
        //                DependencyProperty = dp,
        //                //Value = p.GetValue(c)
        //            });
        //        }
        //        UIControls.Add(control);

        //        var cc = VisualTreeHelper.GetChildrenCount(element);
        //        for (int i = 0; i < cc; i++)
        //        {
        //            var c = VisualTreeHelper.GetChild(element, i);
        //            if (c is FrameworkElement)
        //            {
        //                try
        //                {
        //                    await FillUIControls((FrameworkElement)c);
        //                }
        //                catch (Exception ex) { }
        //            }
        //        }
        //    }

        //}






    }
}

