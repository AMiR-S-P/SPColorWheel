using SP_Color_Wheel.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using XamlAnalyzer.Services;

namespace SP_Color_Wheel.ViewModels
{
    class ErrorReportViewModel : BaseViewModel
    {
        private string error;
        private bool isSuccessFull;

        public string Error { get => error; set { error = value; OnPropertyChanged(); ReportCommand.OnCanExecuteChanged(); } }
        public bool IsSuccessFull { get => isSuccessFull; set { isSuccessFull = value; OnPropertyChanged(); }
    }

        public AsyncRelayCommand<object> ReportCommand { set; get; }
        public AsyncRelayCommand<Window> CloseWindowCommand { get; set; }

        public ErrorReportViewModel(string error)
        {
            ReportCommand = new AsyncRelayCommand<object>(OnReport, CanReport);
            CloseWindowCommand = new AsyncRelayCommand<Window>(OnClose);

            Error = error;

        }

        private Task OnClose(Window arg)
        {
            new WindowsServices().CloseWindow(arg);
            return Task.CompletedTask;
        }

        private bool CanReport(object arg)
        {
            return !string.IsNullOrEmpty(Error);
        }

        private Task OnReport(object arg)
        {
            try
            {
                string uri = $"mailto:AmirSetvatiPaydar@Gmail.com?Subject=SPColorWheel&body="+Error;
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
            catch (Exception ex)
            {

            }
            IsSuccessFull = true;
            return Task.CompletedTask;
        }
    }
}
