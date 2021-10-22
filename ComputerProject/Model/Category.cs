using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class Category : BaseViewModel
    {
        int _id;
        string _name;
        Collection<Specification_type> _specificationTypes;
        Collection<Category> _childCategories;


        public int Id
        {
            get => _id;
        }
        public string Name
        {
            get => _name; set
            {
                if (value != _name)
                {
                    _name = value;
                }
                OnPropertyChanged();
            }
        }

        public Collection<Specification_type> SpecificationTypes
        {
            get => _specificationTypes;
            set
            {
                if (value != _specificationTypes)
                {
                    _specificationTypes = value;
                }
                OnPropertyChanged();
            }
        }

        public Collection<Category> ChildCategories
        {
            get => _childCategories;
            set
            {
                if (value != _childCategories)
                {
                    _childCategories = value;
                }
                OnPropertyChanged();
            }
        }

        public Category()
        {
            _id = 0;
            this.ChildCategories = null;
        }

        public Category(CATEGORY item)
        {
            this._id = item.id;
            this.Name = item.name;
            this.ChildCategories = null;
            //this.ChildCategories = new ObservableCollection<Category>();
            foreach (var i in item.CATEGORY11)
            {
                var c = new Category(i);

                if (i.SPECIFICATION_TYPE != null)
                {
                    c.SpecificationTypes = new Collection<Model.Specification_type>();
                    foreach (var s in i.SPECIFICATION_TYPE)
                    {
                        c.SpecificationTypes.Add(new Specification_type(s));
                    }
                }

                //this.ChildCategories.Add(c);
            }
        }

        public CATEGORY CastToModel()
        {
            CATEGORY c = new CATEGORY() { id = this.Id, name = this.Name, CATEGORY11 = new List<CATEGORY>(), SPECIFICATION_TYPE = new List<SPECIFICATION_TYPE>()};

            //Get child categories
            if (this.ChildCategories != null)
            foreach(var i in this.ChildCategories)
            {
                c.CATEGORY11.Add(i.CastToModel());
            }

            //Get specification if it is child category
            if (this.SpecificationTypes != null)
            foreach(var i in this.SpecificationTypes)
            {
                c.SPECIFICATION_TYPE.Add(i.CastToModel());
            }

            return c;
        }
    }
}
