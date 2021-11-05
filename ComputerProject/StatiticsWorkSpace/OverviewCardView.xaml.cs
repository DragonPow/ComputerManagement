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

namespace ComputerProject.StatiticsWorkSpace
{
    /// <summary>
    /// Interaction logic for OverviewCardView.xaml
    /// </summary>
    public partial class OverviewCardView : UserControl
    {
        public OverviewCardView()
        {
            InitializeComponent();
        }

        private static readonly DependencyProperty Titleproperties = DependencyProperty.Register(nameof(Title), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty Valueproperties = DependencyProperty.Register(nameof(Value), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty Percentproperties = DependencyProperty.Register(nameof(Percent), typeof(string), typeof(OverviewCardView), new FrameworkPropertyMetadata("0", new PropertyChangedCallback(OnPercentChanged), new CoerceValueCallback(CoercePercent)));
        private static readonly DependencyProperty Iconproperties = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(OverviewCardView), new FrameworkPropertyMetadata("ArrowUp", new PropertyChangedCallback(OnIconChanged), new CoerceValueCallback(CoerceIcon)));
        private static readonly DependencyProperty ColorForeproperties = DependencyProperty.Register(nameof(ColorFore), typeof(Brush), typeof(OverviewCardView), new FrameworkPropertyMetadata(new BrushConverter().ConvertFromString("#42BDA1"), new PropertyChangedCallback(OnColorForeChanged), new CoerceValueCallback(CoerceColorFore)));

        public string Title
        {
            set { SetValue(Titleproperties, value); }
            get { return (string)GetValue(Titleproperties); }
        }
        public string Value
        {
            set { SetValue(Valueproperties, value); }
            get { return (string)GetValue(Valueproperties); }
        }
        public string Percent
        {
            set { SetValue(Percentproperties, value); }
            get { return (string)GetValue(Percentproperties); }
        }
        public string Icon
        {
            set { SetValue(Iconproperties, value); }
            get { return (string)GetValue(Iconproperties); }
        }
        public Brush ColorFore
        {
            set { SetValue(ColorForeproperties, value); }
            get { return (Brush)GetValue(ColorForeproperties); }
        }

        private static void OnPercentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OverviewCardView)d).CoerceValue(Iconproperties);
            ((OverviewCardView)d).CoerceValue(ColorForeproperties);
        }
        private static object CoercePercent(DependencyObject d,object value)
        {
            return value.ToString();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }
        private static object CoerceIcon(DependencyObject d, object value)
        {
            double min = double.Parse(((OverviewCardView)d).Percent.Substring(0, ((OverviewCardView)d).Percent.Length -2));
            return min > 0 ? "ArrowUp" : "ArrowDown";
        }
        private static void OnColorForeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }
        private static object CoerceColorFore(DependencyObject d, object value)
        {
            double min = double.Parse(((OverviewCardView)d).Percent.Substring(0, ((OverviewCardView)d).Percent.Length - 2));
            return min > 0 ? (SolidColorBrush)new BrushConverter().ConvertFromString( "#42BDA1") : (SolidColorBrush)new BrushConverter().ConvertFromString("#F04461");
        }
    }
}
