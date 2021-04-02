using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SP_Color_Wheel.Resources.Styles
{
    internal static class ThemeHelper
    {
        public enum Theme
        {
            Light =0,
            Dark = 1
        }

        public static Theme CurrentTheme { get; set; }

        public static ResourceDictionary GetThemePath( Theme theme)
        {
            switch(theme)
            {
                case Theme.Dark:
                    {
                        CurrentTheme = theme;
                        return new ResourceDictionary()
                        {
                            Source = new Uri("/Resources/Styles/Dark/MainTheme.xaml", UriKind.Relative)
                        };
                    }
                case Theme.Light:
                    {
                        CurrentTheme = theme;
                        return new ResourceDictionary()
                        {
                            Source = new Uri("/Resources/Styles/Light/MainTheme.xaml", UriKind.Relative)
                        };
                    }
            }

            CurrentTheme = theme;
            return new ResourceDictionary()
            {
                Source = new Uri("/Resources/Styles/Light/MainTheme.xaml", UriKind.Relative)
            };
        }
    }
}
