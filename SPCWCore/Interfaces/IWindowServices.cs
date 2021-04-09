using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SPCWCore.Interfaces
{

        public interface IWindowsService
        {
            void ShowWindows(object viewModel, Window lastWindow);
            void ShowWindowsCloseLast(Type newWindowType, object dataContext, Window lastWindow);

            void CloseWindow(Window window);


        }

    
}
