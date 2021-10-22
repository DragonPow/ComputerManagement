﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ComputerProject.CustomMessageBox
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox 
    {
        private string messageBoxText;
        private string caption;
        private MessageBoxImage icon;

        public MessageBox()
        {
            InitializeComponent();
        }

        public MessageBox(string messageBoxText, string caption, string okText, string cancelText, PackIconKind icon) : this()
        {
            txtMessage.Text = messageBoxText;
            txtTitle.Text = caption;
            msgLogo.Kind = icon;
            btnOk.Content = okText;
            btnCancel.Content = cancelText;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OnResultChanged(MessageBoxResultCustom.No);
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            OnResultChanged(MessageBoxResultCustom.Yes);
            this.Close();
        }

        private void OnResultChanged(MessageBoxResultCustom result)
        {
            ChangedResultEventHandler?.Invoke(this, result);
        }
        public event EventHandler<MessageBoxResultCustom> ChangedResultEventHandler;
    }
}
