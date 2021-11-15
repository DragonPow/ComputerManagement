using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    class Regulation: BaseViewModel
    {
        string _type;
        string _name;
        string _value;

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
        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                }
                OnPropertyChanged();
            }
        }
        public string Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                }
                OnPropertyChanged();
            }
        }

        public Regulation()
        {
        }

        public Regulation(REGULATION r)
        {
            Name = r.name;
            Type = r.type;
            Value = r.value;
        }
    }
}
