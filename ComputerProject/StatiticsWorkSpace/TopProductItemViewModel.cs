using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerProject.StatiticsWorkSpace
{
    class TopProductItemViewModel : BusyViewModel
    {
        public int ProductID { get; set; }

        public BitmapImage Image { get; set; }

        public int Rank { get; set; }

        public String ProductName { get; set; }
    }
}
