using ComputerProject.CategoryWorkspace;
using ComputerProject.HelperService;
using ComputerProject.SaleWorkSpace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ApplicationWorkspace
{
    public class RequestViewArgs : EventArgs
    {
        public object Value;
        public BaseViewModel TypeViewModel;
        public RequestViewArgs(BaseViewModel type, object value)
        {
            TypeViewModel = type;
            Value = value;
        }
    }
    public class ApplicationViewModel : BaseViewModel
    {
        private ObservableCollection<ITabView> _tabViewModels;
        private ITabView _currentViewModel;

        public ObservableCollection<ITabView> TabViewModels
        {
            get => _tabViewModels;
            set
            {
                if (value != _tabViewModels)
                {
                    _tabViewModels = value;
                    OnPropertyChanged();
                }
            }
        }
        public ITabView CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ApplicationViewModel()
        {
            var customerTab = new CustomerWorkspace.CustomerTabView(true);
            SaleViewModel saleVM = new SaleViewModel();
            saleVM.RequestOpenDetailProductView += SaleVM_RequestOpenDetailProduct;

            TabViewModels = new ObservableCollection<ITabView>()
            {
                saleVM,
                new CategoryViewModel(),
                customerTab,
            };
            CurrentViewModel = TabViewModels[0];
        }

        private void SaleVM_RequestOpenDetailProduct(object sender, RequestViewArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
