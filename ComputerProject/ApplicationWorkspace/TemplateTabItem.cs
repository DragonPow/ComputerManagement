using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComputerProject.ApplicationWorkspace
{
    public class TemplateTabItem : ListViewItem
    {
        static TemplateTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TemplateTabItem), new FrameworkPropertyMetadata(typeof(TemplateTabItem)));
        }

        public static readonly DependencyProperty TabNameProperty
            = DependencyProperty.Register(nameof(TabName), typeof(string), typeof(TemplateTabItem), new PropertyMetadata(string.Empty));
        public string TabName
        {
            set { SetValue(TabNameProperty, value); }
            get { return (string)GetValue(TabNameProperty); }
        }

        public static readonly DependencyProperty IconProperty
            = DependencyProperty.Register(nameof(Icon), typeof(PackIconKind), typeof(TemplateTabItem), new PropertyMetadata(PackIconKind.Null));
        public PackIconKind Icon
        {
            set { SetValue(IconProperty, value); }
            get { return (PackIconKind)GetValue(IconProperty); }
        }
    }
}
