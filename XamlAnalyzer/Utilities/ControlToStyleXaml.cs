using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using XamlAnalyzer.Model;

namespace XamlAnalyzer.Utilities
{
    public class ControlToStyleXaml
    {
        public SPXamlParser xamlParser;
        string targetTypeValue = "";
        public ControlModel Control { get; private set; }
        public string Xaml { get; private set; }
        public ControlToStyleXaml(ControlModel control, string targetTypeValue)
        {
            Control = control;
            this.targetTypeValue = targetTypeValue;
            xamlParser = new SPXamlParser("");
        }

        public async Task ConvertToXaml()
        {
            //style tag
            XmlElement styleTag = xamlParser.Document.CreateElement("Style");

            //attribute : TargetType="{x:Type test:ClassName}"
            //string className = Control.ClassName;//Grid
            //var nameSpace = (Control.Element as FrameworkElement).GetType().FullName;//System.Windows.Controls.Grid
            //var assemblyFullName = (Control.Element as FrameworkElement).GetType().Assembly.FullName;//PresentationFramework, Version=5.0.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
            //var assemblyName = assemblyFullName.Split(',')[0];//presentationFramework

            ////set targetType value
            //string targetTypeValue = $"{{x:Type ";
            //targetTypeValue += assemblyName != "PresentationFramework" ? xmlnsName + ":" : "";
            //targetTypeValue += className + "}";

            styleTag.SetAttribute("TargetType", targetTypeValue);


            //string identifierName = 
            foreach (var b in Control.BrushProperties.Where(x => x.Value != null))
            {
                //setter tag
                XmlElement setterTag = styleTag.OwnerDocument.CreateElement("Setter");
                setterTag.SetAttribute("Property", b.Property.Name);

                //setter value tag
                XmlElement setterValueTag = styleTag.OwnerDocument.CreateElement("Setter.Value");
                setterTag.AppendChild(setterValueTag);

                //brushTag
                BrushToXaml brushToXaml = new BrushToXaml(b.Value as Brush);

                XmlDocument valDoc = new XmlDocument();
                valDoc.LoadXml(brushToXaml.Xaml);
                XmlNode node = valDoc.ChildNodes[0];
                XmlNode imNode = setterValueTag.OwnerDocument.ImportNode(node, true);
                setterValueTag.AppendChild(imNode);

                styleTag.AppendChild(setterTag);
            }


            xamlParser.Document.AppendChild(styleTag);
            Xaml = xamlParser.Document.InnerXml;
        }
    }
}
