using SP_Color_Wheel.Enums;
using SP_Color_Wheel.UserControls.Properties.Wheel.Harmonies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SP_Color_Wheel.TemplateSelectors
{
    public class ColorHarmonyPropertiesTemplateSelector : DataTemplateSelector
    {
        public object DataContext { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            try
            {
                var dc = ((container as ContentPresenter).Parent as DockPanel).DataContext;
                if (item is PointerHarmonyType)
                {
                    PointerHarmonyType val = PointerHarmonyType.Single;
                    Enum.TryParse(item.ToString(), out val);

                    switch (val)
                    {
                        case PointerHarmonyType.Single:
                            {
                                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(SingleProperties));
                                factory.SetValue(AnalogousProperties.DataContextProperty, dc);
                                return new DataTemplate()
                                {
                                    VisualTree = factory
                                };
                            }
                        case PointerHarmonyType.Analogous:
                            {
                                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(AnalogousProperties));
                                factory.SetValue(AnalogousProperties.DataContextProperty, dc);
                                return new DataTemplate()
                                {
                                    VisualTree = factory
                                };
                            }
                        case PointerHarmonyType.Monochromatic:
                            {
                                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(MonochromaticProperties));
                                factory.SetValue(AnalogousProperties.DataContextProperty, dc);
                                return new DataTemplate()
                                {
                                    VisualTree = factory
                                };
                            }
                        case PointerHarmonyType.Square:
                            {
                                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(SquareProperties));
                                factory.SetValue(AnalogousProperties.DataContextProperty, dc);
                                return new DataTemplate()
                                {
                                    VisualTree = factory
                                };
                            }
                        case PointerHarmonyType.Triadic:
                            {
                                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TriadicProperties));
                                factory.SetValue(AnalogousProperties.DataContextProperty, dc);
                                return new DataTemplate()
                                {
                                    VisualTree = factory
                                };
                            }
                        case PointerHarmonyType.Complementary:
                            {
                                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(ComplementaryProperties));
                                factory.SetValue(AnalogousProperties.DataContextProperty, dc);
                                return new DataTemplate()
                                {
                                    VisualTree = factory
                                };
                            }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return null;
        }
    }
}
