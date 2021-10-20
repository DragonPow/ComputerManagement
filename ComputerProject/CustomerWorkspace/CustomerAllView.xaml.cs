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

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerAllView.xaml
    /// </summary>
    public partial class CustomerAllView : UserControl
    {
        private List<CustomerViewModel> _data => this.DataContext as List<CustomerViewModel>;

        public CustomerAllView()
        {
            InitializeComponent();
            mainList.ItemsSource = _data;
            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MockData()
        {
            DataContext = new List<CustomerViewModel>
            {
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
            };
        }
    }
}
