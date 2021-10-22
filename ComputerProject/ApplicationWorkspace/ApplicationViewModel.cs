using ComputerProject.CategoryWorkspace;
using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ApplicationWorkspace
{
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
            TabViewModels = new ObservableCollection<ITabView>()
            {
                new CategoryViewModel(),
            };
            CurrentViewModel = TabViewModels[0];
        }
    }
}
