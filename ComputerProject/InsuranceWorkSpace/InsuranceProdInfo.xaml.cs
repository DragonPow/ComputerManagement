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

namespace ComputerProject.InsuranceWorkSpace
{
    /// <summary>
    /// Interaction logic for InsuranceProdInfo.xaml
    /// </summary>
    public partial class InsuranceProdInfo : UserControl
    {
        public static readonly DependencyProperty ModeInforProperties = DependencyProperty.Register(nameof(ModeInfor), typeof(Mode), typeof(InsuranceProdInfo), new FrameworkPropertyMetadata(Mode.insert, new PropertyChangedCallback(OnModeInfoChange), new CoerceValueCallback(CoreModeInfo)));
        public static readonly DependencyProperty IsreadonlyProperties = DependencyProperty.Register(nameof(Isreadonly), typeof(bool), typeof(InsuranceProdInfo), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsreadonlyChange), new CoerceValueCallback(CoreIsreadonly)));


        public Mode ModeInfor
        {
            get { return (Mode)GetValue(ModeInforProperties); }
            set { SetValue(ModeInforProperties, value); }
        }

        private static void OnModeInfoChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InsuranceProdInfo)d).CoerceValue(IsreadonlyProperties);
        }
        private static object CoreModeInfo(DependencyObject d, object value)
        {
            return (Mode)value;
        }

        public bool Isreadonly
        {
            get { return (bool)GetValue(IsreadonlyProperties); }
            set { SetValue(IsreadonlyProperties, value); }
        }
        private static void OnIsreadonlyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static object CoreIsreadonly(DependencyObject d, object value)
        {
            Mode mode = ((InsuranceProdInfo)d).ModeInfor;
            if (mode == Mode.onlyread) return true;
            else return false;
        }
        public InsuranceProdInfo()
        {
            InitializeComponent();
        }
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
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
        insert = 2,
    }

