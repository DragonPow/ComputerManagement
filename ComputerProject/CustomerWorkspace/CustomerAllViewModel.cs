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
        public CustomerAllViewModel(IList<CustomerViewModel> list)
        {
            customerList = new ObservableCollection<CustomerViewModel>(list);
        }

        private ObservableCollection<CustomerViewModel> customerList;
        public ObservableCollection<CustomerViewModel> CustomerList { get => customerList; 
            set
            {
                customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }

        public int currentStartIndex = 0;
        public int step = 5;

        public String searchContent = "";
        public String SearchContent
        {
            get => searchContent;
            set
            {
                if (value == null) { value = ""; }

                if (searchContent.Equals(value)) return;

                searchContent = value;

                OnPropertyChanged(nameof(SearchContent));
            }
        }

        int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                if (currentPage == value || currentPage < 1) return;

                currentPage = value;

                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private System.Windows.Visibility busyVisibility = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility BusyVisibility
        {
            get => busyVisibility;
            set
            {
                busyVisibility = value;
                OnPropertyChanged(nameof(BusyVisibility));
            }
        }

        public async Task SearchAsycn(System.Threading.CancellationToken cancellationToken)
        {
            var data = await CustomerViewModel.FindByPhoneAsync(searchContent, currentStartIndex, step, cancellationToken);
            CustomerList = new ObservableCollection<CustomerViewModel>(data);
        }
    }
}
