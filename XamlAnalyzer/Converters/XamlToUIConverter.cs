using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;
using XamlAnalyzer.Utilities;

namespace XamlAnalyzer.Converters
{
    public class XamlToUIConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value?.ToString();
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }

            try
            {
                var x = XamlSharper.Sharp(new Model.XamlFileModel() { Content = val });
                XamlReader xamlReader = new XamlReader();
                using (var m = new MemoryStream(Encoding.UTF8.GetBytes(x.SharpedContent)))
                {
                  
                   var o =  xamlReader.LoadAsync(m);
                    
                    return o;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
                Logger.LogAsync(LogLevel.Error, ex.Message);
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
