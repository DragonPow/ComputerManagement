using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerTabViewModel : BaseViewModel
    {
        public Control CurrentMainView { get; set; }

        public List<Control> ListViews { get; set; }

        public int CurrentMainViewIndex
        {
            get => ListViews.IndexOf(CurrentMainView);
            set
            {
                CurrentMainView = ListViews[value];

                OnPropertyChanged(nameof(CurrentMainView));
                OnPropertyChanged(nameof(CurrentMainViewIndex));
            }
        }
    }
}
