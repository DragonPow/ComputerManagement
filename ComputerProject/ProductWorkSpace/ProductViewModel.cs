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
        public PRODUCT Product
        {
            get => product;
            set
            {
                product = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel()
        {

        }

        public ProductViewModel(PRODUCT model)
        {
            this.Product = model;
        }

        public int Id
        {
            get => Product.id;
            set
            {
                Product.id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get => Product.name;
            set
            {
                Product.name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int PriceOrigin => Product.priceOrigin;
        public string PriceOrigin_String
        {
            get => FormatHelper.ToMoney(PriceOrigin);
            set
            {
                int val = FormatHelper.CheckMoney(value);
                if (val < 1) return;

                Product.priceOrigin = val;
                OnPropertyChanged(nameof(PriceOrigin));
                OnPropertyChanged(nameof(PriceOrigin_String));
            }
        }

        public int PriceSales => Product.priceSales;
        public string PriceSales_String
        {
            get => FormatHelper.ToMoney(PriceSales);
            set
            {
                int val = FormatHelper.CheckMoney(value);
                if (val < 1) return;

                Product.priceSales = val;
                OnPropertyChanged(nameof(PriceSales));
                OnPropertyChanged(nameof(PriceSales_String));
            }
        }

        public string Description
        {
            get => Product.description;
            set
            {
                Product.description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public string Producer
        {
            get => Product.producer;
            set
            {
                Product.producer = value;
                OnPropertyChanged(nameof(Producer));
            }
        }
        public int Quantity
        {
            get => Product.quantity;
            set
            {
                Product.quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Status));
            }
        }
        public Nullable<int> WarrantyTime
        {
            get => Product.warrantyTime;
            set
            {
                Product.warrantyTime = value;
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
            get => Product.isStopSelling;
            set
            {
                Product.isStopSelling = value;
                OnPropertyChanged(nameof(IsStopSelling));
                OnPropertyChanged(nameof(Status));
            }
        }
        public int CategoryId
        {
            get => Product.id;
            set
            {
                Product.categoryId = value;
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

        public BitmapImage Image => Product.image != null && Product.image.Length > 0 ? FormatHelper.BytesToImage(Product.image) : (BitmapImage) System.Windows.Application.Current.FindResource("NoImage");

        public void DeleteFromDB(bool hasInBill)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                
                if (hasInBill)
                {
                    db.PRODUCTs.Attach(Product);
                    Product.isStopSelling = true;
                    db.SaveChanges();
                }
                else
                {
                    db.PRODUCTs.Remove(Product);
                    db.SaveChanges();
                }
            }
        }

        public static bool HasInBill(int productId)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.ITEM_BILL.Where(b => b.productId == productId).Select(b => b.unitPrice).FirstOrDefault();
                return data != 0;
            }
        }

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
                            p.description,
                            p.categoryId
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
                            description = p.description,
                            categoryId = p.categoryId
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
                string nameDL = FormatHelper.ConvertTo_TiengDongLao(name);
                int rs = db.PRODUCTs.Where(p => p.nameIndex.Contains(nameDL)).Count();
                return rs;
            }
        }

        public static byte[] GetImage(int productID)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var dbContext = (db as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
                dbContext.CommandTimeout = 120;

                var data = db.PRODUCTs.Where(p => p.id.Equals(productID)).Select(p => p.image).FirstOrDefault();

                return data != null ? data : null;
            }
        }

        public static ICollection<SpecificationViewModel> GetSpecifications(int productID)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL Spec: " + s);
                var data = db.SPECIFICATIONs.Where(s => s.productId == productID).Select(s => new SpecificationViewModel()
                {
                    SpecificationName = s.SPECIFICATION_TYPE.name,
                    SpecificationTypeId = s.specificationTypeId,
                    SpecValue = s.value,
                    ProductID = s.productId
                });

                var rs = new List<SpecificationViewModel>();

                foreach (var row in data)
                {
                    rs.Add(row);
                }

                return rs;
            }
        }

        public static void CopyTo(PRODUCT source, PRODUCT destination)
        {
            // Copy not include image and specification list
            destination.id = source.id;
            destination.categoryId = source.categoryId;
            destination.name = source.name;
            destination.nameIndex = source.nameIndex;

            /*if (source.image != null && !source.image.Equals(destination.image))
            {
                destination.image = new byte[source.image.Length];
                source.image.CopyTo(destination.image, 0);
            }*/

            destination.producer = source.producer;
            destination.quantity = source.quantity;
            destination.priceOrigin = source.priceOrigin;
            destination.priceSales = source.priceSales;

            if (source.SPECIFICATIONs == null)
            {
                destination.SPECIFICATIONs = null;
                return;
            }

            /*if (destination.categoryId != source.categoryId || destination.SPECIFICATIONs == null)
            {
                if (destination.SPECIFICATIONs != null)
                {
                    destination.SPECIFICATIONs.Clear();
                }
                else
                {
                    destination.SPECIFICATIONs = new List<SPECIFICATION>(source.SPECIFICATIONs.Count);
                }

                foreach (var spec in source.SPECIFICATIONs)
                {
                    destination.SPECIFICATIONs.Add(new SPECIFICATION()
                    {
                        productId = spec.productId,
                        specificationTypeId = spec.specificationTypeId,
                        value = spec.value
                    });
                }
            }
            else
            {
                var destinationSpecs = (List<SPECIFICATION>)destination.SPECIFICATIONs;
                foreach (var spec in source.SPECIFICATIONs)
                {
                    for (int i = 0; i < destinationSpecs.Count; i++)
                    {
                        if (destinationSpecs[i].specificationTypeId == spec.specificationTypeId)
                        {
                            destinationSpecs[i].value = spec.value;
                        }
                    }
                }
            }*/
        }
    }
}
