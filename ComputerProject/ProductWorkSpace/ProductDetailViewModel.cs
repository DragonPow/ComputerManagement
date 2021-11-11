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
        public ProductDetailViewModel() : base()
        {
            LoadImage();
            LoadSpecification();
        }

        protected void LoadImage()
        {
            void task(){
                product.image = GetImage(product.id);
                if (product.image == null) product.image = new byte[0];
            }
            void callback()
            {
                selectedImage = Image;
                OnPropertyChanged(nameof(SelectedImage));
            }

            if (product.image == null)
            {
                DoBusyTask(task, callback);
            }
        }

        protected void LoadSpecification()
        {
            void task()
            {
                product.SPECIFICATIONs = GetSpecifications(product.id);
                if (product.SPECIFICATIONs == null) product.SPECIFICATIONs = new List<SPECIFICATION>();
            }
            void callback()
            {
                specificationList = new List<SpecificationViewModel>();
                foreach (var spec in product.SPECIFICATIONs)
                {
                    specificationList.Add(new SpecificationViewModel(spec));
                }
                OnPropertyChanged(nameof(SpecificationList));
            }

            if (product.image == null)
            {
                DoBusyTask(task, callback);
            }
        }
    }
}
