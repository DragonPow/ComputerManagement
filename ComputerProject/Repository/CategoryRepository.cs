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

                IEnumerable<CATEGORY> listEntity = null;
                IQueryable<CATEGORY> query = _context.CATEGORies.Include(c => c.CATEGORY11).AsNoTracking();

                if (name == null)
                {
                    query = query.Where(i => i.parentCategoryId == null);
                }
                else
                {
                    query = query.Where(i => i.name.Contains(name) && i.parentCategoryId == null);
                }

                listEntity = query.AsEnumerable();

                foreach (var i in listEntity)
                {
                    Model.Category c = new Model.Category(i);
                    list.Add(c);
                }
            }
            Console.WriteLine("Load category done");

            return list;
        }

        public CATEGORY LoadDetailCategory(int id, bool isloadSpecifications = false)
        {
            CATEGORY category = null;
            using (var _context = new ComputerManagementEntities())
            {
                IQueryable<CATEGORY> query = _context.CATEGORies.AsNoTracking();

                if (isloadSpecifications)
                {
                    query = query.Include(i => i.CATEGORY11.Select(c => c.SPECIFICATION_TYPE));
                }
                else
                {
                    query = query.Include(i => i.CATEGORY11);
                }

                category = query.Where(i => i.id == id).First();
            }

            return category;
        }

        public ObservableCollection<Model.Category> LoadChildCategories(int rootId)
        {
            var list = new ObservableCollection<Model.Category>();

            using (var _context = new ComputerManagementEntities())
            {
                var listEntity = _context.CATEGORies.Include(i => i.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == rootId).AsNoTracking().ToList();

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
            using (var _context = new ComputerManagementEntities())
            {
                CATEGORY c = new CATEGORY() { id = categoryid };
                var childCategories = _context.CATEGORies.Include(i => i.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == categoryid).ToList();

                foreach (var child in childCategories)
                {
                    var specs = child.SPECIFICATION_TYPE.ToList();
                    //Delete specifications
                    foreach (var s in specs)
                    {
                        _context.Entry(s).State = EntityState.Deleted;
                    }

                    //Delete child category
                    _context.Entry(child).State = EntityState.Deleted;
                }
                _context.Entry(c).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public bool CanDelete(int categoryid)
        {
            using (var db = new ComputerManagementEntities())
            {
                return categoryid == 0 ? true : db.PRODUCTs.AsNoTracking().Any(i => i.categoryId == categoryid);
            }
        }

        //public void Save(Model.Category category)
        //{
        //    //CATEGORY c = category.CastToModel();
        //    using (var _context = new ComputerManagementEntities())
        //    {
        //        _context.Database.Log = Console.WriteLine;
        //        CATEGORY dbCategory = _context.CATEGORies.Include(i=>i.CATEGORY11).Where(i => i.id == category.Id).FirstOrDefault();
        //        if (dbCategory == null)
        //        {
        //            dbCategory = category.CastToModel();
        //            _context.CATEGORies.Add(dbCategory);
        //        }
        //        else
        //        {
        //            _context.Entry(dbCategory).State = EntityState.Modified;

        //            dbCategory.name = category.Name;

        //            //Check change in child category
        //            foreach (var child in dbCategory.CATEGORY11.AsEnumerable())
        //            {
        //                //Load specification_type from db
        //                child.SPECIFICATION_TYPE = _context.CATEGORies.Where(i => i.id == child.id).FirstOrDefault().SPECIFICATION_TYPE;

        //                var checkCategory = category.ChildCategories.FirstOrDefault(i => i.Id == child.id);
        //                if (checkCategory != null)
        //                {
        //                    //Update category & specification_type
        //                    foreach (var spec in child.SPECIFICATION_TYPE.AsEnumerable())
        //                    {
        //                        var checkSpec = checkCategory.SpecificationTypes?.FirstOrDefault(i => i.Id == spec.id);
        //                        if (checkSpec != null)
        //                        {
        //                            //Update specification_type
        //                            spec.name = checkSpec.Name;
        //                            _context.Entry(spec).State = EntityState.Modified;
        //                        }
        //                        else
        //                        {
        //                            //Remove specification_type
        //                            _context.Entry(spec).State = EntityState.Deleted;
        //                            //_context.SPECIFICATION_TYPE.Remove(spec);
        //                        }
        //                    }

        //                    var newSpecs = checkCategory.SpecificationTypes?.Where(i => i.Id == 0).ToList();
        //                    if (newSpecs != null)
        //                        foreach (var newSpec in newSpecs)
        //                        {
        //                            SPECIFICATION_TYPE s = newSpec.CastToModel();
        //                            //child.SPECIFICATION_TYPE.Add(s);

        //                            _context.Entry(s).State = EntityState.Added;
        //                        }

        //                    child.name = checkCategory.Name;
        //                    _context.Entry(child).State = EntityState.Modified;
        //                }
        //                else
        //                {
        //                    //Remove category & spec
        //                    //_context.SPECIFICATION_TYPE.RemoveRange(child.SPECIFICATION_TYPE);
        //                    //_context.CATEGORies.Remove(child);
        //                    foreach(var s in child.SPECIFICATION_TYPE.AsEnumerable())
        //                    {
        //                        _context.Entry(s).State = EntityState.Deleted;
        //                    }
        //                    _context.Entry(child).State = EntityState.Deleted;
        //                }
        //            }

        //            var newCategories = category.ChildCategories.Where(i => i.Id == 0).ToList();
        //            foreach (var newCategory in newCategories)
        //            {
        //                CATEGORY c = new CATEGORY() { name = newCategory.Name, SPECIFICATION_TYPE = new List<SPECIFICATION_TYPE>() };
        //                if (newCategory.SpecificationTypes != null)
        //                    foreach (var s in newCategory.SpecificationTypes)
        //                    {
        //                        c.SPECIFICATION_TYPE.Add(s.CastToModel());
        //                    }
        //                dbCategory.CATEGORY11.Add(c);
        //            }
        //        }

        //        //foreach (var child in c.CATEGORY1)
        //        //{
        //        //    //Add or Update category which insert to UI
        //        //    foreach (var spec in child.SPECIFICATION_TYPE)
        //        //    {
        //        //        _context.Entry(spec).State = spec.id == 0 ? EntityState.Added : EntityState.Modified;
        //        //    }

        //        //    _context.Entry(child).State = child.id == 0 ? EntityState.Added : EntityState.Modified;

        //        //    //Remove specification and spec_product which deleted from UI
        //        //    if (_context.Entry(child).State == EntityState.Modified)
        //        //    {
        //        //        var spec_in_UI = child.SPECIFICATION_TYPE.Select(i => i.id);
        //        //        var spec_deleted = _context.SPECIFICATION_TYPE.Include(i => i.SPECIFICATIONs)
        //        //                                                                                .AsEnumerable()
        //        //                                                                                .Where(i => spec_in_UI.Contains(i.id));
        //        //        foreach (var spec in spec_deleted)
        //        //        {
        //        //            _context.SPECIFICATIONs.RemoveRange(spec.SPECIFICATIONs);
        //        //        }
        //        //        _context.SPECIFICATION_TYPE.RemoveRange(spec_deleted);
        //        //    }
        //        //}
        //        _context.SaveChanges();

        //        //Update Id in UI
        //        ChangeIdRuntime(category, dbCategory);

        //        //Remove category which deleted from UI
        //        //DeleteUICategoryFromDb(dbCategory);
        //    }
        //}

        public void Save(Model.Category UIcategory)
        {
            using (var db = new ComputerManagementEntities())
            {
                if (UIcategory.Id == 0)
                {
                    db.CATEGORies.Add(UIcategory.CastToModel());
                }
                else
                {
                    var category = new CATEGORY() { id = UIcategory.Id, name = UIcategory.Name };

                    db.Entry(category).State = EntityState.Modified;

                    var deletedIdCategories = new List<CATEGORY>();
                    var deletedIdSpecs = new List<SPECIFICATION_TYPE>();

                    //Check modified category
                    db.Entry(category).Collection(i => i.CATEGORY11).Load();

                    foreach (var child in category.CATEGORY11)
                    {
                        db.Entry(child).Collection(i => i.SPECIFICATION_TYPE).Load();
                        Model.Category changeCategory = UIcategory.ChildCategories.FirstOrDefault(i => i.Id == child.id);
                        bool existsCategory = changeCategory != null;

                        if (existsCategory)
                        {
                            child.name = changeCategory.Name;
                        }
                        else
                        {
                            deletedIdCategories.Add(child);
                        }

                        foreach(var spec in child.SPECIFICATION_TYPE)
                        {
                            Model.Specification_type changeSpec = changeCategory?.SpecificationTypes?.FirstOrDefault(i => i.Id == spec.id);
                            bool existsSpec = changeSpec != null;

                            if (existsCategory && existsSpec)
                            {
                                spec.name = changeSpec.Name;
                            }
                            else
                            {
                                deletedIdSpecs.Add(spec);
                            }
                        }

                        //Add new spec
                        IEnumerable<Model.Specification_type> newSpecs = changeCategory?.SpecificationTypes?.Where(i => i.Id == 0).AsEnumerable();
                        if (newSpecs != null)
                        {
                            foreach (var newSpec in newSpecs)
                            {
                                var s = new SPECIFICATION_TYPE() { name = newSpec.Name };
                                child.SPECIFICATION_TYPE.Add(s);
                            }
                        }
                    }


                    //Delete spec and category in db
                    db.SPECIFICATION_TYPE.RemoveRange(deletedIdSpecs);
                    db.CATEGORies.RemoveRange(deletedIdCategories);

                    //Check added category
                    IEnumerable<Model.Category> newCategories = UIcategory.ChildCategories.Where(i => i.Id == 0).AsEnumerable();
                    foreach(var newCategory in newCategories)
                    {
                        var c = newCategory.CastToModel();
                        category.CATEGORY11.Add(c);
                    }
                }
                //var category = db.CATEGORies.Include(c=>c.CATEGORY11.Select(s=>s.SPECIFICATION_TYPE)).First(i=>i.id == UIcategory.Id);

                db.SaveChanges();
            }
        }

        private void DeleteUICategoryFromDb(CATEGORY UICategory)
        {
            using (var _context = new ComputerManagementEntities())
            {
                var listDBCategories = _context.CATEGORies.Include(i => i.SPECIFICATION_TYPE).Where(i => i.parentCategoryId == UICategory.id).AsEnumerable();
                var categoriesDeleted = listDBCategories.Where(i1 => !UICategory.CATEGORY11.Any(i2 => i1.id == i2.id)).AsEnumerable();

                foreach (var i in categoriesDeleted)
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
                    ChangeIdRuntime(child, dbCategory.CATEGORY11.First(c => c.name == child.Name));
                }

            if (category.SpecificationTypes != null)
                foreach (var spec in category.SpecificationTypes)
                {
                    spec.Id = dbCategory.SPECIFICATION_TYPE.First(i => i.name == spec.Name).id;
                }
        }

        public bool IsRootCategoryExists(Model.Category category)
        {
            using (var _context = new ComputerManagementEntities())
            {
                return _context.CATEGORies.AsNoTracking().Any(c => c.name == category.Name && c.id != category.Id && c.parentCategoryId == null);
            }
        }

        public void LoadSpecification(Model.Category parentCategory)
        {
            using (var _context = new ComputerManagementEntities())
            {
                foreach (var child in parentCategory.ChildCategories)
                {
                    if (child.SpecificationTypes == null) child.SpecificationTypes = new ObservableCollection<Model.Specification_type>();
                    var specifications = _context.SPECIFICATION_TYPE.AsNoTracking().Where(s => s.categoryId == child.Id).ToList();
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
