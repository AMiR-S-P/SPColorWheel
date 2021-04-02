using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XamlAnalyzer.Resources.Styles;
using XamlAnalyzer.Utilities;

namespace XamlAnalyzer.View
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            Loaded += SplashScreen_Loaded;
            InitializeComponent();
        }

        private void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            
            //XamlEditor xamlEditor = new XamlEditor();
            //xamlEditor.Resources = new ResourceDictionary();
            //xamlEditor.Resources.MergedDictionaries.Add(ThemeHelper.GetThemePath(ThemeHelper.Theme.Light));

            //xamlEditor.Show();
            //this.Close();
        }
    }
}
