﻿using ComputerProject.BillWorkSpace;
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
                    if (_currentViewModel != null && !_currentViewModel.AllowChangeTab()) return;

                    _currentViewModel = value;
                    OnPropertyChanged();

                    _currentViewModel.LoadDataAsync();
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

            var statisticTab = new StatiticsWorkSpace.StatiticsMainView();

            var prodTab = new ProductWorkSpace.ProductTabView();

            SaleViewModel saleVM = new SaleViewModel();
            saleVM.RequestOpenDetailProductView += SaleVM_RequestOpenDetailProduct;

            TabViewModels = new ObservableCollection<ITabView>()
            {
                 new OverViewWorkSpace.OverViewMainView(),
                saleVM,
                new InsuranceWorkSpace.InsuranceTabView(),
                prodTab,
                new CategoryViewModel(),
                new BillPage(),
                 
                customerTab,
                statisticTab,
                new SettingViewModel(),
                
            };
            CurrentViewModel = TabViewModels[0];
        }

        private void SaleVM_RequestOpenDetailProduct(object sender, RequestViewArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
