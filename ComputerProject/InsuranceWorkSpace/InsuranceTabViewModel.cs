using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceTabViewModel:MultipleControlViewModel
    {
        InsuranceAllViewModel mainViewModel;
        public InsuranceTabViewModel()
        {
            mainViewModel = new InsuranceAllViewModel();
        }

        public void Validation()
        {
            mainViewModel.Validation();
        }
    }
}
