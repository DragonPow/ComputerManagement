using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ComputerProject.Converter
{
    [ValueConversion(typeof(bool),typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v;
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("Target must be type Visibility");
            }

            v = (bool)value ? Visibility.Visible : Visibility.Hidden;
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
