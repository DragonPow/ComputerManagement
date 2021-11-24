using ComputerProject.ApplicationWorkspace;
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ComputerProject.SaleWorkSpace
{
    public class SaleViewModel : HelperService.BaseViewModel, ITabView, IDataErrorInfo
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
        int _pointToMoney = -1;
        int _maxPoint = -1;
        IFilterProductState _currentFilter;
        bool _isPriceLowToHight;

        ICommand _paymentCommand;
        ICommand _addProductInBillCommand;
        ICommand _removeProductInBillCommand;
        //ICommand _filterProductsCommand;
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
                int value = TotalPriceProduct - CurrentPoint * PointToMoney;

                if (value <= 0) return 0;
                return value;
            }
        }
        public int PointToMoney
        {
            get
            {
                if (_pointToMoney < 0)
                {
                    _pointToMoney = _repository.GetPointToMoney();
                    OnPropertyChanged(nameof(TotalPriceBill));
                }
                return _pointToMoney;
            }
        }
        public int MaxPoint
        {
            get
            {
                if (_maxPoint < 0)
                {
                    _maxPoint = _repository.GetMaxPointUse();
                }
                return _maxPoint;
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
                    var dicProduct = (ObservableConcurrentDictionary<Product, int>)_productsInBill;

                    void setOutOfStock(Product product)
                    {
                        int quantityInBill = dicProduct.ContainsKey(product) ? dicProduct[product] : 0;
                        product.IsOutOfStock = (product.Quantity - quantityInBill) == 0;
                    }

                    dicProduct.PropertyChanged += (s, e) =>
                      {
                          if (_products != null)
                          foreach (var product in _products)
                          {
                              setOutOfStock(product);
                          }
                      };
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
                    CurrentPoint = 0;
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
                    VisibleProducts = FilterByCategory(_products);
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

                    VisibleProducts = FilterByCategory(_products);
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
                    if (CurrentCustomer == null && value != 0) throw new InvalidCastException("The user not null");
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
                            MessageBoxCustom.ShowDialog("Vui lòng chọn sản phẩm.", "Thông báo", PackIconKind.Information);
                        }
                        else if (CurrentCustomer == null)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng chọn khách hàng.", "Thông báo", PackIconKind.Information);
                        }
                        else if (this.HasError)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng nhập đúng và đầy đủ tất cả thông tin.", "Thông báo", PackIconKind.Information);
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
                    _addProductInBillCommand = new RelayCommand(product =>
                    {
                        try
                        {
                            AddToBill(product as Model.Product, 1);
                        }
                        catch (InvalidOperationException e1)
                        {
                            MessageBoxCustom.ShowDialog("Không đủ hàng trong kho để bán.", "Thông báo", PackIconKind.WarningBox);
                        }
                    });
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
        //public ICommand FilterProductsCommand
        //{
        //    get
        //    {
        //        if (null == _filterProductsCommand)
        //        {
        //            _filterProductsCommand = new RelayCommand(a => OpenFilterControl());
        //        }
        //        return _filterProductsCommand;
        //    }
        //}
        public ICommand SortProductsCommand
        {
            get
            {
                if (null == _sortProductsCommand)
                {
                    _sortProductsCommand = new RelayCommand(a => SortProductbyPrice());
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
                    _searchProductCommand = new RelayCommand(s => SearchProductbyName(s as string));
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
                    _searchCustomerCommand = new RelayCommand(s => SearchCustomerbyPhone(s as string));
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

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(CurrentPoint):
                        if (CurrentPoint > CurrentCustomer?.point)
                        {
                            error = "Điểm vượt quá điểm của khách hàng.";
                        }
                        else if (CurrentPoint > MaxPoint)
                        {
                            error = "Điểm vượt quá quy định.";
                        }
                        break;
                }

                if (ErrorCollection.ContainsKey(columnName))
                {
                    if (error != null) ErrorCollection[columnName] = error;
                    else ErrorCollection.Remove(columnName);
                }
                else if (error != null)
                {
                    ErrorCollection.Add(columnName, error);
                }

                OnPropertyChanged(nameof(ErrorCollection));
                return error;
            }
        }
        public bool HasError => ErrorCollection.Any();
        #endregion //Properties

        public SaleViewModel()
        {
            _repository = new SaleRepository();
            //LoadData();
        }
        public SaleViewModel(SaleRepository repository)
        {
            this._repository = repository;
            //LoadData();
        }

        public async void LoadData()
        {
            ResetData();
            
            Task.Run(LoadCategoryControl);
            Task.Run(() => VisibleProducts = _products = _repository.LoadProducts());
            Task.Run(LoadFilterControl);
        }
        public bool AllowChangeTab()
        {
            return true;
        }

        private void LoadCategoryControl()
        {
            //Load data
            Categories = _repository.LoadCategories();

            //Add "All" text to UI for category tab
            var null_category = new Category();
            Categories.Insert(0, null_category);
            foreach (var category in Categories)
            {
                if (category.ChildCategories == null) category.ChildCategories = new ObservableCollection<Model.Category>();
                category.ChildCategories.Insert(0, null_category);
            }

            //Set current Category to "All"
            _currentRootCategory = Categories[0];
            _currentCategory = _currentRootCategory.ChildCategories[0];

            OnPropertyChanged(nameof(CurrentRootCategory));
            OnPropertyChanged(nameof(CurrentCategory));
        }
        private void LoadFilterControl()
        {
            if (CurrentFilter == null)
            {
                CurrentFilter = new FilterProductViewModel(CurrentFilter, true);
                CurrentFilter.FilterClickedEvent += new EventHandler((o, e) =>
                {
                    VisibleProducts = FilterByCategory(_products);
                    VisibleProducts = FilterByFilterControl(VisibleProducts, CurrentFilter);
                });
            }
        }
        public void ResetData()
        {
            _pointToMoney = -1;
            _maxPoint = -1;
            _currentCategory = _currentRootCategory = null;
            Clear(CurrentCustomer);
            Clear(ProductsInBill);
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
        private Collection<Product> FilterByCategory(Collection<Product> products)
        {
            if (products == null) return null;

            if (CurrentCategory != null && CurrentCategory.Name != null)
            {
                //Current tab category is child
                return new ObservableCollection<Model.Product>(products.Where(i => i.CategoryProduct.Id == CurrentCategory.Id));
            }
            else if (CurrentRootCategory != null)
            {
                //Current tab category is root
                if (CurrentRootCategory.Name != null)
                {
                    return new ObservableCollection<Model.Product>(products.Where(i => CurrentRootCategory.ChildCategories.Any(child => child.Id == i.CategoryProduct.Id)));
                }
                else
                {
                    //Show all products in store
                    return products;
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
        private void OpenPaymentView(IDictionary<Product, int> productsInBill, CUSTOMER currentCustomer)
        {
            var vm = new BillViewModel(productsInBill, currentCustomer, TotalPriceBill);
            vm.PaymentSuccessEvent += (s, e) => ClearPayment();

            WindowService.ShowSingelWindow(vm, new PaySaleBillView());
        }
        private void ClearPayment()
        {
            Clear(CurrentCustomer);
            Clear(ProductsInBill);
            CurrentPoint = 0;
        }
        private void AddToBill(Product product, int quantity)
        {
            bool containProduct = ProductsInBill.ContainsKey(product);
            //int totalQuantity = quantity + (containProduct ? ProductsInBill[product] : 0);

            if (product.IsOutOfStock)
            {
                throw new InvalidOperationException("Quantity not enough to buy");
            }

            if (containProduct)
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
        //private void OpenFilterControl()
        //{
        //    FilterProductViewModel filter = new FilterProductViewModel(CurrentFilter);
        //    filter.FilterClickedEvent += new EventHandler((o, e) =>
        //      {
        //          //CurrentFilter = filter;
        //          //VisibleProducts = _repository.SearchFilterProduct(CurrentFilter);
        //          FilterByCategory();
        //          VisibleProducts = SearchFilterProduct(VisibleProducts, CurrentFilter);
        //          //if (CurrentFilter.CategoryType != null)
        //          //{
        //          //    CurrentCategory = Categories.Where(i => i.Name == CurrentFilter.CategoryType.Name).FirstOrDefault();
        //          //}
        //      });

        //    WindowService.ShowWindow(filter, new Filtertab());
        //}
        public Collection<Product> FilterByFilterControl(Collection<Product> products, IFilterProductState filter)
        {
            IEnumerable<Model.Product> filteredProducts = products.Where(i => (!(filter.Supplier == null || filter.Supplier.Trim() == "") ? i.Producer.ToLower().Contains(filter.Supplier.Trim().ToLower()) : true)
                                                                && i.PriceSale >= filter.PriceLowest
                                                                && (filter.PriceHighest > 0 ? i.PriceSale <= filter.PriceHighest : true)
                                                                && (filter.TimeWarranty > 0 ? i.Warranty == filter.TimeWarranty : true));
            return new ObservableCollection<Product>(filteredProducts);
        }
        private void OpenAddCustomerView()
        {
            var vm = new CustomerViewModel(true);
            WindowService.ShowWindow(vm, new CustomerAdd());
        }
        private void SearchProductbyName(string text)
        {
            if (text != null)
            {
                VisibleProducts = FilterByCategory(_products);
                text = FormatHelper.ConvertTo_TiengDongLao(text.Trim().ToLower());

                VisibleProducts = new ObservableCollection<Product>(VisibleProducts.Where(i => FormatHelper.ConvertTo_TiengDongLao(i.Name).Trim().ToLower().Contains(text)));
            }
        }
        private void SortProductbyPrice()
        {
            IsPriceLowToHight = !IsPriceLowToHight;
        }
        private void SearchCustomerbyPhone(string phone)
        {
            int number = 5;
            ListSearchCustomer = _repository.SearchCustomer(phone, number);
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
