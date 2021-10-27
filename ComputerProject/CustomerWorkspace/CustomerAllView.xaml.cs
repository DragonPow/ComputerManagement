﻿using System;
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
        CancellationTokenSource searchOperation = new CancellationTokenSource();

        public CustomerAllView()
        {
            InitializeComponent();
            DataContext = new CustomerAllViewModel(new List<CustomerViewModel>());
            btnCreate.Click += BtnCreate_Click;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            Search();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerAllViewModel.SearchContent))
            {
                searchOperation.Cancel();
                searchOperation = new CancellationTokenSource();
                ViewModel.currentStartIndex = 0;
                Search();
            }

            if (e.PropertyName == nameof(CustomerAllViewModel.CustomerList))
            {
                Console.WriteLine("List changed");
            }
        }

        public async void Search()
        {
            ViewModel.BusyVisibility = Visibility.Visible;
            try
            {
                await ViewModel.SearchAsycn(searchOperation.Token); // Busy task
                ViewModel.BusyVisibility = Visibility.Hidden;
            }
            catch (Exception) when (!Helper.Environment.IsDebug)
            {
                ViewModel.BusyVisibility = Visibility.Hidden;
                CustomMessageBox.MessageBox.Show("Lấy dữ liệu thất bại");
            }
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
            // Do something

            // Call-back
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

        private async void Delete(CustomerViewModel vm)
        {
            try
            {
                ViewModel.BusyVisibility = Visibility.Visible; // Show busy

                await vm.DeleteFromDBAsync(); // Busy task

                ViewModel.BusyVisibility = Visibility.Hidden;

                Search();
                // CustomMessageBox.MessageBox.Show("Xóa khách hàng thành công");
            }
            catch (Exception) when (!Helper.Environment.IsDebug)
            {
                ViewModel.BusyVisibility = Visibility.Hidden;
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
