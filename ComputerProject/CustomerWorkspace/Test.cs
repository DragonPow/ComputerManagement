using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.CustomerWorkspace
{
    class Test:CustomerAllViewRow
    {
        public Test()
        {
            this.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
            this.btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            this.cellPoint.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, 3);

            cellName.Text = "Họ tên";
            cellPhone.Text = "Số điện thoại";
            cellAddress.Text = "Địa chỉ";
            cellBirthday.Text = "Ngày sinh";
            cellPoint.Text = "Điểm thưởng";
        }
    }
}
