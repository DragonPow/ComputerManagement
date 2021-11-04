using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject
{
    public class BusyViewModel : BaseViewModel
    {
        public System.Windows.Visibility BusyVisibility => isBusy ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        public bool IsFree => !IsBusy;

        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(BusyVisibility));
                OnPropertyChanged(nameof(IsFree));
            }
        }


        public async void DoBusyTask(Action busyTask, Action callback = null)
        {
            IsBusy = true;
            await Task.Run(busyTask);
            IsBusy = false;
            callback?.Invoke();
        }

        public async void DoBusyTask(Action busyTask, System.Threading.CancellationToken cancellationToken, Action callback = null)
        {
            IsBusy = true;
            await Task.Run(busyTask, cancellationToken);
            IsBusy = false;
            if (!cancellationToken.IsCancellationRequested)
            {
                callback?.Invoke();
            }
        }
    }
}
