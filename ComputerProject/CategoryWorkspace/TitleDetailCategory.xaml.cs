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
        public event EventHandler OnBack;
        public TitleDetailCategory()
        {
            InitializeComponent();
        }

       public void ChangeTitle(string title)
        {
            Title.Text = title;
        }
     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if( OnBack != null)
            {
                OnBack(this, new EventArgs());
            }
        }
    }
}
