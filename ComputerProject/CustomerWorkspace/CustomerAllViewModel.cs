using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerAllViewModel : BusyViewModel
    {
        public CustomerAllViewModel()
        {
            customerList = new List<CustomerViewModel>();
            this.PropertyChanged += CustomerAllViewModel_PropertyChanged;
        }

        public CustomerAllViewModel(IList<CustomerViewModel> list) : this()
        {
            customerList = new List<CustomerViewModel>(list);
        }

        CancellationTokenSource countOperation = new CancellationTokenSource();
        CancellationTokenSource searchOperation = new CancellationTokenSource();

        private void CustomerAllViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchContent))
            {
                countOperation.Cancel();
                countOperation = new CancellationTokenSource();

                DoBusyTask(CountMaxPage, countOperation.Token, () => { CurrentPage = 1; });
            }

            if (e.PropertyName == nameof(CustomerList))
            {
                Console.WriteLine("List changed");
            }

            if (e.PropertyName == nameof(CurrentPage))
            {
                //Console.WriteLine("Page = " + CurrentPage);
                searchOperation.Cancel();
                searchOperation = new CancellationTokenSource();

                DoBusyTask(Search, searchOperation.Token);
            }
        }

        private List<CustomerViewModel> customerList;
        public List<CustomerViewModel> CustomerList { get => customerList; 
            set
            {
                customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }

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


        public int currentStartIndex = 0;
        public int step = 20;

        int maxPage = 1;
        public int MaxPage
        {
            get => maxPage;
            set
            {
                maxPage = value;
                OnPropertyChanged(nameof(MaxPage));
            }
        }

        public int CurrentPage
        {
            get
            {
                return currentStartIndex / step + 1;
            }

            set
            {
                if (value < 1 || value > MaxPage) return;
                currentStartIndex = (value - 1) * step;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public void Search()
        {
            var data = CustomerViewModel.FindByPhone(searchContent, currentStartIndex, step);

            if (!searchOperation.Token.IsCancellationRequested)
            {
                CustomerList = new List<CustomerViewModel>(data);
            }
        }

        public void CountMaxPage()
        {
            int max = CustomerViewModel.CountByPhone(searchContent);
            if (!countOperation.IsCancellationRequested)
            {
                MaxPage = max % step > 0 ? max / step + 1 : max / step;
            }
        }

        public void ReloadCurrentPage()
        {
            searchOperation.Cancel();
            searchOperation = new CancellationTokenSource();

            DoBusyTask(CountMaxPage, searchOperation.Token, () => {
                if (CurrentPage > MaxPage)
                {
                    CurrentPage = maxPage;
                }
                CurrentPage = CurrentPage;
            });
        }

        public void Delete(CustomerViewModel vm)
        {
            var task = new Action(vm.DeleteFromDB);

            void callback()
            {
                customerList.Remove(vm);
                ReloadCurrentPage();
            }

            DoBusyTask(task, callback);
        }

        public void OnPageChanged(object sender, EventArgs e)
        {
            
        }
    }
}
