using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ComputerProject.ProductWorkSpace
{
    /// <summary>
    /// Interaction logic for ProInfor.xaml
    /// </summary>
    public partial class ProInfor : UserControl
    {
        public static readonly DependencyProperty VisibilityAddImageProperties = DependencyProperty.Register(nameof(VisibilityAddImage), typeof(Visibility), typeof(ProInfor), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty ModeInforProperties = DependencyProperty.Register(nameof(ModeInfor), typeof(Mode), typeof(ProInfor), new PropertyMetadata(Mode.edit));
        
        public Visibility VisibilityAddImage
        {
            get { return (Visibility)GetValue(VisibilityAddImageProperties); }
            set { SetValue(VisibilityAddImageProperties, value); }
        }

        public Mode ModeInfor
        {
            get { return (Mode)GetValue(ModeInforProperties); }
            set { SetValue(ModeInforProperties, value); }
        }
        public ProInfor()
        {
            InitializeComponent();
        }
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

   
    public enum Mode : byte
    {
        //
        // Summary:
        //    Disable Name of product
        edit = 0,
        //
        // Summary:
        //     All of the element are readonly
        onlyread = 1,
        //
        // Summary:
        //    All of the element are enable
        insert = 2
    }
}
