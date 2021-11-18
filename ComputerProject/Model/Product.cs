using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public enum stateProduct
    {
        [Description("Đang bán")]
        OnSale,
        [Description("Tạm ngừng")]
        Stop,
    }
    public class StateProductExtension
    {
        public static string TranferToViewNamese(stateProduct _state)
        {
            switch(_state)
            {
                case stateProduct.OnSale:
                    return "Đang bán";
                case stateProduct.Stop:
                    return "Tạm ngừng";
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
        public static IEnumerable<string> TranferToViewNamese(Type enumType)
        {
            foreach(var i in (stateProduct[])Enum.GetValues(enumType))
            {
                yield return StateProductExtension.TranferToViewNamese(i);
            }
        }
    }

    public class Product : BaseViewModel
    {
        int _id;
        string _name;
        int _priceOrigin;
        int _priceSale;
        byte[] _image;
        Model.Category _category;
        string _decription;
        string _producer;
        int _warranty;
        int _quantity;


        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public int PriceOrigin
        {
            get => _priceOrigin; set
            {
                if (value != _priceOrigin)
                {
                    _priceOrigin = value;
                    OnPropertyChanged();
                }
            }
        }
        public int PriceSale
        {
            get => _priceSale; set
            {
                if (value != _priceSale)
                {
                    _priceSale = value;
                    OnPropertyChanged();
                }
            }
        }
        public byte[] Image
        {
            get => _image; set
            {
                if (value != _image)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }
        public Model.Category CategoryProduct
        {
            get => _category;
            set
            {
                if (value!=_category)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Decription
        {
            get => _decription;
            set
            {
                if (value != _decription)
                {
                    _decription = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Producer
        {
            get => _producer;
            set
            {
                if (value != _producer)
                {
                    _producer = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Warranty
        {
            get => _warranty;
            set
            {
                if (value!=_warranty)
                {
                    _warranty = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value != _quantity)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public Product()
        {
            Id = 0;
            //CategoryProduct = new Model.Category();
        }

        public Product(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.PriceOrigin = product.PriceOrigin;
            this.PriceSale = product.PriceSale;
            this.Producer = product.Producer;
            this.Image = product.Image;
            this.Decription = product.Decription;
            this.CategoryProduct = product.CategoryProduct;
            this.Warranty = product.Warranty;
            this.Quantity = product.Quantity;
        }

        public Product(PRODUCT product)
        {
            Id = product.id;
            Name = product.name;
            PriceOrigin = product.priceOrigin;
            PriceSale = product.priceSales;
            Producer = product.producer;
            //Task.Run(() =>
            //{
            //    Image = FormatHelper.TranferToBitmap(product.image);
            //});
            Image = product.image;
            if (product.CATEGORY != null) CategoryProduct = new Model.Category(product.CATEGORY);
            Decription = product.description;
            Warranty = product.warrantyTime?? 0;
            Quantity = product.quantity;
        }

        public PRODUCT CastToModel()
        {
            PRODUCT p = new PRODUCT();
            p.id = this.Id;
            p.name = this.Name;
            p.priceOrigin = this.PriceOrigin;
            p.priceSales = this.PriceSale;
            p.producer = this.Producer;
            p.description = this.Decription;
            p.CATEGORY = this.CategoryProduct.CastToModel();
            p.warrantyTime = this.Warranty == 0 ? this.Warranty : (int?)null;
            p.quantity = this.Quantity;

            return p;
        }

        ~Product()
        {
            Console.WriteLine(this.Name + " is dispose");
            //GC.SuppressFinalize(this);
        }
    }
}
