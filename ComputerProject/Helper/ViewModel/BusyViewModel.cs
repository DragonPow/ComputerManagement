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
        public BusyViewModel()
        {
            IsBusy = false;
        }

        public System.Windows.Visibility BusyVisibility => IsBusy ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
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

        public Task<T> GetTask<T>(Func<T> busyTask, Action callback = null)
        {
            IsBusy = true;
            var rs = Task.Run(busyTask);
            rs.ContinueWith((task) =>
            {
                IsBusy = false;
                callback?.Invoke();
            });

            return rs;
        }

        public Task GetTask(Task task, Action callback = null)
        {
            IsBusy = true;
            return task.ContinueWith(_ =>
            {
                IsBusy = false;
                callback?.Invoke();
            });
        }

        public Task GetTask(Action task, Action callback = null)
        {
            IsBusy = true;
            return Task.Run(task).ContinueWith(_ =>
            {
                IsBusy = false;
                callback?.Invoke();
            });
        }

        public async Task<T> GetTask<T>(Task<T> task, Action callback = null)
        {
            IsBusy = true;
            var rs = await task;
            IsBusy = false;
            callback?.Invoke();
            return rs;
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

        public async void DoBusyTask(Action<System.Threading.CancellationToken> busyTask, System.Threading.CancellationToken cancellationToken, Action callback = null)
        {
            IsBusy = true;
            await Task.Run(() => busyTask(cancellationToken), cancellationToken);
            IsBusy = false;
            if (!cancellationToken.IsCancellationRequested)
            {
                callback?.Invoke();
            }
        }

        public async void DoBusyTask(Action[] busyTasks, System.Threading.CancellationToken cancellationToken, Action[] callback = null)
        {
            IsBusy = true;
            for(int i = 0; i< busyTasks.Length && !cancellationToken.IsCancellationRequested; i++)
            {
                await Task.Run(() => busyTasks[i], cancellationToken);
                if (!cancellationToken.IsCancellationRequested)
                {
                    callback[i]?.Invoke();
                }
            }
            IsBusy = false;
        }

        public async void DoInBackGround(Action busyTask, System.Threading.CancellationToken cancellationToken, Action callback = null)
        {
            await Task.Run(() => busyTask(), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                callback?.Invoke();
            }
        }

        public Task WhenAll(params Task[] tasks)
        {
            IsBusy = true;
            return Task.WhenAll(tasks).ContinueWith((_) =>
            {
                IsBusy = false;
                //callback?.Invoke();
            });

        }
    }
}
