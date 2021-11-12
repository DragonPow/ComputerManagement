using ComputerProject.ProductWorkSpace;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComputerProject.Converter
{
    [ValueConversion(typeof(ProductViewModel), typeof(string))]
    class ProductStatusConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rs;
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("Unsupported target type");
            }

            var input = value as ProductViewModel;
            if (input == null)
            {
                throw new InvalidOperationException("Unsupported input type");
            }

            if (input.IsStopSelling)
            {
                rs = "Ngừng bán";
            }
            else if (input.Quantity < 1)
            {
                rs = "Hết hàng";
            }
            else
            {
                rs = "Đang bán";
            }
            return rs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
