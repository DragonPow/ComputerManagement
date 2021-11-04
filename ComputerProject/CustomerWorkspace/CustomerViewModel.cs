using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerViewModel: BusyViewModel
    {
        private CUSTOMER _model;
        public CUSTOMER Model => _model;

        public CustomerViewModel()
        {
            this._model = new CUSTOMER();
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
                _model.phone = value.Trim();
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
                _model.birthday = value.ToString("dd/MM/yyyy");
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
                    _model.point = int.Parse(value.ToString());
                    OnPropertyChanged(nameof(Point));
                    OnPropertyChanged(nameof(Point_String));
                }
                catch (Exception)
                {
                    OnPropertyChanged(nameof(Point_String));
                }
            }
        }

        public string GetInvalid()
        {
            if (FullName == null || FullName.Trim().Length < 1)
            {
                return "Tên không hợp lệ";
            }

            if (PhoneNumber == null || PhoneNumber.Trim().Length < 10 || !PhoneNumber.Trim().All(c => "0123456789".IndexOf(c) > -1))
            {
                return "Số điện thoại không hợp lệ";
            }

            DateTime dt;
            var formats = new string[] { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };
            if (Birthday == null)
            {
                return "Ngày sinh không hợp lệ";
            }
            if (Birthday.Year > DateTime.Now.Year - 3)
            {
                return "Khách chưa đủ 4 tuổi";
            }

            return null;
        }

        /// <summary>
        /// Check if a customer is already exist in database
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Return null not duplicate. Return duplicate-detail if duplicated.</returns>
        public static async Task<string> GetDuplicateAsync(CustomerViewModel target)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var old = await Task.Run(() => db.CUSTOMERs.Where(c => c.phone == target.PhoneNumber).FirstOrDefault());
                if (old != null)
                {
                    return "Số điện thoại đã đucợ đăng kí trước đó";
                }
                else return null;
            }
        }

        public static string GetDuplicate(CustomerViewModel target)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var old = db.CUSTOMERs.Where(c => c.phone == target.PhoneNumber).FirstOrDefault();
                if (old != null)
                {
                    return "Số điện thoại đã đucợ đăng kí trước đó";
                }
                else return null;
            }
        }

        public void InsertToDB()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
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
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL : " + s);
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
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL : " + s);

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
            string error = GetInvalid(); // Check invalid data

            if (error != null)
            {
                CustomMessageBox.MessageBox.Show(error);
                return;
            }

            DoBusyTask(() =>
            {
                error = CustomerViewModel.GetDuplicate(this); // Busy task
                
            }, ()=>
            {
                if (error == null)
                {
                    try
                    {
                        DoBusyTask(InsertToDB, () =>
                        {
                            CustomMessageBox.MessageBox.Show("Đã thêm khách hàng mới vào cơ sở dữ liệu thành công");
                            callback?.Invoke(); // Callback
                        });
                    }
                    catch (Exception) when (!HelperService.Environment.IsDebug)
                    {
                        IsBusy = false;
                        CustomMessageBox.MessageBox.Show(FormatHelper.GetErrorMessage("Đã xảy ra lỗi khi truy cập cơ sở dữ liệu", "DB-01"));
                    }
                }
                else
                {
                    CustomMessageBox.MessageBox.Show(error);
                }
            });
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
    }
}
