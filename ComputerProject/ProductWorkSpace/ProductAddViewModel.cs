using ComputerProject.HelperService;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Imaging;

namespace ComputerProject.ProductWorkSpace
{
    class ProductAddViewModel : ProductViewModel, IBackable
    {
        public ProductAddViewModel():base()
        {
        }

        public virtual void Prepare()
        {
            this.Product = new PRODUCT()
            {
                warrantyTime = 0,
                quantity = 0,
            };
            quantity_String = Quantity.ToString();
            DoBusyTask(GetCategoryList);
        }

        private Dictionary<int, string> cateDict;
        public string[] CategoryList =>  cateDict != null ? cateDict.Values.ToArray() : null;

        CancellationTokenSource specificationOperation = new CancellationTokenSource();
        protected List<SpecificationViewModel> specificationList;
        public List<SpecificationViewModel> SpecificationList
        {
            get => specificationList;
            protected set
            {
                specificationList = value;
                OnPropertyChanged(nameof(SpecificationList));
            }
        }

        private string selectedImagePath;
        public string SelectedImagePath
        {
            get => selectedImagePath;
            set
            {
                selectedImagePath = value;
                selectedImage = selectedImagePath != null ? new BitmapImage(new Uri(selectedImagePath)) : null;
                OnPropertyChanged(nameof(SelectedImagePath));
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        protected BitmapImage selectedImage;
        public BitmapImage SelectedImage
        {
            get => selectedImage;
        }


        public int SelectedCategoryId
        {
            get => this.Product.categoryId;
            set
            {
                this.Product.categoryId = value;

                specificationOperation.Cancel();
                specificationOperation = new System.Threading.CancellationTokenSource();
                DoBusyTask(GetSpecificationList, specificationOperation.Token);

                OnPropertyChanged(nameof(SelectedCategoryId));
                OnPropertyChanged(nameof(SelectedCategory_String));
            }
        }

        public string SelectedCategory_String
        {
            get => cateDict != null && cateDict.ContainsKey(SelectedCategoryId) ? cateDict[SelectedCategoryId] : null;
            set
            {
               SelectedCategoryId = cateDict.Where(c => c.Value == value).First().Key;
            }
        }

        public static Dictionary<int, string> GetCategoryListFromDB()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.CATEGORies.Where(c => c.parentCategoryId != null).Select(c => new
                {
                    c.id,
                    c.name
                });

                var rs = new Dictionary<int, string>();
                foreach (var row in data)
                {
                    rs.Add(row.id, row.name);
                }

                return rs;
            }
        }

        protected void GetCategoryList()
        {
            cateDict = GetCategoryListFromDB();
            OnPropertyChanged(nameof(CategoryList));
        }

        private void GetSpecificationList()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var data = db.SPECIFICATION_TYPE.Where(s => s.categoryId == SelectedCategoryId).Select(s => new
                {
                    s.id,
                    s.name
                }).ToList();

                var rs = new List<SpecificationViewModel>();
                foreach (var row in data)
                {
                    var spec = new SPECIFICATION()
                    {
                        specificationTypeId = row.id,
                        value = ""
                    };

                    rs.Add(new SpecificationViewModel(spec)
                    {
                        SpecificationName = row.name
                    });
                }

                if (!specificationOperation.IsCancellationRequested)
                {
                    SpecificationList = rs;
                }
            }
        }

        protected string quantity_String;
        public string Quantity_String
        {
            get => quantity_String;
            set
            {
                int a = -1;
                if (value != null && value != string.Empty && int.TryParse(value, out a))
                {
                    Product.quantity = a;
                    OnPropertyChanged(nameof(Quantity));
                }
                else
                {
                    error = "Số lượng không hợp lệ";
                }
                quantity_String = value;
                OnPropertyChanged(nameof(Quantity_String));
            }
        }

        protected string warranty_String;
        public string Warranty_String
        {
            get => Product.warrantyTime.ToString();
            set
            {
                int a = -1;
                if (int.TryParse(value, out a))
                {
                    Product.warrantyTime = a;
                }
                OnPropertyChanged(nameof(WarrantyTime));
                OnPropertyChanged(nameof(Warranty_String));
            }
        }

        public RelayCommand CommandSave => new RelayCommand((o) => OnSave());
        protected virtual void OnSave()
        {
            void task()
            {
                if (error != null) return;

                CheckInvalid();
                if (error != null) return;

                CheckDuplicate();
                if (error != null) return;

                Insert();
            }
            void callback()
            {
                if (error != null)
                {
                    CustomMessageBox.MessageBox.ShowError(error);
                }
                else
                {
                    CustomMessageBox.MessageBox.ShowNotify("Thêm sản phẩm thành công");
                    InsertOK?.Invoke(this, null);
                }

                error = null;
            }
            DoBusyTask(task, callback);
        }

        protected void CheckInvalid()
        {
            if (PriceOrigin < 1 || PriceSales < 1)
            {
                error = "Giá không hợp lệ";
            }
            else if (PriceSales > PriceOrigin)
            {
                error = "Giá bán phải nhỏ hơn hoặc bằng giá gốc";
            }
            else if (Name == null || Name.Trim().Length < 1)
            {
                error = "Vui lòng nhập tên sản phẩm";
            }
            else if (Quantity < 1)
            {
                error = "Số lượng không hợp lệ";
            }
            else if (SelectedCategory_String == null)
            {
                error = "Vui lòng chọn 1 danh mục";
            }
        }

        protected void CheckDuplicate()
        {
            var temp = ProductViewModel.FindByName(Name);
            if (temp != null && temp.IsStopSelling == false)
            {
                error = "Sản phẩm đã tồn tại, vui lòng chọn tên khác";
            }
        }

        void Insert()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                if (selectedImagePath != null)
                {
                    this.Product.image = FormatHelper.ImageToBytes(new System.Drawing.Bitmap(SelectedImagePath));
                }

                Product.nameIndex = FormatHelper.ConvertTo_TiengDongLao(Name);

                this.Product = db.PRODUCTs.Add(this.Product);
                foreach (var spec in SpecificationList)
                {
                    this.Product.SPECIFICATIONs.Add(spec.Model);
                }

                db.SaveChanges();
            }
        }

        public RelayCommand CommandPickImage => new RelayCommand((o) => OnPickImage());
        void OnPickImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Chọn một ảnh";
            fileDialog.Filter = "File hình ảnh|*.jpg;*.jpeg;*.png;*.bmp|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            fileDialog.Multiselect = false;
            fileDialog.FileOk += FileDialog_FileOk;

            fileDialog.ShowDialog();
        }

        private void FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var fileDialog = sender as FileDialog;
            SelectedImagePath = fileDialog.FileName;
        }

        public RelayCommand CommandClickBack => new RelayCommand((o) => OnClickBack());
        public event EventHandler ClickBack;
        protected void OnClickBack()
        {
            ClickBack?.Invoke(this, null);
        }
        public event EventHandler InsertOK;
        protected void OnInsertOK()
        {
            InsertOK?.Invoke(this, null);
        }
    }
}
