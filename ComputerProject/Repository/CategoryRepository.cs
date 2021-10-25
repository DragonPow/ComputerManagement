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
                    listEntity = db.CATEGORies.Include(c => c.CATEGORY11).Where(i=>i.parentCategoryId==null).ToList();
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
                listEntity = db.CATEGORies.Include(c => c.SPECIFICATION_TYPE).Where(i => i.id == rootId).ToList();

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

                foreach(var child in childCategories)
                {
                    db.Entry(child).State = EntityState.Deleted;
                }
                db.Entry(c).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Save(Model.Category category)
        {
            using (var db = new ComputerManagementEntities())
            {
                //TODO: save category and child, specification
                CATEGORY c = category.CastToModel();
                db.Entry(c).State = c.id==0? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool IsRootCategoryExists(Model.Category category)
        {
            using (var db = new ComputerManagementEntities())
            {
                return db.CATEGORies.Where(c => c.name == category.Name && c.id != category.Id).Count() > 0;
            }
        }
    }
}
