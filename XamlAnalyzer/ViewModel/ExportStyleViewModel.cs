using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using XamlAnalyzer.Commands;
using XamlAnalyzer.Model;
using XamlAnalyzer.Services;
using XamlAnalyzer.Utilities;

namespace XamlAnalyzer.ViewModel
{
    public class ExportStyleViewModel : BaseViewModel
    {
        private ExportControlModel selectedControl;
        private bool mustGenerateKey = true;

        public bool MustGenerateKey
        {
            get => mustGenerateKey; set { mustGenerateKey = value; OnPropertyChanged(); }
        }
        public bool IsExported { get; set; } = false;
        public ObservableCollection<ExportControlModel> UIControls { get; set; } = new ObservableCollection<ExportControlModel>();
        public ExportControlModel SelectedControl
        {
            get => selectedControl; set
            {
                if (selectedControl != null)
                {
                    selectedControl.IsSelected = false;
                }
                selectedControl = value;
                if (selectedControl != null)
                {
                    selectedControl.IsSelected = true;
                }
                OnPropertyChanged();
            }
        }


        public AsyncRelayCommand<object> ExportCommand { get; set; }
        public AsyncRelayCommand<object> SelectAllCommand { get; set; }
        public AsyncRelayCommand<object> UnselectAllCommand { get; set; }
        public AsyncRelayCommand<object> InvertSelectionCommand { get; set; }
        public ExportStyleViewModel(ControlModel controls)
        {
            FillUIControls(controls);


            ExportCommand = new AsyncRelayCommand<object>(OnExport);
            SelectAllCommand = new AsyncRelayCommand<object>(OnSelectAll);
            UnselectAllCommand = new AsyncRelayCommand<object>(OnUnselectAll);
            InvertSelectionCommand = new AsyncRelayCommand<object>(InvertSelection);
        }



        private void FillUIControls(ControlModel control)
        {
            
            foreach (var c in control.Children)
            {
                UIControls.Add(new ExportControlModel()
                {
                    BrushProperties = c.BrushProperties,
                    ClassName = c.ClassName,
                    Element = c.Element,
                    Name = c.Name
                });
                FillUIControls(c);
            }
        }
        private async Task OnExport(object arg)
        {
            try
            {
                ExportXaml exportXaml = new ExportXaml(UIControls.Where(x => x.IsMarked == true), MustGenerateKey);
                await exportXaml.CreateResourceDictionary();

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Xaml Files *.xaml|*.xaml|All Files *.txt|*.txt";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        await exportXaml.SaveToFile(sfd.FileName);
                    }
                }
                IsExported = true;

            }
            catch (Exception ex)
            {
                IsExported = false;
            }
        }

        private Task OnSelectAll(object arg)
        {
            foreach (var c in UIControls)
            {
                c.IsMarked = true;
            }
            return Task.CompletedTask;
        }

        private Task OnUnselectAll(object arg)
        {
            foreach (var c in UIControls)
            {
                c.IsMarked = false;
            }
            return Task.CompletedTask;
        }

        private Task InvertSelection(object arg)
        {
            foreach (var c in UIControls)
            {
                c.IsMarked = !c.IsMarked;
            }
            return Task.CompletedTask;
        }
    }
}
