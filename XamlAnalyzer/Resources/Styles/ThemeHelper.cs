using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XamlAnalyzer.Resources.Styles
{
    internal static class ThemeHelper
    {
        public enum Theme
        {
            Light = 0,
            Dark = 1
        }

        public static Theme CurrentTheme { get; set; }

        public static ResourceDictionary GetThemePath(Theme theme)
        {
            switch (theme)
            {
                case Theme.Dark:
                    {
                        CurrentTheme = theme;
                        return new ResourceDictionary()
                        {
                            Source = new Uri("/XamlAnalyzer;component/Resources/Styles/Dark/MainTheme.xaml", UriKind.Relative)
                        };
                    }
                case Theme.Light:
                    {
                        CurrentTheme = theme;
                        return new ResourceDictionary()
                        {
                            Source = new Uri("/XamlAnalyzer;component/Resources/Styles/Light/MainTheme.xaml", UriKind.Relative)
                        };
                    }
            }

            CurrentTheme = theme;
            return new ResourceDictionary()
            {
                Source = new Uri("/XamlAnalyzer;component/Resources/Styles/Light/MainTheme.xaml", UriKind.Relative)
            };
        }
    }
}
