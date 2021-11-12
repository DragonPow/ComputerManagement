using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComputerProject.ProductWorkSpace
{
    class ProductMainViewModel: PagingViewModel<ProductViewModel>
    {
        MultipleControlViewModel viewController;

        public ProductMainViewModel():base()
        {
            PropertyChanged += ProductMainViewModel_PropertyChanged;
        }

        public ProductMainViewModel(MultipleControlViewModel navigation) : this()
        {
            viewController = navigation;
        }

        private void ProductMainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchContent))
            {
                CountPage();
            }

            if (e.PropertyName == nameof(CurrentPage))
            {
                Console.WriteLine("Page = " + CurrentPage);
                Search();
            }
        }


        public void Search()
        {
            Console.WriteLine("Search");
            searchOperation.Cancel();
            searchOperation = new CancellationTokenSource();

            List<ProductViewModel> l = null;
            void task()
            {
                l = ProductViewModel.FindByName(searchContent, currentStartIndex, step);
            }
            void callback()
            {
                ItemList = l;
            }
            DoBusyTask(task, searchOperation.Token, callback);
        }

        public void CountPage(bool resetPage = true)
        {
            Console.WriteLine("CountPage");
            countOperation.Cancel();
            countOperation = new CancellationTokenSource();

            int max = 0;
            void task()
            {
                max = ProductViewModel.CountByName(SearchContent);
            }
            void callback()
            {
                MaxPage = max % step > 0 ? max / step + 1 : max / step;
                if (resetPage)
                {
                    CurrentPage = 1;
                }
                else
                {
                    if (CurrentPage > MaxPage)
                    {
                        CurrentPage = maxPage;
                    }
                    CurrentPage = CurrentPage;
                }
            }

            DoBusyTask(task, countOperation.Token, callback);
        }

        public RelayCommand Clicked_Add => new RelayCommand((obj) => OpenAdd());

        public void OpenAdd()
        {
            var addVM = new ProductAddViewModel();
            addVM.Prepare();
            addVM.ClickBack += BackToMain;
            viewController.CurrentViewModel = addVM;
        }

        public void OnClick_Add(object sender, RoutedEventArgs e)
        {
            OpenAdd();
        }

        public void BackToMain(object sender, EventArgs e)
        {
            viewController.CurrentViewModel = this;
        }


        public RelayCommand CommandDetailItem => new RelayCommand(OnClick_DetailItem);
        public RelayCommand CommandEditItem => new RelayCommand(OnClick_EditItem);
        public RelayCommand CommandDeleteItem => new RelayCommand(OnClick_DeleteItem);

        public void OnClick_DetailItem(object sender)
        {
            var item = (ProductViewModel)(sender as Control).DataContext;

            if (item == null) return;

            var detailVM = new ProductDetailViewModel(item.Product);
            detailVM.Prepare();
            detailVM.ClickBack += BackToMain;
            detailVM.ClickEdit += (s, e) => CommandEditItem.Execute(item);

            viewController.CurrentViewModel = detailVM;
            Console.WriteLine("Edit item : " + item.Name);
        }

        public void OnClick_EditItem(object sender)
        {
            var item = (ProductViewModel)(sender as Control).DataContext;

            if (item == null) return;

            var editlVM = new ProductEditViewModel(item.Product);
            editlVM.Prepare();
            editlVM.ClickBack += BackToMain;

            viewController.CurrentViewModel = editlVM;
            Console.WriteLine("Edit item : " + item.Name);
        }

        private void DetailVM_ClickEdit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnClick_DeleteItem(object sender)
        {
            var item = (ProductViewModel)(sender as Control).DataContext;

            if (item == null) return;

            Console.WriteLine("Delete item : " + item.Name);
        }
    }
}
