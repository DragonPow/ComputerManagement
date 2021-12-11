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
using System.Windows.Shapes;

namespace ComputerProject.InsuranceWorkSpace
{
    /// <summary>
    /// Interaction logic for CustomerInsurView.xaml
    /// </summary>
    public partial class CustomerInsurView : UserControl
    {
        Timer timeDelayText;
        public CustomerInsurView()
        {
            InitializeComponent();
            timeDelayText = new Timer(1500);
            timeDelayText.Elapsed += TimeDelayText_Elapsed;
            completebox.TextChanged += Completebox_TextChanged;
        }

        private void Completebox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timeDelayText.Stop();

            if (completebox.SelectedItem != null)
            {
                InsuranceDetailViewModel dataContext = this.DataContext as InsuranceDetailViewModel;
                var cus = completebox.SelectedItem as CUSTOMER;
                dataContext.CurrentBill.CUSTOMER = cus;
                dataContext.CurrentBill.customerId = cus.id;
                dataContext.OnCustomerChanged();
            }

            completebox.Text = null;
            //completebox.SelectedItem = null;
            //Console.WriteLine((completebox.SelectedItem as CUSTOMER)?.name??"nothing to bind");
        }

        private void TimeDelayText_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeDelayText.Stop();
            this.Dispatcher.Invoke(() =>
            {
                InsuranceDetailViewModel dataContext = this.DataContext as InsuranceDetailViewModel;
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
                if (completebox.SelectedItem != null)
                {
                    var o = completebox.SelectedItem as CUSTOMER;
                }
            }
        }
    }
}
