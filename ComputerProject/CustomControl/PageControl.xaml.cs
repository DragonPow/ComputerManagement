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

namespace ComputerProject.CustomControl
{
    /// <summary>
    /// Interaction logic for PageControl.xaml
    /// </summary>
    public partial class PageControl : UserControl
    {
        public PageControl()
        {
            InitializeComponent();
        }

        private static readonly DependencyProperty CurrentPageProperties = DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(PageControl), new PropertyMetadata(1000));
        private static readonly DependencyProperty MaxPageProperties = DependencyProperty.Register(nameof(MaxPage), typeof(int), typeof(PageControl), new PropertyMetadata(1));
        public event EventHandler PageChanged;

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperties); }
            set
            {
                //if (this.CurrentPage == value) return;
                SetValue(CurrentPageProperties, value);
                PageChanged?.Invoke(this, null);
            }
        }
        public int MaxPage
        {
            get { return (int)GetValue(MaxPageProperties); }
            set { SetValue(MaxPageProperties, value); }
        }
        private void Button_Skip_Next_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = MaxPage;
        }

        private void Button_Skip_Back_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < MaxPage) CurrentPage += 1;
        }
        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1) CurrentPage -= 1;
        }
    }
}
