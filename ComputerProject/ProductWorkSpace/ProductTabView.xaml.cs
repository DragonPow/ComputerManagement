using ComputerProject.ApplicationWorkspace;
using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for ProductTabView.xaml
    /// </summary>
    public partial class ProductTabView : UserControl, ITabView
    {
        public string ViewName => "Sản phẩm";
        public PackIconKind ViewIcon => PackIconKind.DesktopMacDashboard;

        public ProductTabView()
        {
            InitializeComponent();
        }

        public void BeginVM()
        {
            if (DataContext == null)
            {
                var vm = new ProductTabViewModel();
                vm.Validation();
                DataContext = vm;
            }
        }
    }
}
