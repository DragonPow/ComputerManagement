using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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

        public static string ToMoney(int val, bool hasCurrency = false)
        {
            var rs = hasCurrency ? val.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi")) + "VND" : val.ToString("N0");
            return rs;
        }

        public static int CheckMoney(string val)
        {
            int temp;
            if (Int32.TryParse(val, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.CurrentCulture, out temp) && temp > 0)
            {
                return temp;
            }
            else return 0;
        }

        public static byte[] ImageToBytes(Bitmap image, bool needScale = true)
        {
            if (image == null) return null;
            byte[] rs;

            if (needScale)
            {
                var desSize = new SizeF(185*2, 120*2);

                int h = image.Height, w = image.Width;
                var scaleFactor = Math.Min(desSize.Width / w, desSize.Height / h);
                var newSize = new Size((int)(w * scaleFactor), (int)(h * scaleFactor));

                image = new Bitmap(image, newSize);
                //image = Scale(image);
            }

            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);

                rs = ms.ToArray();
            }

            return rs;
        }

        public static Bitmap Scale(Bitmap image)
        {
            float width = 192 * 2, height = 108 * 2;
            int h = image.Height, w = image.Width;
            var scale = Math.Min(width / h, height / w);
            var newSize = new Size((int)(w * scale), (int)(h * scale));

            var bmp = new Bitmap((int)width, (int)height);
            var graph = Graphics.FromImage(bmp);

            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            graph.FillRectangle(new SolidBrush(Color.White), new RectangleF(0, 0, width, height));
            graph.DrawImage(image, newSize.Width / 2, newSize.Height / 2, newSize.Width, newSize.Height);

            return bmp;
        }

        public static System.Windows.Media.Imaging.BitmapImage BytesToImage(byte[] bytes)
        {
            if (bytes == null) return null;
            System.Windows.Media.Imaging.BitmapImage rs = new System.Windows.Media.Imaging.BitmapImage();

            using (var ms = new System.IO.MemoryStream(bytes))
            {
                rs.BeginInit();
                rs.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad; // important here
                rs.StreamSource = ms;
                rs.EndInit();
                rs.Freeze();
            }

            return rs;
        }

        public static Bitmap TranferToBitmap(byte[] bytecode)
        {
            if (bytecode == null) return null;
            Bitmap bm;
            using(var ms = new MemoryStream(bytecode))
            {
                ms.Write(bytecode, 0, bytecode.Length);
                ms.Seek(0, SeekOrigin.Begin);

                bm = new Bitmap(ms);
            }

            return bm;
        }

        public static String getMoneyLabel(int money)
        {
            string[] prefix = new string[] { "k", "tr", "tỷ" };
            int i = 0;
            while (money / 1000000 != 0)
            {
                money /= 1000;
                i++;
            }
            if (money < 1000) { return money.ToString(); }
            double rs = money / 1000d;
            return String.Format("{0:0.#}", rs) + prefix[i];
        }

        public static void SetTimeOut(System.Data.Entity.DbContext db, int secs)
        {
            var dbContext = (db as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            dbContext.CommandTimeout = secs;
        }

        public static DateTime TextToDateTime(string dateText)
        {
            return DateTime.ParseExact(dateText, "dd/MM/yyyy", System.Globalization.CultureInfo.GetCultureInfo("vi"));
        }
        public static string DatetimeToDateString(DateTime date)
        {
            return date.ToShortDateString();
        }

        public static string ConvertBillID(int id)
        {
            return string.Format("HD{0}", id.ToString("000000000"));
        }
        public static string ConvertBillRepairID(int id, bool isWarranty)
        {
            if (isWarranty)
            {
                return string.Format("BH{0}", id.ToString("000000000"));
            }

            return string.Format("HD{0}", id.ToString("000000000"));
        }
    }
}
