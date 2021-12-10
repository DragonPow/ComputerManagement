using ComputerProject.Helper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceTabViewModel:MultipleControlViewModel, IValidable
    {
        InsuranceAllViewModel mainViewModel;
        public InsuranceTabViewModel()
        {
            mainViewModel = new InsuranceAllViewModel(this);
            CurrentViewModel = mainViewModel;
        }

        public void Validation()
        {
            mainViewModel.Validation();
        }
    }
}
