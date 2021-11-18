using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ComputerProject.SaleWorkSpace
{
    /// <summary>
    /// Interaction logic for CustomerSale.xaml
    /// </summary>
    public partial class CustomerSaleView : UserControl
    {
        Timer timeDelayText;
        public CustomerSaleView()
        {
            InitializeComponent();
            timeDelayText = new Timer(1500);
            timeDelayText.Elapsed += TimeDelayText_Elapsed;
            completebox.TextChanged += Completebox_TextChanged;
            completebox.SelectionChanged += Completebox_SelectionChanged;
        }

        private void Completebox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timeDelayText.Stop();
            completebox.Text = null;
            //Console.WriteLine((completebox.SelectedItem as CUSTOMER)?.name??"nothing to bind");
        }

        private void TimeDelayText_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeDelayText.Stop();
            this.Dispatcher.Invoke(() =>
            {
                SaleViewModel dataContext = this.DataContext as SaleViewModel;
                if (dataContext != null)
                {
                    if (dataContext.SearchCustomerCommand.CanExecute(null))
                    {
                        dataContext.SearchCustomerCommand.Execute(completebox.Text);
                        completebox.PopulateComplete();
                    }
                }
            });
        }

        private void Completebox_TextChanged(object sender, RoutedEventArgs e)
        {
            timeDelayText.Stop();
            if (!string.IsNullOrEmpty(completebox.Text))
            {
                timeDelayText.Start();
            }
        }
    }
}
