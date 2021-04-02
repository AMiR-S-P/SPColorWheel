using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    public class EditXamlCloseBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += AssociatedObject_Closing;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closing -= AssociatedObject_Closing;

        }
        private async void AssociatedObject_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EditXamlViewModel vm = AssociatedObject.DataContext as EditXamlViewModel;

            vm.XamlParser.ReloadXaml();

            await vm.CheckForErrorCommand.ExecuteAsync(null);
            if (vm.HasNoError==false)
            {
                if (MessageBox.Show("Contain Errors.\nClose dialog?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }

        }
    }
}
