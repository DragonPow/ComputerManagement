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
    public class CustomerAllViewModel : PagingViewModel<CustomerViewModel>
    {
        public CustomerAllViewModel()
        {
            ItemList = new List<CustomerViewModel>();
            this.PropertyChanged += CustomerAllViewModel_PropertyChanged;
        }

        public CustomerAllViewModel(IList<CustomerViewModel> list) : this()
        {
            ItemList = new List<CustomerViewModel>(list);
        }

        private void CustomerAllViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            /*if (e.PropertyName == nameof(SearchContent))
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
                SearchAsync();
            }*/

            if (e.PropertyName == nameof(SearchContent))
            {
                Validation();
            }

            if (e.PropertyName == nameof(CurrentPage))
            {
                Console.WriteLine("Page = " + CurrentPage);
                Search();
            }
        }

        protected override List<CustomerViewModel> _search()
        {
            return CustomerViewModel.FindByPhone(searchContent, currentStartIndex, step);
        }

        protected override int _countMax()
        {
            return CustomerViewModel.CountByPhone(searchContent);
        }

        public void Delete(CustomerViewModel vm)
        {
            var task = new Action(vm.DeleteFromDB);

            void callback()
            {
                CustomMessageBox.MessageBox.ShowNotify("Xóa khách hàng thành công");
                Validation();
            }

            DoBusyTask(task, callback);
        }

        public void OnPageChanged(object sender, EventArgs e)
        {
            
        }
    }
}
