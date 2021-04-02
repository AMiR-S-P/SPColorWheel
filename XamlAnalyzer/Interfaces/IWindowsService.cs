using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XamlAnalyzer.Interfaces
{
    public interface IWindowsService
    {
        void ShowWindows(object viewModel,Window lastWindow);
        void ShowWindowsCloseLast(Type newWindowType, object dataContext, Window lastWindow);

        void CloseWindow(Window window);


    }
}
