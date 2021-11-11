using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerProject
{
    class PagingViewModel<T> : BusyViewModel
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

    }
}
