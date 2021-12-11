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

namespace ComputerProject.ProductWorkSpace
{
    /// <summary>
    /// Interaction logic for ProdcutAddSpecificationsRow.xaml
    /// </summary>
    public partial class ProdcutAddSpecificationsRow : UserControl
    {
        public static readonly DependencyProperty ModeInforProperties = DependencyProperty.Register(nameof(ModeInfor), typeof(Mode), typeof(ProdcutAddSpecificationsRow), new FrameworkPropertyMetadata(Mode.insert, new PropertyChangedCallback(OnModeInfoChange), new CoerceValueCallback(CoreModeInfo)));
        public static readonly DependencyProperty IsreadonlyProperties = DependencyProperty.Register(nameof(Isreadonly), typeof(bool), typeof(ProdcutAddSpecificationsRow), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsreadonlyChange), new CoerceValueCallback(CoreIsreadonly)));
        public static readonly DependencyProperty IsEnableProperties = DependencyProperty.Register(nameof(IsEnable), typeof(bool), typeof(ProdcutAddSpecificationsRow), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnIsEnableChange), new CoerceValueCallback(CoreIsEnable)));

     

        public Mode ModeInfor
        {
            get { return (Mode)GetValue(ModeInforProperties); }
            set { SetValue(ModeInforProperties, value); }
        }

        private static void OnModeInfoChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProdcutAddSpecificationsRow)d).CoerceValue(IsEnableProperties);
            ((ProdcutAddSpecificationsRow)d).CoerceValue(IsreadonlyProperties);
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
            Mode mode = ((ProdcutAddSpecificationsRow)d).ModeInfor;
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
            Mode mode = ((ProdcutAddSpecificationsRow)d).ModeInfor;
            return mode == Mode.edit ? false : (object)true;
        }

        public ProdcutAddSpecificationsRow()
        {
            InitializeComponent();
        }
    }
   
}
