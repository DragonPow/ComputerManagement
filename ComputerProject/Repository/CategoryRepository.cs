using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Repository
{
    public class CategoryRepository
    {
        public Collection<Model.Category> LoadCategories(string name = null)
        {
            Collection<Model.Category> list = new ObservableCollection<Model.Category>();

            using (var _context = new ComputerManagementEntities())
            {
                //_context.Configuration.AutoDetectChangesEnabled = false;
                _context.Configuration.LazyLoadingEnabled = false;

                IEnumerable<CATEGORY> listEntity = null;
                IQueryable<CATEGORY> query = null;

                if (name == null)
                {
                    query = _context.CATEGORies.Include(c => c.CATEGORY11).Where(i => i.parentCategoryId == null);
                }
                else
                {
                    query = _context.CATEGORies.Include(c => c.CATEGORY11).Where(i => i.name.Contains(name) && i.parentCategoryId == null);
                }

                listEntity = query.ToList();

                foreach (var i in listEntity)
                {
                    Model.Category c = new Model.Category(i);
                    list.Add(c);
                }
            }
            Console.WriteLine("Load category done");

            return list;
        }

        public ObservableCollection<Model.Category> LoadChildCategories(int rootId)
        {
            var list = new ObservableCollection<Model.Category>();

            using (var _context = new ComputerManagementEntities())
            {
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<CATEGORY> listEntity = _context.CATEGORies.Include(i => i.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == rootId).ToList();

                foreach (var i in listEntity)
                {
                    Model.Category c = new Model.Category(i);
                    list.Add(c);
                }
            }

            return list;
        }

        public void Delete(int categoryid)
        {
            CATEGORY c = new CATEGORY() { id = categoryid };

            using (var _context = new ComputerManagementEntities())
            {
                _context.Configuration.LazyLoadingEnabled = false;
                var childCategories = _context.CATEGORies.Include(i => i.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == categoryid).ToList();

                foreach (var child in childCategories)
                {
                    var specs = child.SPECIFICATION_TYPE.ToList();
                    foreach (var s in specs)
                    {
                        _context.Entry(s).State = EntityState.Deleted;
                    }
                    _context.Entry(child).State = EntityState.Deleted;
                }
                _context.Entry(c).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public void Save(Model.Category category)
        {
            using (var _context = new ComputerManagementEntities())
            {
                CATEGORY c = category.CastToModel();
                _context.Entry(c).State = c.id == 0 ? EntityState.Added : EntityState.Modified;
                foreach (var child in c.CATEGORY11)
                {
                    _context.Entry(child).State = child.id == 0 ? EntityState.Added : EntityState.Modified;
                    foreach (var spec in child.SPECIFICATION_TYPE)
                    {
                        _context.Entry(spec).State = spec.id == 0 ? EntityState.Added : EntityState.Modified;
                    }
                }
                _context.SaveChanges();
                ChangeIdRuntime(category, c);

                //Remove category is delete from db
                var listDBCategories = _context.CATEGORies.Include(i=>i.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == c.id).AsEnumerable();
                var categoriesDeleted = listDBCategories.Where(i1 => !category.ChildCategories.Any(i2 => i1.id == i2.Id)).AsEnumerable();

                foreach(var i in categoriesDeleted)
                {
                    _context.SPECIFICATION_TYPE.RemoveRange(i.SPECIFICATION_TYPE);
                }
                _context.CATEGORies.RemoveRange(categoriesDeleted);
                _context.SaveChanges();
            }
        }

        private void ChangeIdRuntime(Model.Category category, CATEGORY dbCategory)
        {
            category.Id = dbCategory.id;
            if (category.ChildCategories != null)
                foreach (var child in category.ChildCategories)
                {
                    ChangeIdRuntime(child, dbCategory.CATEGORY11.Where(c => c.name == child.Name).First());
                }

            if (category.SpecificationTypes != null)
                foreach (var spec in category.SpecificationTypes)
                {
                    if (spec.Id == 0) spec.Id = dbCategory.SPECIFICATION_TYPE.Where(i => i.name == spec.Name).Select(i => i.id).First();
                }

            //if (category.Id == 0)
            //{
            //    var id = _context.CATEGORies.Where(i => i.name == category.Name).Select(i => i.id).Single();
            //    category.Id = id;
            //}

            //if (category.ChildCategories != null)
            //    foreach (var child in category.ChildCategories)
            //    {
            //        ChangeIdRuntime(child, _context);
            //    }

            //if (category.SpecificationTypes != null)
            //    foreach (var spec in category.SpecificationTypes)
            //    {
            //        var id = _context.SPECIFICATION_TYPE.Where(i => i.name == category.Name).Select(i => i.id).Single();
            //        spec.Id = id;
            //    }
        }

        public bool IsRootCategoryExists(Model.Category category)
        {
            using (var _context = new ComputerManagementEntities())
            {
                var findCategory = _context.CATEGORies.FirstOrDefault(c => c.name == category.Name && c.id != category.Id && c.parentCategoryId == null);
                return findCategory != null;
            }
        }

        public void LoadSpecification(Model.Category parentCategory)
        {
            using (var _context = new ComputerManagementEntities())
            {
                foreach (var child in parentCategory.ChildCategories)
                {
                    if (child.SpecificationTypes == null) child.SpecificationTypes = new ObservableCollection<Model.Specification_type>();
                    var specifications = _context.SPECIFICATION_TYPE.Where(s => s.categoryId == child.Id).ToList();
                    foreach (var s in specifications)
                    {
                        child.SpecificationTypes.Add(new Model.Specification_type(s));
                    }
                }
            }
            Console.WriteLine("Specification_Type load done");
        }
    }
}
