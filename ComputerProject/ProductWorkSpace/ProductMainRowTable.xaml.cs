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
        public static DependencyProperty CommandEditProperty = DependencyProperty.Register(nameof(CommandEdit), typeof(ICommand), typeof(ProductMainRowTable));
        public static DependencyProperty CommandDeleteProperty = DependencyProperty.Register(nameof(CommandDelete), typeof(ICommand), typeof(ProductMainRowTable));

        public ProductMainRowTable()
        {
            InitializeComponent();
        }

        public ICommand CommandEdit
        {
            get => (ICommand)GetValue(CommandEditProperty);
            set => SetValue(CommandEditProperty, value);
        }

        public ICommand CommandDelete
        {
            get => (ICommand)GetValue(CommandEditProperty);
            set => SetValue(CommandEditProperty, value);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CommandEdit.Execute(this);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            CommandDelete.Execute(this);
        }
    }
}
