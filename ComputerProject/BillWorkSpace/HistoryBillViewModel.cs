using ComputerProject.CustomMessageBox;
using ComputerProject.Helper;
using ComputerProject.HelperService;
using ComputerProject.Repository;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.BillWorkSpace
{
    public class HistoryBillViewModel : Helper.BaseViewModel
    {
        #region Fields
        NavigationService _navigator;
        BillRepository _repository;
        public BusyViewModel BusyService { get; private set; } = new BusyViewModel();
        CancellationTokenSource cancelSearchBill = new CancellationTokenSource();

        int _maxBillsInPage = 10;
        Collection<BILL> _bills;
        Collection<BILL> _selectionBills;
        int _currentPage;
        int _totalPage;
        string _textSearch;
        DateTime? _timeFrom;
        DateTime? _timeTo;

        ICommand _searchBillCommand;
        ICommand _exportPdfCommand;
        ICommand _deleteBillCommand;
        ICommand _showDetailBillCommand;
        ICommand _changePageCommand;
        #endregion // Fields

        #region Properties
        public Collection<BILL> CurrentBills
        {
            get => _bills;
            set
            {
                if (value != _bills)
                {
                    _bills = value;
                    OnPropertyChanged();
                }
            }
        }
        public Collection<BILL> SelectionBills
        {
            get => _selectionBills;
            set
            {
                if (value != _selectionBills)
                {
                    _selectionBills = value;
                    OnPropertyChanged();
                }
            }
        }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value != _currentPage)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TotalPage
        {
            get => _totalPage;
            set
            {
                if (value != _totalPage)
                {
                    _totalPage = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TextSearch
        {
            get => _textSearch;
            set
            {
                if (value != _textSearch)
                {
                    _textSearch = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? TimeFrom
        {
            get => _timeFrom;
            set
            {
                if (value != _timeFrom)
                {
                    _timeFrom = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? TimeTo
        {
            get => _timeTo;
            set
            {
                if (value != _timeTo)
                {
                    _timeTo = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SearchBillCommand
        {
            get
            {
                if (null == _searchBillCommand)
                {
                    _searchBillCommand = new RelayCommand(s =>
                    {
                        if (isValidSearch())
                        {
                            SearchBill(TextSearch, TimeFrom, TimeTo);
                        }
                    });
                }
                return _searchBillCommand;
            }
        }
        public ICommand ExportPdfCommand
        {
            get
            {
                if (null == _exportPdfCommand)
                {
                    _exportPdfCommand = new RelayCommand(_ => ExportPdf(SelectionBills));
                }
                return _exportPdfCommand;
            }
        }
        public ICommand DeleteBillCommand
        {
            get
            {
                if (null == _deleteBillCommand)
                {
                    _deleteBillCommand = new RelayCommand(b =>
                    {
                        var rs = MessageBoxCustom.ShowDialog("Hóa đơn bị xóa sẽ không thể hoàn tác", "Thông báo", PackIconKind.InformationCircle);
                        if (rs == MessageBoxResultCustom.Yes)
                        {
                            BusyService.DoBusyTask(() =>
                            {
                                DeleteBill(b as BILL);
                                LoadBills(CurrentPage);
                            }, () => MessageBoxCustom.ShowDialog("Xóa hóa đơn thành công", "Thông báo", PackIconKind.DoneOutline));
                        }
                    });
                }
                return _deleteBillCommand;
            }
        }
        public ICommand ShowDetailBillCommand
        {
            get
            {
                if (null == _showDetailBillCommand)
                {
                    _showDetailBillCommand = new RelayCommand(b => ShowDetail(b as BILL));
                }
                return _showDetailBillCommand;
            }
        }
        public ICommand ChangePageCommand
        {
            get
            {
                if (null == _changePageCommand)
                {
                    _changePageCommand = new RelayCommand(pageNumber => ChangePage((int)pageNumber));
                }
                return _changePageCommand;
            }
        }
        #endregion //Properties

        public HistoryBillViewModel(int maxBillsInPage = 8)
        {
            //BusyService = new BusyViewModel();
            this._maxBillsInPage = maxBillsInPage;
            _repository = new BillRepository();
        }

        public void LoadBills(int pageNumber = 1, string text = null, DateTime? timeFrom = null, DateTime? timeTo = null)
        {
            TextSearch = text;
            TimeFrom = timeFrom;
            TimeTo = timeTo;

            TotalPage = _repository.LoadNumberPages(_maxBillsInPage, TextSearch, TimeFrom, TimeTo);

            if (TotalPage < CurrentPage)
            {
                CurrentPage = TotalPage;
            }
            else CurrentPage = pageNumber;

            CurrentBills = _repository.LoadBills(_maxBillsInPage, CurrentPage, TextSearch, TimeFrom, TimeTo);
        }
        public void LoadData()
        {
            ResetSearchData();
            BusyService.DoBusyTask(() => LoadBills(1));
        }
        private void ResetSearchData()
        {
            TextSearch = null;
            TimeFrom = TimeTo = null;
        }
        public void LoadBillsAsync(int pageNumber = 1, string text = null, DateTime? timeFrom = null, DateTime? timeTo = null)
        {
            cancelSearchBill.Cancel();
            cancelSearchBill = new CancellationTokenSource();
            BusyService.DoBusyTask(() => LoadBills(pageNumber, text, timeFrom, timeTo), cancelSearchBill.Token);
        }
        public void SetNavigator(NavigationService navigator)
        {
            this._navigator = navigator;
        }

        private void SearchBill(string text, DateTime? timeFrom, DateTime? timeTo)
        {
            LoadBillsAsync(1, text, timeFrom, timeTo);
        }
        private void ExportPdf(Collection<BILL> bills)
        {
            throw new NotImplementedException();
        }
        private void DeleteBill(BILL bill)
        {
            if (CurrentBills.Contains(bill))
            {
                CurrentBills.Remove(bill);
                _repository.Delete(new Model.Bill(bill));
            }
            else
            {
                throw new InvalidOperationException("List not containt this bill");
            }
        }
        private void ShowDetail(BILL bill)
        {
            if (_navigator == null) throw new NullReferenceException("Navigator is null");

            DetailBillViewModel vm = new DetailBillViewModel(bill.id);
            vm.setNavigator(_navigator);
            vm.BillDeletedEvent += (sender, id) =>
            {
                LoadBillsAsync(CurrentPage);
            };

            _navigator.NavigateTo(vm);
            _navigator.Back = () => _navigator?.NavigateTo(this);
        }
        private void ChangePage(int pageNumber)
        {
            LoadBillsAsync(CurrentPage = pageNumber, TextSearch, TimeFrom, TimeTo);
            //CurrentBills = _repository.LoadBills(_maxBillsInPage, CurrentPage = pageNumber);
        }
        private bool isValidSearch()
        {
            bool rs = true;
            if (TimeFrom.HasValue && TimeTo.HasValue)
            {
                Console.WriteLine("Time from: {0}", TimeFrom.Value.Date);
                Console.WriteLine("Time to: {0}", TimeTo.Value.Date);

                if (TimerService.CheckDayLarger(TimeFrom.Value, TimeTo.Value))
                {
                    rs = rs && false;
                    MessageBoxCustom.ShowDialog("Ngày bắt đầu phải bé hơn ngày kết thúc", "Lỗi", PackIconKind.ErrorOutline);
                }
            }

            return rs;
        }
    }
}
