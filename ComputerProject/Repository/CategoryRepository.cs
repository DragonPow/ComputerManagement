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
                    listEntity = db.CATEGORies.Include(c => c.CATEGORY11).ToList();
                }
                else
                {
                    listEntity = db.CATEGORies.Include(c => c.CATEGORY11).Where(i => i.name == name).ToList();
                }

                foreach (var i in listEntity)
                {
                    Model.Category c = new Model.Category(i);
                    list.Add(c);
                }
            }

            return list;
        }

        public Collection<Model.Category> LoadChildCategories(int rootId)
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

        public void Delete(int castegoryId)
        {
            using (var db = new ComputerManagementEntities())
            {
                var childCategories = db.CATEGORies.Where(i => i.CATEGORY3.id == castegoryId);
                CATEGORY c = new CATEGORY() { id = castegoryId };

                db.Entry(c).State = db.Entry(childCategories).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Save(Model.Category category)
        {
            using (var db = new ComputerManagementEntities())
            {

            }
        }

        public bool IsRootCategoryExists(string name)
        {
            using (var db = new ComputerManagementEntities())
            {
                return db.CATEGORies.Where(c => c.name == name).Count() > 0;
            }
        }
    }
}
