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

namespace ComputerProject.InsuranceWorkSpace
{
    /// <summary>
    /// Interaction logic for InsurancePayBillItem.xaml
    /// </summary>
    public partial class InsurancePayBillItem : UserControl
    {
        public static DependencyProperty NumberProperties = DependencyProperty.Register(nameof(number), typeof(string), typeof(InsurancePayBillItem), new PropertyMetadata("0"));

        public string number
        {
            get { return (string)GetValue(NumberProperties); }
            set { SetValue(NumberProperties, value); }
        }
        public InsurancePayBillItem()
        {
            InitializeComponent();
        }
    }
}
