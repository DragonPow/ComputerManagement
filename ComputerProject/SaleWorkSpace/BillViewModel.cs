using ComputerProject.CustomMessageBox;
using ComputerProject.HelperService;
using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.SaleWorkSpace
{
    public class BillViewModel : BaseViewModel
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
                    _confirmCommand = new RelayCommand(a => Confirm());
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
                    _printCommand = new RelayCommand(a => Print());
                }
                return _printCommand;
            }
        }

        public BillViewModel(BILL bill)
        {
            CurrentBill = new Model.Bill(bill);
        }
        public BillViewModel(IDictionary<Model.Product, int> listproduct, CUSTOMER customer)
        {
            CurrentBill = new Model.Bill(listproduct, customer);
        }

        private void Confirm()
        {
            try
            {
                using (var db = new ComputerManagementEntities())
                {
                    db.BILLs.Add(CurrentBill.CastToModel());
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBoxCustom.ShowDialog("Có lỗi xảy ra khi thực hiện thao tác lưu", "Lỗi", MaterialDesignThemes.Wpf.PackIconKind.Error);
            }
        }
        private void Print()
        {
            throw new NotImplementedException();
        }
    }
}
