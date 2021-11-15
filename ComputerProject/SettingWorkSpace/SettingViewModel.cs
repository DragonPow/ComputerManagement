using ComputerProject.ApplicationWorkspace;
using ComputerProject.CustomMessageBox;
using ComputerProject.HelperService;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerProject.SettingWorkSpace
{
    class SettingViewModel : HelperService.BaseViewModel, ITabView
    {
        #region Field
        public string ViewName = "Thiết lập";
        public PackIconKind ViewIcon => PackIconKind.Settings;

        public string _storeName;
        public string _storePhone;
        public string _storeAddress;
        public string _pointToMoney;
        public string _moneyToPoint;
        public string _maxPoint;

        ICommand _saveStoreCommand;
        ICommand _savePointCommand;
        ICommand _cancelPointCommand;
        ICommand _cancelStoreCommand;
        #endregion

        #region Properties
       
        string ITabView.ViewName => ViewName;

        PackIconKind ITabView.ViewIcon => ViewIcon;

        protected Model.Regulation model;
        public Model.Regulation Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }
        public string StoreName
        {
            get => _storeName;
            set
            {
                this._storeName = value;
                OnPropertyChanged(nameof(_storeName));
            }
        }

        public string StorePhone
        {
            get => _storePhone;
            set
            {
                _storePhone = value;
                OnPropertyChanged(nameof(_storePhone));
            }
        }

        public string StoreAddress
        {
            get => _storeAddress;
            set
            {
                _storeAddress = value;
                OnPropertyChanged(nameof(_storeAddress));
            }
        }
        public string PointToMoney
        {
            get => _pointToMoney;
            set
            {
                _storePhone = value;
                OnPropertyChanged(nameof(_pointToMoney));
            }
        }

        public string MoneyToPoint
        {
            get => _moneyToPoint;
            set
            {
                _moneyToPoint = value;
                OnPropertyChanged(nameof(_moneyToPoint));
            }
        }

        public string MaxPoint
        {
            get => _maxPoint;
            set
            {
                _storePhone = value;
                OnPropertyChanged(nameof(_maxPoint));
            }
        }

        public ICommand SaveStoreCommand
        {
            get
            {
                if (_saveStoreCommand == null)
                {
                    _saveStoreCommand = new RelayCommand(a =>
                    {
                        try
                        {
                            Save_store();
                        }
                        catch (InvalidOperationException e)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng nhập đầy đủ thông tin!", "Lỗi", PackIconKind.Error);
                            return;
                        }

                    });

                }
                return _saveStoreCommand;

            }
        }
        public ICommand CancelStoreCommand
        {
            get
            {
                if (_cancelStoreCommand == null)
                {
                    _cancelStoreCommand = new RelayCommand(a => {
                        LoadStoreData();
                    });
                }
                return _cancelStoreCommand;
            }
        }
        public ICommand SavePointCommand
        {
            get
            {
                if (_savePointCommand == null)
                {
                    _savePointCommand = new RelayCommand(a =>
                    {
                        try
                        {
                            Save_store();
                        }
                        catch (InvalidOperationException e)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng nhập đầy đủ thông tin!", "Lỗi", PackIconKind.Error);
                            return;
                        }

                    });

                }
                return _saveStoreCommand;

            }
        }
        public ICommand CancelPointCommand
        {
            get
            {
                if (_cancelStoreCommand == null)
                {
                    _cancelStoreCommand = new RelayCommand(a => {
                        LoadPointData();
                    });
                }
                return _cancelStoreCommand;
            }
        }
        #endregion

        #region commmand
        public SettingViewModel()
        {
            LoadStoreData();
        }

        public void LoadStoreData()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.REGULATIONs.Where(p => p.name == "StoreName").FirstOrDefault();
                StoreName = data.value.ToString();
                data = db.REGULATIONs.Where(p => p.name == "StorePhone").FirstOrDefault();
                StorePhone = data.value.ToString();
                data = db.REGULATIONs.Where(p => p.name == "StoreAddress").FirstOrDefault();
                StoreAddress = data.value.ToString();
            }
        }
        public void LoadPointData()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.REGULATIONs.Where(p => p.name == "StoreName").FirstOrDefault();
                data = db.REGULATIONs.Where(p => p.name == "PointToMoney").FirstOrDefault();
                PointToMoney = data.value.ToString();
                data = db.REGULATIONs.Where(p => p.name == "MoneyToPoint").FirstOrDefault();
                MoneyToPoint = data.value.ToString();
                data = db.REGULATIONs.Where(p => p.name == "MaxPoint").FirstOrDefault();
                MaxPoint = data.value.ToString();
            }
        }

        public void Save_store()
        {
            if (StoreName == "" || StorePhone == "" || StoreAddress == "")
            {
                throw new InvalidOperationException("Empty data!");
            }
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.REGULATIONs.Where(p => p.name == "StoreName").FirstOrDefault();
                data.value = StoreName;
                data = db.REGULATIONs.Where(p => p.name == "StorePhone").FirstOrDefault();
                data.value = StorePhone;
                data = db.REGULATIONs.Where(p => p.name == "StoreAddress").FirstOrDefault();
                data.value = StoreAddress;
                data = db.REGULATIONs.Where(p => p.name == "PointToMoney").FirstOrDefault();
                data.value = PointToMoney;
                data = db.REGULATIONs.Where(p => p.name == "MoneyToPoint").FirstOrDefault();
                data.value = MoneyToPoint;
                data = db.REGULATIONs.Where(p => p.name == "MaxPoint").FirstOrDefault();
                data.value = MaxPoint;
            }
        }
        public void Save_point()
        {
            if (PointToMoney == "" || MoneyToPoint == "" || MaxPoint == "")
            {
                throw new InvalidOperationException("Empty data!");
            }
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.REGULATIONs.Where(p => p.name == "StoreName").FirstOrDefault();
                data = db.REGULATIONs.Where(p => p.name == "PointToMoney").FirstOrDefault();
                data.value = PointToMoney;
                data = db.REGULATIONs.Where(p => p.name == "MoneyToPoint").FirstOrDefault();
                data.value = MoneyToPoint;
                data = db.REGULATIONs.Where(p => p.name == "MaxPoint").FirstOrDefault();
                data.value = MaxPoint;
            }
        }
        #endregion

    }
}


