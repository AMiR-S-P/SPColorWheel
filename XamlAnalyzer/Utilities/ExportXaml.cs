using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using XamlAnalyzer.Model;

namespace XamlAnalyzer.Utilities
{
    public class ExportXaml
    {
        bool mustGenerateKey;

        public SPXamlParser ResourceDictionaryXml { get; set; } = new SPXamlParser();
        public IEnumerable<ControlModel> Controls { get; set; }

        public ExportXaml(IEnumerable<ControlModel> controls, bool mustGenerateKey)
        {
            Controls = controls;
            this.mustGenerateKey = mustGenerateKey;
        }

        public async Task SaveToFile(string path)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var xmlW = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true, Async = true, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
                {
                    try
                    {

                        //ms.WriteAsync(Encoding.UTF8.GetBytes(Xaml), 0, xaml.Length);
                        ResourceDictionaryXml.Document.WriteContentTo(xmlW);
                        await xmlW.FlushAsync();
                        ms.Position = 0;


                        await File.WriteAllTextAsync(path, (await new StreamReader(ms).ReadToEndAsync()).Replace("xmlns=\"\"", ""), Encoding.UTF8);

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// return toolbox and term in doublecoutation and entireterm like below
        /// xmlns:toolBox=\"clr-namespace:SPGraphic.ToolBox;assembly=SPGraphic\"
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        (string, string, string) GetNamespaceAndXmlnsName(ControlModel control)
        {

            string className = control.ClassName;//Grid
            var nameSpace = (control.Element as FrameworkElement).GetType().FullName;//System.Windows.Controls.Grid
            var assemblyFullName = (control.Element as FrameworkElement).GetType().Assembly.FullName;//PresentationFramework, Version=5.0.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
            var assemblyName = assemblyFullName.Split(',')[0];//presentationFramework
            var xmlnsName = "";

            if (assemblyName != "PresentationFramework")
            {
                xmlnsName = nameSpace.Substring(0, nameSpace.LastIndexOf('.'));
                xmlnsName = xmlnsName.Substring(xmlnsName.LastIndexOf('.') + 1);
                xmlnsName = xmlnsName[0].ToString().ToLower() + xmlnsName.Substring(1);
            }

            string xmlnsValue = $"clr-namespace:{nameSpace.Substring(0, nameSpace.LastIndexOf('.'))};assembly={assemblyName}";


            return (xmlnsName, xmlnsName, xmlnsValue);
        }
        public async Task CreateResourceDictionary()
        {
            XmlElement resourceDictionaryTag = ResourceDictionaryXml.Document.CreateElement("ResourceDictionary", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            resourceDictionaryTag.SetAttribute("xmlns:x", "http://schemas.microsoft.com/winfx/2006/xaml");
            //resourceDictionaryTag.SetAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            foreach (ControlModel cm in Controls)
            {
                var assemblyFullName = (cm.Element as FrameworkElement).GetType().Assembly.FullName;
                var assemblyName = assemblyFullName.Split(',')[0];

                var namespacesAndXmlnsName = GetNamespaceAndXmlnsName(cm);

                if (!string.IsNullOrEmpty(namespacesAndXmlnsName.Item1) &&
                    string.IsNullOrEmpty(resourceDictionaryTag.GetAttribute($"xmlns:{namespacesAndXmlnsName.Item1}")))
                {
                    resourceDictionaryTag.SetAttribute($"xmlns:{namespacesAndXmlnsName.Item1}", namespacesAndXmlnsName.Item3);
                }

                string targetTypeValue = $"{{x:Type ";

                if (assemblyName == "PresentationFramework")
                {
                    targetTypeValue += "";
                }
                else
                {
                    targetTypeValue += namespacesAndXmlnsName.Item1 + ":";
                }
                targetTypeValue += $"{cm.ClassName}}}";
                await AddStyle(targetTypeValue, cm, resourceDictionaryTag);
            }

            ResourceDictionaryXml.Document.AppendChild(resourceDictionaryTag);
        }

        async Task AddStyle(string targetTypeValue, ControlModel control, XmlNode node)
        {
            XmlElement style = node.OwnerDocument.CreateElement("Style", null);
            style.SetAttribute("TargetType", targetTypeValue);
            if (mustGenerateKey)
            {
                style.SetAttribute("x:Key", control.Name);
            }
            foreach (var pm in control.BrushProperties.Where(x => x.Value != null))
            {
                await AddSetter(pm.Property.Name, pm.Value as Brush, style);
            }

            node.AppendChild(style);
        }
        async Task AddSetter(string propertyName, Brush value, XmlNode node)
        {
            XmlElement setter = node.OwnerDocument.CreateElement("Setter");
            setter.SetAttribute("Property", propertyName);


            if (value == null)
            {
                setter.SetAttribute("Property", propertyName);
                setter.SetAttribute("Value", "{x:null}");
            }
            else
            {
                XmlElement setterValue = node.OwnerDocument.CreateElement("Setter.Value");
                await AddBrush(value, setterValue);
                setter.AppendChild(setterValue);
            }
            node.AppendChild(setter);
        }
        Task AddBrush(Brush brush, XmlNode node)
        {
            if (brush is SolidColorBrush)
            {
                XmlElement brushNode = node.OwnerDocument.CreateElement("SolidColorBrush");
                brushNode.SetAttribute("Color", (brush as SolidColorBrush).Color.ToString());
                brushNode.SetAttribute("Opacity", (brush as SolidColorBrush).Opacity.ToString());

                node.AppendChild(brushNode);
            }
            else if (brush is LinearGradientBrush)
            {
                XmlElement brushNode = node.OwnerDocument.CreateElement("LinearGradientBrush");
                brushNode.SetAttribute("Opacity", (brush as LinearGradientBrush).Opacity.ToString());
                brushNode.SetAttribute("StartPoint", (brush as LinearGradientBrush).StartPoint.ToString());
                brushNode.SetAttribute("EndPoint", (brush as LinearGradientBrush).EndPoint.ToString());
                brushNode.SetAttribute("ColorInterpolationMode", (brush as LinearGradientBrush).ColorInterpolationMode.ToString());
                brushNode.SetAttribute("MappingMode", (brush as LinearGradientBrush).MappingMode.ToString());

                if ((brush as LinearGradientBrush).GradientStops.Any())
                {
                    XmlElement gssNode = node.OwnerDocument.CreateElement("LinearGradientBrush.GradientStops");
                    foreach (var gs in (brush as LinearGradientBrush).GradientStops)
                    {
                        XmlElement gsNode = node.OwnerDocument.CreateElement("GradientStops");
                        gsNode.SetAttribute("Color", gs.Color.ToString());
                        gsNode.SetAttribute("Offset", gs.Offset.ToString());

                        gssNode.AppendChild(gsNode);
                    }
                    brushNode.AppendChild(gssNode);
                }
                node.AppendChild(brushNode);
            }
            else if (brush is RadialGradientBrush)
            {
                XmlElement brushNode = node.OwnerDocument.CreateElement("RadialGradientBrush");
                brushNode.SetAttribute("Opacity", (brush as RadialGradientBrush).Opacity.ToString());
                brushNode.SetAttribute("Center", (brush as RadialGradientBrush).Center.ToString());
                brushNode.SetAttribute("RadiusX", (brush as RadialGradientBrush).RadiusX.ToString());
                brushNode.SetAttribute("RadiusY", (brush as RadialGradientBrush).RadiusY.ToString());
                brushNode.SetAttribute("ColorInterpolationMode", (brush as RadialGradientBrush).ColorInterpolationMode.ToString());
                brushNode.SetAttribute("MappingMode", (brush as RadialGradientBrush).MappingMode.ToString());

                if ((brush as RadialGradientBrush).GradientStops.Any())
                {
                    XmlElement gssNode = node.OwnerDocument.CreateElement("RadialGradientBrush.GradientStops");
                    foreach (var gs in (brush as RadialGradientBrush).GradientStops)
                    {
                        XmlElement gsNode = node.OwnerDocument.CreateElement("GradientStops");
                        gsNode.SetAttribute("Color", gs.Color.ToString());
                        gsNode.SetAttribute("Offset", gs.Offset.ToString());

                        gssNode.AppendChild(gsNode);
                    }
                }

                node.AppendChild(brushNode);
            }
            else if (brush is ImageBrush)
            {
                XmlElement brushNode = node.OwnerDocument.CreateElement("ImageBrush");
                brushNode.SetAttribute("AlignmentX", (brush as ImageBrush).AlignmentX.ToString());
                brushNode.SetAttribute("AlignmentY", (brush as ImageBrush).AlignmentY.ToString());
                brushNode.SetAttribute("ImageSource", (brush as ImageBrush).ImageSource.ToString());
                brushNode.SetAttribute("Stretch", (brush as ImageBrush).Stretch.ToString());
                brushNode.SetAttribute("TileMode", (brush as ImageBrush).TileMode.ToString());
                brushNode.SetAttribute("Viewbox", (brush as ImageBrush).Viewbox.ToString());
                brushNode.SetAttribute("ViewboxUnits", (brush as ImageBrush).ViewboxUnits.ToString());
                brushNode.SetAttribute("Viewport", (brush as ImageBrush).Viewport.ToString());
                brushNode.SetAttribute("ViewportUnits", (brush as ImageBrush).ViewportUnits.ToString());
                brushNode.SetAttribute("Opacity", (brush as ImageBrush).Opacity.ToString());


                node.AppendChild(brushNode);
            }
            else
            {
                // Xaml = "{x:null}";
            }


            return Task.CompletedTask;
        }

    }
}
