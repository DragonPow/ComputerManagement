﻿using System;
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

namespace ComputerProject.CategoryWorkspace
{
    /// <summary>
    /// Interaction logic for TitleDetailCategory.xaml
    /// </summary>
    public partial class TitleDetailCategory : UserControl
    {
        public TitleDetailCategory()
        {
            InitializeComponent();
        }

        public static DependencyProperty TextProperties = DependencyProperty.Register(nameof(Text), typeof(TextBlock), typeof(TitleDetailCategory), new PropertyMetadata("Chi tiết danh mục"));
        public string Text
        {
            get { return (string)GetValue(TextProperties); }
            set { SetValue(TextProperties, value); }
        }
    }
}
