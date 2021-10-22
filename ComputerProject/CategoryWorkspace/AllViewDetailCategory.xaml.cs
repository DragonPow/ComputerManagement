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

namespace ComputerProject.CategoryWorkspace
{
    /// <summary>
    /// Interaction logic for AllViewDetailCategory.xaml
    /// </summary>
    public partial class AllViewDetailCategory : UserControl
    {
        public AllViewDetailCategory()
        {
            InitializeComponent();
            Title.OnBack += Title_OnBack;
        }

        private void Title_OnBack(object sender, EventArgs e)
        {
            if (BtnEdit.Content.ToString() != "Chỉnh sửa")
                UIDetailCategory();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (BtnEdit.Content.ToString() == "Chỉnh sửa")
            {
                UiEditCategory();
            }
        }

        private void UiEditCategory()
        {
            Btn_Add_Catelv2.Visiblebtn();
            BtnAddSpeci.Visiblebtn();
            NameCategory.IsEnabled = !NameCategory.IsEnabled;
            ListCategoryLv2.IsEnabled = !ListCategoryLv2.IsEnabled;
            ListProperties.IsEnabled = !ListProperties.IsEnabled;
            Title.ChangeTitle("Chỉnh sửa danh mục");
            BtnDelete.Content = "Hủy";
            BtnDelete.Foreground = BtnDelete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#0477BF");
            BtnEdit.Content = "Lưu";
        }

        private void UIDetailCategory()
        {
            Btn_Add_Catelv2.Visiblebtn();
            BtnAddSpeci.Visiblebtn();
            NameCategory.IsEnabled = !NameCategory.IsEnabled;
            ListCategoryLv2.IsEnabled = !ListCategoryLv2.IsEnabled;
            ListProperties.IsEnabled = !ListProperties.IsEnabled;
            Title.ChangeTitle("Chi tiết danh mục");
            BtnDelete.Content = "Xóa";
            BtnDelete.Foreground = BtnDelete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#EE5E5E");
            BtnEdit.Content = "Chỉnh sửa";
        }
 
    }
}
