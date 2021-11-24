using ComputerProject.CustomMessageBox;
using ComputerProject.HelperService;
using ComputerProject.Repository;
using MaterialDesignThemes.Wpf;
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
        public BusyViewModel BusyService { get; private set; } = new BusyViewModel();

        Collection<Model.Category> _categories;
        Collection<Model.Category> _visibleCategories;
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
                    OnPropertyChanged();
                }
            }
        }
        public Collection<Model.Category> VisibleCategories
        {
            get => _visibleCategories;
            set
            {
                if (value != _visibleCategories)
                {
                    _visibleCategories = value;
                    OnPropertyChanged();
                }
            }
        }
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
                        var rs = MessageBoxCustom.ShowDialog("Chắc chắn xóa danh mục này hay không?", "Thông báo", PackIconKind.QuestionAnswer);
                        if (rs == MessageBoxResultCustom.Yes)
                        {
                            Delete((Model.Category)category);
                            MessageBoxCustom.ShowDialog("Xóa thành công", "Thông báo", PackIconKind.DoneOutline);
                        }
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

        public void setNavigator(NavigationService baseNavigator)
        {
            _navigator = baseNavigator;
        }


        public void LoadAsyncCategories(string name = null)
        {
            BusyService.DoBusyTask(() =>
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

            newPage.LoadAsyncData(parentCategory);
            newPage.DetailCategoryChangedEventHandler += OnDetailCategoryChanged;
            newPage.DeleteCategoryChangedEventHandler += OnDeleteCategoryChanged;

            //Set navigator
            if (_navigator != null) _navigator.Back = () => _navigator?.NavigateTo(this);
            _navigator?.NavigateTo(newPage);
        }

        private void OnDeleteCategoryChanged(object sender, Model.Category e)
        {
            //if (ContainRootCategory(e.Name))
            //{
            //    var category = CurrentCategories.Where(i => i.Name == e.Name).First();
            //    CurrentCategories.Remove(category);
            //}
            Delete(e);
        }

        private void OnDetailCategoryChanged(object sender, Model.Category e)
        {
            if (!ContainRootCategory(e.Name))
            {
                CurrentCategories.Add(e);
                //VisibleCategories = CurrentCategories;
            }
        }

        private bool ContainRootCategory(string name)
        {
            return CurrentCategories.Any(i=>i.Name == name);
        }

        private void Delete(Model.Category category)
        {
            CurrentCategories.Remove(category);
            VisibleCategories = CurrentCategories;
            //Task.Run(() => _repository.Delete(category.Id));
        }

        private void SearchCategory(string name)
        {
            string name_converted = FormatHelper.ConvertTo_TiengDongLao(name).ToLower().Trim();
            VisibleCategories =  new ObservableCollection<Model.Category>(CurrentCategories.Where(c => FormatHelper.ConvertTo_TiengDongLao(c.Name).ToLower().Trim().Contains(name_converted)));
            //OnPropertyChanged(nameof(VisibleCategories));
        }
    }
}
