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
        private static readonly DependencyProperty Percentproperties = DependencyProperty.Register(nameof(Percent), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty Iconproperties = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty ColorForeproperties = DependencyProperty.Register(nameof(ColorFore), typeof(Brush), typeof(OverviewCardView));

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
    }
}
