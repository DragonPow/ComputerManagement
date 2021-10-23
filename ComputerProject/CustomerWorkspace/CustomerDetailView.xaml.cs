
using System.Windows.Controls;
using System.Windows.Media;

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerDetailView.xaml
    /// </summary>
    public partial class CustomerDetailView : UserControl
    {
        public CustomerDetailView()
        {
            InitializeComponent();
            
        }

        private void BtnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Enable();
        }
        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DisEnable();
        }
        private void BtnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (BtnDelete.Content.ToString() == "Hủy")
            {
                Name.Text = Phone.Text = Adress.Text = Point.Text = " ";
                Birthday.Text = "";
            }
        }
        private void Enable()
        {
            CusInfor.IsEnabled = true;
            BtnDelete.Content = "Hủy";
            BtnDelete.Foreground = BtnDelete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#0477BF");
            BtnEdit.Content = "Cập nhật";
            Title.Text = "Chỉnh sửa khách hàng";
        }
        private void DisEnable()
        {
            CusInfor.IsEnabled = false;
            BtnDelete.Content = "Xóa";
            BtnDelete.Foreground = BtnDelete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#EE5E5E");
            BtnEdit.Content = "Chỉnh sửa";
            Title.Text = "Chi tiết khách hàng";
        }

        
    }
}
