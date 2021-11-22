using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerViewModel : BusyViewModel
    {
        private CUSTOMER _model;
        private bool _isWindowView;
        public CUSTOMER Model => _model;

        public CustomerViewModel()
        {
            this._model = new CUSTOMER();
        }

        public CustomerViewModel(bool isWindowView = false)
        {
            this._model = new CUSTOMER();
            this.IsWindowView = isWindowView;
        }

        public CustomerViewModel(CUSTOMER model)
        {
            this._model = model;
        }

        public String FullName
        {
            get => _model.name;
            set
            {
                _model.name = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public String Address
        {
            get => _model.address;
            set
            {
                _model.address = value.Trim();
                OnPropertyChanged(nameof(Address));
            }
        }

        public String PhoneNumber
        {
            get => _model.phone;
            set
            {
                if (value == null) return;

                value = value.Trim();
                if (value.Length != 10)
                {
                    error = "Số điện thoại phải có độ dài 10 chữ số";
                }
                else if (value.IndexOf('0') != 0)
                {
                    error = "Số điện thoại phải bắt đầu bằng 0";
                }
                else if (!value.All(c => "0123456789".IndexOf(c) > -1))
                {
                    error = "Số điện thoại không hợp lệ";
                }
                else
                {
                    // OK 
                    _model.phone = value;
                }
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public String Birthday_String
        {
            get => _model.birthday;
        }

        public DateTime Birthday
        {
            get => _model.birthday == null ? DateTime.Now.AddYears(-4) : DateTime.ParseExact(_model.birthday, "dd/MM/yyyy", System.Globalization.CultureInfo.GetCultureInfo("vi"));
            set
            {
                if (value == null) return;

                var formats = new string[] { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };
                if (Birthday.Year > DateTime.Now.Year - 3)
                {
                    error = "Khách chưa đủ 4 tuổi";
                }
                else
                {
                    // OK
                    _model.birthday = value.ToString("dd/MM/yyyy");
                }
                
                OnPropertyChanged(nameof(Birthday));
                OnPropertyChanged(nameof(Birthday_String));
            }
        }

        public int Point
        {
            get => _model.point;
            set
            {
                _model.point = value;
                OnPropertyChanged(nameof(Point));
                OnPropertyChanged(nameof(Point_String));
            }
        }

        public String Point_String
        {
            get => _model.point.ToString();
            set
            {
                try
                {
                    int a = -1;
                    if (value == "" || !int.TryParse(value, out a))
                    {
                        error = "Điểm thưởng không hợp lệ";
                    }
                    else
                    {
                        //OK
                        _model.point = a;
                    }
                    
                    OnPropertyChanged(nameof(Point));
                    OnPropertyChanged(nameof(Point_String));
                }
                catch (Exception)
                {
                    OnPropertyChanged(nameof(Point_String));
                }
            }
        }

        public bool IsWindowView
        {
            get => _isWindowView;
            private set
            {
                if (value != _isWindowView)
                {
                    _isWindowView = value;
                    OnPropertyChanged();
                }
            }
        }

        protected string error = null;
        public string Error {
            get => error;
            set => error = value;
        }

        public void CheckInvalid()
        {
            if (FullName == null || FullName.Trim().Length < 1)
            {
                error = "Tên không hợp lệ";
            }
        }

        public static bool GetDuplicatePhone(CustomerViewModel target)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var old = db.CUSTOMERs.Where(c => c.phone == target.PhoneNumber).Select(c => new { c.phone, c.id }).FirstOrDefault();
                return old != null;
            }
        }

        public static Task<bool> GetDuplicatePhoneAsync(CustomerViewModel target)
        {
            return Task.Run<bool>(() => GetDuplicatePhone(target));
        }

        public async Task CheckDuplicatePhoneAsync()
        {
            await Task.Run(CheckDuplicatePhoneAsync);
        }

        public void CheckDuplicatePhone()
        {
            if (GetDuplicatePhone(this))
            {
                error = "Số điện đã được sử dụng. Vui lòng chọn số khác.";
            }
        }

        public void InsertToDB()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                this._model.createTime = DateTime.Now;
                this._model = db.CUSTOMERs.Add(this._model);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Update data to database. Insert new one if not exist before.
        /// </summary>
        public void UpdateToDB()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL : " + s);
                var old = db.CUSTOMERs.FirstOrDefault(c => c.id == _model.id);
                if (old == null)
                {
                    // Add new row to db
                    this._model = db.CUSTOMERs.Add(this._model);
                }
                else
                {
                    // Update from old one
                    this.CopyTo(old);
                }
                this.CopyTo(old);
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var a = e.Message;
                }
            }
        }

        public void DeleteFromDB()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var old = db.CUSTOMERs.Where(c => c.id == _model.id).First();
                if (old != null)
                {
                    db.CUSTOMERs.Remove(old);
                }
                db.SaveChanges();
            }
        }

        public async Task InsertToDBAsycn()
        {
            await Task.Run(InsertToDB);
        }

        public async Task UpdateToDBAsycn()
        {
            await Task.Run(UpdateToDB);
        }

        public async Task DeleteFromDBAsync()
        {
            await Task.Run(DeleteFromDB);
        }

        public static List<CustomerViewModel> FindByName(string name, int startIndex, int count)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                string nameDL = FormatHelper.ConvertTo_TiengDongLao(name).ToLower();
                var rgStartWith = new Regex(String.Format("^{0}", nameDL), RegexOptions.IgnoreCase);
                var rgContain = new Regex(nameDL, RegexOptions.IgnoreCase);
                var reader = db.CUSTOMERs.Where(c => rgStartWith.IsMatch(c.name))
                    .Union(db.CUSTOMERs.Where(c => rgContain.IsMatch(c.name)))
                    .Distinct()
                    .Skip(startIndex).Take(count).ToList();

                var rs = new List<CustomerViewModel>(reader.Count);
                foreach (var row in reader)
                {
                    rs.Add(new CustomerViewModel(row));
                }

                return rs;
            }
        }

        public static List<CustomerViewModel> FindByPhone(string phone, int startIndex, int count)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL : " + s);

                var data = db.CUSTOMERs.Where(c => c.phone.StartsWith(phone))
                        .OrderBy(c => c.phone)
                        .Skip(startIndex).Take(count).ToList();

                if (data.Count < count)
                {
                    startIndex -= db.CUSTOMERs.Where(c => c.phone.StartsWith(phone)).Count();

                    if (startIndex < 0) startIndex = 0;
                    var temp1 = db.CUSTOMERs.Where(c => c.phone.Contains(phone) && !c.phone.StartsWith(phone))
                        .OrderBy(c => c.phone)
                        .Skip(startIndex).Take(count - data.Count).ToList();

                    data.AddRange(temp1);
                }

                var rs = new List<CustomerViewModel>(data.Count);
                foreach (var c in data)
                {
                    rs.Add(new CustomerViewModel(c));
                }

                return rs;
            }
        }

        public static int CountByPhone(string phone)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL : " + s);

                return db.CUSTOMERs.Where(c => c.phone.Contains(phone)).Count();
            }
        }

        public static async Task<List<CustomerViewModel>> FindByNameAsync(string name, int startIndex, int count)
        {
            return await Task.Run(() => FindByName(name, startIndex, count));
        }

        public static async Task<List<CustomerViewModel>> FindByPhoneAsync(string phone, int startIndex, int count, System.Threading.CancellationToken cancellationToken)
        {
            return await Task.Run(() => FindByPhone(phone, startIndex, count), cancellationToken);
        }

        public static async Task<int> CountByPhoneAsync(string phone, System.Threading.CancellationToken cancellationToken)
        {
            return await Task.Run(() => CountByPhone(phone), cancellationToken);
        }

        public void Insert(Action callback = null)
        {
            CheckInvalid(); // Check invalid data

            if (error != null)
            {
                CustomMessageBox.MessageBox.Show(error);
                return;
            }

            void save_CB()
            {
                if (Error != null)
                {
                    CustomMessageBox.MessageBox.Show(Error);
                }
                else
                {
                    CustomMessageBox.MessageBox.Show("Đã thêm khách hàng mới vào cơ sở dữ liệu thành công");
                    callback?.Invoke();
                }
                Error = null;
            }
            void save()
            {
                if (Error != null)
                {
                    try
                    {
                        InsertToDB();
                    }
                    catch (Exception) when (!HelperService.Environment.IsDebug)
                    {
                        error = FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01");
                    }
                }
            }

            void checkDup_CB()
            {
                DoBusyTask(save, save_CB);
            }


            DoBusyTask(CheckDuplicatePhone, checkDup_CB);
        }

        public void CopyTo(CUSTOMER other, bool includeID = false)
        {
            other.name = this._model.name;
            other.address = this._model.address;
            other.phone = this._model.phone;
            other.birthday = this._model.birthday;
            other.point = this._model.point;

            if (includeID) other.id = this._model.id;
        }

        public void CopyTo(CustomerViewModel other, bool includeID = false)
        {
            this.CopyTo(other._model, includeID);
        }

        public bool HasInBill()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var bill = db.BILLs.Where(b => b.customerId == Model.id).Select(b => new
                {
                    b.id,
                    b.customerId,
                }).FirstOrDefault();

                return bill != null;
            }
        } 
    }
}
