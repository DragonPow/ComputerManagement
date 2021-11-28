using ComputerProject.ApplicationWorkspace;
using ComputerProject.CustomMessageBox;
using ComputerProject.HelperService;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComputerProject.SettingWorkSpace
{
    class SettingViewModel : HelperService.BaseViewModel, ITabView
    {
        #region Field


        public string ViewName => "Thiết lập";
        public PackIconKind ViewIcon => PackIconKind.Settings;

        Visibility buttonStoreVisibility_Edit = Visibility.Hidden;
        Visibility buttonStoreVisibility_Read = Visibility.Visible;
        Visibility buttonPointVisibility_Edit = Visibility.Hidden;
        Visibility buttonPointVisibility_Read = Visibility.Visible;

        bool _storeEditMode = false;
        bool _pointEditMode = false;

        private string _storeName;
        private string _storePhone;
        private string _storeAddress;
        private string _pointToMoney;
        private string _moneyToPoint;
        private string _maxPoint;
        private Action getStoreinfo;
        private Action getPointinfo;


        ICommand _saveStoreCommand;
        ICommand _changeStoreCommand;
        ICommand _changePointCommand;
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
                OnPropertyChanged();
            }
        }

        public string StorePhone
        {
            get => _storePhone;
            set
            {
                _storePhone = value;
                OnPropertyChanged();
            }
        }

        public string StoreAddress
        {
            get => _storeAddress;
            set
            {
                _storeAddress = value;
                OnPropertyChanged();
            }
        }
        public string PointToMoney
        {
            get => _pointToMoney;
            set
            {
                _pointToMoney = value;
                OnPropertyChanged();
            }
        }

        public string MoneyToPoint
        {
            get => _moneyToPoint;
            set
            {
                _moneyToPoint = value;
                OnPropertyChanged();
            }
        }

        public string MaxPoint
        {
            get => _maxPoint;
            set
            {
                _maxPoint = value;
                OnPropertyChanged();
            }
        }

        public bool StoreEditMode
        {
            get => _storeEditMode;
            set
            {
                _storeEditMode = value;
                OnPropertyChanged();
            }
        }

        public bool PointEditMode
        {
            get => _pointEditMode;
            set
            {
                _pointEditMode = value;
                OnPropertyChanged();
            }
        }

        public Visibility ButtonPointVisibility_Edit
        {
            get => buttonPointVisibility_Edit;
            set
            {
                buttonPointVisibility_Edit = value;
                OnPropertyChanged();
            }
        }

        public Visibility ButtonPointVisibility_Read
        {
            get => buttonPointVisibility_Read;
            set
            {
                buttonPointVisibility_Read = value;
                OnPropertyChanged();
            }
        }

        public Visibility ButtonStoreVisibility_Edit
        {
            get => buttonStoreVisibility_Edit;
            set
            {
                buttonStoreVisibility_Edit = value;
                OnPropertyChanged();
            }
        }

        public Visibility ButtonStoreVisibility_Read
        {
            get => buttonStoreVisibility_Read;
            set
            {
                buttonStoreVisibility_Read = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeStoreCommand
        {
            get
            {
                if (_changeStoreCommand == null)
                {
                    _changeStoreCommand = new RelayCommand(a => 
                    ChangeStore());
                }
                return _changeStoreCommand;
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
                            if (e.Message == "Empty data!") MessageBoxCustom.ShowDialog("Vui lòng nhập đầy đủ thông tin!", "Lỗi", PackIconKind.Error);
                            else if (e.Message == "Wrong PhoneNumber") MessageBoxCustom.ShowDialog("Số điện thoại không hợp lệ!", "Lỗi", PackIconKind.Error);
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
                        MessageBoxResultCustom result = MessageBoxCustom.ShowDialog("Thông tin chỉnh sửa chưa được lưu.\nBạn muốn hủy chỉnh sửa?", "Thông báo", PackIconKind.Error);
                        if (result == MessageBoxResultCustom.Yes)
                        {
                            LoadDataAsync();
                            StoreEditMode = false;
                            ButtonStoreVisibility_Read = Visibility.Visible;
                            ButtonStoreVisibility_Edit = Visibility.Hidden;
                        }    
                           
                    });
                }
                return _cancelStoreCommand;
            }
        }

        public ICommand ChangepointCommand
        {
            get
            {
                if (_changePointCommand == null)
                {
                    _changePointCommand = new RelayCommand(a =>
                    ChangePoint());
                }
                return _changePointCommand;
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
                            Save_point();
                            
                        }
                        catch (InvalidOperationException e)
                        {
                            MessageBoxCustom.ShowDialog("Vui lòng nhập đầy đủ thông tin!", "Lỗi", PackIconKind.Error);
                            
                            return;
                        }

                    });
                   

                }
     
                return _savePointCommand;

            }
        }
        public ICommand CancelPointCommand
        {
            get
            {
                if (_cancelPointCommand == null)
                {
                    _cancelPointCommand = new RelayCommand(a =>{
                        MessageBoxResultCustom result= MessageBoxCustom.ShowDialog("Thông tin chỉnh sửa chưa được lưu. \nBạn muốn hủy chỉnh sửa?", "Thông báo", PackIconKind.Error);
                        if (result==MessageBoxResultCustom.Yes)
                        {
                            LoadDataAsync();
                            PointEditMode = false;
                            ButtonPointVisibility_Read = Visibility.Visible;
                            ButtonPointVisibility_Edit = Visibility.Hidden;
                        }    
                    });
                }
                return _cancelPointCommand;
            }
        }
        #endregion

        #region Constructor
        public SettingViewModel()
        {
            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                await Task.Run(getStoreinfo = ()=>
                {
                    var data = db.REGULATIONs.Where(p => p.name == "StoreName").FirstOrDefault();
                    StoreName = data.value.ToString();
                    data = db.REGULATIONs.Where(p => p.name == "StorePhone").FirstOrDefault();
                    StorePhone = data.value.ToString();
                    data = db.REGULATIONs.Where(p => p.name == "StoreAdress").FirstOrDefault();
                    StoreAddress = data.value.ToString();
                });
                await Task.Run(getPointinfo = () =>
                {
                    var data = db.REGULATIONs.Where(p => p.name == "PointToMoney").FirstOrDefault();
                    PointToMoney = data.value.ToString();
                    data = db.REGULATIONs.Where(p => p.name == "MoneyToPoint").FirstOrDefault();
                    MoneyToPoint = data.value.ToString();
                    data = db.REGULATIONs.Where(p => p.name == "MaxPoint").FirstOrDefault();
                    MaxPoint = data.value.ToString();
                });
            }
        }

        public bool AllowChangeTab()
        {
            return true;
        }

        public void Save_store()
        {
            if (StoreName == "" || StorePhone == "" || StoreAddress == "")
            {
                throw new InvalidOperationException("Empty data!");
            }
            if (!CheckPhoneNumber())
            {
                throw new InvalidOperationException("Wrong PhoneNumber");
            }
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.REGULATIONs.Where(p => p.name == "StoreName").FirstOrDefault();
                data.value = StoreName;
                data = db.REGULATIONs.Where(p => p.name == "StorePhone").FirstOrDefault();
                data.value = StorePhone;
                data = db.REGULATIONs.Where(p => p.name == "StoreAdress").FirstOrDefault();
                data.value = StoreAddress;
                db.SaveChanges();
                StoreEditMode = false;
                ButtonStoreVisibility_Read = Visibility.Visible;
                ButtonStoreVisibility_Edit = Visibility.Hidden;
                MessageBoxCustom.ShowDialog("Lưu thông tin cửa hàng thành công!", "Thông báo", PackIconKind.AlertCircleCheck);
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
                db.SaveChanges();
                PointEditMode = false;
                ButtonPointVisibility_Read = Visibility.Visible;
                ButtonPointVisibility_Edit = Visibility.Hidden;
                MessageBoxCustom.ShowDialog("Lưu thông tin điểm thưởng thành công!", "Thông báo", PackIconKind.AlertCircleCheck);
            }
        }
        private void ChangeStore()
        {
            StoreEditMode = true;
            ButtonStoreVisibility_Read = Visibility.Hidden;
            ButtonStoreVisibility_Edit = Visibility.Visible;
        }
        private void ChangePoint()
        {
            PointEditMode = true;
            ButtonPointVisibility_Read = Visibility.Hidden;
            ButtonPointVisibility_Edit = Visibility.Visible;
        }
        private bool CheckPhoneNumber()
        {
            if (StorePhone == null || StorePhone.Trim().Length != 10 || StorePhone[0]!='0' || !StorePhone.Trim().All(c => "0123456789".IndexOf(c) > -1))
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}


