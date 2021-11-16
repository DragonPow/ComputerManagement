using ComputerProject.HelperService;
using ComputerProject.Model;
using ComputerProject.Repository;
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
        BillRepository _repository;
        NavigationService _navigator;
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
                    _deleteBillCommand = new RelayCommand(_ => DeleteBill(CurrentBill));
                }
                return _deleteBillCommand;
            }
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


        public DetailBillViewModel(BILL bill)
        {
            _repository = new BillRepository();
            CurrentBill = new Model.Bill(_repository.LoadDetailBill(bill.id));
        }
        public void setNavigator(NavigationService navigator)
        {
            this._navigator = navigator;
        }

        private void ExportPDF(Model.Bill bill)
        {
            throw new NotImplementedException();
        }
        private void DeleteBill(Model.Bill bill)
        {
            NavigateBack();
            BillDeletedEvent?.Invoke(this, bill.Id);
        }
        private void NavigateBack()
        {
            if (_navigator == null) throw new ArgumentNullException("_navigator is null");
            _navigator.Back();
        }
    }
}
