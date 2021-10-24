
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerDetailView.xaml
    /// </summary>
    public partial class CustomerDetailView : UserControl
    {
        public CustomerViewModel ViewModel => DataContext as CustomerViewModel;
        object temp = null;

        public CustomerDetailView()
        {
            InitializeComponent();
            SwitchMode_readonly();

            BtnBack.Click += (s, e) => ClickedBack?.Invoke(this, null);
        }

        public event EventHandler ClickedBack; 

        private void OnSaveEdit(object sender, System.Windows.RoutedEventArgs e)
        {
            SwitchMode_readonly();
        }

        private void OnCancelEdit(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DataContext = temp;
            SwitchMode_readonly();
        }

        private void OnStartEdit(object sender, System.Windows.RoutedEventArgs e)
        {
            temp = new CustomerViewModel(ViewModel.Model);
            SwitchMode_edit();
        }

        /*private void BtnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (BtnDelete.Content.ToString() == "Hủy")
            {
                Name.Text = Phone.Text = Adress.Text = Point.Text = " ";
                Birthday.Text = "";
            }
        }*/

        private void SwitchMode_edit()
        {
            CusInfor.IsEnabled = true;
            BtnDelete.Content = "Hủy";
            BtnDelete.Foreground = BtnDelete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#0477BF");
            BtnEdit.Content = "Cập nhật";
            Title.Text = "Chỉnh sửa khách hàng";

            BtnDelete.Visibility = System.Windows.Visibility.Visible;
            BtnDelete.Click += OnCancelEdit;
            BtnEdit.Click -= OnStartEdit;
            BtnEdit.Click += OnSaveEdit;
        }

        private void SwitchMode_readonly()
        {
            CusInfor.IsEnabled = false;
            BtnDelete.Content = "Xóa";
            BtnDelete.Foreground = BtnDelete.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#EE5E5E");
            BtnEdit.Content = "Chỉnh sửa";
            Title.Text = "Chi tiết khách hàng";

            BtnDelete.Visibility = System.Windows.Visibility.Hidden;
            BtnDelete.Click -= OnCancelEdit;
            BtnEdit.Click -= OnSaveEdit;
            BtnEdit.Click += OnStartEdit;
        }

        
    }
}
