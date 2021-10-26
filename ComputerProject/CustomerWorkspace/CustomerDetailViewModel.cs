using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerProject.CustomerWorkspace
{
    public class CustomerDetailViewModel : CustomerViewModel
    {
        public CustomerDetailViewModel() : base()
        {

        }
        public CustomerDetailViewModel(CUSTOMER customer) : base(customer)
        {

        }

        Visibility buttonGroupVisibility_Edit = Visibility.Hidden;
        public Visibility ButtonGroupVisibility_Edit
        {
            get => buttonGroupVisibility_Edit;
            set
            {
                buttonGroupVisibility_Edit = value;
                OnPropertyChanged(nameof(ButtonGroupVisibility_Edit));
            }
        }

        Visibility buttonGroupVisibility_Read = Visibility.Visible;
        public Visibility ButtonGroupVisibility_Read
        {
            get => buttonGroupVisibility_Read;
            set
            {
                buttonGroupVisibility_Read = value;
                OnPropertyChanged(nameof(ButtonGroupVisibility_Read));
            }
        }

        String title;
        public String Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(title));
            }
        }
    }
}
