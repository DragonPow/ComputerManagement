using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerProject.CustomerWorkspace
{
    class CustomerAllRowTitle : CustomerAllViewRow
    {
        public CustomerAllRowTitle()
        {
            this.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
            this.btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            this.cellPoint.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, 3);

            cellName.Text = "Họ tên";
            cellPhone.Text = "Số điện thoại";
            cellAddress.Text = "Địa chỉ";
            cellBirthday.Text = "Ngày sinh";
            cellPoint.Text = "Điểm thưởng";

            this.SetValue(System.Windows.Controls.UserControl.BackgroundProperty, new SolidColorBrush(Color.FromArgb(255, 247, 247, 254)));
        }
    }
}
