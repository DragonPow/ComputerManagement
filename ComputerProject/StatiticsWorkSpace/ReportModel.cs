using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerProject.StatiticsWorkSpace
{
    class ReportModel : BaseViewModel
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public String ProductName { get; set; }
        public String CategoryName { get; set; }
        public long Money { get; set; }
        public int Amount { get; set; }

        /*protected BitmapImage image;
        public BitmapImage Image { get => image; set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }*/

        protected byte[] image;
        public byte[] Image
        {
            get => image; set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public int Rank { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public object Tag;
        public int Day { get; set; }
    }
}
