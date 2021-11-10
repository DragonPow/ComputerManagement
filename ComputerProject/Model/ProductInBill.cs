using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class ProductInBill : Product
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
