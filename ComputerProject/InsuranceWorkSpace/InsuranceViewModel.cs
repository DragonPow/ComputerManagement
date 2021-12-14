using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceViewModel: BusyViewModel
    {
        protected BILL_REPAIR _model;
        protected BILL_REPAIR Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public int Id
        {
            get => Model.id;
            set
            {
                Model.id = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Id_String));
            }
        }
        public string Id_String => FormatHelper.ConvertBillRepairID(Id, IsWarranty);

        public DateTime TimeDelivery
        {
            get => Model.timeDelivery.HasValue ? Model.timeDelivery.Value : DateTime.MinValue;
            set
            {
                Model.timeDelivery = value;
                OnPropertyChanged(nameof(TimeDelivery));
                OnPropertyChanged(nameof(TimeDelivery_String));
            }
        }
        public string TimeDelivery_String => Model.timeDelivery.HasValue ? FormatHelper.DatetimeToDateString(TimeDelivery) : null;

        public System.DateTime TimeReceive
        {
            get => Model.timeReceive.HasValue ? Model.timeReceive.Value : DateTime.MinValue;
            set
            {
                Model.timeReceive = value;
                OnPropertyChanged(nameof(TimeReceive));
                OnPropertyChanged(nameof(TimeReceive_String));
            }
        }
        public string TimeReceive_String => Model.timeReceive.HasValue ? FormatHelper.DatetimeToDateString(TimeReceive) : null; 

        public string DesReceiveItems
        {
            get => Model.desReceiveItems;
            set
            {
                Model.desReceiveItems = value;
                OnPropertyChanged(nameof(DesReceiveItems));
            }
        }
        public string DesProblem
        {
            get => Model.desProblem;
            set
            {
                Model.desProblem = value;
                OnPropertyChanged(nameof(DesProblem));
            }
        }
        public string DesComponents
        {
            get => Model.attachments;
            set
            {
                Model.attachments = value;
                OnPropertyChanged(nameof(DesComponents));
            }
        }
        public string DetailRepair
        {
            get => Model.desDetailRepair;
            set
            {
                Model.desDetailRepair = value;
                OnPropertyChanged(nameof(DetailRepair));
            }
        }
        public long Price
        {
            get => Model.price.HasValue ? Model.price.Value : 0;
            set
            {
                Model.price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public string Price_String
        {
            get => FormatHelper.ToMoney(Price);
            set
            {
                var val = FormatHelper.CheckMoney(value);
                if (val > 0)
                {
                    Price = val;
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(Price_String));
                }
            }
        }

        public long CustomerMoney
        {
            get => Model.customerMoney.HasValue ? Model.customerMoney.Value : 0;
            set
            {
                Model.customerMoney = value;
                OnPropertyChanged(nameof(CustomerMoney));
            }
        }
        public string CustomerMoney_String
        {
            get => FormatHelper.ToMoney(CustomerMoney);
            set
            {
                var val = FormatHelper.CheckMoney(value);
                if (val > 0)
                {
                    CustomerMoney = val;
                    OnPropertyChanged(nameof(CustomerMoney));
                    OnPropertyChanged(nameof(CustomerMoney_String));
                }
            }
        }

        public bool IsWarranty
        {
            get => Model.isWarranty;
            set
            {
                Model.isWarranty = value;
                OnPropertyChanged(nameof(IsWarranty));
            }
        }
        public int Status
        {
            get => Model.status;
            set
            {
                Model.status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(Status_String));
            }
        }
        public string Status_String => StatusToString(Status);

        public int CustomerId
        {
            get => Model.customerId;
            set
            {
                Model.customerId = value;
                OnPropertyChanged(nameof(CustomerId));
            }
        }
        public int SeriId
        {
            get => Model.seriId.HasValue ? Model.seriId.Value : -1;
            set
            {
                Model.seriId = value;
                OnPropertyChanged(nameof(SeriId));
            }
        }
        //public string ProductName
        //{
        //    get => Model.nameProduct;
        //}

        public string CustomerName => Model.CUSTOMER != null ? Model.CUSTOMER.name : null;
        public string CustomerPhone => Model.CUSTOMER != null ? Model.CUSTOMER.phone : null;

        public string Type => IsWarranty ? "Bảo hành" : "Không bảo hành";
        public string ProductSeri => IsWarranty ? Model.ITEM_BILL_SERI.seri : null;
        public string TimeProductSell => IsWarranty ? FormatHelper.DatetimeToDateString(Model.ITEM_BILL_SERI.BILL.createTime) : null;
        public string ProductBillSeri => IsWarranty ? FormatHelper.ConvertBillID(Model.ITEM_BILL_SERI.billId): null;
        public string TimeWarrantyEnd
        {
            get
            {
                if (IsWarranty)
                {
                    int warrantymonth = Model.ITEM_BILL_SERI.PRODUCT.warrantyTime.HasValue ? Model.ITEM_BILL_SERI.PRODUCT.warrantyTime.Value : 0;
                    return FormatHelper.DatetimeToDateString(Model.ITEM_BILL_SERI.BILL.createTime.AddMonths(warrantymonth));
                }
                return null;
            }
        }

        public InsuranceViewModel()
        {
            Model = new BILL_REPAIR();
        }

        public InsuranceViewModel(BILL_REPAIR model)
        {
            Model = model;
        }

        public static List<InsuranceViewModel> FindByPhoneOrID(string str, int statusFilter, int startIndex, int count, int sortType)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL Insur: " + s);
                    string nameDL = FormatHelper.ConvertTo_TiengDongLao(str);
                    var data1 = db.BILL_REPAIR.Where(p => p.CUSTOMER.phone.Contains(nameDL) || p.id.ToString().Contains(str));

                    // Filter by status
                    if (statusFilter > 0 && statusFilter < 4)
                    {
                        data1 = data1.Where(b => b.status == statusFilter - 1);
                    }

                    // Select 
                    var data2 = data1.Select(
                        b => new
                        {
                            b.id,
                            b.customerId,
                            b.customerMoney,
                            b.desProblem,
                            b.desReceiveItems,
                            b.isWarranty,
                            b.price,
                            b.seriId,
                            b.status,
                            b.timeDelivery,
                            b.timeReceive,
                            b.attachments,
                            b.desDetailRepair,
                            customerPhone = b.CUSTOMER.phone,
                            customerName = b.CUSTOMER.name,
                            //b.nameProduct,
                            productSeri = b.isWarranty ? b.ITEM_BILL_SERI.seri : null,
                            productWarrantyTime = b.isWarranty ? b.ITEM_BILL_SERI.PRODUCT.warrantyTime : null,
                            billID = b.isWarranty ? b.ITEM_BILL_SERI.billId : -1,
                            billCreateDate = b.isWarranty ? b.ITEM_BILL_SERI.BILL.createTime : DateTime.MinValue
                        }
                    );

                    // Sort
                    if (sortType == 0)
                    {
                        data2 = data2.OrderBy(b => b.timeReceive);
                    }
                    else if (sortType == 1)
                    {
                        data2 = data2.OrderByDescending(b => b.timeReceive);
                    }
                    else if (sortType == 2)
                    {
                        data2 = data2.OrderBy(b => b.customerPhone);
                    }
                    else if (sortType == 3)
                    {
                        data2 = data2.OrderByDescending(b => b.customerPhone);
                    }

                    // Paging and get data
                    var data = data2.Skip(startIndex).Take(count).ToList();

                    var rs = new List<InsuranceViewModel>();

                    foreach (var b in data)
                    {
                        rs.Add(new InsuranceViewModel(new BILL_REPAIR()
                        {
                            id = b.id,
                            customerId = b.customerId,
                            customerMoney = b.customerMoney,
                            desProblem = b.desProblem,
                            desReceiveItems = b.desReceiveItems,
                            isWarranty = b.isWarranty,
                            price = b.price,
                            seriId = b.seriId,
                            status = b.status,
                            timeDelivery = b.timeDelivery,
                            timeReceive = b.timeReceive,
                            //nameProduct = b.nameProduct,
                            desDetailRepair = b.desDetailRepair,
                            CUSTOMER = new CUSTOMER() { 
                                id = b.customerId, 
                                phone = b.customerPhone },
                            ITEM_BILL_SERI = b.isWarranty ?
                                new ITEM_BILL_SERI() 
                                { 
                                    id = b.seriId.Value, 
                                    seri = b.productSeri ,
                                    billId = b.billID,
                                    BILL = new BILL()
                                    {
                                        id = b.billID,
                                        createTime = b.billCreateDate
                                    }
                                } 
                                : null
                        }));
                    }

                    return rs;
                }
            }
            catch (Exception e) when (!HelperService.Environment.IsDebug)
            {
                string a = e.Message;
                return null;
            }
        }

        public static int CountByPhoneOrID(string str, int statusFilter)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    string nameDL = FormatHelper.ConvertTo_TiengDongLao(str);
                    var data1 = db.BILL_REPAIR.Where(p => p.CUSTOMER.phone.Contains(nameDL) || p.id.ToString().Contains(str));

                    // Filter by status
                    if (statusFilter > 0 && statusFilter < 4)
                    {
                        data1 = data1.Where(b => b.status == statusFilter - 1);
                    }

                    return data1.Count();
                }
            }
            catch (Exception e) when (!HelperService.Environment.IsDebug)
            {
                string a = e.Message;
                return 0;
            }
        }

        public static BILL_REPAIR GetByID(int id)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    var data1 = db.BILL_REPAIR.Where(p => p.id == id);

                    // Select 
                    var data2 = data1.Select(
                        b => new
                        {
                            b.id,
                            b.customerId,
                            b.customerMoney,
                            b.desProblem,
                            b.desReceiveItems,
                            b.isWarranty,
                            b.price,
                            b.seriId,
                            b.status,
                            b.timeDelivery,
                            b.timeReceive,
                            b.attachments,
                            b.desDetailRepair,
                            customerPhone = b.CUSTOMER.phone,
                            customerName = b.CUSTOMER.name,
                            //b.nameProduct,
                            productSeri = !b.isWarranty ? null : b.ITEM_BILL_SERI.seri,
                            productId = !b.isWarranty ? -1 : b.ITEM_BILL_SERI.productId,
                            productWarrantyTime = b.isWarranty ? b.ITEM_BILL_SERI.PRODUCT.warrantyTime : null,
                            billID = b.isWarranty ? b.ITEM_BILL_SERI.billId : -1,
                            billCreateDate = b.isWarranty ? b.ITEM_BILL_SERI.BILL.createTime : DateTime.MinValue
                        }
                    );

                    var data = data2.FirstOrDefault();

                    var rs = new BILL_REPAIR()
                    {
                        id = data.id,
                        customerId = data.customerId,
                        customerMoney = data.customerMoney,
                        desProblem = data.desProblem,
                        desReceiveItems = data.desReceiveItems,
                        isWarranty = data.isWarranty,
                        price = data.price,
                        seriId = data.seriId,
                        status = data.status,
                        timeDelivery = data.timeDelivery,
                        timeReceive = data.timeReceive,
                        //nameProduct = data.nameProduct,
                        desDetailRepair = data.desDetailRepair,
                        CUSTOMER = new CUSTOMER()
                        {
                            id = data.customerId,
                            phone = data.customerPhone
                        },
                        ITEM_BILL_SERI = data.isWarranty ?
                                new ITEM_BILL_SERI()
                                {
                                    id = data.seriId.Value,
                                    seri = data.productSeri,
                                    billId = data.billID,
                                    BILL = new BILL()
                                    {
                                        id = data.billID,
                                        createTime = data.billCreateDate
                                    },
                                    PRODUCT = new PRODUCT()
                                    {
                                        id = data.productId,
                                        warrantyTime = data.productWarrantyTime,
                                    }
                                }
                                : null
                    };

                    return rs;
                }
            }
            catch (Exception e) when (!HelperService.Environment.IsDebug)
            {
                string a = e.Message;
                return null;
            }
        }

        public static string StatusToString(int status) {
            if (status == 0)
            {
                return "Đang sửa chữa";
            }
            else if (status == 1)
            {
                return "Đã sửa xong";
            }
            else if (status == 2)
            {
                return "Đã thanh toán";
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static int StringToStatus(string s)
        {
            if (s == "Đang sửa chữa")
            {
                return 0;
            }
            else if (s == "Đã sửa xong")
            {
                return 1;
            }
            else if (s == "Đã thanh toán")
            {
                return 2;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
