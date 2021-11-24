using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Threading.Tasks;
using ComputerProject.Model;
using ComputerProject.Repository;
using ComputerProject.CustomMessageBox;
using MaterialDesignThemes.Wpf;

namespace ComputerProject.Helper
{
    public class PrintPDF
    {
        #region Fields
        private readonly string dest = @".\Bill\"; //Path.Combine(AppManager.GetPreferencesFolder(), "Bill.pdf");
        private string dir_font = @"..\..\Resource\Font\vuArial.ttf";

        static NumberFormatInfo nfi;
        private Rectangle defaultSize;
        FileStream os;
        Document doc;
        PdfWriter writer;
        BaseFont basef;

        StoreInformation store;
        #endregion //End Fields

        private static PrintPDF instance;
        public static PrintPDF Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PrintPDF();
                }
                return instance;
            }
        }

        public PrintPDF()
        {
            basef = BaseFont.CreateFont(dir_font, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";

            CheckDestExists();
            LoadStoreInformation();
            //open stream to write on the file
        }
        private void CheckDestExists()
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
        }
        public void LoadStoreInformation()
        {
            store = StoreRepository.GetStoreInformation();
        }
        public bool createBill(Bill info, bool openAfterComplete = false)
        {
            var productInBill = info.Products;
            string dirFile = Path.Combine(dest, "Bill" + info.Id + ".pdf");

            //check if file is open
            try
            {
                using (FileStream r = File.OpenWrite(dirFile)) { }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                MessageBoxCustom.ShowDialog("Không thể thao tác trên tệp tin", "Lỗi", PackIconKind.ErrorOutline);
                return false;
            }

            os = new FileStream(dirFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            defaultSize = new Rectangle(PageSize.A4.Width, 310 + productInBill.Count * 24 + productInBill.Count(i => i.Name.Length > 16) * 15);
            doc = new Document(defaultSize);
            doc.SetMargins(0, 0, 0, 0);

            writer = PdfWriter.GetInstance(doc, os);

            //start write
            doc.Open();

            //add title paragraph
            doc.Add(this.createTitle());

            //add name and date of the bill
            doc.Add(createInfoPurchase(info));

            //add product table 
            doc.Add(this.createTableInfoProduct(info));

            //add end paragraph
            doc.Add(createEndBill());

            //end write
            doc.Close();
            os.Close();
            writer.Close();

            doc.Dispose();
            os.Dispose();
            writer.Dispose();

            Console.WriteLine("Create file {0} success", dirFile);

            //Open file PDF
            if (openAfterComplete) Process.Start(dirFile);
            return true;

        }
        private Paragraph createTitle()
        {
            Paragraph title = new Paragraph();
            title.Add(new Paragraph(store.Name, new Font(basef, 20, Font.BOLD))
            { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10 });
            title.Add(new Paragraph("Địa chỉ: " + store.Address, new Font(basef, 10, Font.ITALIC))
            { Alignment = Element.ALIGN_CENTER, SpacingAfter = 2 });
            title.Add(new Paragraph("Phone: " + store.Phone, new Font(basef, 10, Font.ITALIC))
            { Alignment = Element.ALIGN_CENTER, SpacingAfter = 15 });
            title.Add(new Paragraph("HOÁ ĐƠN THANH TOÁN", new Font(basef, 20, Font.BOLD))
            { Alignment = Element.ALIGN_CENTER, SpacingAfter = 25 });
            return title;
        }
        private PdfPTable createTableInfoProduct(Bill info)
        {
            Font font = new Font(basef, 12);
            PdfPTable table = new PdfPTable(4);
            table.DefaultCell.Border = PdfPCell.NO_BORDER;

            //set height column of table
            int[] columnWidths = new int[4];
            columnWidths[0] = 70;
            columnWidths[1] = 30;
            columnWidths[2] = 40;
            columnWidths[3] = 40;
            table.SetWidths(columnWidths);
            int defaultBorder = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;

            table.AddCell(new PdfPCell(new Phrase("Tên sản phẩm", new Font(basef, 12, Font.BOLD)))
            { Border = defaultBorder, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10, PaddingTop = 8 });
            table.AddCell(new PdfPCell(new Phrase("Số lượng", new Font(basef, 12, Font.BOLD)))
            { HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = defaultBorder, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10, PaddingTop = 8 });
            table.AddCell(new PdfPCell(new Phrase("Đơn giá", new Font(basef, 12, Font.BOLD)))
            { HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = defaultBorder, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10, PaddingTop = 8 });
            table.AddCell(new PdfPCell(new Phrase("Thành tiền", new Font(basef, 12, Font.BOLD)))
            { HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = defaultBorder, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10, PaddingTop = 8 });

            //add list product
            foreach (var item in info.Products)
            {
                table.AddCell(new PdfPCell(new Phrase(item.Name, font))
                { Border = PdfPCell.NO_BORDER, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });

                table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), font))
                { Border = PdfPCell.NO_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });

                table.AddCell(new PdfPCell(new Phrase(item.PriceUnit.ToString("N0", nfi) + "đ", font))
                { Border = PdfPCell.NO_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });

                table.AddCell(new PdfPCell(new Phrase((item.PriceUnit * item.Quantity).ToString("N0", nfi) + "đ", font))
                { Border = PdfPCell.NO_BORDER, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });
            }
            //----------------------------------------------------------------------------------------------------
            //add table sum price of bill
            //table.AddCell(new PdfPCell(new Phrase("Thành tiền:", new Font(basef, 12, Font.BOLD)))
            //{ Colspan = 3, Border = PdfPCell.TOP_BORDER, PaddingTop = 5 });

            //table.AddCell(new PdfPCell(new Phrase(info.TotalPrice.ToString("N0", nfi), new Font(basef, 12, Font.BOLD)))
            //{ HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.TOP_BORDER, PaddingTop = 5 });

            //Giảm giá

            //table.AddCell(new PdfPCell(new Phrase("Giảm giá: ", new Font(basef, 10)))
            //{ Colspan = 3, Border = PdfPCell.NO_BORDER, PaddingBottom = 10 });

            //table.AddCell(new PdfPCell(new Phrase((info.TOTAL - info.Price_Bill).ToString("N0", nfi), new Font(basef, 10)))
            //{ HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10 });

            //-----------------------------------------------------------------------------------------------------
            table.AddCell(new PdfPCell(new Phrase("Tổng cộng", new Font(basef, 13, Font.BOLD)))
            { Colspan = 3, Border = PdfPCell.TOP_BORDER, PaddingTop = 15 });

            table.AddCell(new PdfPCell(new Phrase(info.TotalPriceProducts.ToString("N0", nfi) + " đ", new Font(basef, 13, Font.BOLD)))
            { HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.TOP_BORDER, PaddingTop = 15 });

            table.AddCell(new PdfPCell(new Phrase("Tiền khách đưa", new Font(basef, 10, Font.NORMAL)))
            { Colspan = 3, Border = PdfPCell.NO_BORDER, PaddingTop = 10, PaddingLeft = 20 });

            table.AddCell(new PdfPCell(new Phrase(info.MoneyCustomer.ToString("N0", nfi) + " đ", new Font(basef, 10, Font.NORMAL)))
            { HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.NO_BORDER, PaddingTop = 10, PaddingLeft = 20 });

            table.AddCell(new PdfPCell(new Phrase("Tiền trả khách", new Font(basef, 8, Font.NORMAL)))
            { Colspan = 3, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10, PaddingLeft = 20 });

            table.AddCell(new PdfPCell(new Phrase(info.ExcessCash.ToString("N0", nfi) + " đ", new Font(basef, 8, Font.NORMAL)))
            { HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10, PaddingLeft = 20 });
            //-----------------------------------------------------------------------------------------------------           
            return table;
        }
        private Paragraph createEndBill()
        {
            Paragraph para = new Paragraph("Cảm ơn quý khách đã ghé, hẹn gặp lại quý khách!", new Font(basef, 10));
            para.Alignment = Element.ALIGN_CENTER;

            return para;
        }
        private PdfPTable createInfoPurchase(Bill info)
        {
            Font font = new Font(basef, 12);
            PdfPTable table = new PdfPTable(2);
            float[] columns = new float[2];

            columns[0] = 45;
            columns[1] = 55;
            table.SetWidths(columns);
            table.AddCell(new PdfPCell(new Phrase("Khách hàng: " + info.Customer.name, font)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, Border = PdfPCell.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Ngày mua: " + info.TimeCreated.ToString(" hh:mm:ss, dd/MM/yyyy"), font)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.NO_BORDER });
            table.SpacingAfter = 20;

            table.AddCell(new PdfPCell(new Phrase("Số điện thoại: " + info.Customer.phone, font)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, Border = PdfPCell.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Mã hóa đơn: " + info.Id.ToString(), font)) { Border = PdfPCell.NO_BORDER});

            return table;
        }
    }
}