using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class ProductInBill : Product, IDataErrorInfo
    {
        string _seri;
        int _priceUnit;
        //int _quantity;

        public string Seri
        {
            get => _seri;
            set
            {
                if (value!=_seri)
                {
                    _seri = value;
                    OnPropertyChanged();
                }
            }
        }
        public int PriceUnit
        {
            get => _priceUnit;
            set
            {
                if (value!=_priceUnit)
                {
                    _priceUnit = value;
                    OnPropertyChanged();
                }
            }
        }

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch(columnName)
                {
                    case nameof(Seri):
                        if (string.IsNullOrWhiteSpace(Seri))
                        {
                            error = "Không được để trống hoặc khoảng trắng!";
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

        public ProductInBill(Product product) : base(product)
        {
            this.PriceUnit = product.PriceSale;
        }
        public ProductInBill(ITEM_BILL_SERI product, int priceUnit) : base(product.PRODUCT)
        {
            this.Seri = product.seri;
            this.PriceUnit = priceUnit;
        }
    }
}
