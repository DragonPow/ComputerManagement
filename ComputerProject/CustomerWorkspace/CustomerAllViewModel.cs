using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerAllViewModel : BaseViewModel
    {
        private ObservableCollection<CustomerViewModel> customerList;
        public ObservableCollection<CustomerViewModel> CustomerList { get => customerList; 
            set
            {
                customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }

        public CustomerAllViewModel(IList<CustomerViewModel> list)
        {
            CustomerList = new ObservableCollection<CustomerViewModel>(list);
        }
    }
}
