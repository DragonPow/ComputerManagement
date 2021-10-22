using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class Customer : BaseViewModel
    {
        int id;
        string _name;
        string _phone;
        DateTime _birthday;
        string _address;
        int _point;

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
        public string Phone
        {
            get => _phone;
            set
            {
                if (value != _name)
                {
                    _name = value;
                }
                OnPropertyChanged();
            }
        }
        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                if (value != _birthday)
                {
                    _birthday = value;
                }
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get => _address;
            set
            {
                if (value != _address)
                {
                    _address = value;
                }
                OnPropertyChanged();
            }
        }
        public int Point
        {
            get => _point;
            set
            {
                if (value != _point)
                {
                    _point = value;
                }
                OnPropertyChanged();
            }
        }
    }
}
