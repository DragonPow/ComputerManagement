using ComputerProject.HelperService;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerProject.OverViewWorkSpace
{
    class OverViewViewModel : BaseViewModel
    {
        #region Field
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Barlable { get; set; }
        public Func<double, string> Formatter { get; set; } = value => FormatLable(value);
        public ChartValues<double> Values { get; set; }

        DateTime to = DateTime.Now;
        DateTime from = DateTime.Now.AddDays(-29);

        int _totalbill;
        int _totalwarranrybill;
        int _totalnormalbill;

        int _totalpro;
        int _totalprostop;
        int _totalproprovide;

        private Action getbillinfo;
        private Action getproinfo;
        #endregion

        #region Properties
        public int TotalBill {
            get => _totalbill;
            set
            {
                if (value != _totalbill)
                {
                    _totalbill = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TotalWarrantyBill
        {
            get => _totalwarranrybill;
            set
            {
                if (value != _totalwarranrybill)
                {
                    _totalwarranrybill = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TotalNomalBill
        {
            get => _totalnormalbill;
            set
            {
                if (value != _totalnormalbill)
                {
                    _totalnormalbill = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TotalPro
        {
            get => _totalpro;
            set
            {
                if (value != _totalpro)
                {
                    _totalpro = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TotalProProvie
        {
            get => _totalproprovide;
            set
            {
                if (value != _totalproprovide)
                {
                    _totalproprovide = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalProStop
        {
            get => _totalprostop;
            set
            {
                if (value != _totalprostop)
                {
                    _totalprostop = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Constructor

        public OverViewViewModel()
        {

            DateTime date = DateTime.Now.AddDays(-6);
            Values = new ChartValues<double>();

            Barlable = new List<string>();

            using (ComputerManagementEntities context = new ComputerManagementEntities())
            {

                TotalNomalBill = context.BILLs.Where(b => b.createTime <= to && b.createTime >= from).Count();
                TotalWarrantyBill = context.BILL_REPAIR.Where(b => b.timeReceive <= to && b.timeReceive >= from).Count();
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

        public async void LoadRevenueTodayAsyc()
        {
            using (ComputerManagementEntities context = new ComputerManagementEntities())
            {

                await Task.Run(getproinfo = () =>
                {
                    TotalProProvie = context.PRODUCTs.Where(p => p.isStopSelling == false).Count();
                    TotalProStop = context.PRODUCTs.Where(p => p.isStopSelling).Count();
                    TotalPro = TotalProProvie + TotalProStop;

                });
                await Task.Run( getbillinfo = () =>
                {
                    TotalNomalBill = context.BILLs.Where(b => b.createTime <= to && b.createTime >= from).Count();
                    TotalWarrantyBill = context.BILL_REPAIR.Where(b => b.timeReceive <= to && b.timeReceive >= from).Count();
                    TotalBill = TotalNomalBill + TotalWarrantyBill;
                });

                
                Values[Values.Count - 1] = context.BILLs.Where(b => b.createTime.Day == to.Day && b.createTime.Month == to.Month && b.createTime.Year == to.Year).Select(b => b.totalMoney).DefaultIfEmpty(0).Sum();

            }
        }

        
        #endregion

        static string FormatLable(double value)
        {
            if (value >= 100000)
            {
                return (value / 1000000).ToString() + "triệu";
            }

            if (value >= 100000000) return (value / 1000000000).ToString() + "tỉ";
            return null;
        }

    }

    
}

