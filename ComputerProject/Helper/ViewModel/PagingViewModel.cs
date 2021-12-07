using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerProject
{
    public class PagingViewModel<T> : BusyViewModel
    {
        protected List<T> itemList;
        public List<T> ItemList
        {
            get => itemList;
            set
            {
                itemList = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }

        protected CancellationTokenSource countOperation = new CancellationTokenSource();
        protected CancellationTokenSource searchOperation = new CancellationTokenSource();

        public String searchContent = "";
        public String SearchContent
        {
            get => searchContent;
            set
            {
                if (value == null) { value = ""; }

                if (searchContent.Equals(value)) return;

                searchContent = value;

                OnPropertyChanged(nameof(SearchContent));
            }
        }

        protected int currentStartIndex => (CurrentPage - 1) * step;
        protected int step = 20;

        protected int maxPage = 1;
        public int MaxPage
        {
            get => maxPage;
            set
            {
                maxPage = value;
                OnPropertyChanged(nameof(MaxPage));
            }
        }

        protected int currentPage = 1;
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }

            set
            {
                if (value < 1 || value > MaxPage) return;
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        protected virtual List<T> _search()
        {
            throw new NotImplementedException();
        }

        protected virtual int _countMax()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            Console.WriteLine("Search");
            searchOperation.Cancel();
            searchOperation = new CancellationTokenSource();

            List<T> l = null;
            void task()
            {
                l = _search();
            }
            void callback()
            {
                ItemList = l;
            }
            DoBusyTask(task, searchOperation.Token, callback);
        }

        /// <summary>
        /// Count max page
        /// </summary>
        /// <param name="resetPage">Remain current page number if true. Reset page number (to 1) if set true</param>
        public void CalculatePage(bool resetPage)
        {
            Console.WriteLine("CountPage");
            countOperation.Cancel();
            countOperation = new CancellationTokenSource();

            int max = 0;
            void task()
            {
                max = _countMax();
            }
            void callback()
            {
                MaxPage = max % step > 0 ? max / step + 1 : max / step;
                if (resetPage)
                {
                    CurrentPage = 1;
                }
                else
                {
                    if (CurrentPage > MaxPage)
                    {
                        CurrentPage = maxPage;
                    }
                    else
                    {
                        CurrentPage = CurrentPage;
                    }
                }
            }

            DoBusyTask(task, countOperation.Token, callback);
        }

        public void Validation()
        {
            CalculatePage(true);
        }
    }
}
