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
    /// Interaction logic for CustomerAllViewRow.xaml
    /// </summary>
    public partial class CustomerAllViewRow : UserControl
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel), typeof(CustomerViewModel), typeof(CustomerAllViewRow));

        public CustomerAllViewRow()
        {
            InitializeComponent();
        }

        public CustomerViewModel ViewModel
        {
            get => DataContext as CustomerViewModel;
            set => DataContext = value;
        }

        public event EventHandler ClickedEdit;
        public event EventHandler ClickedDelete;

        private void Bind()
        {
            cellName.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CustomerViewModel.FullName)),
                Mode = BindingMode.OneWay
            });
            cellAddress.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CustomerViewModel.Address)),
                Mode = BindingMode.OneWay
            });
            cellBirthday.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CustomerViewModel.Birthday)),
                Mode = BindingMode.OneWay
            });
            cellPhone.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CustomerViewModel.PhoneNumber)),
                Mode = BindingMode.OneWay
            });
            cellPoint.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath(nameof(CustomerViewModel.Point_String)),
                Mode = BindingMode.OneWay
            });

            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClickedEdit?.Invoke(this, e);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ClickedDelete?.Invoke(this, e);
        }

        void MockData()
        {
            CUSTOMER customer = new CUSTOMER()
            {
                name = "Nguyen Pham Duy Bang",
                phone = "096590308",
                birthday = "30/08/2001",
                address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                point = 9000
            };

            ViewModel = new CustomerViewModel(customer);
        }
    }
}
