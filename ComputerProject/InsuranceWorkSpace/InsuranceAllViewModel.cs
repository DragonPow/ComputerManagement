using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.InsuranceWorkSpace
{
    class InsuranceAllViewModel: PagingViewModel<InsuranceViewModel>
    {
        private List<String> sortTypes = new List<string>() {
            "Ngày tiếp nhận: Thấp đến cao",
            "Ngày tiếp nhận: Cao đến thấp",
            "Số điện thoại: A -> Z",
            "Số điện thoại: Z -> A"
            };
        public List<String> SortTypes => sortTypes;
        public String SelectedSortType
        {
            get;set;
        }

        MultipleControlViewModel viewController;
        public InsuranceAllViewModel()
        {
            this.PropertyChanged += InsuranceAllViewModel_PropertyChanged;
            step = 20;
            SelectedSortType = sortTypes[1];
        }

        public InsuranceAllViewModel(MultipleControlViewModel viewController):this()
        {
            this.viewController = viewController;
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

        protected override List<InsuranceViewModel> _search()
        {
            return InsuranceViewModel.FindByPhoneOrID(SearchContent, currentStartIndex, step, sortTypes.IndexOf(SelectedSortType));
        }

        protected override int _countMax()
        {
            return InsuranceViewModel.CountByPhoneOrID(SearchContent);
        }

        public RelayCommand CommandEdit_Item => new RelayCommand(OnClickEdit_Item);
        public RelayCommand CommandDelete_Item => new RelayCommand(OnClickDelete_Item);
        public RelayCommand CommandDetail_Item => new RelayCommand(OnClickDetail_Item);
        public RelayCommand CommandInsert => new RelayCommand((o) => OnClickInsert());

        void OnClickEdit_Item(object itemVM)
        {
            InsuranceViewModel vm = itemVM as InsuranceViewModel;
            CustomMessageBox.MessageBox.ShowNotify("Click edit item : " + vm.Id_String);
        }

        void OnClickDelete_Item(object itemVM)
        {
            InsuranceViewModel vm = itemVM as InsuranceViewModel;
            CustomMessageBox.MessageBox.ShowNotify("Click delete item : " + vm.Id_String);
        }

        void OnClickDetail_Item(object itemVM)
        {
            InsuranceViewModel vm = itemVM as InsuranceViewModel;
            CustomMessageBox.MessageBox.ShowNotify("Click detail item : " + vm.Id_String);
        }

        void OnClickInsert()
        {
            CustomMessageBox.MessageBox.ShowNotify("Click insert");
        }

        public void BackToThisView(bool needReloadData)
        {
            viewController.CurrentViewModel = this;
            if (needReloadData)
            {
                Validation();
            }
        }
    }
}
