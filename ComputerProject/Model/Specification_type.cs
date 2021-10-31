using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class Specification_type : BaseViewModel
    {
        int _id;
        int _number;
        string _name;
        int _categoryId;

        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int Number
        {
            get => _number;
            set => _number = value;
        }
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                }
                OnPropertyChanged();
            }
        }

        public Specification_type()
        {
            _id = 0;
        }

        public Specification_type(SPECIFICATION_TYPE specification_type)
        {
            _id = specification_type.id;
            _name = specification_type.name;
            _categoryId = specification_type.categoryId;
        }

        public SPECIFICATION_TYPE CastToModel()
        {
            SPECIFICATION_TYPE s = new SPECIFICATION_TYPE() { id = this.Id, name = this.Name, categoryId = this._categoryId};
            return s;
        }

        public Specification_type Copy()
        {
            Specification_type s = new Specification_type();
            s._id = this._id;
            s._name = this._name;
            s._categoryId = this._categoryId;

            return s;
        }
    }
}
