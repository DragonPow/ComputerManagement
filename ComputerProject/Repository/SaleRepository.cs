using ComputerProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class SaleRepository
    {
        public Collection<Model.Product> LoadProducts(bool isContainStopSale = false, string name = null)
        {
            Collection<Model.Product> list = new ObservableCollection<Model.Product>();

            using(var db = new ComputerManagementEntities())
            {
                IList<PRODUCT> l;
                if (name ==null)
                {
                    l = db.PRODUCTs.Include(i => i.CATEGORY).Where(i => i.isStopSelling == isContainStopSale).ToList();
                }
                else
                {
                    l = db.PRODUCTs.Include(i => i.CATEGORY).Where(i => i.isStopSelling == isContainStopSale && i.name.Contains(name)).ToList();
                }

                foreach (var item in l)
                {
                    list.Add(new Model.Product(item));
                }
            }

            Console.WriteLine("List product load done");

            return list;
        }

        public Collection<Category> LoadCategories()
        {
            var categoryRepo = new CategoryRepository();
            return categoryRepo.LoadCategories();
        }
    }
}
