using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComputerProject.Helper
{
    public class WindowService
    {
        public static IEnumerable<T> FindVisualChildrent<T>(DependencyObject myObject) where T : DependencyObject
        {
            if (myObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(myObject, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //find childrent of childrent
                    foreach (T childOfChild in FindVisualChildrent<T>(child)) yield return childOfChild;
                }
            }
        }

        public static void ShowWindow(object ViewModel, UserControl View)
        {
            var window = new Window();

            window.Content = View;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.DataContext = ViewModel;
            window.Tag = View.Tag;
            window.ShowDialog();
        }

        public static IEnumerable<Window> FindWindowbyTag(string nameWindow)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is Window && (string)((Window)window).Tag == nameWindow)
                {
                    Console.WriteLine("Find window");
                    yield return (Window)window;
                }
            }
        }
    }
}
