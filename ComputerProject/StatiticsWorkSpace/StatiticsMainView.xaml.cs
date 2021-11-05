using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace ComputerProject.StatiticsWorkSpace
{
    /// <summary>
    /// Interaction logic for StatiticsMainView.xaml
    /// </summary>
    public partial class StatiticsMainView : System.Windows.Controls.UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List <string> Barlable { get; set; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString("N");
        public StatiticsMainView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            DataContext = this;
            SeriesCollection = new SeriesCollection
                {
                  new ColumnSeries
                  {
                    Title = "Doanh thu",
                    Values = new ChartValues<double> {14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 14520000, 100000, 1000000, 5120000, 14520000, 100000, 1000000, 51200000, 14520000, 100000, 1000000, 51200000, 14520000},
                    Width = 50,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#0477BF"),


                  }
                };

            Barlable = new List<string>();
            for (int i = 1; i < 32; ++i) Barlable.Add(i.ToString());
            DateTimePicker.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.GetCultureInfo("vi").IetfLanguageTag);

        }
    }
}
 