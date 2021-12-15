using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceBillViewModel : InsuranceViewModel
    {
        public event EventHandler SubmitOK;
        public event EventHandler ClickBack;

        SettingWorkSpace.SettingViewModel settingvm;
        public InsuranceBillViewModel()
        {
            PropertyChanged += InsuranceBillViewModel_PropertyChanged;

            Model = new BILL_REPAIR();
            settingvm = new SettingWorkSpace.SettingViewModel(false);
        }

        private void InsuranceBillViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerMoney))
            {
                OnPropertyChanged(nameof(CashBack));
            }
            if (e.PropertyName == nameof(Price))
            {
                OnPropertyChanged(nameof(CashBack));
            }
        }

        public void LoadAsync(int id)
        {
            DoBusyTask(() => LoadData(id), ()=> OnPropertyChanged(string.Empty));
        }

        public void LoadData(int id)
        {
            Model = InsuranceViewModel.GetByID(id);
            settingvm.LoadData();
        }

        public string StoreName => settingvm == null ? null : settingvm.StoreName;
        public string StoreAddress => settingvm == null ? null : settingvm.StoreAddress;
        public string StorePhone => settingvm == null ? null : settingvm.StorePhone;

        public string CashBack => CustomerMoney >= Price ? FormatHelper.ToMoney(CustomerMoney - Price) : null;

        public ICommand CommandSubmit => new RelayCommand((o) => OnSubmit());
        public ICommand CommandCancel => new RelayCommand((o) => OnBack());

        void OnBack()
        {
            ClickBack?.Invoke(this, null);
        }

        void OnSubmit()
        {
            if (CustomerMoney >= Price)
            {
                DoBusyTask(SaveCustomerMoney, () => SubmitOK?.Invoke(this, null));
            }
            else
            {
                CustomMessageBox.MessageBox.ShowError("Tiền khách đưa không thể thấp hơn giá trị hóa đơn");
            }
        }

        void SaveCustomerMoney()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var old = db.BILL_REPAIR.Where(b => b.id == Id).FirstOrDefault();
                old.customerMoney = CustomerMoney;
                old.status = 2;
                if (!old.timeDelivery.HasValue) old.timeDelivery = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}
