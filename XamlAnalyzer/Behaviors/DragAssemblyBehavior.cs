using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using XamlAnalyzer.Model;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    public class DragAssemblyBehavior : Behavior<TreeView>
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
                var fileName = ((string[])e.Data.GetData("FileNameW"))[0].ToString();
                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

                if (extension == "dll")
                {
                    lastBackground = AssociatedObject.Background;
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

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            try
            {
                AssociatedObject.Cursor = Cursors.Arrow;
                AssociatedObject.Background = lastBackground;

                var fileName = ((string[])e.Data.GetData("FileNameW"))[0].ToString();
                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

                if (extension != "dll")
                {
                    throw new Exception("Invaid file format.Format must be 'dll' or 'exe'.");
                }

                if (AssociatedObject.DataContext is EditXamlViewModel)
                {
                    AssemblyFileModel assemblyFileModel = new AssemblyFileModel()
                    {
                        Path = fileName,
                        Name = fileName.Substring(fileName.LastIndexOf('\\') + 1).Replace(".dll",""),
                    };

                    if ((AssociatedObject.DataContext as EditXamlViewModel).XamlParser.ImportedAssemblies.Any(x => x.Path == fileName))
                    {
                        //unload assembly in vm imported changed event


                        //remove and re-add

                        var assembly = (AssociatedObject.DataContext as EditXamlViewModel).XamlParser.ImportedAssemblies.FirstOrDefault(x => x.Path == fileName);
                        (AssociatedObject.DataContext as EditXamlViewModel).XamlParser.ImportedAssemblies.Remove(assembly);
                    }
                        (AssociatedObject.DataContext as EditXamlViewModel).XamlParser.ImportedAssemblies.Add(assemblyFileModel);
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
