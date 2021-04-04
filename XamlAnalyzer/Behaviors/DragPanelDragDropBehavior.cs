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
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using XamlAnalyzer.Model;
using XamlAnalyzer.Utilities;
using XamlAnalyzer.ViewModel;

namespace XamlAnalyzer.Behaviors
{
    public class DragPanelDragDropBehavior : Behavior<Grid>
    {
        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    AssociatedObject.Loaded += AssociatedObject_Loaded;
        //    AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        //}

        //protected override void OnDetaching()
        //{
        //    AssociatedObject.Loaded -= AssociatedObject_Loaded;
        //    AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
        //    base.OnDetaching();
        //}
        //private void AssociatedObject_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    AssociatedObject.Drop -= AssociatedObject_Drop;
        //    AssociatedObject.DragEnter -= AssociatedObject_DragEnter;

        //}

        //private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    AssociatedObject.Drop += AssociatedObject_Drop;
        //    AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        //}

        //private void AssociatedObject_DragEnter(object sender, System.Windows.DragEventArgs e)
        //{
        //    try
        //    {
        //        var fileName = ((string[])e.Data.GetData("FileNameW"))[0].ToString();
        //        var extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

        //        if (extension == "xaml")
        //        {
        //            AssociatedObject.Background = Brushes.Green;
        //        }
        //        else
        //        {
        //            AssociatedObject.Background = Brushes.Red;
        //            AssociatedObject.Cursor = Cursors.No;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private void AssociatedObject_Drop(object sender, System.Windows.DragEventArgs e)
        //{
        //    try
        //    {
        //        var fileName = ((string[])e.Data.GetData("FileNameW"))[0].ToString();
        //        var extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

        //        if (extension == "xaml")
        //        {
        //            var root = VisualTreeHelper.GetParent(AssociatedObject);
        //            while (root != null)
        //            {
        //                root = VisualTreeHelper.GetParent(root);
        //            }

        //            var file = File.ReadAllText(fileName, Encoding.UTF8);

        //            AssociatedObject.Background = Brushes.Green;

        //            if (AssociatedObject.DataContext is SplashScreenViewModel)
        //            {
        //                var xamlFile = new Model.XamlFileModel()
        //                {
        //                    FullName = fileName,
        //                    Content = file,
        //                    Extension = extension,
        //                    FileName = fileName.Substring(fileName.LastIndexOf('\\') + 1)
        //                };

        //                (AssociatedObject.DataContext as SplashScreenViewModel).XamlFile = xamlFile;
        //                (AssociatedObject.DataContext as SplashScreenViewModel).DragComplete?.Execute((Window)root);

        //            }
        //        }
        //        else
        //        {
        //            //display error and return
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
