using ComputerProject.ApplicationWorkspace;
using ComputerProject.HelperService;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.CategoryWorkspace
{
    public class CategoryViewModel : BaseViewModel, INavigationBar, ITabView
    {
        readonly NavigationService _navigator;
        public BaseViewModel CurrentPage => _navigator.CurrentPage;
        public string ViewName => "Danh mục";
        public PackIconKind ViewIcon => PackIconKind.Ballot;

        public CategoryViewModel()
        {
            var mainPage = new ListCategoryViewModel();
            mainPage.LoadAsyncCategories();

            _navigator = new NavigationService(mainPage);
            _navigator.OnCurrentPageChangedEvent += OnCurrentPageChanged;

            mainPage.setNaigator(_navigator);
        }

        private void OnCurrentPageChanged()
        {
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
}
