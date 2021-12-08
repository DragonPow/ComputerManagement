using ComputerProject.Test;
using ComputerProject.CategoryWorkspace;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ComputerProject.ApplicationWorkspace;
using ComputerProject.InsuranceWorkSpace;
using ComputerProject.Repository;
using ComputerProject.OverViewWorkSpace;
using System.Data.Entity;

namespace ComputerProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            /*var vm = new ApplicationViewModel();
            var view = new ApplicationWindow();
            view.DataContext = vm;
            view.Show();*/

            //using (var db = new ComputerManagementEntities())
            //{
            //    int id = db.BILLs.OrderByDescending(i => i.id).Select(i => i.id).Skip(1).FirstOrDefault();
            //    var b = (new BillRepository()).LoadDetailBill(id);
            //    Helper.PrintPDF.Instance.createBill(new Model.Bill(b));
            //}

            WindowTest test = new WindowTest();
            var view = new InsuranceTabView();
            test.Content = view;
            view.LoadDataAsync();
            test.Show();
        }
    }
}
