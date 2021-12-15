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

            // Load product image
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
                    OnPropertyChanged(string.Empty);
                }
            }

            // Load category list to select
            void task2()
            {
                GetCategoryList();
            }
            void callback2()
            {
                Console.WriteLine("Loaded cate");
                IsBusy = false;
                StartEdit();
                DoBusyTask(task3, callback3);
            }

            // Load Spec
            void task()
            {
                var tempSpecs = new List<SpecificationViewModel>(GetSpecifications(Product.id));
                foreach (var spec in tempSpecs)
                {
                    Product.SPECIFICATIONs.Add(spec.Model);
                }

                GetSpecificationList();
                foreach (var spec in SpecificationList)
                {
                    spec.ProductID = Product.id;
                    var spe = Product.SPECIFICATIONs.Where(s => s.specificationTypeId == spec.Model.specificationTypeId).FirstOrDefault();
                    if (spe != null)
                    {
                        spec.Model.value = spe.value;
                    }
                    else
                    {
                        Product.SPECIFICATIONs.Add(spec.Model);
                    }
                }
            }
            void callback()
            {
                Console.WriteLine("Loaded spec list");
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

            foreach (var spec in SpecificationList)
            {
                spec.Model.value = oldModel.SPECIFICATIONs.Where(s => s.specificationTypeId.Equals(spec.SpecificationTypeId)).First().value;
            }

            OnPropertyChanged(nameof(SpecificationList));

            oldModel = null;
        }

        void Update()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL Update: " + s);
                var old = db.PRODUCTs.Where(p => p.id == Product.id).First();
                old.SPECIFICATIONs = db.SPECIFICATIONs.Where(s => s.productId == Product.id).ToList();

                Product.nameIndex = FormatHelper.ConvertTo_TiengDongLao(Name);

                // Copy image
                if (SelectedImagePath != null)
                {
                    old.image = FormatHelper.ImageToBytes(new System.Drawing.Bitmap(SelectedImagePath));
                }

                // Copy spec list
                if (old.categoryId != Product.categoryId || old.SPECIFICATIONs == null)
                {
                    if (old.SPECIFICATIONs != null)
                    {
                        old.SPECIFICATIONs.Clear();
                    }
                    else
                    {
                        old.SPECIFICATIONs = new List<SPECIFICATION>(Product.SPECIFICATIONs.Count);
                    }

                    foreach (var spec in SpecificationList)
                    {
                        old.SPECIFICATIONs.Add(new SPECIFICATION()
                        {
                            productId = spec.ProductID,
                            specificationTypeId = spec.SpecificationTypeId,
                            value = spec.SpecValue
                        });
                    }
                }
                else
                {
                    foreach (var spec in specificationList)
                    {
                        var oldSpec = old.SPECIFICATIONs.Where(s => s.specificationTypeId == spec.SpecificationTypeId).FirstOrDefault();
                        if (oldSpec != null)
                        {
                            oldSpec.value = spec.SpecValue;
                        }
                        else
                        {
                            /* spec.Model.productId = oldModel.id;

                             db.SPECIFICATIONs.Add(spec.Model);*/
                            old.SPECIFICATIONs.Add(spec.Model);
                        }
                    }
                }

                // Copy others
                CopyTo(base.Product, old);
                db.SaveChanges();

                CopyTo(old, this.oldModel);
                this.oldModel.SPECIFICATIONs = old.SPECIFICATIONs;
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
