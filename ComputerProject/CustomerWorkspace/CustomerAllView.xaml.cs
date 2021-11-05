using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerAllView.xaml
    /// </summary>
    public partial class CustomerAllView : UserControl
    {
        public CustomerAllViewModel ViewModel => this.DataContext as CustomerAllViewModel;

        public CustomerAllView()
        {
            InitializeComponent();
        }

        public event EventHandler ClickedCreate;
        /// <summary>
        /// Sender is an instance of CustomerAllViewRow
        /// </summary>
        public event EventHandler ClickedEditItem;
        /// <summary>
        /// Sender is an instance of CustomerAllViewRow
        /// </summary>
        public event EventHandler ClickedDetailItem;

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            ClickedCreate?.Invoke(this, e);
        }

        private void BtnEditItem_Click(object sender, EventArgs e)
        {
            ClickedEditItem?.Invoke(sender, e);
        }

        private void Item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClickedDetailItem?.Invoke(sender, e);
        }

        private void BtnDeleteItem_Click(object sender, EventArgs e)
        {
            var msb = new CustomMessageBox.MessageBox("Bạn muốn xóa khách hàng?" + Environment.NewLine + "Dữ liệu đã xóa sẽ không thể hoàn tác.", "", "Tôi hiểu", "Hủy", MaterialDesignThemes.Wpf.PackIconKind.Warning
                , () =>
                {
                    Delete((sender as CustomerAllViewRow).ViewModel);
                });
            msb.ShowDialog();
        }

        private void Delete(CustomerViewModel item)
        {
            try
            {
                ViewModel.Delete(item);
            }
            catch (Exception) when (!HelperService.Environment.IsDebug)
            {
                ViewModel.IsBusy = false;
                CustomMessageBox.MessageBox.Show(FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01"));
            }
        }

        private void MockData()
        {
            var list = new List<CustomerViewModel>
            {
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
                new CustomerViewModel(new CUSTOMER(){
                    name = "Nguyen Pham Duy Bang",
                    phone = "096590308",
                    birthday = "30/08/2001",
                    address = "to 4, ap Thanh khuong, thi tran Tan Quoi, huyen Binh Tan, tinh Vinh Long",
                    point = 9000
                }),
            };

            DataContext = new CustomerAllViewModel(list);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SearchContent = (sender as TextBox).Text;
        }
    }
}
