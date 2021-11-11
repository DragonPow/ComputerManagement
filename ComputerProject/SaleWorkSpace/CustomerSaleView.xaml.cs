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
            timeDelayText = new Timer(3000);
            timeDelayText.Elapsed += TimeDelayText_Elapsed;
            completebox.TextChanged += Completebox_TextChanged;
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
                        Console.WriteLine("Find customer with phone number start is {0}", completebox.Text);
                        dataContext.SearchCustomerCommand.Execute(completebox.Text);
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
