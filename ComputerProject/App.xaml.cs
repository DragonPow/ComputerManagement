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
using ComputerProject.Repository;

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
            var view = new ApplicationWindow();
            view.DataContext = vm;
            view.Show();

            //var vm = new ListCategoryViewModel();
            //var view = new WindowTest();
            //vm.LoadAsyncCategories();
            //view.DataContext = vm;
            //view.Show();

            //Model.Category c = new Model.Category()
            //{
            //    Name = "Thử nghiệm",
            //    ChildCategories = new System.Collections.ObjectModel.Collection<Model.Category>()
            //    {
            //        new Model.Category() {Name = "Test child 1"},
            //        new Model.Category() {Name = "Test child 2"},
            //    },
            //    SpecificationTypes = new System.Collections.ObjectModel.Collection<Model.Specification_type>()
            //    {
            //        new Model.Specification_type() { Name = "Spec 1"},
            //        new Model.Specification_type() { Name = "Spec 2"},
            //    }
            //};
            //var repo = new CategoryRepository();
            //repo.Save(c);

            //WindowTest test = new WindowTest();
            //test.Show();
        }
    }
}
