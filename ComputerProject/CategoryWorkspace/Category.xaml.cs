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
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class Category : UserControl
    {
        public Category()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var datacontext = this.DataContext as ListCategoryViewModel;

            var param = new object[] { (sender as ListViewItem).DataContext, false };
            if (datacontext.OpenDetailCategoryCommand.CanExecute(param))
            {
                datacontext.OpenDetailCategoryCommand.Execute(param);
            }
        }
    }
}
