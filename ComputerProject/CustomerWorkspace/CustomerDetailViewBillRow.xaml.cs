using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;


namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerDetailViewBillRow.xaml
    /// </summary>
    public partial class CustomerDetailViewBillRow : UserControl
    {
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
            Content.Text = "Laptop HP 15s-fq2602TU";
            Total.Text = "4.3000.000";
        }
    }
}
