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

namespace ComputerProject.ProductWorkSpace
{
    /// <summary>
    /// Interaction logic for ProductMainRowTable.xaml
    /// </summary>
    public partial class ProductMainRowTable : UserControl
    {
        public ProductMainRowTable()
        {
            InitializeComponent();
        }

        public event EventHandler ClickEdit;
        public event EventHandler ClickDelete;

        void OnClick_Edit(object obj, RoutedEventArgs e)
        {
            ClickEdit?.Invoke(this, null);
        }

        void OnClick_Delete(object obj, RoutedEventArgs e)
        {
            ClickDelete?.Invoke(this, null);
        }
    }
}
