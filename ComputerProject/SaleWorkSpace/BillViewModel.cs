using ComputerProject.CustomMessageBox;
using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using ComputerProject.Helper;
using ComputerProject.HelperService;

namespace ComputerProject.SaleWorkSpace
{
    public class BillViewModel : Helper.BaseViewModel
    {
        Model.Bill _currentBill;
        ICommand _confirmCommand;
        ICommand _printCommand;


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
                        if (!HasErrorData())
                        {
                            if (SuccessConfirm())
                            {
                                MessageBoxCustom.ShowDialog("Thanh toán hóa đơn thành công", "Thông báo", MaterialDesignThemes.Wpf.PackIconKind.Done);
                                var view = WindowService.FindWindowbyTag(nameof(BillViewModel))[0];
                                view.Close();
                            }
                        }
                        else
                        {
                            MessageBoxCustom.ShowDialog("Số tiền phải là số dương", "Lỗi", MaterialDesignThemes.Wpf.PackIconKind.Error);
                        }
                    });
                }
                return _confirmCommand;
            }
        }

        private bool HasErrorData()
        {
            return false;
        }

        public ICommand PrintCommand
        {
            get
            {
                if (null == _printCommand)
                {
                    _printCommand = new RelayCommand(a => Print());
                }
                return _printCommand;
            }
        }

        public BillViewModel(BILL bill)
        {
            CurrentBill = new Model.Bill(bill);
        }
        public BillViewModel(IDictionary<Model.Product, int> listproduct, CUSTOMER customer, int totalMoney)
        {
            CurrentBill = new Model.Bill(listproduct, customer, totalMoney);
        }

        private bool SuccessConfirm()
        {
            //try
            {
                using (var db = new ComputerManagementEntities())
                {
                    db.BILLs.Add(CurrentBill.CastToModel());
                    db.SaveChanges();
                }
                return true;
            }
            //catch (Exception e)
            //{
            //    MessageBoxCustom.ShowDialog("Có lỗi xảy ra khi thực hiện thao tác lưu", "Lỗi", MaterialDesignThemes.Wpf.PackIconKind.Error);
            //    return false;
            //}
        }
        private void Print()
        {
            throw new NotImplementedException();
        }
    }
}
