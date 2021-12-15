using ComputerProject.CustomMessageBox;
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
        protected string error;
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
                if (val < 1)
                {
                    error = "Giá gốc không hợp lệ";
                    val = 0;
                }

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
                if (val < 1)
                {
                    error = "Giá bán không hợp lệ";
                    val = 0;
                }

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
                if (WarrantyTime == null || WarrantyTime.Value == 0) return "";

                // return Math.Round(WarrantyTime.Value / 12d, 1).ToString() + " năm";

                return WarrantyTime.Value.ToString() +  " tháng";
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
            get => Product.categoryId;
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
                if (IsStopSelling)
                {
                    return "Ngừng bán";
                }
                else if (Quantity < 1)
                {
                    return "Hết hàng";
                }
                else
                {
                    return "Đang bán";
                }
            }
        }

        public BitmapImage Image => Product.image != null && Product.image.Length > 0 ? FormatHelper.BytesToImage(Product.image) : (BitmapImage) System.Windows.Application.Current.FindResource("NoImage");

        public void DeleteFromDB(bool hasInBill)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    var old = db.PRODUCTs.Where(p => p.id == Product.id).FirstOrDefault();
                    //if (old == null) return;

                    if (hasInBill)
                    {
                        old.isStopSelling = true;
                    }
                    else
                    {
                        db.PRODUCTs.Remove(old);
                    }
                    db.SaveChanges();
                }
            }
            catch(Exception)
            {
                MessageBoxCustom.ShowDialog("Sản phẩm đã bán ra, không thể xóa", "Thông báo", MaterialDesignThemes.Wpf.PackIconKind.InformationCircleOutline);
            }
        }

        public static bool HasInBill(int productId)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.ITEM_BILL.Where(b => b.productId == productId).Select(b => new
                {
                    b.billId,
                    b.productId
                }).FirstOrDefault();
                return data != null;
            }
        }

        public static List<ProductViewModel> FindByNameOrID(string str, int startIndex, int count, int orderMode = 0, SaleWorkSpace.IFilterProductState filter = null)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    string nameDL = FormatHelper.ConvertTo_TiengDongLao(str);
                    var data1 = db.PRODUCTs.Where(p => p.isStopSelling == false && (p.nameIndex.Contains(nameDL) || p.id.ToString().Contains(str))).Select(
                        p => new
                        {
                            name = p.name,
                            id = p.id,
                            priceOrigin = p.priceOrigin,
                            priceSales = p.priceSales,
                            quantity = p.quantity,
                            warrantyTime = p.warrantyTime,
                            producer = p.producer,
                            description = p.description,
                            categoryId = p.categoryId,
                            isStopSelling = p.isStopSelling,
                        }
                    );

                    if (filter != null)
                    {
                        data1 = data1.Where(i => (!(filter.Supplier == null || filter.Supplier.Trim() == "") ? i.producer.ToLower().Contains(filter.Supplier.Trim().ToLower()) : true)
                                                                && i.priceSales >= filter.PriceLowest
                                                                && (filter.PriceHighest > 0 ? i.priceSales <= filter.PriceHighest : true)
                                                                && (filter.TimeWarranty > 0 ? i.warrantyTime == filter.TimeWarranty : true)
                                                                );
                    }

                    switch (orderMode)
                    {
                        case 0:
                            data1 = data1.OrderByDescending(p => p.priceSales).Skip(startIndex).Take(count);
                            break;
                        case 1:
                            data1 = data1.OrderBy(p => p.priceSales).Skip(startIndex).Take(count);
                            break;
                    }


                    var data = data1.ToList();

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
                            categoryId = p.categoryId,
                            isStopSelling = p.isStopSelling,
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

        public static int CountByNameOrId(string str)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                string nameDL = FormatHelper.ConvertTo_TiengDongLao(str);
                int rs = db.PRODUCTs.Where(p => p.isStopSelling == false && (p.nameIndex.Contains(nameDL) || p.id.ToString().Contains(str))).Count();
                return rs;
            }
        }

        public static byte[] GetImage(int productID)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                FormatHelper.SetTimeOut(db, 120);

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
            destination.warrantyTime = source.warrantyTime;
            destination.description = source.description;

            destination.producer = source.producer;
            destination.quantity = source.quantity;
            destination.priceOrigin = source.priceOrigin;
            destination.priceSales = source.priceSales;
        }
    }
}
