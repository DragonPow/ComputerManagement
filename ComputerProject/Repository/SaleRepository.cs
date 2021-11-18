using ComputerProject.Model;
using ComputerProject.SaleWorkSpace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class SaleRepository
    {
        int countLoadProduct = 0;
        public Collection<Model.Product> LoadProducts(Model.Category category = null, bool isContainStopSale = false, string name = null)
        {
            try
            {
                using (var db = new ComputerManagementEntities())
                {
                    //string q = "SELECT Id, name, priceorigin, pricesale,"
                    IQueryable<PRODUCT> query = db.PRODUCTs.Include(i => i.CATEGORY);

                    if (name == null)
                    {
                        query = query.Where(i => i.isStopSelling == isContainStopSale);
                    }
                    else
                    {
                        query = query.Where(i => i.isStopSelling == isContainStopSale && i.name.Contains(name));
                    }
                    if (category != null)
                    {
                        query = query.Where(i => i.CATEGORY.id == category.Id || i.CATEGORY.parentCategoryId == category.Id);
                    }

                    return new ObservableCollection<Model.Product>(GetProductsFromQuery(db, query));
                }
            }
            catch (SqlException e)
            {
                if (e.Number == -2) //Timeout
                {
                    countLoadProduct++;
                    Console.WriteLine("Timeout connect SQL when load Product: {0}", countLoadProduct);
                    if (countLoadProduct < 2)
                        return LoadProducts(category, isContainStopSale, name);
                }
                else throw e;
            }
            return null;
        }

        public async Task LoadAsyncImageProducts(IEnumerable<Model.Product> products)
        {
            using (var db = new ComputerManagementEntities())
            {
                foreach (var pro in products)
                {
                    pro.Image = await db.PRODUCTs.Where(i => i.id == pro.Id).Select(i => i.image).FirstAsync();
                    Console.WriteLine("image name {0} load done", pro.Name);
                }
            }
        }

        public int GetPointToMoney()
        {
            using (var db = new ComputerManagementEntities())
            {
                return int.Parse(db.REGULATIONs.Where(i => i.name == "PointToMoney").Select(i => i.value).First());
            }
        }

        public int GetMaxPointUse()
        {
            using (var db = new ComputerManagementEntities())
            {
                return int.Parse(db.REGULATIONs.Where(i => i.name == "MaxPoint").Select(i => i.value).First());
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
                IQueryable<PRODUCT> query = db.PRODUCTs.AsNoTracking();

                query = query.Where(i => (!(filter.Supplier == null || filter.Supplier.Trim() == "") ? i.producer.ToLower().Contains(filter.Supplier.Trim().ToLower()) : true)
                && i.priceSales >= filter.PriceLowest
                && i.isStopSelling == (filter.StateProduct == stateProduct.Stop)
                && (filter.PriceHighest > 0 ? i.priceSales <= filter.PriceHighest : true)
                && (filter.TimeWarranty > 0 ? i.warrantyTime == filter.TimeWarranty : true));

                return new ObservableCollection<Model.Product>(GetProductsFromQuery(db, query));
            }
        }

        private IEnumerable<Product> GetProductsFromQuery(ComputerManagementEntities db, IQueryable<PRODUCT> query)
        {
            List<Model.Product> listBaseDb = query.Select(i => new Product
            {
                Id = i.id,
                Name = i.name,
                PriceOrigin = i.priceOrigin,
                PriceSale = i.priceSales,
                Producer = i.producer,
                Warranty = i.warrantyTime ?? 0,
                Quantity = i.quantity,
            }).ToList();

            LoadAsyncImageProducts(listBaseDb);
            LoadAsyncCategory(listBaseDb);

            Console.WriteLine("Load category done");
            return listBaseDb;
        }

        private async Task LoadAsyncCategory(List<Product> listBaseDb)
        {
            using (var db = new ComputerManagementEntities())
            {
                foreach (var item in listBaseDb)
                {
                    var task_c = db.PRODUCTs.Include(i => i.CATEGORY).Where(i => i.id == item.Id).AsNoTracking().Select(i => i.CATEGORY).FirstAsync();
                    item.CategoryProduct = new Category(await task_c);
                }
            }
        }

        public Collection<CUSTOMER> SearchCustomer(string phone, int number)
        {
            using (var db = new ComputerManagementEntities())
            {
                var query = db.CUSTOMERs.AsNoTracking().Where(i => i.phone.Contains(phone)).OrderBy(p => p.phone).Take(number);
                Collection<CUSTOMER> list = new ObservableCollection<CUSTOMER>(query.AsEnumerable());

                return list;
            }
        }
    }
}
