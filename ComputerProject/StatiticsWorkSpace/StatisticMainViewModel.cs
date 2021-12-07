using ComputerProject.Helper;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ComputerProject.StatiticsWorkSpace
{
    class StatisticMainViewModel : BusyViewModel
    {
        int maxTopSale = 5;

        public StatisticMainViewModel()
        {
            maxTopSale = 5;
        }

        System.Threading.CancellationTokenSource operationSelecteMonth = new System.Threading.CancellationTokenSource();
        protected DateTime selectedDate = DateTime.Now.AddMonths(-1);
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                if (value == selectedDate) return;

                operationSelecteMonth.Cancel();
                operationSelecteMonth = new System.Threading.CancellationTokenSource();

                selectedDate = value;

                /*CountNewBill = 0;
                CountNewCustomer = 0;
                CountNewRepair = 0;
                Revenue = 0;*/

                DoBusyTask(() => GetReport(selectedDate.Year, selectedDate.Month), operationSelecteMonth.Token, () =>
                {
                    OnPropertyChanged(nameof(CountNewBill));
                    OnPropertyChanged(nameof(RateBill));

                    OnPropertyChanged(nameof(CountNewCustomer));
                    OnPropertyChanged(nameof(countNewCustomer_Rate));

                    OnPropertyChanged(nameof(CountNewRepair));
                    OnPropertyChanged(nameof(RateRepair));

                    OnPropertyChanged(nameof(Revenue_String));
                    OnPropertyChanged(nameof(RateRevenue));

                    OnPropertyChanged(nameof(CollectionTopSale));
                    OnPropertyChanged(nameof(CategoteryPieSeries));
                    OnPropertyChanged(nameof(RevenueCollection));

                    DoInBackGround(LoadProductImage, operationSelecteMonth.Token);
                });
            }
        }
        public int YearSelected => SelectedDate.Year;
        public int MonthSelected => SelectedDate.Month;

        protected int lastBillCount;
        protected int lastRepairCount;
        protected int lastCustomerCount;
        protected long lastRevenue;

        protected int countNewCustomer;
        public int CountNewCustomer
        {
            get => countNewCustomer;
            set
            {
                countNewCustomer = value;
            }
        }
        public String CountNewCustomer_String => CountNewCustomer.ToString();
        protected String countNewCustomer_Rate => Rate(CountNewCustomer, lastCustomerCount);

        protected int countNewBill;
        public int CountNewBill
        {
            get => countNewBill;
            set
            {
                countNewBill = value;
            }
        }
        public String CountNewBill_String => CountNewBill.ToString();
        public String RateBill => Rate(CountNewBill, lastBillCount);

        protected int countNewRepair;
        public int CountNewRepair
        {
            get => countNewRepair;
            set
            {
                countNewRepair = value;
            }
        }
        public String CountNewRepair_String => CountNewRepair.ToString();
        public String RateRepair => Rate(CountNewRepair, lastRepairCount);

        protected long revenue;
        public long Revenue
        {
            get => revenue;
            set
            {
                revenue = value;
            }
        }
        public String Revenue_String => FormatHelper.ToMoney(Revenue, true);
        public String RateRevenue => Rate(Revenue, lastRevenue);

        protected List<ReportModel> revenuePerCate = new List<ReportModel>();
        public SeriesCollection CategoteryPieSeries
        {
            get
            {
                var series = new SeriesCollection();

                for (int i = 0; i < maxTopSale && i < revenuePerCate.Count; i++)
                {
                    var c = revenuePerCate[i];
                    series.Add(new PieSeries()
                    {
                        Title = c.CategoryName,
                        Values = new ChartValues<long>() { c.Money },
                    });
                }

                if (series.Count == maxTopSale)
                {
                    long money = 0;
                    for (int i = maxTopSale; i < revenuePerCate.Count; i++)
                    {
                        var c = revenuePerCate[i];
                        money += c.Money;
                    }
                    series.Add(new PieSeries()
                    {
                        Title = "Khác",
                        Values = new ChartValues<long>() { money },
                    });
                }

                return series;
            }
        }

        protected List<ReportModel> revenuePerProd = new List<ReportModel>();
        public List<ReportModel> CollectionTopSale
        {
            get
            {
                var rs = revenuePerProd.Take(5).ToList();

                return rs;
            }
        }

        protected List<ReportModel> revenuePerDay = new List<ReportModel>();
        public ChartValues<long> RevenueCollection
        {
            get
            {
                var values = new ChartValues<long>();
                for (int i = 1; i <= DateTime.DaysInMonth(YearSelected, MonthSelected); i++)
                {
                    var revenueInDay = revenuePerDay.Where(r => r.StartTime.Day == i).FirstOrDefault();
                    if (revenueInDay != null)
                    {
                        values.Add(revenueInDay.Money);
                    }
                    else
                    {
                        values.Add(0);
                    }
                }

                return values;
            }
        }

        void LoadProductImage()
        {
            for (int i = 0; i < revenuePerProd.Count && i < maxTopSale; i++)
            {
                Console.WriteLine("Load image for " + revenuePerProd[i].ProductName);
                var pd = revenuePerProd.Where(prod => prod.ProductID == revenuePerProd[i].ProductID).FirstOrDefault();
                if (pd != null)
                {
                    pd.Image = ProductWorkSpace.ProductViewModel.GetImage(revenuePerProd[i].ProductID);
                }
            }
        }

        protected void GetReport(int _year, int _month)
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL Get: " + s);

                FormatHelper.SetTimeOut(db, 120);

                var lastMonth = (new DateTime(_year, _month, 1)).AddMonths(-1);

                var rp = db.REPORTs.Where(r => r.year == YearSelected && r.month == MonthSelected).Select(r => new
                {
                    r.id,
                    r.newBill,
                    r.newCustomer,
                    r.newRepair,
                    revenue = r.totalMoney,

                }).FirstOrDefault();

                if (rp == null)
                {
                    CalcReport(_year, _month, db);
                    if (SelectedDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                    {
                        //SaveResult();
                    }
                }
                else
                {
                    CountNewBill = rp.newBill.Value;
                    CountNewCustomer = rp.newCustomer.Value;
                    CountNewRepair = rp.newRepair.Value;
                    Revenue = rp.revenue;

                    revenuePerCate = db.DETAIL_REPORT_CATEGORY.Where(dt => dt.reportId == rp.id).Select(dt => new ReportModel()
                    {
                        CategoryID = dt.categoryId,
                        CategoryName = dt.CATEGORY.name,
                        Money = dt.money,
                    }).ToList();
                    //revenuePerCate = rp.rpCate.ToList();

                    revenuePerProd = db.DETAIL_REPORT_PRODUCT.Where(dt => dt.reportId == rp.id).Select(dt => new ReportModel()
                    {
                        ProductID = dt.productId,
                        ProductName = dt.PRODUCT.name,
                        Amount = dt.amount,
                        Money = dt.money,
                    }).ToList();
                    //revenuePerProd = rp.rpProd.ToList();
                    revenuePerProd.Sort((a1, a2) => a2.Money.CompareTo(a1.Money));

                    revenuePerDay = db.DETAIL_REPORT_REVENUE.Where(dt => dt.reportId == rp.id).Select(dt => new ReportModel()
                    {
                        Day = dt.day,
                        Amount = dt.amount,
                        Money = dt.money,
                    }).ToList();
                    //revenuePerDay = rp.rpDay.ToList();

                    foreach (var d in revenuePerDay)
                    {
                        d.StartTime = new DateTime(_year, _month, d.Day);
                    }
                }


                var lastRp = db.REPORTs.Where(r => r.year == lastMonth.Year && r.month == lastMonth.Month).FirstOrDefault();
                if (lastRp != null)
                {
                    lastBillCount = lastRp.newBill.Value;
                    lastRepairCount = lastRp.newRepair.Value;
                    lastRevenue = lastRp.totalMoney;
                    lastCustomerCount = lastRp.newCustomer.Value;
                }
                else
                {
                    lastCustomerCount = db.CUSTOMERs.Where(c => c.createTime.Value.Year == lastMonth.Year && c.createTime.Value.Month == lastMonth.Month).Count();
                    lastBillCount = db.BILLs.Where(b => b.createTime.Year == lastMonth.Year && b.createTime.Month == lastMonth.Month).Count();
                    lastRepairCount = db.BILL_REPAIR.Where(b => b.timeReceive.Year == lastMonth.Year && b.timeReceive.Month == lastMonth.Month).Count();
                    lastRevenue = db.BILLs.Where(b => b.createTime.Year == lastMonth.Year && b.createTime.Month == lastMonth.Month).GroupBy(d => d.createTime.Month).Select(g => g.Sum(d => d.totalMoney)).FirstOrDefault();
                }
            }
        }

        protected void CalcReport(int _year, int _month, ComputerManagementEntities db)
        {
            //db.Database.Log = s => System.Diagnostics.Debug.WriteLine("MSSQL Calc: " + s);
            var data1 = db.ITEM_BILL.Where(b => b.BILL.createTime.Month == _month && b.BILL.createTime.Year == _year).GroupBy(b => new
            {
                productID = b.PRODUCT.id,
                categoryID = b.PRODUCT.categoryId,
                productName = b.PRODUCT.name,
                categoryName = b.PRODUCT.CATEGORY.name,
                b.unitPrice,
                b.quantity,
                b.BILL.createTime.Day
            }).Select(g => new
            {
                ProductID = g.Key.productID,
                CategoryID = g.Key.categoryID,
                ProductName = g.Key.productName,
                CategoryName = g.Key.categoryName,
                Money = g.Sum(it => it.unitPrice * it.quantity),
                Amount = g.Sum(it => it.quantity),
                g.Key.Day
            }).ToList();

            var productSaleByDay = new List<ReportModel>();
            foreach (var day in data1)
            {
                productSaleByDay.Add(new ReportModel()
                {
                    ProductID = day.ProductID,
                    ProductName = day.ProductName,
                    CategoryID = day.CategoryID,
                    CategoryName = day.CategoryName,
                    Money = day.Money,
                    Amount = day.Amount,
                    StartTime = new DateTime(_year, _month, day.Day)
                });
            }

            var rootCateList = db.CATEGORies.Where(c => c.parentCategoryId == null).Select(c => new
            {
                c.id,
                c.name,
                child = c.CATEGORY11.Select(r => r.id)
            }).ToList();

            // Change category in product to root category
            foreach (var p in productSaleByDay)
            {
                var root = rootCateList.Where(r => r.child.Contains(p.CategoryID)).FirstOrDefault();
                if (root != null)
                {
                    p.CategoryID = root.id;
                    p.CategoryName = root.name;
                }
            }

            revenuePerCate = productSaleByDay.GroupBy(p => new { p.CategoryID, p.CategoryName }).Select(
            g => new ReportModel()
            {
                CategoryID = g.Key.CategoryID,
                CategoryName = g.Key.CategoryName,
                Money = g.Sum(p => p.Money),
                Amount = g.Sum(p => p.Amount),
                StartTime = new DateTime(_year, _month, 1),
            }
            ).ToList();

            revenuePerProd = productSaleByDay.GroupBy(p => new { p.ProductID, p.ProductName }).Select(g => new ReportModel()
            {
                ProductID = g.Key.ProductID,
                ProductName = g.Key.ProductName,
                Money = g.Sum(p => p.Money),
                Amount = g.Sum(p => p.Amount),
                StartTime = new DateTime(_year, _month, 1),
            }).ToList();

            revenuePerDay = productSaleByDay.GroupBy(p => p.StartTime.Day).Select(
            g => new ReportModel()
            {
                StartTime = new DateTime(_year, _month, g.Key),
                Money = g.Sum(p => p.Money),
                Amount = g.Sum(p => p.Amount),
            }).ToList();

            CountNewBill = db.BILLs.Where(b => b.createTime.Year == _year && b.createTime.Month == _month).Count();

            CountNewRepair = db.BILL_REPAIR.Where(b => b.timeReceive.Year == _year && b.timeReceive.Month == _month).Count();

            Revenue = productSaleByDay.GroupBy(d => d.StartTime.Month).Select(g => g.Sum(d => d.Money)).FirstOrDefault();

            CountNewCustomer = db.CUSTOMERs.Where(c => c.createTime.Value.Year == _year && c.createTime.Value.Month == _month).Count();
        }

        void SaveResult()
        {
            using (ComputerManagementEntities db = new ComputerManagementEntities())
            {
                var rp = db.REPORTs.Add(new REPORT()
                {
                    year = (short)YearSelected,
                    month = (byte)MonthSelected,
                    totalMoney = Revenue,
                    newBill = CountNewBill,
                    newCustomer = CountNewCustomer,
                    newRepair = CountNewRepair,
                });

                //var rpCate = new List<DETAIL_REPORT_CATEGORY>();
                foreach (var c in revenuePerCate)
                {
                    rp.DETAIL_REPORT_CATEGORY.Add(new DETAIL_REPORT_CATEGORY()
                    {
                        categoryId = c.CategoryID,
                        money = c.Money,
                        day = 0
                    });
                }

                //var rpProd = new List<DETAIL_REPORT_PRODUCT>();
                foreach (var c in revenuePerProd)
                {
                    rp.DETAIL_REPORT_PRODUCT.Add(new DETAIL_REPORT_PRODUCT()
                    {
                        productId = c.ProductID,
                        money = c.Money,
                        amount = c.Amount,
                        day = 0
                    });
                }

                //var rpRevenue = new List<DETAIL_REPORT_REVENUE>();
                foreach (var c in revenuePerDay)
                {
                    rp.DETAIL_REPORT_REVENUE.Add(new DETAIL_REPORT_REVENUE()
                    {
                        money = c.Money,
                        amount = c.Amount,
                        day = (byte)c.StartTime.Day
                    });
                }

                db.SaveChanges();
            }
        }

        Func<double, string> formaterLabelMoney = new Func<double, string>((d) => FormatHelper.getMoneyLabel((int)d));
        public Func<double, string> FormaterLabelMoney => formaterLabelMoney;

        String Rate(long nw, long old)
        {
            if (old == 0) return "999%";
            int rate = (int)((nw - old) * 100 / old);
            if (rate > 0) return "+$1%".Replace("$1", rate.ToString());
            return "$1%".Replace("$1", rate.ToString());
        }

        public String[] LabelDays
        {
            get
            {
                var rs = new List<string>();
                int maxDay = DateTime.DaysInMonth(YearSelected, MonthSelected);
                for (int i = 0; i < maxDay; i++)
                {
                    rs.Add("$1".Replace("$1", (i + 1).ToString()));
                }

                return rs.ToArray();
            }
        }
    }
}
