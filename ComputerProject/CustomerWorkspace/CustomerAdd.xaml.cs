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

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerAdd.xaml
    /// </summary>
    public partial class CustomerAdd : UserControl
    {
        public CustomerViewModel ViewModel => this.DataContext as CustomerViewModel;

        public CustomerAdd()
        {
            InitializeComponent();

            BtnBack.Click += (s, e) => Closed_NotSave?.Invoke(this, e);
            btnCancel.Click += (s, e) => Closed_NotSave?.Invoke(this, e);

            btnSave.Click += (s, e) =>
            {
                Save();
            };
        }

        public event EventHandler Closed_NotSave;
        public event EventHandler SaveOK;

        /// <summary>
        /// Save data asycn on UI thread
        /// </summary>
        public async void Save()
        {
            string error = ViewModel.GetInvalid(); // Check invalid data

            if (error != null)
            {
                CustomMessageBox.MessageBox.Show(error);
                return;
            }

            ViewModel.BusyVisibility = Visibility.Visible; // Show busy

            error = await CustomerViewModel.GetDuplicate(ViewModel); // Busy task
            if (error != null)
            {
                ViewModel.BusyVisibility = Visibility.Hidden;
                CustomMessageBox.MessageBox.Show(error);
                return;
            }

            // Data is safe now
            try
            {
                await ViewModel.InsertToDBAsycn(); // Busy task

                ViewModel.BusyVisibility = Visibility.Hidden;

                CustomMessageBox.MessageBox.Show("Đã thêm khách hàng mới vào cơ sở dữ liệu thành công");
                SaveOK?.Invoke(this, null); // Callback
            }
            catch (Exception) when (!HelperService.Environment.IsDebug)
            {
                ViewModel.BusyVisibility = Visibility.Hidden;
                CustomMessageBox.MessageBox.Show(FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01"));
            }
        }
    }
}
