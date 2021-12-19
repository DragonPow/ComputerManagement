using ComputerProject.CustomMessageBox;
using ComputerProject.HelperService;
using ComputerProject.Model;
using ComputerProject.Repository;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.BillWorkSpace
{
    public class DetailBillViewModel : BaseViewModel
    {
        #region Fields
        BillRepository _repository = new BillRepository();
        NavigationService _navigator;
        public BusyViewModel BusyService { get; private set; } = new BusyViewModel();

        Model.Bill _bill;

        ICommand _exportBillCommand;
        ICommand _deleteBillCommand;
        ICommand _backViewCommand;
        #endregion //Fields


        #region Properties
        public Bill CurrentBill
        {
            get => _bill;
            set
            {
                if (value != _bill)
                {
                    _bill = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand ExportBillCommand
        {
            get
            {
                if (null == _exportBillCommand)
                {
                    _exportBillCommand = new RelayCommand(_ => ExportPDF(CurrentBill));
                }
                return _exportBillCommand;
            }
        }
        public ICommand DeleteBillComamnd
        {
            get
            {
                if (null == _deleteBillCommand)
                {
                    _deleteBillCommand = new RelayCommand(_ =>
                    {
                        if (isContainsRepairBill(CurrentBill.Id))
                        {
                            MessageBoxCustom.ShowDialog("Hóa đơn này chứa sản phẩm đang bảo hành, vui lòng xóa hóa đơn bảo hành trước", "Lỗi", PackIconKind.ErrorOutline);
                            return;
                        }
                        var rs = MessageBoxCustom.ShowDialog("Hóa đơn bị xóa sẽ không thể hoàn tác", "Thông báo", PackIconKind.InformationCircle);
                        if (rs == MessageBoxResultCustom.Yes)
                        {
                            BusyService.DoBusyTask(() => DeleteBill(CurrentBill),
                                () =>
                                {
                                    MessageBoxCustom.ShowDialog("Xóa hóa đơn thành công", "Thông báo", PackIconKind.DoneOutline);
                                    NavigateBack();
                                });
                        }
                    });
                }
                return _deleteBillCommand;
            }
        }

        private bool isContainsRepairBill(int billid)
        {
            return _repository.CheckHaveRepairBill(billid);
        }

        public ICommand BackViewCommand
        {
            get
            {
                if (null == _backViewCommand)
                {
                    _backViewCommand = new RelayCommand(_ => NavigateBack());
                }
                return _backViewCommand;
            }
        }

        public event EventHandler<int> BillDeletedEvent = null;
        #endregion //Properties

        public DetailBillViewModel(int billId, System.Windows.Controls.UserControl control)
        {
            LoadAsyncData(billId);
            //CurrentBill = new Model.Bill(_repository.LoadDetailBill(billId));

            var commandClose = new RelayCommand((o) =>
            {
                System.Windows.Window.GetWindow(control).Close();
            });
            _backViewCommand = commandClose;
        }

        public DetailBillViewModel(int billId)
        {
            LoadAsyncData(billId);
        }

        public void setNavigator(NavigationService navigator)
        {
            this._navigator = navigator;
        }
        public void LoadAsyncData(int billId)
        {
            BusyService.DoBusyTask(() => LoadData(billId));
        }
        public void LoadData(int billId)
        {
            CurrentBill = new Model.Bill(_repository.LoadDetailBill(billId));
        }

        private void ExportPDF(Model.Bill bill)
        {
            var p = new Helper.PrintPDF();
            p.createBill(bill, true);
        }
        private void DeleteBill(Model.Bill bill)
        {
            _repository.Delete(CurrentBill);
            BillDeletedEvent?.Invoke(this, bill.Id);
        }
        private void NavigateBack()
        {
            if (_navigator == null) throw new ArgumentNullException("_navigator is null");
            _navigator.Back();
        }
    }
}
