using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerProject
{
    public static class FormatHelper
    {
        public static String GetErrorMessage(String description, String errorCode)
        {
            return string.Format("{0}.{1}Mã lỗi: {2}", description, Environment.NewLine, errorCode);
        }

        /// <summary>
        /// Convert from "Bằng pro" to "Bang pro"
        /// </summary>
        /// <param name="vn_str">Text in Vietnammese</param>
        /// <returns>Text in Dong Lao's language</returns>
        public static string ConvertTo_TiengDongLao(string vn_str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = vn_str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}
