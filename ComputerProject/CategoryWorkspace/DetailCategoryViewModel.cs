using ComputerProject.ApplicationWorkspace;
using ComputerProject.CustomMessageBox;
using ComputerProject.HelperService;
//using ComputerProject.Model;
using ComputerProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using ComputerProject.Model;

namespace ComputerProject.CategoryWorkspace
{
    public class DetailCategoryViewModel : BaseViewModel
    {
        #region Fields
        NavigationService _navigator;
        CategoryRepository _repository;
        public BusyViewModel BusyService { get; private set; } = new BusyViewModel();
        public string TitleViewName { get; private set; }

        static readonly string DUPLICATE_CATEGORY_NAME = "At least two item have same name";
        static readonly string DUPLICATE_SPECIFICATION_NAME = "At least two spec in one category have same name";

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

        public bool HasErrorData => CurrentParentCategory.HasErrorData;
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
                    OnPropertyChanged();
                }
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
                    OnPropertyChanged();
                }
            }
        }
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (value != _isEditMode)
                {
                    _isEditMode = value;
                    OnPropertyChanged();
                }
            }
        }
        public event EventHandler<Model.Category> DetailCategoryChangedEventHandler;

        public ICommand AddSpecificationCommand
        {
            get
            {
                if (_addSpecificationCommand == null)
                {
                    _addSpecificationCommand = new RelayCommand(a => AddSpecification(CurrentChildCategory), b => IsEditMode && CurrentChildCategory != null);
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
                    _deleteCategoryCommand = new RelayCommand(category =>
                    {
                        var rs = MessageBoxCustom.ShowDialog("Chắc chắn xóa danh mục này hay không?", "Thông báo", PackIconKind.QuestionAnswer);
                        if (rs == MessageBoxResultCustom.Yes)
                        {
                            Delete((Model.Category)category);
                        }
                    }, category => IsEditMode || category == CurrentParentCategory);
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
                        if (String.IsNullOrWhiteSpace(CurrentParentCategory.Name))
                        {
                            MessageBoxCustom.ShowDialog("Tên danh mục không được để trống", "Lỗi", PackIconKind.ErrorOutline);
                        }
                        else if (CurrentParentCategory.ChildCategories != null && CurrentParentCategory.ChildCategories.Any(i => i.HasErrorData))
                        {
                            MessageBoxCustom.ShowDialog("Phải điền đầy đủ thông tin danh mục con", "Thông báo", PackIconKind.InformationCircle);
                        }
                        else
                        {
                            try
                            {
                                Save();
                            }
                            catch (ArgumentException)
                            {
                                MessageBoxCustom.ShowDialog("Tên danh mục đã tồn tại", "Lỗi", PackIconKind.ErrorOutline);
                            }
                            catch (InvalidOperationException e)
                            {
                                if (e.Message == DUPLICATE_CATEGORY_NAME)
                                {
                                    MessageBoxCustom.ShowDialog("Tên danh mục con không được trùng nhau", "Lỗi", PackIconKind.ErrorOutline);
                                }
                                else if (e.Message == DUPLICATE_SPECIFICATION_NAME)
                                {
                                    MessageBoxCustom.ShowDialog("Tên thông số trong cùng 1 danh mục không được trùng nhau", "Lỗi", PackIconKind.ErrorOutline);
                                }
                                
                            }
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
                if (null == _backPageCommand)
                {
                    _backPageCommand = new RelayCommand(a =>
                    {
                        try
                        {
                            if (IsEditMode)
                            {
                                throw new ArgumentException("Is not save");
                            }
                            NavigateBackPage();
                        }
                        catch (ArgumentException e)
                        {
                            var rs = MessageBoxCustom.ShowDialog("Thay đổi chưa được lưu, đồng ý thoát?", "Thông báo");
                            if (rs == MessageBoxResultCustom.Yes)
                            {
                                NavigateBackPage();
                            }
                        }
                    });
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
                TitleViewName = "AddCategory";
                CurrentParentCategory = new Model.Category();
                CurrentParentCategory.ChildCategories = new ObservableCollection<Model.Category>();
                CurrentParentCategory.SpecificationTypes = new ObservableCollection<Model.Specification_type>();
            }
            else
            {
                TitleViewName = "DetailCategory";
                CurrentParentCategory = parentCategory;
                CurrentParentCategory.ChildCategories = _repository.LoadChildCategories(CurrentParentCategory.Id);
                //Task.Run(() => _repository.LoadSpecification(parentCategory));
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
            CurrentChildCategory = new Model.Category() { Name = "Danh mục mới" };
            CurrentParentCategory.ChildCategories.Add(CurrentChildCategory);
        }

        public void AddSpecification(Model.Category category)
        {
            Model.Specification_type specification = new Model.Specification_type();

            if (category?.SpecificationTypes == null) specification.Number = 1;
            else specification.Number = (int)CurrentChildCategory?.SpecificationTypes.Count() + 1;

            if (CurrentChildCategory.SpecificationTypes == null)
                CurrentChildCategory.SpecificationTypes = new ObservableCollection<Specification_type>();
            CurrentChildCategory.SpecificationTypes.Add(specification);

        }

        public void DeleteSpecificationType(Model.Specification_type specification)
        {
            CurrentChildCategory.SpecificationTypes.Remove(specification);
        }

        public void Delete(Model.Category category)
        {
            if (category == CurrentParentCategory)
            {
                if (category.Id != 0) _repository.Delete(category.Id);
                NavigateBackPage();
            }
            else
            {
                CurrentParentCategory.ChildCategories.Remove(category);
            }
        }

        public void Save()
        {
            if (isParentCategoryExists())
            {
                throw new ArgumentException("Root Category Exsists");
            }
            if (CurrentParentCategory.ChildCategories != null)
            {
                //Check if 2 category have same name
                if (CurrentParentCategory.ChildCategories.Select(i => i.Name.Trim()).Distinct().Count() < CurrentParentCategory.ChildCategories.Count())
                {
                    throw new InvalidOperationException(DUPLICATE_CATEGORY_NAME);
                }

                foreach(var childCategory in CurrentChildCategory.ChildCategories)
                {
                    if (childCategory.SpecificationTypes.Select(i=>i.Name.Trim()).Distinct().Count() < childCategory.SpecificationTypes.Count())
                    {
                        throw new InvalidCastException(DUPLICATE_SPECIFICATION_NAME);
                    }
                }
            }

            BusyService.DoBusyTask(() => _repository.Save(CurrentParentCategory), () =>
            {
                MessageBoxCustom.ShowDialog("Lưu thành công", "Thông báo", PackIconKind.InformationCircle);
                NavigateBackPage();
            });

            IsEditMode = false;
            CurrentChildCategory = null;

            DetailCategoryChangedEventHandler?.Invoke(this, this.CurrentParentCategory);
        }
        private bool isParentCategoryExists()
        {
            return _repository.IsRootCategoryExists(this.CurrentParentCategory);
        }

        public void OpenEditMode()
        {
            IsEditMode = true;
        }

        public void Cancel()
        {
            ReloadData();
            IsEditMode = false;
        }

        private void NavigateBackPage()
        {
            foreach (var child in CurrentParentCategory.ChildCategories)
            {
                child.SpecificationTypes = null;
            }
            _navigator?.Back();
        }
    }
}
