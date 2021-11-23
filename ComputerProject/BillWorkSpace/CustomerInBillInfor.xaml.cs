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
    /// Interaction logic for CustomerInBillInfor.xaml
    /// </summary>
    public partial class CustomerInBillInfor : UserControl
    {
        public CustomerInBillInfor()
        {
            InitializeComponent();
            Birthday.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.GetCultureInfo("vi").IetfLanguageTag);
        }
    }
}
