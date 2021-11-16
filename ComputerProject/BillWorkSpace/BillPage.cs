using ComputerProject.ApplicationWorkspace;
using ComputerProject.HelperService;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.BillWorkSpace
{
    public class BillPage : BaseViewModel, ITabView, INavigationBar
    {
        public string ViewName => "Hóa đơn";
        public PackIconKind ViewIcon => PackIconKind.ClipboardTextSearch;

        public BaseViewModel CurrentPage => _navigator?.CurrentPage;
        private NavigationService _navigator;

        public BillPage()
        {
            var initView = new HistoryBillViewModel();
            initView.LoadBillsAsync();

            _navigator = new NavigationService(initView);
            initView.SetNavigator(_navigator);
            _navigator.OnCurrentPageChangedEvent += OnCurrentPageChanged;
        }

        private void OnCurrentPageChanged()
        {
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
}
