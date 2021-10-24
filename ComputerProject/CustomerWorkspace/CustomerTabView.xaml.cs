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
    public partial class CustomerTabView : UserControl
    {
        public CustomerTabViewModel _vm => this.DataContext as CustomerTabViewModel;
        //private CustomerAllView mainView;

        public CustomerTabView()
        {
            InitializeComponent();

            var mainView = new CustomerAllView();
            mainView.ClickedCreate += OnClick_Create;
            mainView.ClickedDeleteItem += OnClick_DeleteItem;
            mainView.ClickedEditItem += OnClick_EditItem;


            var addView = new CustomerAdd();
            addView.Closed_NotSave += (s, e) => _vm.CurrentMainViewIndex = 0;
            addView.Saved += AddView_Saved;

            var detailView = new CustomerDetailView();

            DataContext = new CustomerTabViewModel()
            {
                ListViews = new List<Control>()
                {
                    mainView, addView//, detailView
                }
            };

            _vm.CurrentMainViewIndex = 0;
        }

        private void OnClick_Create(object sender, EventArgs e)
        {
            MessageBox.Show("hora! it's worked");
            var addView = _vm.ListViews[1] as CustomerAdd;
            addView.DataContext = new CustomerViewModel();

            _vm.CurrentMainViewIndex = 1;
        }

        private void AddView_Saved(object sender, EventArgs e)
        {
            var cus = (sender as CustomerAdd).ViewModel;
            MessageBox.Show(cus.FullName);

            _vm.CurrentMainViewIndex = 0;
        }

        private void OnClick_EditItem(object sender, EventArgs e)
        {
            MessageBox.Show("hora! it's worked");
            //_vm.CurrentMainViewIndex = 1;
        }

        private void OnClick_DeleteItem(object sender, EventArgs e)
        {
            MessageBox.Show("hora! it's worked");
            //_vm.CurrentMainViewIndex = 1;
        }
    }
}
