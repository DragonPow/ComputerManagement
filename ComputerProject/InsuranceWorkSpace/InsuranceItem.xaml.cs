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

namespace ComputerProject.InsuranceWorkSpace
{
    /// <summary>
    /// Interaction logic for InsuranceItem.xaml
    /// </summary>
    public partial class InsuranceItem : UserControl
    {
        public static DependencyProperty CommandClickEditProperty = DependencyProperty.Register(nameof(CommandClickEdit), typeof(ICommand), typeof(InsuranceItem));
        public static DependencyProperty CommandClickDeleteProperty = DependencyProperty.Register(nameof(CommandClickDelete), typeof(ICommand), typeof(InsuranceItem));
        public static DependencyProperty CommandDoubleClickProperty = DependencyProperty.Register(nameof(CommandDoubleClick), typeof(ICommand), typeof(InsuranceItem));

        public InsuranceItem()
        {
            InitializeComponent();
        }

        public ICommand CommandClickEdit
        {
            get => (ICommand) GetValue(CommandClickEditProperty);
            set => SetValue(CommandClickEditProperty, value);
        }

        public ICommand CommandClickDelete
        {
            get => (ICommand)GetValue(CommandClickDeleteProperty);
            set => SetValue(CommandClickDeleteProperty, value);
        }

        public ICommand CommandDoubleClick
        {
            get => (ICommand)GetValue(CommandDoubleClickProperty);
            set => SetValue(CommandDoubleClickProperty, value);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CommandClickEdit?.Execute(DataContext);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            CommandClickDelete?.Execute(DataContext);
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CommandDoubleClick?.Execute(DataContext);
        }
    }
}
