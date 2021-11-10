using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerProject.ProductWorkSpace
{
    class ProductMainViewModel: BusyViewModel
    {
        MultipleControlViewModel viewController;

        public ProductMainViewModel()
        {
        }
        public ProductMainViewModel(MultipleControlViewModel navigation)
        {
            viewController = navigation;
        }

        public RelayCommand Clicked_Add => new RelayCommand((obj) => OpenAdd());
        public RelayCommand Clicked_Detail => new RelayCommand((obj) => OpenDetail());

        public void OpenAdd()
        {
            var addVM = new ProductAddViewModel();
            viewController.CurrentViewModel = addVM;
        }

        public void OpenDetail()
        {
            var detailVM = new ProductDetailViewModel();
            viewController.CurrentViewModel = detailVM;
        }

        public void OnClick_Add(object sender, RoutedEventArgs e)
        {
            OpenAdd();
        }
    }
}
