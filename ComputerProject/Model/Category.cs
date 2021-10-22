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
        ICollection<Specification_type> _specificationTypes;
        ICollection<Category> _childCategories;


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

        public ICollection<Specification_type> SpecificationTypes
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

        public ICollection<Category> ChildCategories
        {
            get => _childCategories; set
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
        }

        public Category(CATEGORY item)
        {
            this._id = item.id;
            this.Name = item.name;
            this.ChildCategories = new ObservableCollection<Category>();
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

                this.ChildCategories.Add(c);
            }
        }

        public CATEGORY Cast(Model.Category category)
        {
            throw new NotImplementedException();
        }
    }
}
