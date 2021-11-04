using ComputerProject.ApplicationWorkspace;
using ComputerProject.CustomerWorkspace;
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

namespace ComputerProject.SaleWorkSpace
{
    public class SaleViewModel : BaseViewModel, ITabView
    {
        #region Fields
        public string ViewName => "Bán hàng";
        public PackIconKind ViewIcon => PackIconKind.CashRegister;
        readonly SaleRepository _repository;

        Collection<Model.Product> _products;
        Collection<Model.Product> _visibleProduct;
        IDictionary<Model.Product, int> _productsInBill;
        Collection<Model.Category> _categories;
        CustomerViewModel _currentCustomer;
        Model.Category _currentCategory;
        Model.Category _currentRootCategory;
        int _currentPoint;
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
        public int TotalPriceBill => 0;

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
        public CustomerViewModel CurrentCustomer
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
        public Model.Category CurrentCategory
        {
            get => _currentCategory;
            set
            {
                if (value != _currentCategory)
                {
                    _currentCategory = value;
                    OnCategoryChanged();
                    OnPropertyChanged();
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
                    _currentCategory = null;
                    OnCategoryChanged();
                    OnPropertyChanged();
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
                    _paymentCommand = new RelayCommand(a => Payment(ProductsInBill, CurrentCustomer));
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
        #endregion //Properties


        public SaleViewModel()
        {
            _repository = new SaleRepository();
            LoadData();
        }
        public SaleViewModel(SaleRepository repository)
        {
            this._repository = repository;
        }

        public void LoadData()
        {
            Task.Run(() => VisibleProducts = _products = _repository.LoadProducts());
            Task.Run(() => Categories = _repository.LoadCategories());
        }
        public void OnCategoryChanged()
        {
            if (CurrentCategory != null)
            {
                VisibleProducts = new ObservableCollection<Model.Product>(_products.Where(i => i.CategoryProduct.Id == CurrentCategory.Id));
            }
            else if (CurrentRootCategory != null)
            {
                VisibleProducts = new ObservableCollection<Model.Product>(_products.Where(i => CurrentRootCategory.ChildCategories.Any(child => child.Id == i.CategoryProduct.Id)));
            }
            else
            {
                VisibleProducts = _products;
            }
        }

        private void Payment(IDictionary<Product, int> productsInBill, CustomerViewModel currentCustomer)
        {
            var vm = new BillViewModel();
            WindowService.ShowWindow(vm, new PaySaleBillView());
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
            filter.FilterEvent += new EventHandler((o, e) =>
              {
                  CurrentFilter = filter;
                  VisibleProducts = _products = _repository.SearchFilterProduct(CurrentFilter);
                  if (CurrentFilter.CategoryType != null)
                  {
                      CurrentCategory = Categories.Where(i => i.Name == CurrentFilter.CategoryType.Name).FirstOrDefault();
                  }
              });

            WindowService.ShowWindow(filter, new Filtertab());
        }
        private void OpenAddCustomerView()
        {
            throw new NotImplementedException();
        }
        private void SearchProduct(string v)
        {
            throw new NotImplementedException();
        }
        private void Sort()
        {
            IsPriceLowToHight = !IsPriceLowToHight;
        }
        private void SearchCustomer(string v)
        {
            throw new NotImplementedException();
        }
        private void ShowDetail(Model.Product product)
        {
            //RequestOpenView?.Invoke(this, new RequestViewArgs(ProductViewModel, product));
        }
        private void Clear(object list)
        {
            if (list == null) return;
            if (list == CurrentCustomer)
            {
                CurrentCustomer = null;
            }
            if (list == ProductsInBill)
            {
                ProductsInBill.Clear();
            }
        }
    }
}
