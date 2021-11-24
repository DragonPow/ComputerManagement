using ComputerProject.BillWorkSpace;
using ComputerProject.CategoryWorkspace;
using ComputerProject.HelperService;
using ComputerProject.OverViewWorkSpace;
using ComputerProject.SaleWorkSpace;
using ComputerProject.SettingWorkSpace;
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
                    if (value != null)
                    {
                        RequestChangeTab?.Invoke(this, value.ViewName);
                    }
                    else RequestChangeTab?.Invoke(this, null);

                    _currentViewModel = value;
                    OnPropertyChanged();

                    _currentViewModel.LoadData();
                }
            }
        }

        /// <summary>
        /// Event occurs before tab change
        /// </summary>
        public event EventHandler<string> RequestChangeTab;

        public ApplicationViewModel()
        {
            var customerTab = new CustomerWorkspace.CustomerTabView();
            customerTab.BeginVM();

            var statisticTab = new StatiticsWorkSpace.StatiticsMainView();
            statisticTab.BeginVM();

            var prodTab = new ProductWorkSpace.ProductTabView();
            prodTab.BeginVM();

            SaleViewModel saleVM = new SaleViewModel();
            saleVM.RequestOpenDetailProductView += SaleVM_RequestOpenDetailProduct;

            TabViewModels = new ObservableCollection<ITabView>()
            {
                 new OverViewWorkSpace.OverViewMainView(),
                saleVM,
                prodTab,
                new CategoryViewModel(),
                new BillPage(),
                customerTab,
                statisticTab,
                new SettingViewModel()
            };
            CurrentViewModel = TabViewModels[0];
        }

        private void SaleVM_RequestOpenDetailProduct(object sender, RequestViewArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
