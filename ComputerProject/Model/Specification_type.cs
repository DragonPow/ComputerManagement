using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Model
{
    public class Specification_type : BaseViewModel, IDataErrorInfo
    {
        int _id;
        int _number;
        string _name;
        int _categoryId;

        #region Properties
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

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(Name):
                        {
                            if (String.IsNullOrWhiteSpace(Name))
                            {
                                error = "Name is not null";
                            }
                            break;
                        }
                }

                if (ErrorCollection.ContainsKey(columnName))
                {
                    if (error != null) ErrorCollection[columnName] = error;
                    else ErrorCollection.Remove(columnName);
                }
                else if (error != null)
                {
                    ErrorCollection.Add(columnName, error);
                }

                OnPropertyChanged(nameof(ErrorCollection));
                return error;
            }
        }
        public bool HasErrorData => ErrorCollection.Any(); 
        #endregion //Properties

        public Specification_type()
        {

        }

        public Specification_type(SPECIFICATION_TYPE specification_type)
        {
            _id = specification_type.id;
            _name = specification_type.name;
            _categoryId = specification_type.categoryId;
        }

        public SPECIFICATION_TYPE CastToModel()
        {
            SPECIFICATION_TYPE s = new SPECIFICATION_TYPE() { id = this.Id, name = this.Name};
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
