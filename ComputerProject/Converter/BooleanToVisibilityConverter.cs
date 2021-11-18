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
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v;
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("Target must be type Visibility");
            }

            v = (bool)value ? TrueValue : FalseValue;
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
