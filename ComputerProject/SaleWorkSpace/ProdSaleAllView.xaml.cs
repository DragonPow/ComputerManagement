using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerProject.SaleWorkSpace
{
    /// <summary>
    /// Interaction logic for ProdSaleAllView.xaml
    /// </summary>
    public partial class ProdSaleAllView : UserControl
    {
        public ProdSaleAllView()
        {
            InitializeComponent();
        }
    }

    public class ListProductTabSelector : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null)
            {
                var category = item as Model.Category;

                if (category == null)
                    return
                        element.FindResource("NullObjectTemplate") as DataTemplate;
                //else if (category.ChildCategories == null)
                //    return element.FindResource("NullChildObjectTemplate") as DataTemplate;
                else
                    return
                        element.FindResource("NotNullObjectTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
