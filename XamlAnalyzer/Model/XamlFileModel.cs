using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlAnalyzer.Model
{
    public class XamlFileModel : BaseModel
    {
        private string content;


        #region events
        public event EventHandler ContentChanged;
        public void OnContentChanged()
        {
            ContentChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        public ObservableCollection<NamespaceModel> Namespaces { get; set; } = new ObservableCollection<NamespaceModel>();
        public ObservableCollection<StaticResourceModel> RequiredStaticResources { get; set; } = new ObservableCollection<StaticResourceModel>();
        public ObservableCollection<DynamicResourceModel> RequiredDynamicResources { get; set; } = new ObservableCollection<DynamicResourceModel>();
        public ObservableCollection<TagResourcesModel> Resources { get; set; } = new ObservableCollection<TagResourcesModel>();

        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Content { get => content; set { content = value; OnPropertyChanged();OnContentChanged(); } }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public string SharpedContent { get; set; }
    }
}
