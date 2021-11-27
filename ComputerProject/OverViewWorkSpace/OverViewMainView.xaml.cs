using ComputerProject.ApplicationWorkspace;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
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
    public partial class OverViewMainView : UserControl, ITabView
    {
        public string ViewName => "Tổng quan";
        public PackIconKind ViewIcon => PackIconKind.ChartArc;
        OverViewViewModel vm = new OverViewViewModel();
        public OverViewMainView()
        {
            InitializeComponent();
            if ( DataContext == null )
                 DataContext = vm;
        }

       
        public bool AllowChangeTab()
        {
            return true;
        }

        public void LoadDataAsync()
        {
            vm.LoadRevenueTodayAsyc();
        }
    }
}
