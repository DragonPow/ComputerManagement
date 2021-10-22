using ComputerProject.CustomMessageBox;
using ComputerProject.Helper;
//using ComputerProject.Model;
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
    public interface IMemento
    {
        void StoreState();
        void RestoreState();
    }
    public interface IDetailCategoryMemento
    {
        Collection<Model.Category> ChildCategories { get; }
        Collection<Model.Specification_type> Specifications { get; }
    }
    public interface IBaseMemento
    {
        void ClearState();
    }
    public class DetailCategoryMemento : IDetailCategoryMemento, IBaseMemento
    {
        public Collection<Model.Category> ChildCategories { get; set; } = null;
        public Collection<Model.Specification_type> Specifications { get; set; } = null;
        public DetailCategoryMemento(Collection<Model.Category> childCategories, Collection<Model.Specification_type> specifications)
        {
            this.ChildCategories = new Collection<Model.Category>(childCategories);
            this.Specifications = new Collection<Model.Specification_type>(specifications);
        }
        public void ClearState()
        {
            ChildCategories.Clear();
            Specifications.Clear();

            ChildCategories = null;
            Specifications = null;
        }
    }

    public class DetailCategoryViewModel : BaseViewModel
    {
        #region Fields
        NavigationService _navigator;
        CategoryRepository _repository;

        Model.Category _currentParentCategory;
        Model.Category _currentChildCateogry;
        bool _isEditMode;

        ICommand _addSpecificationCommand;
        ICommand _deleteSpecificationCommand;
        ICommand _addChildCategoryCommand;
        ICommand _deleteCategoryCommand;
        ICommand _saveEditCommand;
        ICommand _openEditCommand;
        ICommand _cancelEditCommand;
        ICommand _backPageCommand;
        #endregion //Fields

        #region Properties
        public Model.Category CurrentParentCategory
        {
            get => _currentParentCategory;
            set
            {
                if (value != _currentParentCategory)
                {
                    _currentParentCategory = value;
                }
                OnPropertyChanged();
                //OnPropertyChanged(nameof(ChildCategories));
            }
        }
        public Model.Category CurrentChildCategory
        {
            get => _currentChildCateogry;
            set
            {
                if (value != _currentChildCateogry)
                {
                    _currentChildCateogry = value;
                }
                OnPropertyChanged();
                //OnPropertyChanged(nameof(SpecificationTypes));
            }
        }
        //public Collection<Model.Category> ChildCategories
        //{
        //    get => (Collection<Model.Category>)CurrentParentCategory.ChildCategories;
        //    //set
        //    //{
        //    //    var categories = (Collection<Model.Category>)CurrentParentCategory.ChildCategories;
        //    //    if (value != categories)
        //    //    {
        //    //        CurrentParentCategory.ChildCategories = value;
        //    //    }
        //    //    OnPropertyChanged();
        //    //}
        //}
        //public Collection<Model.Specification_type> SpecificationTypes
        //{
        //    get => CurrentChildCategory != null ? (Collection<Model.Specification_type>)CurrentChildCategory.SpecificationTypes : null;
        //    //set
        //    //{
        //    //    if (value != _specifications)
        //    //    {
        //    //        _specifications = value;
        //    //    }
        //    //    OnPropertyChanged();
        //    //}
        //}
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (value != _isEditMode)
                {
                    _isEditMode = value;
                }
                OnPropertyChanged();
            }
        }

        public ICommand AddSpecificationCommand
        {
            get
            {
                if (_addSpecificationCommand == null)
                {
                    _addSpecificationCommand = new RelayCommand(a => AddSpecification(), b => IsEditMode);
                }
                return _addSpecificationCommand;
            }
        }
        public ICommand DeleteSpecificationCommand
        {
            get
            {
                if (_deleteSpecificationCommand == null)
                {
                    _deleteSpecificationCommand = new RelayCommand(a => DeleteSpecificationType((Model.Specification_type)a), b => IsEditMode);
                }
                return _deleteSpecificationCommand;
            }
        }
        public ICommand AddChildCategoryCommand
        {
            get
            {
                if (_addChildCategoryCommand == null)
                {
                    _addChildCategoryCommand = new RelayCommand(a => AddChildCategory());
                }
                return _addChildCategoryCommand;
            }
        }
        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (_deleteCategoryCommand == null)
                {
                    _deleteCategoryCommand = new RelayCommand(category => Delete((Model.Category)category));
                }
                return _deleteCategoryCommand;
            }
        }
        public ICommand SaveEditCommand
        {
            get
            {
                if (_saveEditCommand == null)
                {
                    _saveEditCommand = new RelayCommand(a =>
                    {
                        try
                        {
                            Save();
                        }
                        catch (ArgumentException e)
                        {
                            MessageBoxCustom.ShowDialog("Tên danh mục đã tồn tại", "Thông báo");
                        }
                    });
                }
                return _saveEditCommand;
            }
        }
        public ICommand OpenEditCommand
        {
            get
            {
                if (null == _openEditCommand)
                {
                    _openEditCommand = new RelayCommand(a => OpenEditMode());
                }
                return _openEditCommand;
            }
        }
        public ICommand CancelEditCommand
        {
            get
            {
                if (_cancelEditCommand == null)
                {
                    _cancelEditCommand = new RelayCommand(a => Cancel());
                }
                return _cancelEditCommand;
            }
        }
        public ICommand BackPageCommand
        {
            get
            {
                if (null==_backPageCommand)
                {
                    _backPageCommand = new RelayCommand(a => NavigateBackPage());
                }
                return _backPageCommand;
            }
        }
        #endregion //Properties


        public DetailCategoryViewModel(NavigationService baseNavigator = null)
        {
            _navigator = baseNavigator;
            _repository = new CategoryRepository();
        }

        public void LoadData(Model.Category parentCategory = null)
        {
            CurrentChildCategory = null;

            if (parentCategory == null)
            {
                CurrentParentCategory = new Model.Category();
                //ChildCategories = null;
                IsEditMode = true;
            }
            else
            {
                CurrentParentCategory = parentCategory;
                //ChildCategories = (Collection<Model.Category>)parentCategory.ChildCategories;
                IsEditMode = false;
            }
        }

        public void ReloadData()
        {
            if (CurrentParentCategory.Id == 0)
            {
                CurrentParentCategory.ChildCategories.Clear();
            }
            else
            {
                CurrentParentCategory.ChildCategories.Clear();
                CurrentParentCategory.ChildCategories = _repository.LoadChildCategories(CurrentParentCategory.Id);
            }
        }

        public void AddChildCategory()
        {
            CurrentChildCategory = new Model.Category();
            CurrentParentCategory.ChildCategories.Add(CurrentChildCategory);
        }

        public void AddSpecification()
        {
            CurrentChildCategory.SpecificationTypes.Add(new Model.Specification_type());
        }

        public void DeleteSpecificationType(Model.Specification_type specification)
        {
            CurrentChildCategory.SpecificationTypes.Remove(specification);
        }

        public void Delete(Model.Category category)
        {
            if (category == CurrentParentCategory)
            {
                _repository.Delete(category.Id);
                NavigateBackPage();
            }
            else
            {
                category.SpecificationTypes.Clear();
                category.SpecificationTypes = null;
                CurrentParentCategory.ChildCategories.Remove(category);
            }
        }

        public void Save()
        {
            if (isParentCategoryExists())
            {
                throw new ArgumentException("Parent Category Exsists");
            }
            else
            {
                Task.Run(() => _repository?.Save(CurrentParentCategory));
            }
        }
        private bool isParentCategoryExists()
        {
            return _repository.IsRootCategoryExists(this.CurrentParentCategory.Name);
        }

        public void OpenEditMode()
        {
            IsEditMode = true;
        }

        public void Cancel()
        {
            ReloadData();
        }

        private void NavigateBackPage()
        {
            _navigator?.Back();
        }
    }
}
