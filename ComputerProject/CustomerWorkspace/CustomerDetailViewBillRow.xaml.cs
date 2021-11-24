using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerDetailViewBillRow.xaml
    /// </summary>
    public partial class CustomerDetailViewBillRow : UserControl
    {
        public static DependencyProperty NumberProperties = DependencyProperty.Register(nameof(number), typeof(string), typeof(CustomerDetailViewBillRow), new PropertyMetadata("0"));

        public string number
        {
            get { return (string)GetValue(NumberProperties); }
            set { SetValue(NumberProperties, value); }
        }
        public CustomerDetailViewBillRow()
        {
            InitializeComponent();
            //InitData();
        }

        public void InitData()
        {
            Number.Text = "1";
            CreateDay.Text = "12/2/2021";
            Type.Text = "Bảo hành, sữa chữa";
          //  Content.Text = "Laptop HP 15s-fq2602TU";
            Total.Text = "4.3000.000";
        }
    }
}
