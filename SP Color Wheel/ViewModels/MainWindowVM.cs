using SP_Color_Wheel.Commands;
using SP_Color_Wheel.Interfaces;
using SP_Color_Wheel.Views;
using SPCWCore.Interfaces;
using SPCWCore.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using XamlAnalyzer.Model;
using XamlAnalyzer.View;
using XamlAnalyzer.ViewModel;

namespace SP_Color_Wheel.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        List<XamlAnalyzer.ViewModel.XamlEditorViewModel> ViewModels = new List<XamlAnalyzer.ViewModel.XamlEditorViewModel>();

        //XamlEditorViewModel xamlEditorViewModel = new XamlEditorViewModel();

        private IColorSelector currentSelector;
        private ObservableCollection<IColorSelector> selectors = new ObservableCollection<IColorSelector>();
        private IColorProperties currentProperties;

        public IColorSelector CurrentSelector
        {
            get => currentSelector; set
            {

                ((ISettingsHelper)currentSelector)?.SaveSettings();//save settings before change DC
                currentSelector = value;
                OpenXamlEditorCommand.OnCanExecuteChanged();

                //foreach viewmodel set binding and clear binding here
                foreach(var vm in ViewModels)
                {
                    ClearSelectorBindings(vm);
                    SetSelectorBindings(vm);
                }


                OnPropertyChanged();
            }
        }

        public IColorProperties CurrentProperties
        {
            get => currentProperties;
            set
            {
                currentProperties = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IColorSelector> Selectors
        {
            get => selectors; set
            {
                selectors = value;
                OnPropertyChanged();
            }
        }

        public AsyncRelayCommand<Window> OpenXamlEditorCommand { get; set; }
        public AsyncRelayCommand<object> AboutCommand { get; set; }
        public AsyncRelayCommand<Window> ExitCommand { get; set; }
        public MainWindowVM()
        {
            Selectors.Add(new WheelViewModel());
            Selectors.Add(new RGBViewModel());

            OpenXamlEditorCommand = new AsyncRelayCommand<Window>(OnOpenXamlEditor);
            AboutCommand = new AsyncRelayCommand<object>(OnAbout);
            ExitCommand = new AsyncRelayCommand<Window>(OnExit);
        }

        private Task OnExit(Window arg)
        {
            new WindowsService(arg).CloseWindow();

            return Task.CompletedTask;
        }

        void ClearSelectorBindings(XamlEditorViewModel xamlEditorViewModel)
        {
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.MainColorProperty);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Color2Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Color3Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Color4Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Color5Property);


            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tint2Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tint3Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tint4Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tint5Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tint6Property);

            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tone2Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tone3Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tone4Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tone5Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Tone6Property);

            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Shade2Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Shade3Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Shade4Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Shade5Property);
            BindingOperations.ClearBinding(xamlEditorViewModel, XamlEditorViewModel.Shade6Property);

        }

        void SetSelectorBindings(XamlEditorViewModel xamlEditorViewModel)
        {
            if (CurrentSelector != null)
            {
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.MainColorProperty, new Binding("MainColor") { Source = CurrentSelector });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Color2Property, new Binding("Color2") { Source = CurrentSelector });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Color3Property, new Binding("Color3") { Source = CurrentSelector });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Color4Property, new Binding("Color4") { Source = CurrentSelector });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Color5Property, new Binding("Color5") { Source = CurrentSelector });

                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tint2Property, new Binding("Tint2") { Source = CurrentSelector.MainColorTint });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tint3Property, new Binding("Tint3") { Source = CurrentSelector.MainColorTint });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tint4Property, new Binding("Tint4") { Source = CurrentSelector.MainColorTint });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tint5Property, new Binding("Tint5") { Source = CurrentSelector.MainColorTint });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tint6Property, new Binding("Tint6") { Source = CurrentSelector.MainColorTint });

                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tone2Property, new Binding("Tone2") { Source = CurrentSelector.MainColorTone });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tone3Property, new Binding("Tone3") { Source = CurrentSelector.MainColorTone });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tone4Property, new Binding("Tone4") { Source = CurrentSelector.MainColorTone });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tone5Property, new Binding("Tone5") { Source = CurrentSelector.MainColorTone });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Tone6Property, new Binding("Tone6") { Source = CurrentSelector.MainColorTone });

                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Shade2Property, new Binding("Shade2") { Source = CurrentSelector.MainColorShade });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Shade3Property, new Binding("Shade3") { Source = CurrentSelector.MainColorShade });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Shade4Property, new Binding("Shade4") { Source = CurrentSelector.MainColorShade });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Shade5Property, new Binding("Shade5") { Source = CurrentSelector.MainColorShade });
                BindingOperations.SetBinding(xamlEditorViewModel, XamlEditorViewModel.Shade6Property, new Binding("Shade6") { Source = CurrentSelector.MainColorShade });
            }
        }
        private Task OnAbout(object arg)
        {
            AboutViewModel aboutViewModel = new AboutViewModel();
            WindowsService windowsServices = new WindowsService(typeof(AboutView), aboutViewModel);
            windowsServices.ShowDialog( null);
            return Task.CompletedTask;
        }


        private Task OnOpenXamlEditor(Window arg)
        {
            //xamlEditorViewModel.ResetVM();
            var xamlEditorViewModel = new XamlEditorViewModel();
            WindowsService services = new WindowsService(typeof(XamlEditor), xamlEditorViewModel);


            services.Window.DataContext = xamlEditorViewModel;
            services.Window.Loaded += (s, e) =>
            {
                ViewModels.Add(xamlEditorViewModel);
                SetSelectorBindings(xamlEditorViewModel);
            };
            services.Window.Unloaded += (s, e) =>
            {
                ViewModels.Remove(xamlEditorViewModel);
                ClearSelectorBindings(xamlEditorViewModel);
            };
            services.ShowWindows(arg);

            return Task.CompletedTask;
        }
    }
}
