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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XamlAnalyzer.Utilities;

namespace XamlAnalyzer.UserControls.Editor
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Grid
    {
        public Editor()
        {
            InitializeComponent();
        }

        private void XamlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string s = "";
            //foreach(var n in XamlSharper.GetAllStaticResourcesName(XamlTextBox.Text))
            //{
            //    s += $"{n}\n";
            //}
            //MessageBox.Show(s);

        }
    }
}
