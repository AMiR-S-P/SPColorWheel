using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Xml;
using XamlAnalyzer.Commands;
using XamlAnalyzer.Model;
using XamlAnalyzer.Services;
using XamlAnalyzer.Utilities;
using MessageBox = System.Windows.Forms.MessageBox;

namespace XamlAnalyzer.ViewModel
{
    public class EditXamlViewModel : BaseViewModel
    {
        private bool isSaved;
        private TextDocument document;
        private bool? hasNoError;
        SPXamlParser originalXamlParser = new SPXamlParser();
        string fileName = null;
        private string error;

        public bool IsSaved { get => isSaved; set { isSaved = value; OnPropertyChanged(); } }
        public bool? HasNoError { get => hasNoError; set { hasNoError = value; OnPropertyChanged(); } }
        public SPXamlParser XamlParser { set; get; }
        public string Error { get => error; set { error = value; OnPropertyChanged(); } }
        public TextDocument Document { get => document; set { document = value; OnPropertyChanged(); } }

        public AsyncRelayCommand<Window> DragComplete { get; set; }

        public AsyncRelayCommand<object> FormatXamlCommand { get; set; }
        public AsyncRelayCommand<object> SaveCommand { set; get; }
        public AsyncRelayCommand<object> NewCommand { get; set; }
        public AsyncRelayCommand<object> OpenCommand { get; set; }
        public AsyncRelayCommand<Window> ExitCommand { get; set; }
        public AsyncRelayCommand<object> CheckForErrorCommand { get; set; }
        public AsyncRelayCommand<ResourceFileModel> DeleteResourceFileCommand { get; set; }
        public AsyncRelayCommand<ResourceFileModel> IncludeResourceCommand { get; set; }

        public AsyncRelayCommand<object> WindowsClosingCommand { get; set; }
        public AsyncRelayCommand<object> AddResourceFileCommand { get; set; }
        public AsyncRelayCommand<object> AddAssemblyFileCommand { get; set; }

        public EditXamlViewModel(SPXamlParser xamlParser)
        {
            originalXamlParser = xamlParser;
            if (xamlParser == null)
            {
                XamlParser = new SPXamlParser("");
            }
            else
            {
                XamlParser = xamlParser;
                CheckForErrorCommand?.ExecuteAsync(null);
            }
            //this should be removed on windows closing
            XamlParser.ImportedResources.CollectionChanged += async (s, e) =>
            {
                await FormatXamlCommand.ExecuteAsync(null);
            };

            Document = new TextDocument(XamlParser.Xaml);
            Document.TextChanged += (s, e) =>
            {
                //XamlParser.LoadXaml(Document.Text);
                HasNoError = null;
                IsSaved = false;
            };
            BindingOperations.SetBinding(
                XamlParser,
                SPXamlParser.XamlProperty,
                new System.Windows.Data.Binding("Text")
                {
                    Source = Document,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            DragComplete = new AsyncRelayCommand<Window>(OnDragComplete);

            FormatXamlCommand = new AsyncRelayCommand<object>(OnFormatXaml);
            SaveCommand = new AsyncRelayCommand<object>(OnSave);
            NewCommand = new AsyncRelayCommand<object>(OnNew);
            OpenCommand = new AsyncRelayCommand<object>(OnOpen);
            ExitCommand = new AsyncRelayCommand<Window>(OnExit);

            CheckForErrorCommand = new AsyncRelayCommand<object>(OnCheckForError);
            WindowsClosingCommand = new AsyncRelayCommand<object>(OnWindowsClosing);
            DeleteResourceFileCommand = new AsyncRelayCommand<ResourceFileModel>(OnDeleteResourceFile);
            IncludeResourceCommand = new AsyncRelayCommand<ResourceFileModel>(OnIncludeResource);
            AddResourceFileCommand = new AsyncRelayCommand<object>(OnAddResourceFile);
            AddAssemblyFileCommand = new AsyncRelayCommand<object>(OnAddAssemblyFile);
        }

        private async Task OnAddAssemblyFile(object arg)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "DLL files *.dll| *.dll";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fn in ofd.FileNames)
                    {
                        await XamlParser.AddAssemblyFile(new AssemblyFileModel()
                        {
                            Path = fn,
                            Name = fn.Substring(fileName.LastIndexOf('\\') + 1).Replace(".dll", ""),
                        });
                    }
                }
            }
        }
        private Task OnExit(Window arg)
        {
            new WindowsServices().CloseWindow(arg);
            return Task.CompletedTask;
        }

        private async Task OnAddResourceFile(object arg)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Xaml files *.xaml| *.xaml|All files *.*| *.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fn in ofd.FileNames)
                    {
                        await XamlParser.AddResourceFromFile(fn);
                    }
                }
            }
        }

        private async Task OnWindowsClosing(object arg)
        {

        }

        private Task OnCheckForError(object arg)
        {
            try
            {
                bool lastSave = IsSaved;
                XamlParser.LoadXaml(Document.Text);
                ParserContext parserContext = new ParserContext();
                parserContext.XamlTypeMapper = new XamlTypeMapper(new string[] { });
                XamlParser.ReloadAssembelies();
                XamlReader.Parse(XamlParser?.Xaml, parserContext);
                HasNoError = true;
                IsSaved = lastSave;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                HasNoError = false;
            }
            return Task.CompletedTask;
        }

        private async Task OnOpen(object arg)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Xaml files (*.xaml)|*.xaml|All files (*.*)|*.*";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    using (StreamReader sr = new StreamReader(ofd.FileName))
                    {
                        Document.Text = await sr.ReadToEndAsync();
                    }
                }
            }

        }


        private Task OnNew(object arg)
        {
            XamlParser.ResetXamlParser();
            return Task.CompletedTask;
        }

        private async Task OnSave(object arg)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Xaml files *.xaml|*.Xaml|All files *.*|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        fileName = saveFileDialog.FileName;
                    }
                }
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    using (var file = File.Create(fileName))
                    {
                        await file.WriteAsync(Encoding.UTF8.GetBytes(Document.Text), 0, Document.TextLength);
                        IsSaved = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    IsSaved = false;
                }
            }
        }

        private async Task OnFormatXaml(object arg)
        {
            await XamlParser.FormatXaml();
        }

        public async Task OnDragComplete(Window obj)
        {

            //Document.Text = XamlParser.Xaml;
            //await FormatXamlCommand.ExecuteAsync(null);
            //using (var file = File.CreateText(Path.Combine("Workplace", XamlParser.ImportedResources.Last().Name)))
            //{
            //    await file.WriteAsync(XamlParser.ImportedResources.Last().Content);
            //}
        }
        private async Task OnDeleteResourceFile(ResourceFileModel arg)
        {
            if (arg != null)
            {
                var r = XamlParser.ImportedResources.FirstOrDefault(x => x.Source == arg.Source);
                XamlParser.ImportedResources.Remove(r);
            }
            await FormatXamlCommand.ExecuteAsync(null);
        }
        async Task OnIncludeResource(ResourceFileModel arg)
        {
            if (arg.IsUsing)
            {
                await XamlParser.AddResourceToXaml(arg);
            }
            else
            {
                await XamlParser.RemoveResourceFromXaml(arg);
            }
            await FormatXamlCommand.ExecuteAsync(null);
        }

    }
}
