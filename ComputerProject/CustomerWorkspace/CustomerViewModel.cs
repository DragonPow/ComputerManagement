using ComputerProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerViewModel: BaseViewModel
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
                _model.address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public String PhoneNumber
        {
            get => _model.phone;
            set
            {
                _model.phone = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public String Birthday
        {
            get => _model.birthday;
            set
            {
                _model.birthday = value;
                OnPropertyChanged(nameof(Birthday));
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
                _model.point = int.Parse(value.ToString());
                OnPropertyChanged(nameof(Point));
                OnPropertyChanged(nameof(Point_String));
            }
        }

        private System.Windows.Visibility busyVisibility = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility BusyVisibility { get => busyVisibility;
            set
            {
                busyVisibility = value;
                OnPropertyChanged(nameof(BusyVisibility));
            }
        }

        public string GetError()
        {
            if (FullName == null || FullName.Trim().Length < 1)
            {
                return "Tên không hợp lệ";
            }

            if (PhoneNumber == null || PhoneNumber.Trim().Length < 10 || !PhoneNumber.All(c => "0123456789".IndexOf(c) > -1))
            {
                return "Số điện thoại không hợp lệ";
            }

            DateTime dt;
            var formats = new string[] { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };
            if (Birthday == null || !DateTime.TryParseExact(Birthday, formats, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dt))
            {
                return "Ngày sinh không hợp lệ";
            }
            if (dt.Year > DateTime.Now.Year - 3)
            {
                return "Khách chưa đủ 4 tuổi";
            }

            return null;
        }

        /// <summary>
        /// Check if a customer is already exist in database
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Return true if already exist (may not exactly equal to target)</returns>
        public static async Task<bool> CheckDuplicate(CustomerViewModel target)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                return await Task.Run<object>(() => db.CUSTOMERs.Where(c => c.phone == target.PhoneNumber).FirstOrDefault()) != null;
            }
        }

        void save()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var old = db.CUSTOMERs.Where(c => c.id == _model.id).FirstOrDefault();
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
                db.SaveChanges();
            }
        }

        public async Task Save()
        {
            await Task.Run(save);
        }


        public void DeleteSync()
        {
            this.Delete().Wait();
        }

        public Task Delete()
        {
            try
            {
                using (ComputerManagementEntities db = new ComputerManagementEntities())
                {
                    if (_model.id == -1)
                    {
                        return Task.CompletedTask;
                    }
                    else
                    {
                        // Update from old one
                        var old = db.CUSTOMERs.Where(c => c.id == _model.id).First();
                        if (old != null)
                        {
                            db.CUSTOMERs.Remove(old);
                        }
                    }
                    return db.SaveChangesAsync();
                }
            }
            catch (Exception e) when (!Helper.Environment.IsDebug)
            {
                string des = "Đã xảy ra lỗi khi truy cập cơ sở dữ liệu";
                string errorCode = "DB-01";
                string msg = FormatHelper.GetErrorMessage(des, errorCode);

                return Task.CompletedTask;
            }
        }

        public static List<CustomerViewModel> FindName(string name, int startIndex, int count)
        {
            try
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
            catch (Exception e) when (!Helper.Environment.IsDebug)
            {
                string des = "Đã xảy ra lỗi khi truy cập cơ sở dữ liệu";
                string errorCode = "DB-01";
                string msg = FormatHelper.GetErrorMessage(des, errorCode);

                // Should log exception to a file

                return null;
            }
        }

        public void CopyTo(CUSTOMER other)
        {
            other.name = this._model.name;
            other.address = this._model.address;
            other.phone = this._model.phone;
            other.birthday = this._model.birthday;
            other.point = this._model.point;
        }
    }
}
