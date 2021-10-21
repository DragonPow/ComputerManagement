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

namespace ComputerProject.CategoryWorkspace
{
    /// <summary>
    /// Interaction logic for PropertiesRow.xaml
    /// </summary>
    public partial class PropertiesRow : UserControl
    {
        public PropertiesRow()
        {
            InitializeComponent();
            InitData();
        }
        public void InitData()
        {
            Number.Text = "1";
            NameProperties.Text = "Kết nối có dây (LAN) ";
        }
        public void VisibleBtnDel()
        {
            btnDelete.Visibility = Visibility.Visible;
        }
    }
}
