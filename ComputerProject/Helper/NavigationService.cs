using ComputerProject.CustomerWorkspace;
using System;
using System.Collections.Generic;

namespace ComputerProject.HelperService
{
    public interface INavigationBar
    {
        BaseViewModel CurrentPage { get; }
    }
    public class NavigationService : INavigationBar
    {
        BaseViewModel _currentPageViewModel;

        public BaseViewModel CurrentPage
        {
            get => _currentPageViewModel;
            private set
            {
                if (value!=_currentPageViewModel)
                {
                    _currentPageViewModel = value;
                }
                //OnPropertyChanged();
                OnCurrentPageChangedEvent?.Invoke();
            }
        }

        public event Action OnCurrentPageChangedEvent;
        public Action Back;


        public NavigationService(BaseViewModel initPage = null)
        {
            CurrentPage = initPage;
        }


        public void NavigateTo(BaseViewModel newPage)
        {
            CurrentPage = newPage;
        }
    }
}