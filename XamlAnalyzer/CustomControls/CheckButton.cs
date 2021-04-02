using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XamlAnalyzer.CustomControls
{
    public class CheckButton:Button
    {
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(CheckButton), new PropertyMetadata(false));

        static CheckButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckButton), new FrameworkPropertyMetadata(typeof(CheckButton)));
        }
    }
}
