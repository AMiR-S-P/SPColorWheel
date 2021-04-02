using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XamlAnalyzer.Interfaces;

namespace XamlAnalyzer.Services
{
    public class WindowsServices : IWindowsService
    {
        public void ShowWindows(object viewModel,Window lastwindow)
        {
            Window window = new Window();
            window.Content = viewModel;
            window.Show();
            lastwindow?.Close();
        }
        public void ShowWindowsCloseLast(Type newWindowType, object dataContext, Window lastWindow)
        {
            Window window = (Window)Activator.CreateInstance(newWindowType);
            window.DataContext = dataContext;
            window.Show();
            lastWindow?.Close();
        }
        public void ShowWindows(Type newWindowType, object dataContext, Window lastWindow)
        {
            object window = Activator.CreateInstance(newWindowType);
            if (lastWindow != null)
            {
                //((Window)window).Owner = lastWindow;
            }
            
            ((Window)window).DataContext = dataContext;
            ((Window)window).Show();
        }
        public void ShowDialog(Type newWindowType, object dataContext, Window lastWindow)
        {
            Window window = (Window)Activator.CreateInstance(newWindowType);
            if (lastWindow != null)
            {
                window.Owner = lastWindow;
            }
            window.DataContext = dataContext;
            window.ShowDialog();
        }

        public void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
