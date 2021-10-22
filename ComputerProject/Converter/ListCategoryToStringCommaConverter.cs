using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ComputerProject.Model;

namespace ComputerProject.Converter
{
    //[ValueConversion(typeof(ICollection<Model.Category>), typeof(string))]
    public class ListCategoryToStringCommaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = "";
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("The target must be string");
            }
            if (value == null) return s;
            if (!(value is Collection<Category>))
            {
                throw new InvalidOperationException("Value must be List Category");
            }

            foreach (var i in (Collection<Category>)value)
            {
                if (s == "") s += i.Name;
                s += "," + i.Name;
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
