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
                OnPropertyChanged(nameof(Status));
            }
        }
        public Nullable<int> WarrantyTime
        {
            get => product.warrantyTime;
            set
            {
                product.warrantyTime = value;
                OnPropertyChanged(nameof(WarrantyTime));
                OnPropertyChanged(nameof(WarrantyYear_String));
            }
        }

        public string WarrantyYear_String
        {
            get
            {
                if (WarrantyTime == null) return "";

                return Math.Round(WarrantyTime.Value / 12d, 1).ToString() + " năm";
            }
        }

        public bool IsStopSelling
        {
            get => product.isStopSelling;
            set
            {
                product.isStopSelling = value;
                OnPropertyChanged(nameof(IsStopSelling));
                OnPropertyChanged(nameof(Status));
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
        public string Status
        {
            get
            {
                string rs;
                var input = this;
                if (input.IsStopSelling)
                {
                    rs = "Ngừng bán";
                }
                else if (input.Quantity < 1)
                {
                    rs = "Hết hàng";
                }
                else
                {
                    rs = "Đang bán";
                }
                return rs;
            }
        }

        public BitmapImage Image => FormatHelper.BytesToImage(product.image);

        public static List<ProductViewModel> FindByName(string name, int startIndex, int count)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    string nameDL = FormatHelper.ConvertTo_TiengDongLao(name);
                    var data = db.PRODUCTs.Where(p => p.nameIndex.Contains(nameDL)).Select(
                        p => new
                        {
                            p.name,
                            p.id,
                            p.priceOrigin,
                            p.priceSales,
                            p.quantity,
                            p.warrantyTime,
                            p.producer,
                            p.description
                        }
                        ).OrderBy(p => p.name).Skip(startIndex).Take(count).ToList();

                    Console.WriteLine("l = " + data.Count);
                    var rs = new List<ProductViewModel>();

                    foreach (var p in data)
                    {
                        rs.Add(new ProductViewModel(new PRODUCT()
                        {
                            name = p.name,
                            id = p.id,
                            priceOrigin = p.priceOrigin,
                            priceSales = p.priceSales,
                            quantity = p.quantity,
                            warrantyTime = p.warrantyTime,
                            producer = p.producer,
                            description = p.description
                        }));
                    }

                    return rs;
                }
            }
            catch (Exception e)
            {
                string a = e.Message;
                return null;
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

        public static int CountByName(string name)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                int rs = db.PRODUCTs.Where(p => p.name.Contains(name)).Count();
                return rs;
            }
        }
    }
}
