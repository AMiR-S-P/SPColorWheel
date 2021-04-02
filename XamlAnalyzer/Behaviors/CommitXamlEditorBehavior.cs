using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using XamlAnalyzer.UserControls.Editor;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    public class CommitXamlEditorBehavior:Behavior<Editor>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= AssociatedObject_KeyDown;

            base.OnDetaching();
        }
        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key== System.Windows.Input.Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                //AssociatedObject.XamlTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                (AssociatedObject.DataContext as XamlEditorViewModel).IsCommited = true;
            }
        }
    }
}
