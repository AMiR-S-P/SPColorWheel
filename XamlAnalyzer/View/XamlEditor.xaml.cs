using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.View
{
    /// <summary>
    /// Interaction logic for XamlEditor.xaml
    /// </summary>
    public partial class XamlEditor : Window
    {
        public XamlEditor()
        {
            InitializeComponent();
            Closing += XamlEditor_Closing;
        }

        private void XamlEditor_Closing(object sender, CancelEventArgs e)
        {
            if (!((XamlEditorViewModel)DataContext).CanCloseWindow())
            {
                e.Cancel = true;
            }
        }
    }
}
