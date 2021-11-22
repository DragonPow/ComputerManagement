using ComputerProject.HelperService;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerTabViewModel : ListControlViewModel
    {

        private CustomerAllViewModel mainVM;

        public CustomerTabViewModel()
        {
            var mainView = new CustomerAllView();
            mainView.ClickedCreate += OnClick_Create;
            mainView.ClickedDetailItem += OnClick_DetailItem;
            mainView.ClickedEditItem += OnClick_EditItem;

            mainVM = new CustomerAllViewModel();
            mainView.DataContext = mainVM;

            var addView = new CustomerAdd();
            addView.Closed_NotSave += OnBackFrom_Create;
            addView.SaveOK += AddView_SavedOK;

            var detailView = new CustomerDetailView();
            detailView.ClickedBack += OnBackFrom_EditItem;

            ListViews = new List<Control>()
                {
                    mainView, addView, detailView
                };

            CurrentMainViewIndex = 0;
        }

        private void OnClick_Create(object sender, EventArgs e)
        {
            // MessageBox.Show("hora! it's worked");
            var addView = ListViews[1] as CustomerAdd;
            addView.DataContext = new CustomerViewModel();

            CurrentMainViewIndex = 1;
        }

        private void OnBackFrom_Create(object sender, EventArgs e)
        {
            var addView = sender as CustomerAdd;
            addView.DataContext = null;

            CurrentMainViewIndex = 0;
            mainVM.ReloadCurrentPage();
        }
        private void OnBackFrom_EditItem(object sender, EventArgs e)
        {
            var editView = sender as CustomerDetailView;
            editView.DataContext = null;

            CurrentMainViewIndex = 0;
            mainVM.ReloadCurrentPage();
        }

        private void AddView_SavedOK(object sender, EventArgs e)
        {
            OnBackFrom_Create(sender, e);
        }

        private void OnClick_EditItem(object sender, EventArgs e)
        {
            var rowVM = (sender as CustomerAllViewRow).ViewModel;

            var editView = ListViews[2] as CustomerDetailView;
            editView.DataContext = new CustomerDetailViewModel(rowVM.Model);

            editView.OnStartEdit(null, null);
            CurrentMainViewIndex = 2;
        }

        private void OnClick_DetailItem(object sender, EventArgs e)
        {
            var rowVM = (sender as CustomerAllViewRow).ViewModel;

            var editView = ListViews[2] as CustomerDetailView;
            editView.DataContext = new CustomerDetailViewModel(rowVM.Model);

            CurrentMainViewIndex = 2;
        }

        public void Validation()
        {
            mainVM.SearchAsync();
        }
    }
}
