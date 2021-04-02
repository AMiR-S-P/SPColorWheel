using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace XamlAnalyzer.Behaviors
{
    public class RefreshGradientStopsBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty ItemsControlProperty = DependencyProperty.Register("ItemsControl", typeof(ItemsControl), typeof(RefreshGradientStopsBehavior));

        public ItemsControl ItemsControl
        {
            set { SetValue(ItemsControlProperty, value); }
            get { return (ItemsControl)GetValue(ItemsControlProperty); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += AssociatedObject_Click;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= AssociatedObject_Click;
        }
        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BindingOperations.GetBindingExpression(ItemsControl, ItemsControl.ItemsSourceProperty).UpdateTarget();
        }
    }
}
