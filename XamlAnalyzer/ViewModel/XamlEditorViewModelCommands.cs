using SPCWCore.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XamlAnalyzer.Commands;
using XamlAnalyzer.Model;
using XamlAnalyzer.Utilities;
using XamlAnalyzer.View;
using MessageBox = System.Windows.Forms.MessageBox;

namespace XamlAnalyzer.ViewModel
{
    public partial class XamlEditorViewModel
    {
        public AsyncRelayCommand<object> SelectPropertyCommand { get; set; }

        public AsyncRelayCommand<SolidColorBrush> SelectSolidBrushCommand { get; set; }
        public AsyncRelayCommand<GradientStop> SelectGradientStopBrushCommand { get; set; }

        public AsyncRelayCommand<PropertyModel> SetBackgroundToNoneCommand { get; set; }
        public AsyncRelayCommand<PropertyModel> SetBackgroundToSolidCommand { get; set; }
        public AsyncRelayCommand<PropertyModel> SetBackgroundToLinearCommand { get; set; }
        public AsyncRelayCommand<PropertyModel> SetBackgroundToRadialCommand { get; set; }
        public AsyncRelayCommand<PropertyModel> SetBackgroundToImageCommand { get; set; }
        public AsyncRelayCommand<GradientStop> DeleteGradientStopCommand { get; set; }
        public AsyncRelayCommand<SolidColorBrush> SetBrushColorCommand { get; set; }
        public AsyncRelayCommand<object> AddGradientStopCommand { get; set; }
        public AsyncRelayCommand<ImageBrush> SelectImageBrushCommand { get; set; }


        public AsyncRelayCommand<Window> EditXamlCommand { get; set; }
        public AsyncRelayCommand<Window> ExportCommand { get; set; }
        public AsyncRelayCommand<Window> ExportAllCommand { get; set; }
        public AsyncRelayCommand<object> BrowseImageSourceCommand { get; set; }
        public AsyncRelayCommand<Window> ExitCommand { get; set; }

        public AsyncRelayCommand<string> SampleCommand { get; set; }
        public AsyncRelayCommand<ControlModel> ApplyStyleToAllCommand { get; set; }
        public AsyncRelayCommand<ControlModel> ApplyStyleToSubCommand { get; set; }

        public AsyncRelayCommand<PropertyModel> ApplyBrushToAllCommand { get; set; }
        public AsyncRelayCommand<PropertyModel> ApplyBrushToSubCommand { get; set; }

        public AsyncRelayCommand<object> RefreshUICommand { set; get; }
        private void initCommands()
        {
            SelectPropertyCommand = new AsyncRelayCommand<object>(OnSelectProperty);

            SelectSolidBrushCommand = new AsyncRelayCommand<SolidColorBrush>(OnSelectSolidBrush);
            SelectGradientStopBrushCommand = new AsyncRelayCommand<GradientStop>(OnSelectGradientStopBrush);

            SetBackgroundToImageCommand = new AsyncRelayCommand<PropertyModel>(OnSetBackgroundToImage, CanSelectBrush);
            SetBackgroundToLinearCommand = new AsyncRelayCommand<PropertyModel>(OnSetBackgroundToLinear, CanSelectBrush);
            SetBackgroundToNoneCommand = new AsyncRelayCommand<PropertyModel>(OnSetBackgroundToNone, CanSelectBrush);
            SetBackgroundToRadialCommand = new AsyncRelayCommand<PropertyModel>(OnSetBackgroundToRadial, CanSelectBrush);
            SetBackgroundToSolidCommand = new AsyncRelayCommand<PropertyModel>(OnSetBackgroundToSolid, CanSelectBrush);

            DeleteGradientStopCommand = new AsyncRelayCommand<GradientStop>(OnDeleteGradientStop);
            SetBrushColorCommand = new AsyncRelayCommand<SolidColorBrush>(OnSetBrushCommand, CanSetBrushColor);

            AddGradientStopCommand = new AsyncRelayCommand<object>(OnAddGradientStopCommand);
            SelectImageBrushCommand = new AsyncRelayCommand<ImageBrush>(OnSelectImageBrush);

            EditXamlCommand = new AsyncRelayCommand<Window>(OnEditXaml);
            ExportCommand = new AsyncRelayCommand<Window>(OnExport, CanExport);
            ExportAllCommand = new AsyncRelayCommand<Window>(OnAllExport);

            BrowseImageSourceCommand = new AsyncRelayCommand<object>(OnBrowseImageSource);
            ExitCommand = new AsyncRelayCommand<Window>(OnExit);

            SampleCommand = new AsyncRelayCommand<string>(OnSample);
            ApplyStyleToAllCommand = new AsyncRelayCommand<ControlModel>(OnApplyStyleToAll);
            ApplyStyleToSubCommand = new AsyncRelayCommand<ControlModel>(OnApplyStyleToSub);

            ApplyBrushToAllCommand = new AsyncRelayCommand<PropertyModel>(OnApplyBrushToAll);
            ApplyBrushToSubCommand = new AsyncRelayCommand<PropertyModel>(OnApplyBrushToSub);

            RefreshUICommand = new AsyncRelayCommand<object>(OnRefreshUI);
        }

        private bool CanRefreshUI(object arg)
        {
            return UI != null || WindowUI != null;
        }

        private async Task OnRefreshUI(object arg)
        {
            if (UI != null || WindowUI != null)
            {
                await UpdateUI();
            }
        }

        private async Task OnApplyBrushToSub(PropertyModel arg)
        {
            await ApplyBrushToAll(SelectedControl, SelectedControl, arg);
        }

        private async Task OnApplyBrushToAll(PropertyModel arg)
        {
            await ApplyBrushToAll(UIControl, SelectedControl, arg);
        }
        async Task ApplyBrushToAll(ControlModel control, ControlModel baseControl, PropertyModel value)
        {
            foreach (var c in control.Children)
            {
                if (c.Element.GetType() == baseControl.Element.GetType())
                {
                    c.BrushProperties.FirstOrDefault(x => x.Property.Name == value.Property.Name).SetProperty(value.Value);

                }
                await ApplyBrushToAll(c, baseControl, value);
            }
        }
        private async Task OnApplyStyleToSub(object arg)
        {
            await ApplyStyleToAll(SelectedControl, SelectedControl);

        }

        private async Task OnApplyStyleToAll(object arg)
        {
            await ApplyStyleToAll(UIControl, SelectedControl);
        }
        async Task ApplyStyleToAll(ControlModel controlModel, ControlModel baseControl)
        {
            foreach (var c in controlModel.Children)
            {
                if (c.Element.GetType() == baseControl.Element.GetType())
                {
                    foreach (var p in c.BrushProperties)
                    {
                        p.SetProperty(baseControl.BrushProperties.FirstOrDefault(x => x.Property.Name == p.Property.Name).Value);
                    }
                }
                await ApplyStyleToAll(c, baseControl);
            }
        }
        private async Task OnSample(string arg)
        {
            Uri uri = new Uri("Samples/Calculator.txt", UriKind.RelativeOrAbsolute);

            switch (arg)
            {
                case "Calculator":
                    {
                        uri = new Uri("pack://application:,,,/XamlAnalyzer;component/Samples/Calculator.txt", UriKind.RelativeOrAbsolute);
                        break;
                    }
                case "Sign in form":
                    {
                        uri = new Uri("pack://application:,,,/XamlAnalyzer;component/Samples/SignInForm.txt", UriKind.RelativeOrAbsolute);
                        break;
                    }
                case "Shapes":
                    {
                        uri = new Uri("pack://application:,,,/XamlAnalyzer;component/Samples/Shapes.txt", UriKind.RelativeOrAbsolute);
                        break;
                    }
            }
            if (CanCloseWindow())
            {

                try
                {
                    var content = await new StreamReader(System.Windows.Application.GetResourceStream(uri).Stream).ReadToEndAsync();
                    SPXamlParser xamlParser = new SPXamlParser(content);
                    await xamlParser.LoadXaml(content);
                    XamlParser = xamlParser;
                }
                catch (Exception ex)
                {

                }
            }
        }

        private bool CanExport(Window arg)
        {
            return SelectedControl != null;
        }

        private Task OnExit(Window arg)
        {
            if (CanCloseWindow())
            {
                new WindowsService(arg).CloseWindow();
            }
            return Task.CompletedTask;
        }

        private Task OnSelectImageBrush(ImageBrush arg)
        {
            SelectedBrush = arg;
            return Task.CompletedTask;
        }

        private Task OnBrowseImageSource(object arg)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Png *.png|*.Png|jpg *.jpg|*.jpg|jpeg *.jpeg|*.jpeg|All Files *.*|*.*", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var extension = ofd.FileName.Substring(ofd.FileName.LastIndexOf('.')).ToLower();
                    using (StreamReader sr = new StreamReader(ofd.FileName))
                    {
                        BitmapImage bitmapImage = new BitmapImage(new Uri(ofd.FileName));

                        (SelectedBrush as ImageBrush).ImageSource = bitmapImage;
                    }

                }
            }

            BrushToXaml.Brush = SelectedBrush ;

            return Task.CompletedTask;
        }

        private Task OnDeleteGradientStop(GradientStop arg)
        {
            try
            {
                ((SelectedProperty as PropertyModel).Value as GradientBrush).GradientStops.Remove(arg);
                GradientBrush val = ((SelectedProperty as PropertyModel).Value as GradientBrush).CloneCurrentValue();

                SelectedProperty.SetOwnerProperty(val);
                BrushToXaml.Brush = val ;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
            return Task.CompletedTask;
        }



        private bool CanSetBrushColor(SolidColorBrush arg)
        {
            return SelectedBrush != null;
        }

        private Task OnExport(Window arg)
        {
            var control = new ControlModel();
            control.Children.Add(SelectedControl);
            ExportStyleViewModel viewModel = new ExportStyleViewModel(control);
            WindowsService windowsService = new WindowsService(typeof(ExportStyleView), viewModel);
            windowsService.ShowDialog(arg);
            IsExported = viewModel.IsExported;
            return Task.CompletedTask;
        }
        private Task OnAllExport(Window arg)
        {
            ExportStyleViewModel viewModel = new ExportStyleViewModel(UIControl);
            WindowsService windowsService = new WindowsService(typeof(ExportStyleView), viewModel);
            windowsService.ShowDialog(arg);
            IsExported = viewModel.IsExported;


            return Task.CompletedTask;
        }
        private Task OnEditXaml(Window arg)
        {
            if (CanCloseWindow())
            {
                UIControl = new ControlModel();
                EditXamlViewModel viewModel = new EditXamlViewModel(XamlParser);
                WindowsService windowsService = new WindowsService(typeof(EditXamlView), viewModel);
                windowsService.ShowDialog(arg);

                XamlParser = viewModel.XamlParser;
            }
            return Task.CompletedTask;

        }
        public bool CanCloseWindow()
        {
            if ((UI != null || WindowUI != null) && IsExported == false)
            {
                var dialogResult = MessageBox.Show("All your changes will be lost.\nDiscard Changes ?", "Style Editor", MessageBoxButtons.YesNo, icon: MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No)
                {
                    return false;
                }
            }
            return true;

        }
        private Task OnAddGradientStopCommand(object arg)
        {
            try
            {
                ((arg as PropertyModel).Value as GradientBrush).GradientStops.Add(new GradientStop());
                GradientBrush val = ((arg as PropertyModel).Value as GradientBrush).CloneCurrentValue();

                SelectedProperty.SetOwnerProperty(val);
                BrushToXaml.Brush = val;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " On: " + MethodInfo.GetCurrentMethod().Name);
            }
            return Task.CompletedTask;

        }

        private Task OnSelectGradientStopBrush(GradientStop arg)
        {
            SelectedBrush = arg;
            return Task.CompletedTask;
        }

        private Task OnSetBrushCommand(SolidColorBrush arg)
        {
            if (SelectedProperty != null)
            {
                if (SelectedBrush.GetType() == typeof(SolidColorBrush))
                {
                    SelectedProperty.SetProperty(arg);
                }
                else if (SelectedBrush.GetType() == typeof(GradientStop))
                {
                    GradientStop gradientStop = (SelectedProperty.Value as GradientBrush).GradientStops.FirstOrDefault(x => x == (SelectedBrush as GradientStop));
                    System.Windows.Data.BindingOperations.ClearBinding(gradientStop, GradientStop.ColorProperty);
                    System.Windows.Data.BindingOperations.SetBinding(gradientStop, GradientStop.ColorProperty, new System.Windows.Data.Binding("Color") { Source = arg });
                }
                IsExported = false;
            }
            return Task.CompletedTask;
        }

        private Task OnSetBackgroundToImage(PropertyModel arg)
        {
            var element = SelectedControl.
                      BrushProperties.Where(x => x.DependencyProperty == SelectedProperty.DependencyProperty).FirstOrDefault();

            element.SetOwnerProperty(new ImageBrush());

            SelectedProperty = null;
            SelectedProperty = element;
            SelectedBrush = element.Value;

             return Task.CompletedTask;
        }

        private Task OnSetBackgroundToLinear(PropertyModel arg)
        {
            var element = SelectedControl.
                          BrushProperties.Where(x => x.DependencyProperty == SelectedProperty.DependencyProperty).FirstOrDefault();

            element.SetOwnerProperty(new LinearGradientBrush());

            SelectedProperty = null;
            SelectedProperty = element;
            SelectedBrush = element.Value;

            return Task.CompletedTask;
        }

        private Task OnSetBackgroundToNone(PropertyModel arg)
        {
            var element = SelectedControl.
                                      BrushProperties.Where(x => x.DependencyProperty == SelectedProperty.DependencyProperty).FirstOrDefault();

            element.SetOwnerProperty(null);

            SelectedProperty = null;
            SelectedProperty = element;
            SelectedBrush = element.Value;

             return Task.CompletedTask;
        }

        private Task OnSetBackgroundToRadial(PropertyModel arg)
        {
            var element = SelectedControl.
                              BrushProperties.Where(x => x.DependencyProperty == SelectedProperty.DependencyProperty).FirstOrDefault();

            element.SetOwnerProperty(new RadialGradientBrush());

            SelectedProperty = null;
            SelectedProperty = element;
            SelectedBrush = element.Value as RadialGradientBrush;

            return Task.CompletedTask;
        }

        private Task OnSetBackgroundToSolid(PropertyModel arg)
        {
            var element = SelectedControl.
                              BrushProperties.Where(x => x.DependencyProperty == SelectedProperty.DependencyProperty).FirstOrDefault();

            element.SetOwnerProperty(new SolidColorBrush());

            SelectedProperty = null;
            SelectedProperty = element;
            SelectedBrush = element.Value;

            return Task.CompletedTask;
        }

        private Task OnSelectSolidBrush(SolidColorBrush arg)
        {
            SelectedBrush = arg;
            return Task.CompletedTask;
        }



        private async Task OnSelectProperty(object arg)
        {
            await Task.Run(() =>
            {
                //fake task 
            });
            //if (arg is PropertyModel)
            //{
            SelectedProperty = arg as PropertyModel;
            //}
        }
        private bool CanSelectBrush(object arg)
        {
            return SelectedProperty != null;
        }
    }
}
