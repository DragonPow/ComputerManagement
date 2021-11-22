using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.Helper.Interface
{
    interface IPagingViewModel<T>
    {
        List<T> _search();

        int _countMax();
    }
}
