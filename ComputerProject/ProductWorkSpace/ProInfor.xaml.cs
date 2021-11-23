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

        public static readonly DependencyProperty ModeInforProperties = DependencyProperty.Register(nameof(ModeInfor), typeof(Mode), typeof(ProInfor), new FrameworkPropertyMetadata(Mode.insert, new PropertyChangedCallback(OnModeInfoChange), new CoerceValueCallback(CoreModeInfo)));
        public static readonly DependencyProperty IsreadonlyProperties = DependencyProperty.Register(nameof(Isreadonly), typeof(bool), typeof(ProInfor), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsreadonlyChange), new CoerceValueCallback(CoreIsreadonly)));
        public static readonly DependencyProperty IsEnableProperties = DependencyProperty.Register(nameof(IsEnable), typeof(bool), typeof(ProInfor), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnIsEnableChange), new CoerceValueCallback(CoreIsEnable)));


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

        private static void OnModeInfoChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProInfor)d).CoerceValue(IsEnableProperties);
            ((ProInfor)d).CoerceValue(IsreadonlyProperties);
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
            Mode mode  = ((ProInfor)d).ModeInfor;
            if (mode == Mode.onlyread) return true;
            else return false;
        }
        public bool IsEnable
        {
            get { return (bool)GetValue(IsEnableProperties); }
            set { SetValue(IsEnableProperties, value); }
        }
        private static void OnIsEnableChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static object CoreIsEnable(DependencyObject d, object value)
        {
            Mode mode = ((ProInfor)d).ModeInfor;
            return mode == Mode.edit ? false : (object)true;
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
        insert = 2,
    }
}
