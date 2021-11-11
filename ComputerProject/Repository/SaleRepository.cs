using ComputerProject.Model;
using ComputerProject.SaleWorkSpace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class SaleRepository
    {
        public Collection<Model.Product> LoadProducts(bool isContainStopSale = false, string name = null)
        {
            using (var db = new ComputerManagementEntities())
            {
                Stopwatch timeExecute = new Stopwatch();
                timeExecute.Start();
                Collection<Model.Product> list = new ObservableCollection<Model.Product>();
                IQueryable<PRODUCT> query = db.PRODUCTs.Include(i => i.CATEGORY).AsNoTracking();

                if (name == null)
                {
                    query = query.Where(i => i.isStopSelling == isContainStopSale);
                }
                else
                {
                    query = query.Where(i => i.isStopSelling == isContainStopSale && i.name.Contains(name));
                }

                IEnumerable<PRODUCT> l = query.AsEnumerable();
                foreach (var item in l)
                {
                    list.Add(new Model.Product(item));
                }

                timeExecute.Stop();
                Console.WriteLine("List product load done, time execute: {0}", timeExecute.ElapsedMilliseconds);
                return list;
            }
        }

        public Collection<Category> LoadCategories()
        {
            var categoryRepo = new CategoryRepository();
            return categoryRepo.LoadCategories();
        }

        public Collection<Product> SearchFilterProduct(IFilterProductState filter)
        {
            using (var db = new ComputerManagementEntities())
            {
                Collection<Model.Product> list = new ObservableCollection<Model.Product>();
                IQueryable<PRODUCT> query = db.PRODUCTs.Include(i => i.CATEGORY);

                query = query.Where(i => (!(filter.Supplier == null || filter.Supplier.Trim() == "") ? i.producer.ToLower().Contains(filter.Supplier.Trim().ToLower()) : true)
                && i.priceSales >= filter.PriceLowest
                && i.isStopSelling == (filter.StateProduct == stateProduct.Stop)
                && (filter.PriceHighest > 0 ? i.priceSales <= filter.PriceHighest : true)
                && (filter.TimeWarranty > 0 ? i.warrantyTime == filter.TimeWarranty : true));

                IEnumerable<PRODUCT> l = query.AsEnumerable();
                foreach (var item in l)
                {
                    list.Add(new Model.Product(item));
                }

                return list;
            }
        }

        public Collection<CUSTOMER> SearchCustomer(string text)
        {
            using (var db = new ComputerManagementEntities())
            {
                var query = db.CUSTOMERs.Where(i => i.phone.Contains(text));
                Collection<CUSTOMER> list = new ObservableCollection<CUSTOMER>(query.AsEnumerable());

                return list;
            }
        }
    }
}
