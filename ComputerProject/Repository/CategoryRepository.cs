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
        public Collection<Model.Category> LoadCategories(string name)
        {
            var list = new ObservableCollection<Model.Category>();

            using (var db = new ComputerManagementEntities())
            {
                List<CATEGORY> listEntity;

                if (name == null)
                {
                    listEntity = db.CATEGORies.Include(c => c.CATEGORY11).Where(i => i.parentCategoryId == null).ToList();
                    //listEntity = db.CATEGORies.Where(i => i.parentCategoryId == null).ToList();
                }
                else
                {
                    listEntity = db.CATEGORies.Include(c => c.CATEGORY11).Where(i => i.name == name && i.parentCategoryId == null).ToList();
                }

                foreach (var i in listEntity)
                {
                    Model.Category c = new Model.Category(i);
                    list.Add(c);
                }
            }

            return list;
        }

        public ObservableCollection<Model.Category> LoadChildCategories(int rootId)
        {
            var list = new ObservableCollection<Model.Category>();

            using (var db = new ComputerManagementEntities())
            {
                List<CATEGORY> listEntity;
                listEntity = db.CATEGORies.Include(c => c.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == rootId).ToList();

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
            using (var db = new ComputerManagementEntities())
            {
                var childCategories = db.CATEGORies.Where(i => i.CATEGORY3.id == categoryid).ToList();
                CATEGORY c = new CATEGORY() { id = categoryid };

                foreach (var child in childCategories)
                {
                    db.Entry(child).State = EntityState.Deleted;
                }
                db.Entry(c).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public async void Save(Model.Category category)
        {
            using (var db = new ComputerManagementEntities())
            {
                CATEGORY c = category.CastToModel();
                db.Entry(c).State = c.id == 0 ? EntityState.Added : EntityState.Modified;

                //Modified child category changed
                foreach (var child in c.CATEGORY11)
                {
                    db.Entry(child).State = child.id == 0 ? EntityState.Added : EntityState.Modified;
                    foreach (var spec in child.SPECIFICATION_TYPE)
                    {
                        db.Entry(spec).State = spec.id == 0 ? EntityState.Added : EntityState.Modified;
                    }
                }
                db.SaveChanges();

                ChangeIdRuntime(category, db);
            }
        }

        private void ChangeIdRuntime(Model.Category category, ComputerManagementEntities db)
        {
            if (category.Id == 0)
            {
                var id = db.CATEGORies.Where(i => i.name == category.Name).Select(i => i.id).Single();
                category.Id = id;
            }

            if (category.ChildCategories != null)
                foreach (var child in category.ChildCategories)
                {
                    ChangeIdRuntime(child, db);
                }

            if (category.SpecificationTypes != null)
                foreach (var spec in category.SpecificationTypes)
                {
                    var id = db.SPECIFICATION_TYPE.Where(i => i.name == category.Name).Select(i => i.id).Single();
                    spec.Id = id;
                }
        }

        public bool IsRootCategoryExists(Model.Category category)
        {
            using (var db = new ComputerManagementEntities())
            {
                return db.CATEGORies.Where(c => c.name == category.Name && c.id != category.Id).Count() > 0;
            }
        }

        public void LoadSpecification(Model.Category parentCategory)
        {
            using (var db = new ComputerManagementEntities())
            {
                foreach(var child in parentCategory.ChildCategories)
                {
                    if (child.SpecificationTypes == null) child.SpecificationTypes = new ObservableCollection<Model.Specification_type>();
                    var specifications = db.SPECIFICATION_TYPE.Where(s => s.categoryId == child.Id);
                    foreach(var s in specifications)
                    {
                        child.SpecificationTypes.Add(new Model.Specification_type(s));
                    }
                }
            }
        }
    }
}
