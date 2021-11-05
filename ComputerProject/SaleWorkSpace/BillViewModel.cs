using ComputerProject.HelperService;
using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.SaleWorkSpace
{
    public class BillViewModel : BaseViewModel
    {
        BILL _currentBill;
        ICommand _confirmCommand;
        ICommand _printCommand;
        public int TotalBill => CurrentBill.customerMoney - CurrentBill.totalMoney;

        public BILL CurrentBill
        {
            get => _currentBill;
            set
            {
                if (value != _currentBill)
                {
                    _currentBill = value;
                    OnPropertyChanged();
                }
            }
        }

        //public Collection<ProductInBill> Products
        //{
        //    get => _products;
        //    set
        //    {
        //        if (value!=_products)
        //        {
        //            _products = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        
        //public int TotalPrice
        //{
        //    get => _totalPrice;
        //    set
        //    {
        //        if (value != _totalPrice)
        //        {
        //            _totalPrice = value;
        //            OnPropertyChanged();
        //            OnPropertyChanged(nameof(TotalBill));
        //        }
        //    }
        //}
        //public int PriceCustomer
        //{
        //    get => _priceCustomer;
        //    set
        //    {
        //        if (value != _priceCustomer)
        //        {
        //            _priceCustomer = value;
        //            OnPropertyChanged();
        //            OnPropertyChanged(nameof(TotalBill));
        //        }
        //    }
        //}
        public ICommand ConfirmCommand
        {
            get
            {
                if (null == _confirmCommand)
                {
                    _confirmCommand = new RelayCommand(a => Confirm());
                }
                return _confirmCommand;
            }
        }
        public ICommand PrintCommand
        {
            get
            {
                if (null == _printCommand)
                {
                    _printCommand = new RelayCommand(a => Print());
                }
                return _printCommand;
            }
        }

        public BillViewModel()
        {

        }
        public BillViewModel(BILL bill)
        {
            CurrentBill = bill;
        }
        public BillViewModel(IDictionary<Model.Product, int> listproduct)
        {
            CurrentBill = new BILL();
            CurrentBill.ITEM_BILL = new ObservableCollection<ITEM_BILL>();
            CurrentBill.ITEM_BILL_SERI = new ObservableCollection<ITEM_BILL_SERI>();
            foreach (var i in listproduct)
            {
                CurrentBill.ITEM_BILL.Add(new ITEM_BILL()
                {
                    PRODUCT = i.Key.CastToModel(),
                    productId = i.Key.Id,
                    quantity = i.Value,
                    unitPrice = i.Key.PriceSale,
                });
                CurrentBill.ITEM_BILL_SERI.Add(new ITEM_BILL_SERI()
                {
                    PRODUCT = i.Key.CastToModel(),
                    productId = i.Key.Id,
                    //seri
                });
            }
        }

        private void Confirm()
        {
            using (var db = new ComputerManagementEntities())
            {
                db.BILLs.Add(CurrentBill);
                db.SaveChangesAsync();
            }
        }
        private void Print()
        {
            throw new NotImplementedException();
        }
    }
}
