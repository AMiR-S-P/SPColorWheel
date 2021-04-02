using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XamlAnalyzer.Converters
{
    public class LineCounterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<int> list = new List<int>();
            int count = 1;
            var c = value.ToString().Where(x => x == '\n').Select(x => { list.Add(count++); return x; }); 
            //for (int i = 1; i <= int.Parse(value.ToString().); i++)
            //{
            //    list.Add(i);
            //}
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
