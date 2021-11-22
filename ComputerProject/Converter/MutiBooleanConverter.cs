using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComputerProject.Converter
{
    public class MutiBooleanConverter : IMultiValueConverter
    {
        public enum TypeResult
        {
            AllTrue,
            AnyTrue,
        };

        public TypeResult typeResult { get; set; } = TypeResult.AllTrue;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(i => !(i is bool))) {
                throw new InvalidCastException("Type of value must be boolean");
            }

            bool rs;

            switch(typeResult)
            {
                case TypeResult.AllTrue:
                    {
                        rs = true;

                        foreach (bool value in values)
                        {
                            rs = rs && value;
                        }

                        break;
                    }
                case TypeResult.AnyTrue:
                    {
                        rs = false;

                        foreach (bool value in values)
                        {
                            rs = rs || value;
                        }

                        break;
                    }
                default:
                    throw new InvalidOperationException("Type must be in enum TypeResult");
            }

            return rs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
