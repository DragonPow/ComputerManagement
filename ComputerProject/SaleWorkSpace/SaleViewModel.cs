﻿using ComputerProject.ApplicationWorkspace;
using ComputerProject.CustomerWorkspace;
using ComputerProject.CustomMessageBox;
using ComputerProject.Helper;
using ComputerProject.HelperService;
using ComputerProject.Model;
using ComputerProject.ProductWorkSpace;
using ComputerProject.Repository;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ComputerProject.SaleWorkSpace
{
    public class SaleViewModel : HelperService.BaseViewModel, ITabView
    {
        #region Fields
        public string ViewName => "Bán hàng";
        public PackIconKind ViewIcon => PackIconKind.CashRegister;
        readonly SaleRepository _repository;

        Collection<Model.Product> _products;
        Collection<Model.Product> _visibleProduct;
        IDictionary<Model.Product, int> _productsInBill;
        Collection<Model.Category> _categories;
        Collection<CUSTOMER> _listSearchCustomer;
        CUSTOMER _currentCustomer;
        Model.Category _currentCategory;
        Model.Category _currentRootCategory;
        int _currentPoint;
        int _pointMoney = -1;
        IFilterProductState _currentFilter;
        bool _isPriceLowToHight;

        ICommand _paymentCommand;
        ICommand _addProductInBillCommand;
        ICommand _removeProductInBillCommand;
        ICommand _filterProductsCommand;
        ICommand _sortProductsCommand;
        ICommand _addCustomerCommand;
        ICommand _searchProductCommand;
        ICommand _searchCustomerCommand;
        ICommand _showDetailProductCommand;
        ICommand _clearCommand;
        #endregion //Fields


        #region Properties
        public int TotalPriceProduct => ProductsInBill == null ? 0 : ProductsInBill.Sum(p => p.Value * p.Key.PriceSale);
        public int TotalPriceBill
        {
            get
            {
                int value = TotalPriceProduct - CurrentPoint * PointMoney;

                if (value <= 0) return 0;
                return value;
            }
        }
        public int PointMoney
        {
            get
            {
                if (_pointMoney < 0)
                {
                    _pointMoney = _repository.GetPointToMoney();
                    OnPropertyChanged(nameof(TotalPriceBill));
                }
                return _pointMoney;
            }
        }
        public Collection<Product> VisibleProducts
        {
            get => _visibleProduct;
            set
            {
                if (value != _visibleProduct)
                {
                    _visibleProduct = value;
                    OnPropertyChanged();
                }
            }
        }
        public IDictionary<Product, int> ProductsInBill
        {
            get
            {
                if (null == _productsInBill)
                {
                    _productsInBill = new ObservableConcurrentDictionary<Product, int>();
                }
                return _productsInBill;
            }
        }
        public Collection<Model.Category> Categories
        {
            get => _categories;
            set
            {
                if (value != _categories)
                {
                    _categories = value;
                    OnPropertyChanged();
                }
            }
        }
        public CUSTOMER CurrentCustomer
        {
            get => _currentCustomer;
            set
            {
                if (value != _currentCustomer)
                {
                    _currentCustomer = value;
                    OnPropertyChanged();
                }
            }
        }
        public Collection<CUSTOMER> ListSearchCustomer
        {
            get => _listSearchCustomer;
            set
            {
                if (value != _listSearchCustomer)
                {
                    _listSearchCustomer = value;
                    OnPropertyChanged();
                }
            }
        }
        public Model.Category CurrentCategory
        {
            get => _currentCategory;
            set
            {
                if (value != _currentCategory)
                {
                    _currentCategory = value;
                    OnPropertyChanged();
                    OnCategoryChanged();
                }
            }
        }
        public Model.Category CurrentRootCategory
        {
            get => _currentRootCategory;
            set
            {
                if (value != _currentRootCategory)
                {
                    _currentRootCategory = value;
                    OnPropertyChanged();

                    _currentCategory = _currentRootCategory?.ChildCategories[0] ?? null;
                    OnPropertyChanged(nameof(CurrentCategory));
                    OnCategoryChanged();
                }
            }
        }
        public int CurrentPoint
        {
            get => _currentPoint;
            set
            {
                if (value != _currentPoint)
                {
                    _currentPoint = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPriceBill));
                }
            }
        }
        public IFilterProductState CurrentFilter
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
        public bool IsPriceLowToHight
        {
            get => _isPriceLowToHight;
            set
            {
                if (value != _isPriceLowToHight)
                {
                    _isPriceLowToHight = value;
                    if (_isPriceLowToHight)
                    {
                        VisibleProducts = new ObservableCollection<Product>(VisibleProducts?.OrderBy(i => i.PriceSale));
                    }
                    else
                    {
                        VisibleProducts = new ObservableCollection<Product>(VisibleProducts?.OrderByDescending(i => i.PriceSale));
                    }
                    OnPropertyChanged();
                }
            }
        }

        public ICommand PaymentCommand
        {
            get
            {
                if (null == _paymentCommand)
                {
                    _paymentCommand = new RelayCommand(a =>
                    {
                        if (ProductsInBill == null || ProductsInBill.Count == 0)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng chọn sản phẩm.", "Lỗi", PackIconKind.Error);
                        }
                        else if (CurrentCustomer == null)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng chọn khách hàng.", "Thông báo", PackIconKind.Information);
                        }
                        else OpenPaymentView(ProductsInBill, CurrentCustomer);
                    });
                }
                return _paymentCommand;
            }
        }
        public ICommand AddProductInBillCommand
        {
            get
            {
                if (null == _addProductInBillCommand)
                {
                    _addProductInBillCommand = new RelayCommand(product => AddToBill(product as Model.Product, 1));
                }
                return _addProductInBillCommand;
            }
        }
        public ICommand RemoveProductInBillCommand
        {
            get
            {
                if (null == _removeProductInBillCommand)
                {
                    _removeProductInBillCommand = new RelayCommand(product => RemoveToBill(product as Model.Product, 1));
                }
                return _removeProductInBillCommand;
            }
        }
        public ICommand FilterProductsCommand
        {
            get
            {
                if (null == _filterProductsCommand)
                {
                    _filterProductsCommand = new RelayCommand(a => OpenFilterControl());
                }
                return _filterProductsCommand;
            }
        }
        public ICommand SortProductsCommand
        {
            get
            {
                if (null == _sortProductsCommand)
                {
                    _sortProductsCommand = new RelayCommand(a => Sort());
                }
                return _sortProductsCommand;
            }
        }
        public ICommand AddCustomerCommand
        {
            get
            {
                if (null == _addCustomerCommand)
                {
                    _addCustomerCommand = new RelayCommand(a => OpenAddCustomerView());
                }
                return _addCustomerCommand;
            }
        }
        public ICommand SearchProductCommand
        {
            get
            {
                if (null == _searchProductCommand)
                {
                    _searchProductCommand = new RelayCommand(s => SearchProduct(s as string));
                }
                return _searchProductCommand;
            }
        }
        public ICommand SearchCustomerCommand
        {
            get
            {
                if (null == _searchCustomerCommand)
                {
                    _searchCustomerCommand = new RelayCommand(s => SearchCustomer(s as string));
                }
                return _searchCustomerCommand;
            }
        }
        public ICommand ShowDetailProductCommand
        {
            get
            {
                if (null == _showDetailProductCommand)
                {
                    _showDetailProductCommand = new RelayCommand(product => ShowDetail(product as Model.Product));
                }
                return _showDetailProductCommand;
            }
        }
        public ICommand ClearCommand
        {
            get
            {
                if (null == _clearCommand)
                {
                    _clearCommand = new RelayCommand(information => Clear(information));
                }
                return _clearCommand;
            }
        }
        public event EventHandler<RequestViewArgs> RequestOpenDetailProductView;
        public event EventHandler RequestAddNewCustomer;
        #endregion //Properties


        public SaleViewModel()
        {
            _repository = new SaleRepository();
            CurrentFilter = new FilterProductViewModel(CurrentFilter);
            CurrentFilter.FilterClickedEvent += new EventHandler((o, e) =>
            {
                VisibleProducts = _repository.SearchFilterProduct(CurrentFilter);
                if (CurrentFilter.CategoryType != null)
                {
                    CurrentCategory = Categories.Where(i => i.Name == CurrentFilter.CategoryType.Name).FirstOrDefault();
                }
            });

            LoadData();
        }
        public SaleViewModel(SaleRepository repository)
        {
            this._repository = repository;
        }

        public void LoadData()
        {
            var loadProductTask = Task.Run(() => VisibleProducts = _products = _repository.LoadProducts());

            Task.Run(() =>
            {
                Categories = _repository.LoadCategories();

                //Add "all" to UI for category tab
                var null_category = new Category();
                Categories.Insert(0, null_category);
                foreach (var category in Categories)
                {
                    if (category.ChildCategories == null) category.ChildCategories = new ObservableCollection<Model.Category>();
                    category.ChildCategories.Insert(0, null_category);
                }

                _currentRootCategory = Categories[0];
                _currentCategory = _currentRootCategory.ChildCategories[0];
                OnPropertyChanged(nameof(CurrentRootCategory));
                OnPropertyChanged(nameof(CurrentCategory));
            });
        }
        //private void SearchRootCategory()
        //{
        //    VisibleProducts = _products = _repository.LoadProducts(CurrentRootCategory);
        //}
        //private void SearchChildCategory()
        //{
        //    if (CurrentCategory != null)
        //    {
        //        VisibleProducts = new ObservableCollection<Model.Product>(_products.Where(i => i.CategoryProduct.Id == CurrentCategory.Id).AsEnumerable());
        //    }
        //    else
        //    {
        //        VisibleProducts = _products;
        //    }
        //}
        private void OnCategoryChanged()
        {
            if (_products == null) return;
            if (CurrentCategory != null && CurrentCategory.Name != null)
            {
                //Current tab category is child
                VisibleProducts = new ObservableCollection<Model.Product>(_products.Where(i => i.CategoryProduct.Id == CurrentCategory.Id));
            }
            else if (CurrentRootCategory != null)
            {
                //Current tab category is root
                if (CurrentRootCategory.Name != null)
                {
                    VisibleProducts = new ObservableCollection<Model.Product>(_products.Where(i => CurrentRootCategory.ChildCategories.Any(child => child.Id == i.CategoryProduct.Id)));
                }
                else
                {
                    //Show all products in store
                    VisibleProducts = _products;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        private void OpenPaymentView(IDictionary<Product, int> productsInBill, CUSTOMER currentCustomer)
        {
            var vm = new BillViewModel(productsInBill, currentCustomer, TotalPriceBill);
            vm.PaymentSuccessEvent += (s, e) => ClearPayment();

            WindowService.ShowWindow(vm, new PaySaleBillView());
        }

        private void ClearPayment()
        {
            Clear(CurrentCustomer);
            Clear(ProductsInBill);
        }

        private void AddToBill(Product product, int quantity)
        {
            if (ProductsInBill.ContainsKey(product))
            {
                ProductsInBill[product] += quantity;
            }
            else
            {
                ProductsInBill.Add(product, quantity);
            }
            OnPropertyChanged(nameof(TotalPriceProduct));
            OnPropertyChanged(nameof(TotalPriceBill));
        }
        private void RemoveToBill(Product product, int quantity)
        {
            if (ProductsInBill.ContainsKey(product))
            {
                if (ProductsInBill[product] > quantity)
                {
                    ProductsInBill[product] -= quantity;
                }
                else ProductsInBill.Remove(product);
            }
            else
            {
                throw new NullReferenceException();
            }
            OnPropertyChanged(nameof(TotalPriceProduct));
            OnPropertyChanged(nameof(TotalPriceBill));
        }
        private void OpenFilterControl()
        {
            FilterProductViewModel filter = new FilterProductViewModel(CurrentFilter);
            filter.FilterClickedEvent += new EventHandler((o, e) =>
              {
                  CurrentFilter = filter;
                  VisibleProducts = _repository.SearchFilterProduct(CurrentFilter);
                  if (CurrentFilter.CategoryType != null)
                  {
                      CurrentCategory = Categories.Where(i => i.Name == CurrentFilter.CategoryType.Name).FirstOrDefault();
                  }
              });

            WindowService.ShowWindow(filter, new Filtertab());
        }
        private void OpenAddCustomerView()
        {
            RequestAddNewCustomer?.Invoke(this, null);
        }
        private void SearchProduct(string text)
        {
            if (text != null)
            {
                OnCategoryChanged();
                text = text.Trim();
                VisibleProducts = new ObservableCollection<Product>(VisibleProducts.Where(i => i.Name.Contains(text)));
            }
        }
        private void Sort()
        {
            IsPriceLowToHight = !IsPriceLowToHight;
        }
        private void SearchCustomer(string text)
        {
            int number = 5;
            ListSearchCustomer = _repository.SearchCustomer(text, number);
        }
        private void ShowDetail(Model.Product product)
        {
            //RequestOpenView?.Invoke(this, new RequestViewArgs(ProductViewModel, product));
        }
        private void Clear(object information)
        {
            if (information == null) return;

            if (information == CurrentCustomer)
            {
                CurrentCustomer = null;
            }
            else if (information == ProductsInBill)
            {
                ProductsInBill.Clear();
                OnPropertyChanged(nameof(TotalPriceProduct));
                OnPropertyChanged(nameof(TotalPriceBill));
            }
        }
    }
}
