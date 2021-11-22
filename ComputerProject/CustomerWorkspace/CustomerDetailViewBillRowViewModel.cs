using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerDetailViewBillRowViewModel: BaseViewModel
    {
        public string BillId { get; set; }
        public DateTime BillDay { get; set; }
        public string BillType { get; set; }
        public int BillTotalMoney { get; set; }

        public string BillTotalMoney_String => FormatHelper.ToMoney(BillTotalMoney);
        public string BillDay_String => BillDay.ToString("dd/MM/yyyy");
    }
}
