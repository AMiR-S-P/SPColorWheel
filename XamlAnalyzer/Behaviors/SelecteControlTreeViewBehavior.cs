using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using XamlAnalyzer.Model;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    class SelecteControlTreeViewBehavior:Behavior<TreeView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += AssociatedObject_SelectedItemChanged;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectedItemChanged -= AssociatedObject_SelectedItemChanged;
        }
        private void AssociatedObject_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            (AssociatedObject.DataContext as XamlEditorViewModel).SelectedControl = (ControlModel)e.NewValue??null;
        }


    }
}
