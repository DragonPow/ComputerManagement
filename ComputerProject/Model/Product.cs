using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class Product : BaseViewModel
    {
        int _id;
        string _name;
        int _priceOrigin;
        int _priceSale;
        Bitmap _image;
        Model.Category _category;
        string _decription;


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
        public Bitmap Image
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


        public Product()
        {
            Id = 0;
            Image = null;
            //CategoryProduct = new Model.Category();
        }

        public Product(PRODUCT product)
        {
            Id = product.id;
            Name = product.name;
            PriceOrigin = product.priceOrigin;
            PriceSale = product.priceSales;
            Task.Run(() => {
                Image = FormatHelper.TranferToBitmap(product.image);
                Console.WriteLine("Image of {0} load done.", Name);
                });
            CategoryProduct = new Model.Category(product.CATEGORY);
            Decription = product.description;
        }

        public PRODUCT CastToModel()
        {
            PRODUCT p = new PRODUCT();
            p.id = this.Id;
            p.name = this.Name;
            p.priceOrigin = this.PriceOrigin;
            p.priceSales = this.PriceSale;
            p.description = this.Decription;
            p.CATEGORY = this.CategoryProduct.CastToModel();

            return p;
        }
    }
}
