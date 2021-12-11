using ComputerProject.CustomerWorkspace;
using ComputerProject.CustomMessageBox;
using ComputerProject.Helper;
using ComputerProject.HelperService;
using ComputerProject.Repository;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.InsuranceWorkSpace
{
    public enum TypeObject
    {
        Customer,
        Product,
        InforRepair,
    }
    public class InsuranceDetailViewModel : Helper.BaseViewModel
    {
        public enum StatusView
        {
            Add,
            Edit,
            View,
        }

        readonly BILL_REPAIR _currentBill;
        DateTime? _timeWarranty;
        StatusView _status;
        readonly BusyViewModel _busyService = new BusyViewModel();
        readonly InsuranceRepository _repository = new InsuranceRepository();
        List<String> _billStatus = new List<String>() {
            InsuranceViewModel.StatusToString(0),
            InsuranceViewModel.StatusToString(1),
            InsuranceViewModel.StatusToString(2)
            };
        string _statusBillSelected;
        Collection<CUSTOMER> _listSearchCustomer;

        ICommand _paymentCommand;
        ICommand _saveCommand;
        ICommand _cancelCommand;
        ICommand _deleteCommand;
        ICommand _exportCommand;
        ICommand _clearObjectCommand;
        ICommand _checkSeriCommand;
        ICommand _navigateBackCommand;
        ICommand _searchCustomerCommand;
        ICommand _addCustomerCommand;


        public BILL_REPAIR CurrentBill
        {
            get => _currentBill;
            //set
            //{
            //    if (value != _currentBill)
            //    {
            //        _currentBill = value;
            //        OnPropertyChanged();
            //    }
            //}
        }
        public DateTime? TimeWarranty => CurrentBill.ITEM_BILL_SERI?.warrantyEndTime;
        //{
        //    get => _timeWarranty;
        //    private set
        //    {
        //        if (value != _timeWarranty)
        //        {
        //            _timeWarranty = value;

        //            OnPropertyChanged();
        //        }
        //    }
        //}
        public StatusView Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }
        public BusyViewModel BusyService => _busyService;
        public List<String> BillStatus => _billStatus;
        public string StatusBillSelected
        {
            get => _statusBillSelected;
            set
            {
                if (value != _statusBillSelected)
                {
                    _statusBillSelected = value;
                    CurrentBill.status = InsuranceViewModel.StringToStatus(value);
                    OnPropertyChanged();
                }
            }
        }
        public Collection<CUSTOMER> ListSearchCustomer
        {
            get => _listSearchCustomer;
            set
            {
                if (value != _listSearchCustomer)
                {
                    _listSearchCustomer = value;
                    OnPropertyChanged();
                }
            }
        }
        public string BillID => CurrentBill.ITEM_BILL_SERI?.id.ToString();
        public long? ExcessCashMoney => CurrentBill.customerMoney - CurrentBill.price;
        public CUSTOMER CurrentCustomer => CurrentBill.CUSTOMER;

        public ICommand PaymentCommand
        {
            get
            {
                if (_paymentCommand == null)
                {
                    _paymentCommand = new RelayCommand(_ => OpenPaymentView(CurrentBill));
                }
                return _paymentCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(async _ =>
                    {
                        var success = await Save(CurrentBill);
                        if (success)
                        {
                            OnNavigateBack();
                        }
                        else
                        {
                            MessageBoxCustom.ShowDialog("Không thể thực hiện thao tác lưu", "Lỗi", PackIconKind.ErrorOutline);
                        }
                    });
                }
                return _saveCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(_ =>
                    {
                        if (Status == StatusView.Add || Status == StatusView.Edit)
                        {
                            var rs = MessageBoxCustom.ShowDialog("Mọi thao tác chưa được lưu sẽ bị mất, đồng ý trở về", "Thông báo", PackIconKind.InformationCircleOutline);
                            if (rs == MessageBoxResultCustom.No) return;
                        }
                        Cancel();
                    });
                }
                return _cancelCommand;
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(async _ =>
                    {
                        var confirmDelete = MessageBoxCustom.ShowDialog("Xác nhận xóa hóa đơn", "Thông báo", PackIconKind.InformationCircleOutline);
                        if (confirmDelete == MessageBoxResultCustom.No) return;

                        bool success = await Delete(CurrentBill);

                        if (success)
                        {
                            OnNavigateBack();
                        }
                        else
                        {
                            MessageBoxCustom.ShowDialog("Xảy ra lỗi khi thực hiện thao tác xóa", "Lỗi", PackIconKind.ErrorOutline);
                        }
                    });
                }
                return _deleteCommand;
            }
        }
        public ICommand ExportCommand
        {
            get
            {
                if (_exportCommand == null)
                {
                    _exportCommand = new RelayCommand(_ => ExportPDF(CurrentBill));
                }
                return _exportCommand;
            }
        }
        public ICommand ClearObjectCommand
        {
            get
            {
                if (_clearObjectCommand == null)
                {
                    _clearObjectCommand = new RelayCommand(objecttype => ClearObject((TypeObject)objecttype));
                }
                return _clearObjectCommand;
            }
        }
        public ICommand CheckSeriCommand
        {
            get
            {
                if (_checkSeriCommand == null)
                {
                    _checkSeriCommand = new RelayCommand(s =>
                    {
                        try
                        {
                            CheckSeri(s as string);
                        }
                        catch (NoneTimeWarrantyTimeException)
                        {
                            MessageBoxCustom.ShowDialog("Đơn hàng này không có thời hạn bảo hành", "Thông báo", PackIconKind.InformationCircleOutline);
                        }
                        catch (ExpiryWarrantyException)
                        {
                            MessageBoxCustom.ShowDialog("Đã hết thời gian bảo hành cho món hàng này", "Thông báo", PackIconKind.InformationCircleOutline);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    });
                }
                return _checkSeriCommand;
            }
        }
        public ICommand NavigateBackCommand
        {
            get
            {
                if (_navigateBackCommand == null)
                {
                    _navigateBackCommand = new RelayCommand(_ => OnNavigateBack());
                }
                return _navigateBackCommand;
            }
        }
        public ICommand SearchCustomerCommand
        {
            get
            {
                if (null == _searchCustomerCommand)
                {
                    _searchCustomerCommand = new RelayCommand(s => SearchCustomerbyPhone(s as string));
                }
                return _searchCustomerCommand;
            }
        }
        public ICommand AddCustomerCommand
        {
            get
            {
                if (null == _addCustomerCommand)
                {
                    _addCustomerCommand = new RelayCommand(a => OpenAddCustomerView());
                }
                return _addCustomerCommand;
            }
        }

        public event EventHandler NavigateBack;


        public InsuranceDetailViewModel(BILL_REPAIR bill = null, StatusView status = StatusView.Add)
        {
            _currentBill = bill ?? new BILL_REPAIR() { timeReceive = DateTime.Now};
            OnPropertyChanged(nameof(CurrentBill));

            Status = status;
            StatusBillSelected = InsuranceViewModel.StatusToString(_currentBill.status);
        }

        public InsuranceDetailViewModel(int id, StatusView status)
        {
            _currentBill = new BILL_REPAIR() { id = id, timeReceive = DateTime.Now };
            OnPropertyChanged(nameof(CurrentBill));

            LoadDataAsync();

            Status = status;
            StatusBillSelected = InsuranceViewModel.StatusToString(_currentBill.status);
        }

        public async void LoadData()
        {
            var customerTask = Task.Run(LoadCustomer);
            var insurTask = Task.Run(LoadInformationInsurance);

            await BusyService.WhenAll(customerTask, insurTask);
            //BusyService.GetTask(Task.WhenAll(customerTask, productTask, insurTask));
        }

        public Task LoadDataAsync()
        {
            //var customerTask = Task.Run(LoadCustomer);
            var insurTask = Task.Run(LoadInformationInsurance).ContinueWith((_) => Task.Run(LoadCustomer));

            return insurTask;
            //return BusyService.WhenAll(customerTask, insurTask);
        }

        private void LoadInformationInsurance()
        {
            if (CurrentBill.id != 0)
            {
                _repository.LoadInsurance(CurrentBill);
            }
        }
        //{
        //    get => c;
        //    set
        //    {
        //        c = value;
        //        OnPropertyChanged();
        //    }
        //}
        private void LoadCustomer()
        {
            CurrentBill.CUSTOMER = _repository.GetCustomer(CurrentBill.customerId) ?? new CUSTOMER();
        }
        public void OnCustomerChanged()
        {
            OnPropertyChanged(nameof(CurrentCustomer));
        }
        public void OnNavigateBack()
        {
            NavigateBack?.Invoke(this, null);
        }

        private void OpenPaymentView(BILL_REPAIR currentBill)
        {
            throw new NotImplementedException();
        }
        private Task<bool> Save(BILL_REPAIR currentBill)
        {
            return BusyService.GetTask(_repository.SaveAsync(currentBill));
        }
        private async void Cancel()
        {
            await LoadDataAsync();
            OnNavigateBack();
        }
        private Task<bool> Delete(BILL_REPAIR currentBill)
        {
            return BusyService.GetTask(_repository.Delete(currentBill.id));
        }
        private void ExportPDF(BILL_REPAIR currentBill)
        {
            //Helper.PrintPDF.Instance.createBill(currentBill, true);
        }
        private void ClearObject(TypeObject objectType)
        {
            switch (objectType)
            {
                case TypeObject.Customer:
                    CurrentBill.CUSTOMER = null;
                    OnPropertyChanged(nameof(CurrentCustomer));
                    break;
                case TypeObject.Product:
                    CurrentBill.nameProduct = null;
                    CurrentBill.seriId = null;
                    CurrentBill.ITEM_BILL_SERI = null;
                    CurrentBill.attachments = null;
                    OnPropertyChanged(nameof(CurrentBill));
                    break;
                case TypeObject.InforRepair:
                    CurrentBill.timeDelivery = null;
                    CurrentBill.timeReceive = null;
                    CurrentBill.desDetailRepair = null;
                    CurrentBill.desProblem = null;
                    CurrentBill.desReceiveItems = null;
                    CurrentBill.customerMoney = null;
                    CurrentBill.price = null;
                    OnPropertyChanged(nameof(CurrentBill));
                    break;
                default:
                    throw new NullReferenceException();
            }
        }
        private void CheckSeri(string seri)
        {
            CurrentBill.ITEM_BILL_SERI = _repository.GetBillFromSeri(seri);
            // TODO: Get TimeWarranty from item_bill_seri
            OnPropertyChanged(nameof(TimeWarranty));
            OnPropertyChanged(nameof(BillID));

            if (!TimeWarranty.HasValue)
            {
                throw new NoneTimeWarrantyTimeException();
            }
            else if (TimeWarranty.Value.Date < DateTime.Now)
            {
                throw new ExpiryWarrantyException();
            }
        }
        private void SearchCustomerbyPhone(string phone)
        {
            int number = 5;
            var repo = new SaleRepository();
            ListSearchCustomer = repo.SearchCustomer(phone, number);
        }
        private void OpenAddCustomerView()
        {
            var vm = new CustomerViewModel(true);
            WindowService.ShowWindow(vm, new CustomerAdd());
        }

        public class NoneTimeWarrantyTimeException : Exception { }
        public class ExpiryWarrantyException : Exception { }
        public class NotFoundSeriException : Exception { }
    }
}
