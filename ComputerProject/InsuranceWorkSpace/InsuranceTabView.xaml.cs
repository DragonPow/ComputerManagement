using ComputerProject.ApplicationWorkspace;
using ComputerProject.Helper.Interface;
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

namespace ComputerProject.InsuranceWorkSpace
{
    /// <summary>
    /// Interaction logic for InsuranceTabView.xaml
    /// </summary>
    public partial class InsuranceTabView : UserControl, ITabView
    {
        public InsuranceTabView()
        {
            InitializeComponent();
        }

        public string ViewName => "Sửa chữa";

        public PackIconKind ViewIcon => PackIconKind.HammerWrench;

        public bool AllowChangeTab()
        {
            return true;
        }

        public void LoadDataAsync()
        {
            if (DataContext == null)
            {
                DataContext = new InsuranceTabViewModel();
            }
            (DataContext as IValidable).Validation();
        }
    }
}
