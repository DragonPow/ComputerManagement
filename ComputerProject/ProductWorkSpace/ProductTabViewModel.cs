using ComputerProject.Helper.Interface;
using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ProductWorkSpace
{
    class ProductTabViewModel: MultipleControlViewModel, IValidable
    {
        ProductMainViewModel mainVM;
        public ProductTabViewModel()
        {
            mainVM = new ProductMainViewModel(this);
            CurrentViewModel = mainVM;
        }

        public void Validation()
        {
            mainVM.Validation();
        }
    }
}
