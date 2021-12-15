using ComputerProject.HelperService;
using ComputerProject.SaleWorkSpace;
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
        int orderMode = 0;
        MultipleControlViewModel viewController;
        
        private FilterProductViewModel _currentFilter;
        public FilterProductViewModel CurrentFilter
        {
            get => _currentFilter;
            set
            {
                if (value != _currentFilter)
                {
                    _currentFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        private void LoadFilterControl()
        {
            CurrentFilter = new FilterProductViewModel(null, true);
            CurrentFilter.FilterClickedEvent += new EventHandler((o, e) =>
            {
                //Console.WriteLine("Filter clicked submit");
                Validation();
            });
            CurrentFilter.UndoFilterEvent += new EventHandler((o, e) =>
            {
                //Console.WriteLine("Filter clicked reset");
                Validation();
            });
        }


        public ProductMainViewModel():base()
        {
            PropertyChanged += ProductMainViewModel_PropertyChanged;
            step = 20;
            LoadFilterControl();
        }

        public ProductMainViewModel(MultipleControlViewModel navigation) : this()
        {
            viewController = navigation;
            orderMode = 0;
        }

        private void ProductMainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
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


        protected override List<ProductViewModel> _search()
        {
            return ProductViewModel.FindByNameOrID(SearchContent, currentStartIndex, step, orderMode, CurrentFilter);
        }

        protected override int _countMax()
        {
            return ProductViewModel.CountByNameOrId(SearchContent);
        }

        public RelayCommand Clicked_Add => new RelayCommand((obj) => OpenAdd());

        public void OpenAdd()
        {
            var addVM = new ProductAddViewModel();
            addVM.Prepare();
            addVM.ClickBack += BackToMain;
            addVM.InsertOK += BackAndRefresh;
            viewController.CurrentViewModel = addVM;
        }

        public void BackToMain(object sender, EventArgs e)
        {
            viewController.CurrentViewModel = this;
        }
        public void BackAndRefresh(object sender, EventArgs e)
        {
            viewController.CurrentViewModel = this;
            CalculatePage(false);
        }


        public RelayCommand CommandDetailItem => new RelayCommand(OnClick_DetailItem);
        public RelayCommand CommandEditItem => new RelayCommand(OnClick_EditItem);
        public RelayCommand CommandDeleteItem => new RelayCommand(OnClick_DeleteItem);
        public RelayCommand CommandSortPrice => new RelayCommand(OnClick_ButtonPrice);

        public void OnClick_DetailItem(object sender)
        {
            var item = (ProductViewModel)(sender as Control).DataContext;

            if (item == null || item.IsStopSelling) return;

            var detailVM = new ProductDetailViewModel(item.Product);
            detailVM.Prepare();
            detailVM.ClickBack += BackToMain;
            detailVM.ClickEdit += (s, e) => CommandEditItem.Execute(sender);
            detailVM.DeleteOK += BackAndRefresh;

            viewController.CurrentViewModel = detailVM;
            Console.WriteLine("Detail item : " + item.Name);
        }

        public void OnClick_EditItem(object sender)
        {
            var item = (ProductViewModel)(sender as Control).DataContext;

            if (item == null || item.IsStopSelling) return;

            var editlVM = new ProductEditViewModel(item.Product);
            editlVM.Prepare();
            editlVM.ClickBack += BackToMain;
            editlVM.UpdateOK += (s,e)=>
            {
                CommandDetailItem.Execute(sender);
            };

            viewController.CurrentViewModel = editlVM;
            Console.WriteLine("Edit item : " + item.Name);
        }

        public void OnClick_DeleteItem(object sender)
        {
            var item = (ProductViewModel)(sender as Control).DataContext;

            if (item == null || item.IsStopSelling) return;

            Console.WriteLine("Delete item : " + item.Name);

            bool hasInBill = false;
            void task1()
            {
                hasInBill = ProductViewModel.HasInBill(item.Id);
            }
            void task2()
            {
                string msg = hasInBill ? "Sản phẩm đã được bán trước đó. Sẽ tiến hành ngừng bán sản phẩm." : "Dữ liệu đã xóa sẽ không thể hoàn tác.";
                var msb = new CustomMessageBox.MessageBox(msg, "Xóa sản phẩm", "Tôi hiểu", "Hủy", MaterialDesignThemes.Wpf.PackIconKind.Warning
                , () =>
                {
                    DoBusyTask(() => item.DeleteFromDB(hasInBill), callback);
                });
                msb.ShowDialog();
            }

            void callback()
            {
                BackAndRefresh(null, null);
            }

            DoBusyTask(task1, task2);
        }

        public void OnClick_ButtonPrice(object obj)
        {
            orderMode = orderMode == 0 ? 1 : 0;
            CalculatePage(false);
        }
    }
}
