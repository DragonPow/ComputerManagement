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
            CheckFileOpen(dirFile);

            //Tranfer seri to quantity
            Dictionary<int, int> quantity_product = new Dictionary<int, int>();
            foreach (var item in info.Products)
            {
                if (quantity_product.ContainsKey(item.Id))
                {
                    quantity_product[item.Id]++;
                }
                else
                {
                    quantity_product.Add(item.Id, 1);
                }
            }

            os = new FileStream(dirFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            defaultSize = new Rectangle(PageSize.A4.Width, 350 + quantity_product.Count * 24 +
                                        quantity_product.Sum(i => productInBill.Where(pro => pro.Id == i.Key).First().Name.Length / 30 * 13));// productInBill.Count(i => i.Name.Length > 16) * 15);
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
            doc.Add(this.createTableInfoProduct(info, quantity_product));

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
        private bool CheckFileOpen(string dirFile)
        {
            try
            {
                using (FileStream r = File.OpenWrite(dirFile)) { }
                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                MessageBoxCustom.ShowDialog("Không thể thao tác trên tệp tin", "Lỗi", PackIconKind.ErrorOutline);
                return false;
            }
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
        private PdfPTable createTableInfoProduct(Bill info, Dictionary<int, int> quantity_product)
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
            foreach (var product in quantity_product)
            {
                ProductInBill item = info.Products.Where(i => i.Id == product.Key).First();

                table.AddCell(new PdfPCell(new Phrase(item.Name, font))
                { Border = PdfPCell.NO_BORDER, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });

                table.AddCell(new PdfPCell(new Phrase(product.Value.ToString(), font))
                { Border = PdfPCell.NO_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });

                table.AddCell(new PdfPCell(new Phrase(item.PriceUnit.ToString("N0", nfi) + "đ", font))
                { Border = PdfPCell.NO_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, PaddingBottom = 10 });

                table.AddCell(new PdfPCell(new Phrase((item.PriceUnit * product.Value).ToString("N0", nfi) + "đ", font))
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
            para.SpacingBefore = 10;

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
            table.AddCell(new PdfPCell(new Phrase("Mã hóa đơn: " + info.Id.ToString(), font)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT, Border = PdfPCell.NO_BORDER });

            return table;
        }

        public bool createBillInsur(BILL_REPAIR bill, bool openAfterComplete = false)
        {
            string dirFile = Path.Combine(dest, "BillInsur" + bill.id + ".pdf");

            if (!CheckFileOpen(dirFile)) return false;

            os = new FileStream(dirFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

            defaultSize = new Rectangle(PageSize.A4);
            doc = new Document(defaultSize);
            doc.SetMargins(0, 0, 0, 0);

            writer = PdfWriter.GetInstance(doc, os);

            //start write
            doc.Open();

            //add title paragraph
            doc.Add(this.createTitle2());

            //add name and date of the bill
            doc.Add(createInfoInsur(bill));

            //add end paragraph
            doc.Add(createPrice(bill));

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

        private IElement createPrice(BILL_REPAIR bill)
        {
            Paragraph price = new Paragraph();

            int[] columnWidths = new int[2];
            columnWidths[0] = 600;
            columnWidths[1] = 200;

            Font priceFont = new Font(basef, 14, Font.BOLD, new BaseColor(154,154,176));

            //Total Price
            PdfPTable totalPrice = new PdfPTable(2);
            totalPrice.DefaultCell.Border = PdfPCell.NO_BORDER;
            totalPrice.SetWidths(columnWidths);
            totalPrice.HorizontalAlignment = Element.ALIGN_CENTER;
            totalPrice.SpacingBefore = 10;
            totalPrice.WidthPercentage = 100;

            totalPrice.AddCell(new PdfPCell(new Phrase(new Chunk(
                    "Tổng hóa đơn",
                    new Font(basef, 16, Font.BOLD, BaseColor.WHITE)
                )))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                BackgroundColor = new BaseColor(4, 119, 191),
                PaddingBottom = 10,
                PaddingLeft = 20
            });
            totalPrice.AddCell(new PdfPCell(new Phrase(new Chunk(
                    FormatHelper.ToMoney(bill.price.Value),
                    new Font(basef, 16, Font.NORMAL, BaseColor.WHITE)
                )))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                BackgroundColor = new BaseColor(4, 119, 191),
                PaddingBottom = 10,
                PaddingRight = 40
            });
            price.Add(totalPrice);

            //Customer Price
            PdfPTable customerPrice = new PdfPTable(2);
            customerPrice.DefaultCell.Border = PdfPCell.NO_BORDER;
            customerPrice.SetWidths(columnWidths);
            customerPrice.HorizontalAlignment = Element.ALIGN_CENTER;
            customerPrice.WidthPercentage = 100;

            customerPrice.AddCell(new PdfPCell(new Phrase(new Chunk(
                    "Tiền khách đưa",
                    priceFont
                )))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                PaddingBottom = 5,
                PaddingLeft = 20
            });
            customerPrice.AddCell(new PdfPCell(new Phrase(new Chunk(
                    FormatHelper.ToMoney(bill.customerMoney.Value),
                    priceFont
                )))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                PaddingBottom = 5,
                PaddingRight = 40
            });
            price.Add(customerPrice);


            //Excessh Price
            PdfPTable excesshPrice = new PdfPTable(2);
            excesshPrice.DefaultCell.Border = PdfPCell.NO_BORDER;
            excesshPrice.SetWidths(columnWidths);
            excesshPrice.HorizontalAlignment = Element.ALIGN_CENTER;
            excesshPrice.WidthPercentage = 100;

            excesshPrice.AddCell(new PdfPCell(new Phrase(new Chunk(
                    "Tiền thừa",
                    priceFont
                )))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                PaddingBottom = 5,
                PaddingLeft = 20
            });
            excesshPrice.AddCell(new PdfPCell(new Phrase(new Chunk(
                    FormatHelper.ToMoney(bill.customerMoney.Value - bill.price.Value),
                    priceFont
                )))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                PaddingBottom = 5,
                PaddingRight = 40
            });
            price.Add(excesshPrice);

            return price;
        }
        private IElement createTitle2()
        {
            Paragraph title = new Paragraph();

            //Logo, name, address

            //var uri = new Uri("pack://application:,,,/ComputerProject;component/Resource/Image/logo.png");
            //var pic = Image.GetInstance(uri);
            //title.Add(pic);

            title.Add(new Paragraph(
                store.Name,
                new Font(basef, 28, Font.NORMAL)
            )
            { Alignment = Element.ALIGN_LEFT, SpacingAfter = 5, IndentationLeft = 10 });

            title.Add(new Paragraph(
                store.Address,
                new Font(basef, 14, Font.ITALIC, new BaseColor(154, 154, 176))
            )
            { Alignment = Element.ALIGN_LEFT, SpacingAfter = 5, IndentationLeft = 10 });

            //Straight line with color blue

            //Title name of bill
            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            int[] columnWidths = new int[2];
            columnWidths[0] = 0;
            columnWidths[1] = 200;
            table.SetWidths(columnWidths);
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.SpacingBefore = 10;

            table.AddCell(new PdfPCell(new Phrase(new Chunk(
                    "Hóa đơn sửa chữa, bảo hành",
                    new Font(basef, 28, Font.BOLD, BaseColor.WHITE)
                )))
            {
                Border = PdfPCell.NO_BORDER,
                Colspan = 2,
                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                BackgroundColor = new BaseColor(4, 119, 191),
                PaddingBottom = 10,
            });
            title.Add(table);

            title.SpacingAfter = 20;

            return title;
        }
        private IElement createInfoInsur(BILL_REPAIR bill)
        {
            Paragraph infor = new Paragraph();

            Font titleFont = new Font(basef, 16, Font.BOLD);
            Font inforFont = new Font(basef, 15, Font.NORMAL);
            int lefttilteInden = 20;
            int leftInforInden = 50;

            //Infor bill
            infor.Add(new Paragraph("Thông tin hóa đơn", titleFont)
            { IndentationLeft = lefttilteInden });

            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            int[] columnWidths = new int[2];
            columnWidths[0] = 800;
            columnWidths[1] = 600;
            table.SetWidths(columnWidths);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.WidthPercentage = 100;

            table.AddCell(new PdfPCell(new Paragraph("Khách hàng: " + bill.CUSTOMER.name, inforFont))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                PaddingBottom = 10,
                PaddingLeft = leftInforInden,
            });
            table.AddCell(new PdfPCell(new Paragraph("Mã phiếu: " + bill.id, inforFont))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                PaddingBottom = 10,
                PaddingLeft = leftInforInden,
            });
            table.AddCell(new PdfPCell(new Paragraph("Số điện thoại: " + bill.CUSTOMER.phone, inforFont))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                //PaddingBottom = 10,
                PaddingLeft = leftInforInden,
            });
            table.AddCell(new PdfPCell(new Paragraph("Ngày tạo: " + bill.timeReceive?.ToString("dd/MM/yyyy"), inforFont))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_CENTER,
                //PaddingBottom = 10,
                PaddingLeft = leftInforInden,
            });
            infor.Add(table);

            //Infor product
            infor.Add(new Paragraph("Thông tin sản phẩm", titleFont)
            { IndentationLeft = lefttilteInden });

            infor.Add(new Paragraph("Tên sản phẩm: " + (bill.ITEM_BILL_SERI == null ? "Không có" : bill.ITEM_BILL_SERI.PRODUCT.name), inforFont)
            { IndentationLeft = leftInforInden });
            infor.Add(new Paragraph("Số Seri: " + (bill.ITEM_BILL_SERI == null ? "Không có" : bill.ITEM_BILL_SERI.seri.ToString()), inforFont)
            { IndentationLeft = leftInforInden });
            infor.Add(new Paragraph("Hình thức: " + (bill.isWarranty ? "Bảo hành" : "Sửa chữa"), inforFont)
            { IndentationLeft = leftInforInden });
            infor.Add(new Paragraph("Ngày hết hạn bảo hành: " + (bill.ITEM_BILL_SERI == null ? "Không có" : bill.ITEM_BILL_SERI.warrantyEndTime?.ToString("dd/MM/yyyy")), inforFont)
            { IndentationLeft = leftInforInden });
            infor.Add(new Paragraph("Mã hóa đơn mua sản phẩm: " + (bill.ITEM_BILL_SERI == null ? "Không có" : bill.ITEM_BILL_SERI.id.ToString()), inforFont)
            { IndentationLeft = leftInforInden });

            //Infor status
            infor.Add(new Paragraph("Tình trạng sản phẩm", titleFont)
            { IndentationLeft = lefttilteInden });

            infor.Add(new Paragraph("Mô tả máy: " + bill.desReceiveItems, inforFont)
            { IndentationLeft = leftInforInden });
            infor.Add(new Paragraph("Mô tả lỗi: " + bill.desProblem, inforFont)
            { IndentationLeft = leftInforInden });
            infor.Add(new Paragraph("Sản phẩm đính kèm: " + bill.attachments, inforFont)
            { IndentationLeft = leftInforInden });

            //Infor detail repair
            infor.Add(new Paragraph("Chi tiết sửa chữa, bảo hành", titleFont)
            { IndentationLeft = lefttilteInden });

            infor.Add(new Paragraph(bill.desDetailRepair, inforFont)
            { IndentationLeft = leftInforInden });

            return infor;
        }
    }
}