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

        protected void StartEdit()
        {
            PRODUCT temp = new PRODUCT();
            CopyTo(product, temp);
            oldModel = product;
            product = temp;
        }

        protected void EndEdit()
        {
            product = oldModel;
            oldModel = null;
        }

        void Update()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                if (SelectedImagePath != null)
                {
                    this.product.image = FormatHelper.ImageToBytes(new System.Drawing.Bitmap(SelectedImagePath));
                }

                product.nameIndex = FormatHelper.ConvertTo_TiengDongLao(Name);

                db.PRODUCTs.Attach(oldModel);

                CopyTo(product, oldModel);

                db.SaveChanges();
            }
        }
    }
}
