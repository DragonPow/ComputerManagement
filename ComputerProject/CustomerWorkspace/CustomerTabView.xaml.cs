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

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerTabView.xaml
    /// </summary>
    public partial class CustomerTabView : UserControl, ApplicationWorkspace.ITabView
    {
        public CustomerTabViewModel _vm => this.DataContext as CustomerTabViewModel;

        public string ViewName => "Khách hàng";
        public PackIconKind ViewIcon => PackIconKind.AccountGroup;


        public CustomerTabView()
        {
            InitializeComponent();
        }

        public void BeginVM()
        {
            if (DataContext == null)
            {
                var vm = new CustomerTabViewModel();
                vm.Validation();
                DataContext = vm;
            }
        }

        public void LoadDataAsync()
        {

        }
        public bool AllowChangeTab()
        {
            return true;
        }
    }
}
