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

        #region Fields
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
        string _currentSeriId;
        List<ITEM_BILL_SERI> _listSearchSeri;

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
        ICommand _tranferToEditModeCommand;
        #endregion //Fields

        #region Properties
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
            get => InsuranceViewModel.StatusToString(CurrentBill.status);//_statusBillSelected;
            set
            {
                if (value != StatusBillSelected)
                {
                    //_statusBillSelected = value;
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
        //public string BillID => CurrentBill.ITEM_BILL_SERI?.id.ToString();
        public long? ExcessCashMoney => CurrentBill.customerMoney - CurrentBill.price;
        public CUSTOMER CurrentCustomer => CurrentBill.CUSTOMER;
        public string CurrentSeriId
        {
            get => _currentSeriId;
            set
            {
                if (value != _currentSeriId)
                {
                    _currentSeriId = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<ITEM_BILL_SERI> ListSearchSeri
        {
            get => _listSearchSeri;
            set
            {
                if (value != _listSearchSeri)
                {
                    _listSearchSeri = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand PaymentCommand
        {
            get
            {
                if (_paymentCommand == null)
                {
                    _paymentCommand = new RelayCommand(async _ =>
                    {
                        //Must check seri before save
                        if (!isCheckSeri())
                        {
                            var rs = MessageBoxCustom.ShowDialog("Vui lòng kiểm tra mã Seri trước khi thực hiện thao tác", "Thông báo", PackIconKind.InformationCircleOutline);
                            return;
                        }
                        if (!CurrentBill.price.HasValue)
                        {
                            var rs = MessageBoxCustom.ShowDialog("Vui lòng nhập số tiền sửa chữa", "Thông báo", PackIconKind.InformationCircleOutline);
                            return;
                        }
                        if (CurrentBill.HasErrorData)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng điền đầy đủ thông tin trước khi thanh toán", "Thông báo", PackIconKind.InformationCircleOutline);
                            return;
                        }

                        if (Status != StatusView.View)
                        {
                            var success = await Save(CurrentBill);
                            if (!success) MessageBoxCustom.ShowDialog("Không thể thực hiện thanh toán này", "Thông báo", PackIconKind.InformationCircleOutline);
                        }
                        OpenPaymentView(CurrentBill);
                    });
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
                        //Must check seri before save
                        if (!isCheckSeri())
                        {
                            var rs = MessageBoxCustom.ShowDialog("Vui lòng kiểm tra mã Seri trước khi thực hiện thao tác", "Thông báo", PackIconKind.InformationCircleOutline);
                            return;
                        }

                        if (CurrentBill.HasErrorData)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng điền đầy đủ thông tin trước khi lưu", "Thông báo", PackIconKind.InformationCircleOutline);
                            return;
                        }
                        var success = await Save(CurrentBill);
                        if (success)
                        {
                            MessageBoxCustom.ShowDialog("Cập nhật thành công", "Thông báo", PackIconKind.InformationCircleOutline);
                            Status = StatusView.View;
                            //OnNavigateBack();
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
                        if (!String.IsNullOrWhiteSpace(CurrentSeriId))
                        {
                            CheckSeriWithException();
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
                    _navigateBackCommand = new RelayCommand(_ =>
                    {
                        if (Status == StatusView.Edit) 
                        {
                            var rs = MessageBoxCustom.ShowDialog("Hủy bỏ thao tác lưu", "Thông báo", PackIconKind.QuestionMarkCircleOutline);
                            if (rs == MessageBoxResultCustom.No) return;
                        }
                        OnNavigateBack();
                    });
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
        public ICommand TranferToEditModeCommand
        {
            get
            {
                if (_tranferToEditModeCommand == null)
                {
                    _tranferToEditModeCommand = new RelayCommand(_ =>
                    {
                        Status = StatusView.Edit;
                    });
                }
                return _tranferToEditModeCommand;
            }
        }

        public event EventHandler NavigateBack; 
        #endregion //Properties


        public InsuranceDetailViewModel(BILL_REPAIR bill = null, StatusView status = StatusView.Add)
        {
            _currentBill = bill ?? new BILL_REPAIR() { timeReceive = DateTime.Now };
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
            var insurTask = BusyService.GetTask(LoadInformationInsurance, LoadCustomer);
            await insurTask;
        }

        public Task LoadDataAsync()
        {
            var insurTask = BusyService.GetTask(LoadInformationInsurance, LoadCustomer);
            return insurTask;
        }

        private void LoadInformationInsurance()
        {
            if (CurrentBill.id != 0)
            {
                _repository.LoadInsurance(CurrentBill);

                if (CurrentBill.ITEM_BILL_SERI != null)
                {
                    ListSearchSeri = new List<ITEM_BILL_SERI>();
                    ListSearchSeri.Add(CurrentBill.ITEM_BILL_SERI);
                    CurrentSeriId = CurrentBill.ITEM_BILL_SERI.seri;
                }
                
                OnPropertyChanged(nameof(CurrentBill));

                int statusPayment = 2;
                if (CurrentBill.status != statusPayment) BillStatus.RemoveAt(statusPayment);
            }
        }
        private void LoadCustomer()
        {
            CurrentBill.CUSTOMER = _repository.GetCustomer(CurrentBill.customerId) ?? new CUSTOMER();
            OnPropertyChanged(nameof(CurrentCustomer));
        }
        public void OnCustomerChanged()
        {
            OnPropertyChanged(nameof(CurrentCustomer));
        }
        public void OnItemBillChanged()
        {
            CurrentBill.seriId = CurrentBill.ITEM_BILL_SERI?.id;
            OnPropertyChanged(nameof(TimeWarranty));

            if (!TimeWarranty.HasValue)
            {
                throw new NoneTimeWarrantyTimeException();
            }
            else if (TimeWarranty.Value.Date < DateTime.Now)
            {
                throw new ExpiryWarrantyException();
            }
        }
        public void OnNavigateBack()
        {
            NavigateBack?.Invoke(this, null);
        }

        private void OpenPaymentView(BILL_REPAIR currentBill)
        {
            var view = new InsuranceBillView();

            InsuranceBillViewModel vm = new InsuranceBillViewModel();

            void CloseWindow() {
                var window = WindowService.FindWindowbyTag("PaymentInsuranceBill").First();
                window.Close();
            }

            vm.SubmitOK += (s, e) =>
              {
                  CloseWindow();
                  MessageBoxCustom.ShowDialog("Thanh toán thành công", "Thông báo", PackIconKind.DoneOutline);
                  this.NavigateBack?.Invoke(this, null);
              };
            vm.ClickBack += (s, e) =>
             {
                 var rs = MessageBoxCustom.ShowDialog("Bạn có chắc hủy bỏ thanh toán hay không?", "Thông báo", PackIconKind.QuestionMarkCircleOutline);
                 if (rs == MessageBoxResultCustom.Yes) CloseWindow();
             };

            vm.LoadAsync(CurrentBill.id);
            //void close(object sender, EventArgs e)
            //{
            //    var l = WindowService.FindWindowbyTag(tag);
            //    if (l != null && l.Count > 0)
            //    {
            //        l[0].Close();
            //    }
            //}
            //vm.SubmitOK += (s, e) =>
            //{
            //    CustomMessageBox.MessageBox.ShowNotify("Thanh toán thành công");
            //    //close(s, e);
            //    CancelCommand.Execute(this);
            //};
            //vm.ClickBack += close;
            WindowService.ShowSingelWindow(vm, view);
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
            PrintPDF.Instance.createBillInsur(currentBill, true);
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
            ListSearchSeri = _repository.GetBillFromSeri(seri);

            if (ListSearchSeri == null || ListSearchSeri.Count == 0)
            {
                throw new NotFoundSeriException();
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
        private bool CheckSeriWithException()
        {
            try
            {
                CheckSeri(CurrentSeriId);
                return true;
            }
            catch (NotFoundSeriException)
            {
                MessageBoxCustom.ShowDialog("Không tìm thấy sản phẩm tương ứng với mã số seri", "Thông báo", PackIconKind.InformationCircleOutline);
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }
        public bool isCheckSeri()
        {
            return (CurrentBill.ITEM_BILL_SERI == null && String.IsNullOrWhiteSpace(CurrentSeriId)) ||
                    (CurrentBill.ITEM_BILL_SERI != null && CurrentSeriId == CurrentBill.ITEM_BILL_SERI?.seri);
        }
    }

    public class NoneTimeWarrantyTimeException : Exception { }
    public class ExpiryWarrantyException : Exception { }
    public class NotFoundSeriException : Exception { }
}
