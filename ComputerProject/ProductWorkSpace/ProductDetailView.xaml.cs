using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerProject.ProductWorkSpace
{
    /// <summary>
    /// Interaction logic for ProductDetailView.xaml
    /// </summary>
    public partial class ProductDetailView : UserControl
    {
        public ProductDetailView()
        {
            InitializeComponent();
        }

        private void Btnedit_Click(object sender, RoutedEventArgs e)
        {
            if (Btnedit.Content.ToString() == "Chỉnh sửa") Enable();
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            DisEnable();
        }
        private void Enable()
        {
            
            Specifications.IsEnabled = true;
            Title.Text = "Chỉnh sửa sản phẩm";
            ProInfor.IsEnabled = true;
            BtnAdd.Visibility = TblChoosseImage.Visibility =  BtnAddImage.Visibility = Visibility.Visible;
            BtnDel.Content = "Hủy";
            BtnDel.Foreground = BtnDel.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#0477BF");
            Btnedit.Content = "Cập nhật";
        }
        private void DisEnable()
        {
            Specifications.IsEnabled = false;
            Title.Text = "Chi tiết sản phẩm";
            ProInfor.IsEnabled = false;
            BtnAdd.Visibility =  TblChoosseImage.Visibility = BtnAddImage.Visibility = Visibility.Hidden;
            BtnDel.Content = "Xóa";
            BtnDel.Foreground = BtnDel.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#EE5E5E");
            Btnedit.Content = "Chỉnh sửa";

        }

       
    }
}
