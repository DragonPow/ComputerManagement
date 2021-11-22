using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ProductWorkSpace
{
    class ProductDetailViewModel: ProductAddViewModel
    {
        public ProductDetailViewModel(PRODUCT product)
        {
            this.Product = product;
        }

        public override void Prepare()
        {
            quantity_String = Quantity.ToString();
            warranty_String = WarrantyTime.ToString();
            LoadCatagotery();
            LoadImage();
            LoadSpecification();
        }

        protected void LoadCatagotery()
        {
            void task()
            {
                GetCategoryList();
            }
            void callback()
            {
                Console.WriteLine("Loaded cate");
                OnPropertyChanged(nameof(SelectedCategory_String));
            }
            DoBusyTask(task, callback);
        }

        protected void LoadImage()
        {
            void task()
            {
                if (Product.image == null)
                {
                    Product.image = GetImage(Product.id);
                }
            }
            void callback()
            {
                Console.WriteLine("Loaded image");
                selectedImage = Image;
                OnPropertyChanged(nameof(SelectedImage));
            }
            DoBusyTask(task, callback);
        }

        protected void LoadSpecification()
        {
            void task()
            {
                specificationList = new List<SpecificationViewModel>(GetSpecifications(Product.id));
            }
            void callback()
            {
                Console.WriteLine("Loaded spec list");
                if (SpecificationList != null)
                {
                    Product.SPECIFICATIONs = new List<SPECIFICATION>();
                    foreach (var spec in SpecificationList)
                    {
                        Product.SPECIFICATIONs.Add(spec.Model);
                    }
                }
                OnPropertyChanged(nameof(SpecificationList));
            }

            DoBusyTask(task, callback);
        }

        public event EventHandler ClickEdit;
        public event EventHandler DeleteOK;

        public RelayCommand CommandClickEdit => new RelayCommand(OnClickEdit);
        void OnClickEdit(object obj)
        {
            ClickEdit?.Invoke(this, null);
        }

        public RelayCommand CommandClickDelete => new RelayCommand(OnClickDelete);
        void OnClickDelete(object obj)
        {
            bool hasInBill = false;
            void task1()
            {
                hasInBill = HasInBill(Id);
            }
            void task2()
            {
                string msg = hasInBill ? "Sản phẩm đã được bán trước đó. Sẽ tiến hành ngừng bán sản phẩm." : "Dữ liệu đã xóa sẽ không thể hoàn tác.";
                var msb = new CustomMessageBox.MessageBox(msg, "Xóa sản phẩm", "Tôi hiểu", "Hủy", MaterialDesignThemes.Wpf.PackIconKind.Warning
                , () =>
                {
                    DoBusyTask(() => DeleteFromDB(hasInBill), callback);
                });
                msb.ShowDialog();
            }

            void callback()
            {

                DeleteOK?.Invoke(this, null);
            }

            DoBusyTask(task1, task2);
        }
    }
}
