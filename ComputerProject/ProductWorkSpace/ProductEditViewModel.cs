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
            quantity_String = Quantity.ToString();
            void task3()
            {
                if (oldModel.image == null)
                {
                    oldModel.image = GetImage(oldModel.id);
                    if (oldModel.image == null) oldModel.image = new byte[0];
                }
            }
            void callback3()
            {
                Console.WriteLine("Loaded image");

                Product.image = oldModel.image;

                if (SelectedImagePath == null || SelectedImagePath.Length == 0)
                {
                    selectedImage = Image;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
            void task2()
            {
                GetCategoryList();
            }
            void callback2()
            {
                Console.WriteLine("Loaded cate");
                OnPropertyChanged(nameof(SelectedCategory_String));
                IsBusy = false;
                StartEdit();
                DoBusyTask(task3, callback3);
            }

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
                DoBusyTask(task2, callback2);
            }

            DoBusyTask(task, callback);
        }

        protected void StartEdit()
        {
            PRODUCT temp = new PRODUCT();
            CopyTo(Product, temp);

            oldModel = Product;
            Product = temp;

            foreach (var spec in oldModel.SPECIFICATIONs)
            {
                specificationList.Where(s => s.SpecificationTypeId.Equals(spec.specificationTypeId)).First().Model = new SPECIFICATION()
                {
                    productId = spec.productId,
                    specificationTypeId = spec.specificationTypeId,
                    value = spec.value
                };
            }
            OnPropertyChanged(nameof(SpecificationList));
        }

        protected void EndEdit()
        {
            Product = oldModel;

            foreach (var spec in oldModel.SPECIFICATIONs)
            {
                specificationList.Where(s => s.SpecificationTypeId.Equals(spec.specificationTypeId)).First().Model = spec;
            }
            OnPropertyChanged(nameof(SpecificationList));

            oldModel = null;
        }

        void Update()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL Update: " + s);

                Product.nameIndex = FormatHelper.ConvertTo_TiengDongLao(Name);

                db.PRODUCTs.Attach(this.oldModel);

                // Copy image
                if (SelectedImagePath != null)
                {
                    this.oldModel.image = FormatHelper.ImageToBytes(new System.Drawing.Bitmap(SelectedImagePath));
                }

                // Copy spec list
                if (oldModel.categoryId != Product.categoryId || oldModel.SPECIFICATIONs == null)
                {
                    if (oldModel.SPECIFICATIONs != null)
                    {
                        oldModel.SPECIFICATIONs.Clear();
                    }
                    else
                    {
                        oldModel.SPECIFICATIONs = new List<SPECIFICATION>(Product.SPECIFICATIONs.Count);
                    }

                    foreach (var spec in SpecificationList)
                    {
                        oldModel.SPECIFICATIONs.Add(new SPECIFICATION()
                        {
                            productId = spec.ProductID,
                            specificationTypeId = spec.SpecificationTypeId,
                            value = spec.SpecValue
                        });
                    }
                }
                else
                {
                    var destinationSpecs = (List<SPECIFICATION>)oldModel.SPECIFICATIONs;
                    foreach (var spec in specificationList)
                    {
                        oldModel.SPECIFICATIONs.Where(s => s.specificationTypeId == spec.SpecificationTypeId)
                            .First().value = spec.SpecValue;
                    }
                }

                // Copy others
                CopyTo(base.Product, this.oldModel);

                db.SaveChanges();
            }
        }

        public RelayCommand CommandUpdate => new RelayCommand(OnUpdate);
        void OnUpdate(object obj)
        {
            void task()
            {
                if (error != null) return;

                CheckInvalid();
                if (error != null) return;

                if (oldModel.name != Product.name)
                {
                    CheckDuplicate();
                    if (error != null) return;
                }

                Update();
            }
            void callback()
            {
                if (error != null)
                {
                    CustomMessageBox.MessageBox.ShowError(error);
                }
                else
                {
                    CustomMessageBox.MessageBox.ShowNotify("Lưu thay đổi thành công");
                    EndEdit();
                    UpdateOK?.Invoke(this, null);
                }

                error = null;
            }
            DoBusyTask(task, callback);
        }
    }
}
