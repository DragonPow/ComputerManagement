using ComputerProject.Helper;
using MaterialDesignThemes.Wpf;
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
        private Control currentMainView;
        public Control CurrentMainView { get => currentMainView;
            set
            {
                currentMainView = value;
            }
        }

        private List<Control> listViews;
        public List<Control> ListViews { get => listViews;
            set
            {
                listViews = value;
            }
        }

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
