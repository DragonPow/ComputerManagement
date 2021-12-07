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
            get => _model.timeDelivery.Value;
            set
            {
                _model.timeDelivery = value;
                OnPropertyChanged(nameof(TimeDelivery));
                OnPropertyChanged(nameof(TimeDelivery_String));
            }
        }
        public string TimeDelivery_String => FormatHelper.DatetimeToDateString(TimeDelivery);

        public System.DateTime TimeReceive
        {
            get => _model.timeReceive;
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
            get => "DesComponents";
            set
            {
                //_model.desProblem = value;
                OnPropertyChanged(nameof(DesComponents));
            }
        }
        public string DetailRepair
        {
            get => "DetailRepair";
            set
            {
                //_model.desProblem = value;
                OnPropertyChanged(nameof(DetailRepair));
            }
        }
        public int Price
        {
            get => _model.price.HasValue ? _model.price.Value : -1;
            set
            {
                _model.price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Price_String));
            }
        }
        public string Price_String => FormatHelper.ToMoney(Price);

        public int CustomerMoney
        {
            get => _model.customerMoney.HasValue ? _model.customerMoney.Value : -1;
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
            get => "ProductName";
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

        public static List<InsuranceViewModel> FindByPhoneOrID(string str, int startIndex, int count, int sortType)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    string nameDL = FormatHelper.ConvertTo_TiengDongLao(str);
                    var data1 = db.BILL_REPAIR.Where(p => p.CUSTOMER.phone.Contains(nameDL) || p.id.ToString().Contains(str)).Select(
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
                            customerPhone = b.CUSTOMER.phone,
                            customerName = b.CUSTOMER.name,
                            //productName = b.ITEM_BILL_SERI.nameProduct,
                            productSeri = b.ITEM_BILL_SERI.seri,
                            productWarrantyTime = b.ITEM_BILL_SERI.PRODUCT.warrantyTime,
                            billID = b.ITEM_BILL_SERI.billId,
                            billCreateDate = b.ITEM_BILL_SERI.BILL.createTime
                        }
                    );

                    if (sortType == 0)
                    {
                        data1 = data1.OrderBy(b => b.timeReceive).Skip(startIndex).Take(count);
                    }
                    else if (sortType == 1)
                    {
                        data1 = data1.OrderByDescending(b => b.timeReceive).Skip(startIndex).Take(count);
                    }
                    else if (sortType == 2)
                    {
                        data1 = data1.OrderBy(b => b.customerPhone).Skip(startIndex).Take(count);
                    }
                    else if (sortType == 3)
                    {
                        data1 = data1.OrderByDescending(b => b.customerPhone).Skip(startIndex).Take(count);
                    }

                    var data = data1.ToList();

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
                            CUSTOMER = new CUSTOMER() { id = b.customerId, phone = b.customerPhone },
                            ITEM_BILL_SERI = b.seriId.HasValue ? new ITEM_BILL_SERI() { 
                                id = b.seriId.Value, 
                                seri = b.productSeri , 
                                billId = b.billID,
                                BILL = new BILL()
                                {
                                    id = b.billID
                                }
                                } : null,
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

        public static int CountByPhoneOrID(string str)
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    string nameDL = FormatHelper.ConvertTo_TiengDongLao(str);
                    var rs = db.BILL_REPAIR.Where(p => p.CUSTOMER.phone.Contains(nameDL) || p.id.ToString().Contains(str)).Count();

                    return rs;
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
    }
}
