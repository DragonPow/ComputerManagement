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

        public int Id
        {
            get => _model.id;
            set
            {
                _model.id = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Id_String));
            }
        }
        public string Id_String => FormatHelper.ConvertBillRepairID(Id, IsWarranty);

        public DateTime TimeDelivery
        {
            get => _model.timeDelivery.HasValue ? _model.timeDelivery.Value : DateTime.MinValue;
            set
            {
                _model.timeDelivery = value;
                OnPropertyChanged(nameof(TimeDelivery));
                OnPropertyChanged(nameof(TimeDelivery_String));
            }
        }
        public string TimeDelivery_String => _model.timeDelivery.HasValue ? FormatHelper.DatetimeToDateString(TimeDelivery) : null;

        public System.DateTime TimeReceive
        {
            get => _model.timeReceive.HasValue ? _model.timeReceive.Value : DateTime.MinValue;
            set
            {
                _model.timeReceive = value;
                OnPropertyChanged(nameof(TimeReceive));
                OnPropertyChanged(nameof(TimeReceive_String));
            }
        }
        public string TimeReceive_String => FormatHelper.DatetimeToDateString(TimeReceive);

        public string DesReceiveItems
        {
            get => _model.desReceiveItems;
            set
            {
                _model.desReceiveItems = value;
                OnPropertyChanged(nameof(DesReceiveItems));
            }
        }
        public string DesProblem
        {
            get => _model.desProblem;
            set
            {
                _model.desProblem = value;
                OnPropertyChanged(nameof(DesProblem));
            }
        }
        public string DesComponents
        {
            get => _model.attachments;
            set
            {
                _model.attachments = value;
                OnPropertyChanged(nameof(DesComponents));
            }
        }
        public string DetailRepair
        {
            get => _model.desDetailRepair;
            set
            {
                _model.desDetailRepair = value;
                OnPropertyChanged(nameof(DetailRepair));
            }
        }
        public long Price
        {
            get => _model.price.HasValue ? _model.price.Value : 0;
            set
            {
                _model.price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Price_String));
            }
        }
        public string Price_String => FormatHelper.ToMoney(Price);

        public long CustomerMoney
        {
            get => _model.customerMoney.HasValue ? _model.customerMoney.Value : 0;
            set
            {
                _model.customerMoney = value;
                OnPropertyChanged(nameof(CustomerMoney));
            }
        }
        public bool IsWarranty
        {
            get => _model.isWarranty;
            set
            {
                _model.isWarranty = value;
                OnPropertyChanged(nameof(IsWarranty));
            }
        }
        public int Status
        {
            get => _model.status;
            set
            {
                _model.status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(Status_String));
            }
        }
        public string Status_String => StatusToString(Status);

        public int CustomerId
        {
            get => _model.customerId;
            set
            {
                _model.customerId = value;
                OnPropertyChanged(nameof(CustomerId));
            }
        }
        public int SeriId
        {
            get => _model.seriId.HasValue ? _model.seriId.Value : -1;
            set
            {
                _model.seriId = value;
                OnPropertyChanged(nameof(SeriId));
            }
        }
        public string ProductName
        {
            get => _model.nameProduct;
        }

        public string CustomerName => _model.CUSTOMER.name;
        public string CustomerPhone => _model.CUSTOMER.phone;

        public string Type => IsWarranty ? "Bảo hành" : "Không bảo hành";
        public string ProductSeri => IsWarranty ? _model.ITEM_BILL_SERI.seri : null;
        public string TimeProductSell => IsWarranty ? FormatHelper.DatetimeToDateString(_model.ITEM_BILL_SERI.BILL.createTime) : null;
        public string ProductBillSeri => IsWarranty ? FormatHelper.ConvertBillID(_model.ITEM_BILL_SERI.billId): null;
        public string TimeWarrantyEnd
        {
            get
            {
                if (IsWarranty)
                {
                    int warrantymonth = _model.ITEM_BILL_SERI.PRODUCT.warrantyTime.HasValue ? _model.ITEM_BILL_SERI.PRODUCT.warrantyTime.Value : 0;
                    return FormatHelper.DatetimeToDateString(_model.ITEM_BILL_SERI.BILL.createTime.AddMonths(warrantymonth));
                }
                return null;
            }
        }

        public InsuranceViewModel()
        {
            _model = new BILL_REPAIR();
        }

        public InsuranceViewModel(BILL_REPAIR model)
        {
            _model = model;
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
                            b.nameProduct,
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
                            nameProduct = b.nameProduct,
                            desDetailRepair = b.desDetailRepair,
                            CUSTOMER = new CUSTOMER() { 
                                id = b.customerId, 
                                phone = b.customerPhone },
                            ITEM_BILL_SERI = b.seriId.HasValue ?
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
