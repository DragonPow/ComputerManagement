using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ComputerProject.CustomerWorkspace;
using ComputerProject.CategoryWorkspace;

namespace ComputerProject.ApplicationWorkspace
{
    /// <summary>
    /// Interaction logic for ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void TabSelected(object sender, MouseButtonEventArgs e)
        {
            var tab = sender as ListViewItem;
            if (tab!=null && tab.IsSelected)
            {
                var dc = DataContext as ApplicationViewModel;
                //dc.CurrentViewModel = tab;
            }
        }

    }
}
