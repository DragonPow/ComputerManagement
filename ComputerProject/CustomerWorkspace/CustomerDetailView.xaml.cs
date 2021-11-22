
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerDetailView.xaml
    /// </summary>
    public partial class CustomerDetailView : UserControl
    {
        public CustomerDetailViewModel ViewModel => DataContext as CustomerDetailViewModel;
        CustomerDetailViewModel oldVM = null;

        public CustomerDetailView()
        {
            InitializeComponent();

            if (DataContext == null)
            {
                DataContext = new CustomerDetailViewModel();
            }

            SwitchMode_readonly();
            BtnBack.Click += OnBack;
        }

        public CustomerDetailView(int state) : base()
        {
            if (state == 2)
            {
                this.OnStartEdit(null, null);
            }
        }

        public event EventHandler ClickedBack;
        public event EventHandler DeletedOK;
        public event EventHandler EditOK;

        private void OnSaveEdit(object sender, System.Windows.RoutedEventArgs e)
        {
            Save();
        }

        private void OnCancelEdit(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DataContext = oldVM;
            oldVM = null;
            SwitchMode_readonly();
        }

        public void OnStartEdit(object sender, System.Windows.RoutedEventArgs e)
        {
            oldVM = ViewModel;
            var temp = new CustomerDetailViewModel(new CUSTOMER());
            ViewModel.CopyTo(temp as CustomerViewModel, true);

            DataContext = temp;
            SwitchMode_edit();
        }

        private void OnDelete(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = ViewModel;
            if (!item.HasInBill())
            {
                var msb = new CustomMessageBox.MessageBox("Bạn muốn xóa khách hàng?" + Environment.NewLine + "Dữ liệu đã xóa sẽ không thể hoàn tác.", "Thông báo", "Tôi hiểu", "Hủy", MaterialDesignThemes.Wpf.PackIconKind.Warning
                    , Delete);
                msb.ShowDialog();
            }
            else
            {
                CustomMessageBox.MessageBox.ShowNotify("Không thể xóa khách đã mua hàng");
            }
        }

        private void OnBack(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel.ButtonGroupVisibility_Edit == Visibility.Visible)
            {
                var msb = new CustomMessageBox.MessageBox("Bạn chưa lưu dữ liệu" + Environment.NewLine + "Những thay đổi chưa lưu sẽ bị mất.", "", "Tôi hiểu", "Hủy", MaterialDesignThemes.Wpf.PackIconKind.Warning
                    , () =>
                    {
                        OnCancelEdit(sender, e);
                        ClickedBack?.Invoke(this, null);
                    });
                msb.ShowDialog();
            }
            else
            {
                ClickedBack?.Invoke(this, null);
            }
        }

        private async void Save()
        {
            ViewModel.CheckInvalid(); // Check invalid data

            if (ViewModel.Error != null)
            {
                CustomMessageBox.MessageBox.ShowError(ViewModel.Error);
                return;
            }

            try
            {
                ViewModel.IsBusy = true;
                if (!oldVM.PhoneNumber.Trim().Equals(ViewModel.PhoneNumber.Trim()))
                {
                    await ViewModel.CheckDuplicatePhoneAsync();
                }

                if (ViewModel.Error == null)
                {
                    await ViewModel.UpdateToDBAsycn();
                }

                ViewModel.IsBusy = false;
                if (ViewModel.Error != null)
                {
                    CustomMessageBox.MessageBox.ShowError(ViewModel.Error);
                }
                else
                {
                    CustomMessageBox.MessageBox.ShowNotify("Cập nhật thông tin thành công");
                    ViewModel.CopyTo(oldVM);
                    OnCancelEdit(null, null);
                    EditOK?.Invoke(ViewModel, null);
                }
                ViewModel.Error = null;
            }
            catch (Exception) when (!HelperService.Environment.IsDebug)
            {
                ViewModel.IsBusy = false;
                CustomMessageBox.MessageBox.ShowError(FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01"));
            }
        }

        private async void Delete()
        {
            try
            {
                ViewModel.IsBusy = true;
                await ViewModel.DeleteFromDBAsync();
                ViewModel.IsBusy = false;

                CustomMessageBox.MessageBox.ShowNotify("Xóa khách hàng thành công");
                SwitchMode_readonly();
                DeletedOK?.Invoke(this, null);
            }
            catch (Exception) when (!HelperService.Environment.IsDebug)
            {
                ViewModel.IsBusy = false;
                CustomMessageBox.MessageBox.ShowNotify(FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01"));
            }
        }

        private void SwitchMode_edit()
        {
            CusInfor.IsEnabled = true;
            ViewModel.ButtonGroupVisibility_Edit = System.Windows.Visibility.Visible;
            ViewModel.ButtonGroupVisibility_Read = System.Windows.Visibility.Hidden;
            ViewModel.Title = "Chỉnh sửa khách hàng";

            ViewModel.GetBills();
        }

        public void SwitchMode_readonly()
        {
            CusInfor.IsEnabled = false;
            ViewModel.ButtonGroupVisibility_Edit = System.Windows.Visibility.Hidden;
            ViewModel.ButtonGroupVisibility_Read = System.Windows.Visibility.Visible;
            ViewModel.Title = "Chi tiết khách hàng";
            ViewModel.GetBills();
        }

       
    }
}
