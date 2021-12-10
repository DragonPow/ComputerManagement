using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComputerProject.InsuranceWorkSpace.InsuranceDetailViewModel;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceAllViewModel: PagingViewModel<InsuranceViewModel>
    {
        MultipleControlViewModel viewController;
        public InsuranceAllViewModel()
        {
            this.PropertyChanged += InsuranceAllViewModel_PropertyChanged;
            step = 20;
            selectedSortType = sortTypes[1];
            selectedBillStatus = billStatus[0];
        }

        public InsuranceAllViewModel(MultipleControlViewModel viewController):this()
        {
            this.viewController = viewController;
        }

        protected override List<InsuranceViewModel> _search()
        {
            return InsuranceViewModel.FindByPhoneOrID(SearchContent, billStatus.IndexOf(SelectedBillStatus), currentStartIndex, step, sortTypes.IndexOf(SelectedSortType));
        }

        protected override int _countMax()
        {
            return InsuranceViewModel.CountByPhoneOrID(SearchContent, billStatus.IndexOf(SelectedBillStatus));
        }

        private void InsuranceAllViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(SearchContent)))
            {
                Validation();
            }
            else if (e.PropertyName.Equals(nameof(CurrentPage)))
            {
                Search();
            }
        }

        private List<String> sortTypes = new List<string>() {
            "Ngày tiếp nhận: Thấp đến cao",
            "Ngày tiếp nhận: Cao đến thấp",
            "Số điện thoại: A -> Z",
            "Số điện thoại: Z -> A"
            };
        public List<String> SortTypes => sortTypes;
        protected String selectedSortType;
        public String SelectedSortType
        {
            get => selectedSortType;
            set
            {
                if (value.Equals(selectedSortType)) return;
                selectedSortType = value;
                OnPropertyChanged(nameof(SelectedSortType));
                Validation();
            }
        }

        private List<String> billStatus = new List<string>() {
            "Tất cả",
            InsuranceViewModel.StatusToString(0),
            InsuranceViewModel.StatusToString(1),
            InsuranceViewModel.StatusToString(2)
            };
        public List<String> BillStatus => billStatus;
        protected String selectedBillStatus;
        public String SelectedBillStatus
        {
            get => selectedBillStatus;
            set
            {
                if (value.Equals(selectedBillStatus)) return;
                selectedBillStatus = value;
                OnPropertyChanged(nameof(SelectedBillStatus));
                Validation();
            }
        }

        public RelayCommand CommandEdit_Item => new RelayCommand(OnClickEdit_Item);
        public RelayCommand CommandDelete_Item => new RelayCommand(OnClickDelete_Item);
        public RelayCommand CommandDetail_Item => new RelayCommand(OnClickDetail_Item);
        public RelayCommand CommandInsert => new RelayCommand((o) => OnClickInsert());

        void OnClickEdit_Item(object itemVM)
        {
            InsuranceViewModel vm = itemVM as InsuranceViewModel;
            OpenDetailView(vm.Id, StatusView.Edit);
            //CustomMessageBox.MessageBox.ShowNotify("Click edit item : " + vm.Id_String);
        }

        void OnClickDelete_Item(object itemVM)
        {
            InsuranceViewModel vm = itemVM as InsuranceViewModel;
            //CustomMessageBox.MessageBox.ShowNotify("Click delete item : " + vm.Id_String);
        }

        void OnClickDetail_Item(object itemVM)
        {
            InsuranceViewModel vm = itemVM as InsuranceViewModel;
            OpenDetailView(vm.Id, StatusView.View);
            //CustomMessageBox.MessageBox.ShowNotify("Click detail item : " + vm.Id_String);
        }

        void OnClickInsert()
        {
            OpenDetailView(0, StatusView.Add);
            //CustomMessageBox.MessageBox.ShowNotify("Click insert");
        }

        public void BackToThisView(bool needReloadData)
        {
            viewController.CurrentViewModel = this;
            if (needReloadData)
            {
                Validation();
            }
        }

        void OpenDetailView(int id, StatusView status)
        {
            InsuranceDetailViewModel newView = new InsuranceDetailViewModel(id, status);
            newView.NavigateBack += (s, e) =>
            {
                BackToThisView(true);
            };
            viewController.CurrentViewModel = newView;
        }
    }
}
