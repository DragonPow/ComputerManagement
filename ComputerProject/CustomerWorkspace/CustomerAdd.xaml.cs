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

        /// <summary>
        /// This function being call asycn on UI thread
        /// </summary>
        async void Save()
        {
            string error = ViewModel.GetError(); // Check invalid data

            if (await CustomerViewModel.CheckDuplicate(ViewModel)) // Check duplicate customer
            {
                error = "Khách hàng có số điện thoại trên đã tồn tại. Vui lòng nhập số khác.";
            }

            if (error == null)
            {
                busy.Visibility = Visibility.Visible; // Show busy
                try
                {
                    await ViewModel.Save();

                    busy.Visibility = Visibility.Hidden; // Hide busy

                    SaveOK?.Invoke(this, null); // Callback
                }
                catch (Exception) when (!Helper.Environment.IsDebug)
                {
                    busy.Visibility = Visibility.Hidden; // Hide busy
                    CustomMessageBox.MessageBox.Show(FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01"));
                }
            }
            else
            {
                CustomMessageBox.MessageBox.Show(error);
            }
        }

        public event EventHandler Closed_NotSave;
        public event EventHandler SaveOK;
    }
}
