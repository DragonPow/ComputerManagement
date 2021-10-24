using ComputerProject.Helper;
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
        string _name;

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
        }
    }
}
