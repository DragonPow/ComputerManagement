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
    /// Interaction logic for TableRow.xaml
    /// </summary>
    public partial class TableRow : UserControl
    {
        public TableRow()
        {
            InitializeComponent();
            InitData();
        }
        public void InitData()
        {
            CategoryLv1.Text = "Lap top";
            CategoryLv2.Text = "Macbook,  acer, HP, MSI, ";
        }
    }
}
