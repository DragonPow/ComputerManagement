using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerProject.ProductWorkSpace
{
    class ProductViewModel : BusyViewModel
    {
        protected PRODUCT product;

        public ProductViewModel()
        {

        }

        public ProductViewModel(PRODUCT model)
        {
            this.product = model;
        }

        public int Id
        {
            get => product.id;
            set
            {
                product.id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get => product.name;
            set
            {
                product.name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int PriceOrigin => product.priceOrigin;
        public string PriceOrigin_String
        {
            get => FormatHelper.ToMoney(PriceOrigin);
            set
            {
                int val = FormatHelper.CheckMoney(value);
                if (val < 1) return;

                product.priceOrigin = val;
                OnPropertyChanged(nameof(PriceOrigin));
                OnPropertyChanged(nameof(PriceOrigin_String));
            }
        }

        public int PriceSales => product.priceSales;
        public string PriceSales_String
        {
            get => FormatHelper.ToMoney(PriceSales);
            set
            {
                int val = FormatHelper.CheckMoney(value);
                if (val < 1) return;

                product.priceSales = val;
                OnPropertyChanged(nameof(PriceSales));
                OnPropertyChanged(nameof(PriceSales_String));
            }
        }

        public string Description
        {
            get => product.description;
            set
            {
                product.description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public string Producer
        {
            get => product.producer;
            set
            {
                product.producer = value;
                OnPropertyChanged(nameof(Producer));
            }
        }
        public int Quantity
        {
            get => product.quantity;
            set
            {
                product.quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public Nullable<int> WarrantyTime
        {
            get => product.warrantyTime;
            set
            {
                product.warrantyTime = value;
                OnPropertyChanged(nameof(WarrantyTime));
            }
        }
        public bool IsStopSelling
        {
            get => product.isStopSelling;
            set
            {
                product.isStopSelling = value;
                OnPropertyChanged(nameof(IsStopSelling));
            }
        }
        public int CategoryId
        {
            get => product.id;
            set
            {
                product.categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }

        public BitmapImage Image => FormatHelper.BytesToImage(product.image);

        public static List<ProductViewModel> FindByName(string name, int startIndex, int count)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.PRODUCTs.Where(p => p.nameIndex.Contains(name)).OrderBy(p => p.name).Skip(startIndex).Take(count);

                var rs = new List<ProductViewModel>();
                foreach (var row in data)
                {
                    rs.Add(new ProductViewModel(row));
                }

                return rs;
            }
        }

        public static ProductViewModel FindByName(string name)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.PRODUCTs.Where(p => p.name.Equals(name)).FirstOrDefault();

                return data != null ? new ProductViewModel(data) : null;
            }
        }
    }
}
