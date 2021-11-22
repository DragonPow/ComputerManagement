using ComputerProject.CustomControl;
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

namespace ComputerProject.CategoryWorkspace
{
    /// <summary>
    /// Interaction logic for FeatureBar.xaml
    /// </summary>
    public partial class FeatureBar : UserControl
    {
        public FeatureBar()
        {
            InitializeComponent();
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string textSearch = (sender as Search).Text;
                var datacontext = (sender as Control).DataContext as ListCategoryViewModel;
                if (datacontext != null && datacontext.SearchCommand.CanExecute(textSearch)) 
                {
                    datacontext.SearchCommand.Execute(textSearch);
                }
            }
        }
    }
}
