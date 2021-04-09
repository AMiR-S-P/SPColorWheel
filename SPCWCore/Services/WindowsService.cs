using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SPCWCore.Services
{
    public class WindowsService
    {
        public Window Window { get; set; }


        public WindowsService(Type windowType)
        {
            try
            {
                Window = (Window)Activator.CreateInstance(windowType);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public WindowsService(Window window)
        {
            try
            {
                Window = window;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public WindowsService(Type windowType,object dataContext)
        {
            try
            {
                Window = (Window)Activator.CreateInstance(windowType);
                Window.DataContext = dataContext;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ShowWindowsCloseLast(Window lastWindow)
        {
            Window.Show();
            lastWindow?.Close();
        }
        public void ShowWindows(Window lastWindow)
        {
            if (lastWindow != null)
            {
                //((Window)window).Owner = lastWindow;
            }

            Window.Show();
         }
        public void ShowDialog(Window lastWindow)
        {
            if (lastWindow != null)
            {
                Window.Owner = lastWindow;
            }
            Window.ShowDialog();
        }

        public void CloseWindow()
        {
            Window.Close();
        }
    }
}

