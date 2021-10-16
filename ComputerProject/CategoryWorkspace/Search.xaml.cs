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

namespace ComputerProject.Category
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
          
        }

        public static readonly DependencyProperty TexthintProperty =
           DependencyProperty.Register(nameof(Texthint), typeof(string),
                           typeof(Search), new PropertyMetadata("Tìm kiếm"));

        public string Texthint
        {
            get { return (string)GetValue(TexthintProperty); }
            set { SetValue(TexthintProperty, value); }
        }

    }
}
