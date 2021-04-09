using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    class DeselectBrushPropertyBehavior:Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
        }

        private async void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult htr = VisualTreeHelper.HitTest(AssociatedObject, e.GetPosition(AssociatedObject));
            if(htr.VisualHit.GetType()== typeof(ScrollViewer))
            {
                await (AssociatedObject.DataContext as XamlEditorViewModel).SelectPropertyCommand.ExecuteAsync(null);
                AssociatedObject.SelectedItem = null;
            }
        }
    }
}
