using ComputerProject.HelperService;

namespace ComputerProject.Model
{
    public class Specification : BaseViewModel
    {
        int _id;
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

        public Specification(SPECIFICATION specification)
        {
            
        }
    }
}