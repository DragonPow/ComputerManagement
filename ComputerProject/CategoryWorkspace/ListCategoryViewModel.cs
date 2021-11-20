using ComputerProject.HelperService;
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
        bool _openWithEditMode;

        ICommand _openDetailCategoryCommand;
        ICommand _deleteCategoryCommand;
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
        public bool OpenDetailCategoryWithEditMode
        {
            get => _openWithEditMode;
            set
            {
                if (value != _openWithEditMode)
                {
                    _openWithEditMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenDetailCategoryCommand
        {
            get
            {
                if (null == _openDetailCategoryCommand)
                {
                    _openDetailCategoryCommand = new RelayCommand(parameter =>
                    {
                        var values = (object[])parameter;
                        ShowDetail((Model.Category)values[0], (bool)values[1]);
                    });
                }
                return _openDetailCategoryCommand;
            }
        }
        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (null == _deleteCategoryCommand)
                {
                    _deleteCategoryCommand = new RelayCommand(category =>
                    {
                        if (category is Model.Category) Delete((Model.Category)category);
                        else throw new ArgumentException("Not is a Category");
                    });
                }
                return _deleteCategoryCommand;
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                if (null == _searchCommand)
                {
                    _searchCommand = new RelayCommand(name => SearchCategory((string)name));
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

        public void setNaigator(NavigationService baseNavigator)
        {
            _navigator = baseNavigator;
        }


        public void LoadAsyncCategories(string name = null)
        {
            var task = Task.Run(() =>
            {
                CurrentCategories = _repository.LoadCategories(name);
            });
        }

        private void ShowDetail(Model.Category parentCategory, bool isEditMode)
        {
            var newPage = new DetailCategoryViewModel(_navigator);

            //Init Data
            if (parentCategory == null) newPage.IsEditMode = true;
            else newPage.IsEditMode = isEditMode;

            newPage.LoadData(parentCategory);
            newPage.DetailCategoryChangedEventHandler += OnDetailCategoryChanged;

            //Set navigator
            if (_navigator != null) _navigator.Back = () => _navigator?.NavigateTo(this);
            _navigator?.NavigateTo(newPage);
        }

        private void OnDetailCategoryChanged(object sender, Model.Category e)
        {
            if (!CurrentCategories.Contains(e))
            {
                CurrentCategories.Add(e);
            }
            OnPropertyChanged(nameof(CurrentCategories));
        }

        private void Delete(Model.Category category)
        {
            CurrentCategories.Remove(category);
            Task.Run(() => _repository.Delete(category.Id));
        }

        private void SearchCategory(string name)
        {
            VisibleCategories = (Collection<Model.Category>)CurrentCategories.Where(c => c.Name == name);
            //OnPropertyChanged(nameof(VisibleCategories));
        }
    }
}
