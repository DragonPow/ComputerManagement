using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject
{
    public partial class CUSTOMER
    {
        public DateTime formatedBirthday
        {
            get => FormatHelper.FormatDateText(birthday);
        }
    }
}
