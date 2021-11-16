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
    /// Interaction logic for CustomerInfor.xaml
    /// </summary>
    public partial class CustomerInfor : UserControl
    {
        public CustomerInfor()
        {
            InitializeComponent();
            Birthday.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.GetCultureInfo("vi").IetfLanguageTag);
        }

        public static readonly DependencyProperty TextNameProperty = DependencyProperty.Register(nameof(TextName), typeof(string), typeof(CustomerInfor), new PropertyMetadata(string.Empty));

        public string TextName
        {
            get { return (string)GetValue(TextNameProperty); }
            set { SetValue(TextNameProperty, value); }
        }

        public static readonly DependencyProperty TextPhoneProperty = DependencyProperty.Register(nameof(TextPhone), typeof(string), typeof(CustomerInfor), new PropertyMetadata(string.Empty));

        public string TextPhone
        {
            get { return (string)GetValue(TextPhoneProperty); }
            set { SetValue(TextPhoneProperty, value); }
        }

        public static readonly DependencyProperty TextDateProperty = DependencyProperty.Register(nameof(TextDate), typeof(DateTime?), typeof(CustomerInfor), new PropertyMetadata(DateTime.Now));

        public DateTime TextDate
        {
            get { return (DateTime)GetValue(TextDateProperty); }
            set { SetValue(TextDateProperty, value); }
        }

        public static readonly DependencyProperty TextAddressProperty = DependencyProperty.Register(nameof(TextAddress), typeof(string), typeof(CustomerInfor), new PropertyMetadata(string.Empty));

        public string TextAddress
        {
            get { return (string)GetValue(TextAddressProperty); }
            set { SetValue(TextAddressProperty, value); }
        }

        public static readonly DependencyProperty TextPointProperty = DependencyProperty.Register(nameof(TextPoint), typeof(int), typeof(CustomerInfor), new PropertyMetadata(0));

        public int TextPoint
        {
            get { return (int)GetValue(TextPointProperty); }
            set { SetValue(TextPointProperty, value); }
        }
    }
}
