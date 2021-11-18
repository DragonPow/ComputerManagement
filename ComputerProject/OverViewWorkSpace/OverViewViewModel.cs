using ComputerProject.ApplicationWorkspace;
using ComputerProject.HelperService;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerProject.OverViewWorkSpace
{
    class OverViewViewModel : BaseViewModel, ITabView
    {
        #region Field
        public string ViewName => "Tổng quan";
        public PackIconKind ViewIcon => PackIconKind.ChartArc;
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Barlable { get; set; }

        
        public Func<double, string> Formatter { get; set; } = value => FormatLable(value);
        DateTime to = DateTime.Now;
        DateTime from = DateTime.Now.AddDays(-29);
        public ChartValues<double> Values { get; set; }

        #endregion

        #region Properties
        public int TotalBill{ get; set; }
        public int TotalWarrantyBill { get; set; }
        public int TotalNomalBill { get; set; }
        public int TotalPro { get; set; }
        public int TotalProProvie { get; set; }

        public int TotalProStop { get; set; }

        string ITabView.ViewName => ViewName;

        PackIconKind ITabView.ViewIcon => ViewIcon;
        #endregion

        #region Constructor
        public OverViewViewModel()
        {
            DateTime date = DateTime.Now.AddDays(-6);
            Values = new ChartValues<double>();

            Barlable = new List<string>();

            
            using (ComputerManagementEntities context = new ComputerManagementEntities())
            {
                TotalNomalBill = context.BILLs.Where(b=> b.createTime <=to && b.createTime >= from).Count();
                TotalWarrantyBill = context.BILL_REPAIR.Count();
                TotalBill = TotalNomalBill + TotalWarrantyBill;

                TotalProProvie = context.PRODUCTs.Where(p => p.isStopSelling == false).Count();
                TotalProStop = context.PRODUCTs.Where(p => p.isStopSelling).Count();
                TotalPro = TotalProProvie + TotalProStop;
                double dayrevenue;
                for (int i = 0; i < 7; ++i)
                {
                    Barlable.Add(date.Day.ToString() + "/" + date.Month.ToString());
                    dayrevenue = context.BILLs.Where(b => b.createTime.Day == date.Day && b.createTime.Month == date.Month && b.createTime.Year == date.Year).Select(b => b.totalMoney).DefaultIfEmpty(0).Sum();
                    if (dayrevenue != 0) Values.Add(dayrevenue); else Values.Add(0);
                    date = date.AddDays(1);
                }
                #region query ver02
                //IQueryable<Revenue> query = context.BILLs.GroupBy(b => b.createTime).Where(b => b.First().createTime >= date && b.First().createTime <= date.AddDays(6)).Select(r => new Revenue
                //{
                //    Day = r.First().createTime,
                //    Total = r.Sum(c => c.totalMoney)
                //});

                //List<double> v = query.Select(r => r.Total).ToList();
                //Values = new ChartValues<double>(v);
                #endregion
            }
           
        }
        #endregion

        static string FormatLable(double value)
        {
            if (value >= 100000) return (value / 1000000).ToString() + "tr";
            if (value >= 100000000) return (value / 1000000000).ToString() + "tỉ";
            return null;
        }

    }

    
}

