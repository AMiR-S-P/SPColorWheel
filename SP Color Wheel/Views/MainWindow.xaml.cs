using SP_Color_Wheel.Interfaces;
using SP_Color_Wheel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace SP_Color_Wheel.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Closing += MainWindow_Closing;
            InitializeComponent();
            
            DataContext = new MainWindowVM();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((this.DataContext as MainWindowVM).CurrentSelector != null)
            {
                ((this.DataContext as MainWindowVM).CurrentSelector as ISettingsHelper).SaveSettings();
            }
        }

    }
}
