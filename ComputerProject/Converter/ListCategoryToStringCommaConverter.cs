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
    [ValueConversion(typeof(IList<Model.Category>), typeof(string))]
    public class ListCategoryToStringCommaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = "";
            if (value == null) return s;
            
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("The target must be string");
            }

            foreach (var i in (Collection<Category>)value)
            {
                if (s == "") s += i.Name;
                else s += ", " + i.Name;
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
