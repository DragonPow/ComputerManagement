using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ComputerProject
{
    public class MultipleControlViewModel : BaseViewModel
    {
        private BaseViewModel currentVM;
        public BaseViewModel CurrentViewModel { get => currentVM;
            set
            {
                currentVM = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

    }
}
