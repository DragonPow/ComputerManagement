using ComputerProject.CustomMessageBox;
using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using ComputerProject.Helper;
using ComputerProject.HelperService;
using ComputerProject.Repository;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerProject.SaleWorkSpace
{
    public class BillViewModel : Helper.BaseViewModel
    {
        #region Fields
        StoreInformation _currentStoreInformation;
        Model.Bill _currentBill;
        ICommand _confirmCommand;
        ICommand _printCommand;
        ICommand _cancelCommand;
        #endregion //Fields

        #region Properties
        public StoreInformation CurrentStoreInformation
        {
            get => _currentStoreInformation;
            private set
            {
                if (value != _currentStoreInformation)
                {
                    _currentStoreInformation = value;
                    OnPropertyChanged();
                }
            }
        }
        public Model.Bill CurrentBill
        {
            get => _currentBill;
            set
            {
                if (value != _currentBill)
                {
                    _currentBill = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand ConfirmCommand
        {
            get
            {
                if (null == _confirmCommand)
                {
                    _confirmCommand = new RelayCommand(a =>
                    {
                        if (!CurrentBill.HasErrorData)
                        {
                            if (SuccessConfirm())
                            {
                                PaymentSuccessEvent?.Invoke(this, null);
                                var rs = MessageBoxCustom.ShowDialog("Thanh toán hóa đơn thành công! \nBạn có muốn in hóa đơn không?", "Thông báo", MaterialDesignThemes.Wpf.PackIconKind.QuestionAnswer);
                                if (rs == MessageBoxResultCustom.Yes) PrintPDF();

                                CloseWindow();
                            }
                            else
                            {
                                MessageBoxCustom.ShowDialog("Cập nhật lên cơ sở dữ liệu thất bại", "Lỗi", MaterialDesignThemes.Wpf.PackIconKind.Error);
                            }
                        }
                        else
                        {
                            MessageBoxCustom.ShowDialog("Yêu cầu nhập đúng và đầy đủ thông tin!", "Thông báo", MaterialDesignThemes.Wpf.PackIconKind.InformationCircle);
                        }
                    });
                }
                return _confirmCommand;
            }
        }
        public ICommand PrintCommand
        {
            get
            {
                if (null == _printCommand)
                {
                    _printCommand = new RelayCommand(a => PrintPDF());
                }
                return _printCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (null == _cancelCommand)
                {
                    _cancelCommand = new RelayCommand(_ => CloseWindow());
                }
                return _cancelCommand;
            }
        }

        public event EventHandler PaymentSuccessEvent;
        #endregion //Properties

        public BillViewModel(BILL bill)
        {
            LoadData();
            CurrentBill = new Model.Bill(bill);
        }
        public BillViewModel(IDictionary<Model.Product, int> listproduct, CUSTOMER customer, int totalMoney)
        {
            LoadData();
            CurrentBill = new Model.Bill(listproduct, customer, totalMoney);
        }

        public void LoadData()
        {
            LoadStoreInformation();
        }
        private void LoadStoreInformation()
        {
            CurrentStoreInformation = StoreRepository.GetStoreInformation();
        }
        private bool SuccessConfirm()
        {
            try
            {
                using (var db = new ComputerManagementEntities())
                {
                    db.Database.Log = Console.WriteLine;
                    BILL b = CurrentBill.CastToModel();
                    db.BILLs.AddOrUpdate(b);
                    db.Entry(b.CUSTOMER).State = System.Data.Entity.EntityState.Unchanged;

                    foreach(var item in b.ITEM_BILL)
                    {
                        var product = db.PRODUCTs.Where(i => i.id == item.productId).First();
                        product.quantity -= item.quantity;
                    }

                    db.SaveChanges();

                    Task.Run(() => AddBonusPointToCustomer(b));
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private void AddBonusPointToCustomer(BILL b)
        {
            using (var db = new ComputerManagementEntities())
            {
                int moneyToPoint = int.Parse(db.REGULATIONs.Where(i => i.name == "MoneyToPoint").Select(i => i.value).First());
                var child = db.CUSTOMERs.Where(i => i.id == b.customerId).First();
                child.point += b.totalMoney / moneyToPoint;

                db.SaveChanges();

                Console.WriteLine("Add {0} point for {1} success", b.totalMoney / moneyToPoint, b.CUSTOMER.name);
            }
        }
        private void PrintPDF()
        {
            var p = new Helper.PrintPDF();
            p.createBill(CurrentBill);
        }
        private void CloseWindow()
        {
            var view = WindowService.FindWindowbyTag(nameof(BillViewModel))[0];
            view.Close();
        }
    }
}
