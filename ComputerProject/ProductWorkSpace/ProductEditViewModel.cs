using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ProductWorkSpace
{
    class ProductEditViewModel : ProductDetailViewModel
    {
        protected PRODUCT oldModel;
        public event EventHandler UpdateOK;

        public ProductEditViewModel(PRODUCT product) : base(product)
        {
        }

        public override void Prepare()
        {
            void task3()
            {
                GetCategoryList();
            }
            void callback3()
            {
                Console.WriteLine("Loaded cate");
                OnPropertyChanged(nameof(SelectedCategory_String));
                IsBusy = false;
                StartEdit();
            }
            void task2()
            {
                Product.image = GetImage(Product.id);
                if (Product.image == null) Product.image = new byte[0];
            }
            void callback2()
            {
                Console.WriteLine("Loaded image");
                selectedImage = Image;
                OnPropertyChanged(nameof(SelectedImage));
                DoBusyTask(task3, callback3);
            }
            void task1()
            {
                Product.SPECIFICATIONs = GetSpecifications(Product.id);
                if (Product.SPECIFICATIONs == null) Product.SPECIFICATIONs = new List<SPECIFICATION>();
            }
            void callback1()
            {
                Console.WriteLine("Loaded spec list");
                specificationList = new List<SpecificationViewModel>();
                if (Product.SPECIFICATIONs != null)
                {
                    foreach (var spec in Product.SPECIFICATIONs)
                    {
                        specificationList.Add(new SpecificationViewModel(spec));
                    }
                }
                OnPropertyChanged(nameof(SpecificationList));
                DoBusyTask(task2, callback2);
            }

            DoBusyTask(task1, callback1);
        }

        protected void StartEdit()
        {
            PRODUCT temp = new PRODUCT();
            CopyTo(Product, temp);
            oldModel = Product;
            Product = temp;
            OnPropertyChanged();
        }

        protected void EndEdit()
        {
            Product = oldModel;
            oldModel = null;
        }

        void Update()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                if (SelectedImagePath != null)
                {
                    this.Product.image = FormatHelper.ImageToBytes(new System.Drawing.Bitmap(SelectedImagePath));
                }

                Product.nameIndex = FormatHelper.ConvertTo_TiengDongLao(Name);

                db.PRODUCTs.Attach(oldModel);
                CopyTo(Product, oldModel);

                db.SaveChanges();
            }
        }

        public RelayCommand CommandUpdate => new RelayCommand(OnUpdate);
        void OnUpdate(object obj)
        {
            void task()
            {
                CheckInvalid();
                if (error != null) return;

                CheckDuplicate();
                if (error != null) return;

                Update();
            }
            void callback()
            {
                if (error != null)
                {
                    CustomMessageBox.MessageBox.Show(error);
                }
                else
                {
                    CustomMessageBox.MessageBox.Show("Lưu thay đổi thành công");
                    EndEdit();
                    UpdateOK?.Invoke(this, null);
                }
            }
            DoBusyTask(task, callback);
        }
    }
}
