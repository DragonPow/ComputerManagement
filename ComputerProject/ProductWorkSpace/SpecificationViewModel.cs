using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ProductWorkSpace
{
    public class SpecificationViewModel : BaseViewModel
    {
        protected SPECIFICATION model;
        public SPECIFICATION Model => model;

        private string specificationName;
        public string SpecificationName
        {
            get => specificationName;
            set
            {
                this.specificationName = value;
                OnPropertyChanged(nameof(specificationName));
            }
        }

        public int SpecificationTypeId
        {
            get => model.specificationTypeId;
            set
            {
                model.specificationTypeId = value;
                OnPropertyChanged(nameof(SpecificationTypeId));
            }
        }

        public int ProductID
        {
            get => model.productId;
            set
            {
                model.productId = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }

        public string Value
        {
            get => model.value;
            set
            {
                model.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public SpecificationViewModel()
        {
            this.model = new SPECIFICATION();
        }

        public SpecificationViewModel(SPECIFICATION model)
        {
            this.model = model;
        }
    }
}
