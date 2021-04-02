using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XamlAnalyzer.Model;

namespace XamlAnalyzer.TemplateSelectors
{
    public class BrushTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SolidColorBrushTemplate { get; set; }
        public DataTemplate LinearBrushTemplate { get; set; }
        public DataTemplate RadialBrushTemplate { get; set; }
        public DataTemplate ImageBrushTemplate { get; set; }
        public DataTemplate NoneTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //container.SetValue(ContentPresenter.ContentProperty, item);
            if (item != null && item is PropertyModel)
            {
                if ((item as PropertyModel).Value is SolidColorBrush)
                {
                    return SolidColorBrushTemplate;
                }
                else if ((item as PropertyModel).Value is LinearGradientBrush)
                {
                    return LinearBrushTemplate;
                }
                else if ((item as PropertyModel).Value is RadialGradientBrush)
                {
                    return RadialBrushTemplate;
                }
                else if ((item as PropertyModel)?.Value is ImageBrush)
                {
                    return ImageBrushTemplate;
                }
            }
            return NoneTemplate;
        }
    }
}
