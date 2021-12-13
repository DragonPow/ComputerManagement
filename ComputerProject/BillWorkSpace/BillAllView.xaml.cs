using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for BillAllView.xaml
    /// </summary>
    public partial class BillAllView : UserControl
    {
        public BillAllView()
        {
            InitializeComponent();
            TimeFrom.Language = TimeTo.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.GetCultureInfo("vi").IetfLanguageTag);
            tbxSearch.TextChanged += TbxSearch_TextChanged;
        }

        private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBill();
        }

        private void tbxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchBill();
            }
        }

        private void SearchBill()
        {
            HistoryBillViewModel dataContext = this.DataContext as HistoryBillViewModel;
            if (dataContext != null)
            {
                if (dataContext.SearchBillCommand.CanExecute(null))
                {
                    dataContext.SearchBillCommand.Execute(tbxSearch.Text);
                }
            }
        }

        private void PageControl_PageChanged(object sender, int e)
        {
            var datacontext = this.DataContext as HistoryBillViewModel;
            if (datacontext.ChangePageCommand.CanExecute(e))
                datacontext.ChangePageCommand.Execute(e);
        }
    }
}
