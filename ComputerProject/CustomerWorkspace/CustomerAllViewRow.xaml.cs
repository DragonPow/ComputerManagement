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
        public CustomerAllViewRow()
        {
            InitializeComponent();
            MockData();
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
            cellName.Text = customer.name.ToString();
            cellPhone.Text = customer.phone.ToString();
            cellBirthday.Text = customer.birthday.ToString();
            cellAddress.Text = customer.address.ToString();
            cellPoint.Text = customer.point.ToString();
        }
    }
}
