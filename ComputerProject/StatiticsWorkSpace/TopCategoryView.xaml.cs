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
    /// Interaction logic for TopCategoryView.xaml
    /// </summary>
    public partial class TopCategoryView : UserControl
    {
        public TopCategoryView()
        {
            InitializeComponent();
        }

        private static readonly DependencyProperty Categoryproperties = DependencyProperty.Register(nameof(Category), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty Iconproperties = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty Valueproperties = DependencyProperty.Register(nameof(Value), typeof(string), typeof(OverviewCardView), new PropertyMetadata(string.Empty));

        public string Category
        {
            set { SetValue(Categoryproperties, value); }
            get { return (string)GetValue(Categoryproperties); }
        }

        public string Value
        {
            set { SetValue(Valueproperties, value); }
            get { return (string)GetValue(Valueproperties); }
        }
        public string Icon
        {
            set { SetValue(Iconproperties, value); }
            get { return (string)GetValue(Iconproperties); }
        }
    }
}
