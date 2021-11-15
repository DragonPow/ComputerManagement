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

namespace ComputerProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            var vm = new ApplicationViewModel();
            var view = new WindowTest();
            //view.DataContext = vm;
            view.Show();



            //var vm = new ListCategoryViewModel();
            //var view = new WindowTest();
            //vm.LoadAsyncCategories();
            //view.DataContext = vm;
            //view.Show();

            //using (var db = new ComputerManagementEntities())
            //{
            //    CATEGORY c = new CATEGORY()
            //    {
            //        name = "Thử nghiệm",
            //        CATEGORY11 = new List<CATEGORY>()
            //    {
            //        new CATEGORY() {name = "Test child 1"},
            //        new CATEGORY() {name = "Test child 2"},
            //    },
            //        SPECIFICATION_TYPE = new List<SPECIFICATION_TYPE>()
            //    {
            //        new SPECIFICATION_TYPE() { name = "Spec 1"},
            //        new SPECIFICATION_TYPE() { name = "Spec 2"},
            //    }
            //    };
            //    db.SaveChanges();
            //}

            //WindowTest test = new WindowTest();
            //test.Show();
        }
    }
}
