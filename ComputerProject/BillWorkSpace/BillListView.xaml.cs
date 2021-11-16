using ComputerProject.Model;
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

namespace ComputerProject.BillWorkSpace
{
    /// <summary>
    /// Interaction logic for BillListView.xaml
    /// </summary>
    public partial class BillListView : UserControl
    {
        public BillListView()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dc = ListBill.DataContext as HistoryBillViewModel;
            if (dc.ShowDetailBillCommand.CanExecute(null))
            {
                dc.ShowDetailBillCommand.Execute((sender as ListViewItem).DataContext);
            }
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
