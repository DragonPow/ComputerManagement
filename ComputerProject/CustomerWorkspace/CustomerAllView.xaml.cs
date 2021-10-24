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
        public CustomerAllViewModel ViewModel => this.DataContext as CustomerAllViewModel;

        public CustomerAllView()
        {
            InitializeComponent();
            DataContext = new CustomerAllViewModel(new List<CustomerViewModel>());
            btnCreate.Click += BtnCreate_Click;
            MockData();
        }

        public event EventHandler ClickedCreate;
        public event EventHandler ClickedEditItem;
        public event EventHandler ClickedDeleteItem;

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Do something

            // Call-back
            ClickedCreate?.Invoke(this, e);
        }

        private void BtnEditItem_Click(object sender, EventArgs e)
        {
            // Do something
            Console.Write("here");
            // Call-back
            ClickedEditItem?.Invoke(sender, e);
        }

        private void BtnDeleteItem_Click(object sender, EventArgs e)
        {
            // Do something
            
            // Call-back
            ClickedDeleteItem?.Invoke(sender, e);
        }

        private void MockData()
        {
            var list = new List<CustomerViewModel>
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

            DataContext = new CustomerAllViewModel(list);
        }
    }
}
