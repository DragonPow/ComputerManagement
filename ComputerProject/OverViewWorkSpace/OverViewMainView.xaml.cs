using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerProject.OverViewWorkSpace
{
    /// <summary>
    /// Interaction logic for OverViewMainView.xaml
    /// </summary>
    public partial class OverViewMainView : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Barlable { get; set; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString("C");
        public OverViewMainView()
        {
            InitializeComponent();
            InitData();
        }
        private void InitData()
        {
            DataContext = this;
            SeriesCollection = new SeriesCollection
                {
                  new LineSeries
                  {
                    Title = "Doanh số",
                    Values = new ChartValues<double> {14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 100000, 1000000, 51200000, 14520000},
                    Width = 50,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#20EE9EA7"), 
                    PointForeground = Brushes.Transparent,
                    PointGeometry = null


                  },
                  new LineSeries
                  {
                    Title = "Hóa đơn",
                    Values = new ChartValues<double> { 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 100000, 1000000, 51200000, 14520000, 17820000},
                    Width = 50,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#200477BF"),
                    PointForeground = Brushes.Transparent,
                    PointGeometry = null
                  }
                };

            Barlable = new List<string>();
            for (int i = 1; i < 32; ++i) Barlable.Add(i.ToString());

        }
    }
}
