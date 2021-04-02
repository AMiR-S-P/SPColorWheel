using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Xml;
using XamlAnalyzer.Model;
using XamlAnalyzer.Utilities;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    class DragResourceBehavior : Behavior<TreeView>
    {
        Brush lastBackground = null;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            base.OnDetaching();
        }
        private void AssociatedObject_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.Drop -= AssociatedObject_Drop;
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;

        }

        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.Drop += AssociatedObject_Drop;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            AssociatedObject.DragLeave += AssociatedObject_DragLeave;

        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                lastBackground = AssociatedObject.Background;

                var fileName = ((string[])e.Data.GetData("FileNameW"))[0].ToString();
                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

                if (extension == "xaml")
                {
                    AssociatedObject.Background = Brushes.Green;
                }
                else
                {
                    AssociatedObject.Background = Brushes.Red;
                    AssociatedObject.Cursor = Cursors.No;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            try
            {
                AssociatedObject.Background = lastBackground;
                AssociatedObject.Cursor = Cursors.Arrow;
                var fileName = ((string[])e.Data.GetData("FileNameW"))[0].ToString();
                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

                if (extension == "xaml")
                {
                    var root = VisualTreeHelper.GetParent(AssociatedObject);
                    while (root != null)
                    {
                        root = VisualTreeHelper.GetParent(root);
                    }

                    var file = File.ReadAllText(fileName, Encoding.UTF8);

                    if (AssociatedObject.DataContext is EditXamlViewModel)
                    {
                       await (AssociatedObject.DataContext as EditXamlViewModel).XamlParser.AddResourceFromFile(fileName); }
                }
                else
                {
                    //display error and return
                    throw new Exception("File is not a resource file.");
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            AssociatedObject.Cursor = Cursors.Arrow;
            AssociatedObject.Background = lastBackground;
        }

    }
}
