using ComputerProject.Helper;
using ComputerProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.CategoryWorkspace
{
    public class ListCategoryViewModel : BaseViewModel
    {
        #region Fields
        NavigationService _navigator;
        CategoryRepository _repository;

        Collection<Model.Category> _categories;
        ICommand _openDetailCategoryCommand;
        ICommand _deleteParentCategoryCommand;
        ICommand _searchCommand;
        #endregion //Fields

        #region Propeties
        public Collection<Model.Category> CurrentCategories
        {
            get => _categories;
            set
            {
                if (value != _categories)
                {
                    VisibleCategories = _categories = value;
                }
                OnPropertyChanged();
            }
        }
        public Collection<Model.Category> VisibleCategories { get; private set; }

        public ICommand OpenDetailCategoryCommand
        {
            get
            {
                if (null == _openDetailCategoryCommand)
                {
                    _openDetailCategoryCommand = new RelayCommand(parentCategory => {
                        if (parentCategory is Model.Category) ShowDetail((Model.Category)parentCategory);
                        else throw new ArgumentException("Not is a Category");
                    });
                }
                return _openDetailCategoryCommand;
            }
        }
        public ICommand DeleteParentCategoryCommand
        {
            get
            {
                if (null == _deleteParentCategoryCommand)
                {
                    _deleteParentCategoryCommand = new RelayCommand(category =>
                    {
                        if (category is Model.Category) Delete((Model.Category)category);
                        else throw new ArgumentException("Not is a Category");
                    });
                }
                return _deleteParentCategoryCommand;
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                if (null == _searchCommand)
                {
                    _searchCommand = new RelayCommand(name => SearchCategory(name));
                }
                return _searchCommand;
            }
        }
        #endregion //Properties

        public ListCategoryViewModel(NavigationService baseNavigator = null)
        {
            _navigator = baseNavigator;
            _repository = new CategoryRepository();
        }


        public async void LoadAsyncCategories(string name = null)
        {
            var task = Task.Run(() =>
            {
                CurrentCategories = _repository.LoadCategories(name);
            });
            await task;
        }

        private void ShowDetail(Model.Category parentCategory)
        {
            var newPage = new DetailCategoryViewModel(_navigator);
            newPage.LoadData(parentCategory);
            _navigator.Back = () => _navigator?.NavigateTo(this);
            _navigator?.NavigateTo(newPage);
        }

        private void Delete(Model.Category category)
        {
            CurrentCategories.Remove(category);
            Task.Run(()=>_repository.Delete(category.Id));
        }

        private void SearchCategory(object name)
        {
            VisibleCategories = (Collection<Model.Category>)CurrentCategories.Where(c => c.Name == name);
            //OnPropertyChanged(nameof(VisibleCategories));
        }
    }
}
