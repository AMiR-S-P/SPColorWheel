using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XamlAnalyzer.Commands;
using XamlAnalyzer.Model;
using XamlAnalyzer.Services;
using XamlAnalyzer.View;

namespace XamlAnalyzer.ViewModel
{
    public class SplashScreenViewModel : BaseViewModel
    {

        public RelayCommand<Window> NewEmptyCommand { get; set; }
        public RelayCommand<Window> DragComplete { get; set; }
        public XamlFileModel XamlFile { get; set; }

        public SplashScreenViewModel()
        {
            NewEmptyCommand = new RelayCommand<Window>(OnNewEmptyCommand);
            DragComplete = new RelayCommand<Window>(OnDragComplete);
        }

        private void OnNewEmptyCommand(Window obj)
        {
            WindowsServices services = new WindowsServices();

            XamlFileModel xamlFile = new XamlFileModel();
            xamlFile.FileName = ($"{DateTime.Now.ToString()}.xaml").Replace('/','-').Replace(':','-');
            xamlFile.FullName = Path.Combine(Environment.CurrentDirectory, "Workplace") + "\\" + xamlFile.FileName;
            xamlFile.Extension = "xaml";

            XamlEditorViewModel viewModel = new XamlEditorViewModel(xamlFile);
            File.CreateText(viewModel.XamlFile.FullName);
            services.ShowWindowsCloseLast(typeof(XamlEditor), viewModel, obj);
        }
        public void OnDragComplete(Window obj)
        {
            WindowsServices services = new WindowsServices();
            XamlEditorViewModel viewModel = new XamlEditorViewModel();


            services.ShowWindowsCloseLast(typeof(XamlEditor), viewModel, obj);
        }
    }
}
