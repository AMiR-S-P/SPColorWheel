using SP_Color_Wheel.Commands;
using SP_Color_Wheel.Interfaces;
using SP_Color_Wheel.UserControls;
using SP_Color_Wheel.UserControls.RGB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private IColorSelector currentSelector;
        private ObservableCollection<IColorSelector> selectors = new ObservableCollection<IColorSelector>();
        private IColorProperties currentProperties;

        public IColorSelector CurrentSelector
        {
            get => currentSelector; set
            {
                ((ISettingsHelper)currentSelector)?.SaveSettings();//save settings before change DC
                currentSelector = value;
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
        public MainWindowVM()
        {
            Selectors.Add(new WheelViewModel());
            Selectors.Add(new RGBViewModel());

            OpenXamlEditorCommand = new AsyncRelayCommand<Window>(OnOpenXamlEditor);
        }

        private Task OnOpenXamlEditor(Window arg)
        {
            var vm = new XamlEditorViewModel();

            //File.CreateText(vm.XamlFile.FullName);

            BindingOperations.SetBinding(vm, XamlEditorViewModel.MainColorProperty, new Binding("MainColor") { Source = CurrentSelector });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Color2Property, new Binding("Color2") { Source = CurrentSelector });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Color3Property, new Binding("Color3") { Source = CurrentSelector });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Color4Property, new Binding("Color4") { Source = CurrentSelector });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Color5Property, new Binding("Color5") { Source = CurrentSelector });

            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tint2Property, new Binding("Tint2") { Source = CurrentSelector.MainColorTint });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tint3Property, new Binding("Tint3") { Source = CurrentSelector.MainColorTint });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tint4Property, new Binding("Tint4") { Source = CurrentSelector.MainColorTint });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tint5Property, new Binding("Tint5") { Source = CurrentSelector.MainColorTint });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tint6Property, new Binding("Tint6") { Source = CurrentSelector.MainColorTint });

            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tone2Property, new Binding("Tone2") { Source = CurrentSelector.MainColorTone });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tone3Property, new Binding("Tone3") { Source = CurrentSelector.MainColorTone });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tone4Property, new Binding("Tone4") { Source = CurrentSelector.MainColorTone });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tone5Property, new Binding("Tone5") { Source = CurrentSelector.MainColorTone });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Tone6Property, new Binding("Tone6") { Source = CurrentSelector.MainColorTone });

            BindingOperations.SetBinding(vm, XamlEditorViewModel.Shade2Property, new Binding("Shade2") { Source = CurrentSelector.MainColorShade });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Shade3Property, new Binding("Shade3") { Source = CurrentSelector.MainColorShade });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Shade4Property, new Binding("Shade4") { Source = CurrentSelector.MainColorShade });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Shade5Property, new Binding("Shade5") { Source = CurrentSelector.MainColorShade });
            BindingOperations.SetBinding(vm, XamlEditorViewModel.Shade6Property, new Binding("Shade6") { Source = CurrentSelector.MainColorShade });


            XamlAnalyzer.Services.WindowsServices services = new XamlAnalyzer.Services.WindowsServices();
            services.ShowWindows(typeof(XamlEditor), vm, arg);

            return Task.CompletedTask;
        }
    }
}
