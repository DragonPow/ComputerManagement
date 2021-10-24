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
            "ViewModel", typeof(CustomerViewModel), typeof(CustomerAllViewRow)
            );

        public CustomerViewModel ViewModel
        {
            get => DataContext as CustomerViewModel;
            set => DataContext = value;
        }

        public CustomerAllViewRow()
        {
            InitializeComponent();
            this.DataContext = new CustomerViewModel();
            Bind();
            MockData();
        }

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
