using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject
{
    public partial class BILL_REPAIR : IDataErrorInfo
    {
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(customerMoney):
                        CheckExccesh(ref error);
                        break;
                    case nameof(CUSTOMER):
                        if (customerId == 0)
                        {
                            error = "CUSTOMER not null";
                        }
                        break;
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

        private void CheckExccesh(ref string error)
        {
            if (customerMoney.HasValue && price.HasValue)
            {
                if (customerMoney.Value < price.Value)
                {
                    error = "Customer money must be larger or equal than price bill";
                }
            }
        }

        public bool HasErrorData => ErrorCollection.Any();
    }
}
