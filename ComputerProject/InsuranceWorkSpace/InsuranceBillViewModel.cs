using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceBillViewModel : InsuranceViewModel
    {
        SettingWorkSpace.SettingViewModel settingvm;
        public InsuranceBillViewModel()
        {
            settingvm = new SettingWorkSpace.SettingViewModel();
        }

        public string StoreName => settingvm.StoreName;
        public string StoreAddress => settingvm.StoreAddress;
        public string StorePhone => settingvm.StorePhone;

        public string CashBack => FormatHelper.ToMoney(CustomerMoney - Price);
    }
}
