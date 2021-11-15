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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComputerProject.SettingWorkSpace;

namespace ComputerProject.SettingWorkSpace
{
    /// <summary>
    /// Interaction logic for SettingAllView.xaml
    /// </summary>
    public partial class SettingAllView : UserControl
    {
        public SettingAllView()
        {
            InitializeComponent();
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMenu.Children.Clear();

            switch (((ListBoxItem)((ListBox)sender).SelectedItem).Name)
            {
                case "ItemHelp":
                    usc = new SettingHelp();
                    GridMenu.Children.Add(usc);
                    break;
                case "ItemPoint":
                    usc = new SettingPointView();
                    GridMenu.Children.Add(usc);
                    break;
                case "ItemStore":
                    usc = new SettingStoreView();
                    GridMenu.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
    }
}
