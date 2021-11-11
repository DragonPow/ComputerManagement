using ComputerProject.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ProductWorkSpace
{
    class ProductMainRowTableViewModel: ProductViewModel
    {
        public ProductMainRowTableViewModel() : base()
        {
            CommandClickDelete = new RelayCommand(OnClick_Delete);
            CommandClickEdit = new RelayCommand(OnClick_Edit);
        }

        public event EventHandler ClickEdit;
        public event EventHandler ClickDelete;

        public RelayCommand CommandClickEdit;
        public RelayCommand CommandClickDelete;

        void OnClick_Edit(object obj)
        {
            ClickEdit?.Invoke(this, null);
        }

        void OnClick_Delete(object obj)
        {
            ClickDelete?.Invoke(this, null);
        }
    }
}
