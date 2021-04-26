using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SP_Color_Wheel.Helper;
using SP_Color_Wheel.ViewModels;
using SP_Color_Wheel.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using SPCWCore.Services;
namespace SP_Color_Wheel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
           
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });
            Startup += App_Startup;
            Settings.Path = Path.Combine(Environment.CurrentDirectory, "Settings.spcw");
            //Resources = new ResourceDictionary();
            //Resources.MergedDictionaries.Add(ThemeHelper.GetThemePath(ThemeHelper.Theme.Light));


            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ErrorReportViewModel errorReport = new ErrorReportViewModel(
                $"{e.Exception.Message}{Environment.NewLine}{Environment.NewLine}" +
                $"INNER:{Environment.NewLine}{e.Exception.InnerException?.ToString()}{Environment.NewLine}" +
                $"INNER MESSAGE:{Environment.NewLine}{e.Exception.InnerException?.Message }");
            WindowsService windowServices = new WindowsService(typeof(ErrorReportView), errorReport);
            windowServices.ShowDialog( null);
            Application.Current.Shutdown(10);

            e.Handled = true;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //StartupUri = new Uri("pack://application:,,,/XamlAnalyzer;component/View/SplashScreen.xaml");
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            AppCenter.Start("79f122cc-edcd-4823-ae5c-3784fa0a6f5a",
                   typeof(Analytics), typeof(Crashes));
            base.OnStartup(e);
        }
    }
}
