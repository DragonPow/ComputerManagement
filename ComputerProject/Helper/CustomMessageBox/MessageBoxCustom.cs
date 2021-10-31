using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ComputerProject.CustomMessageBox
{
    public class MessageBoxCustom
    {
        private static MessageBoxResultCustom _result;
        public static MessageBoxResultCustom ShowDialog(string messageBoxText, string caption, string ConfirmText = "Xác nhận", string CancelText = "Hủy", PackIconKind icon = PackIconKind.Information)
        {
            MessageBox m = new MessageBox(messageBoxText, caption, ConfirmText, CancelText, icon);
            m.ChangedResultEventHandler += (s, e) => { _result = e; };
            m.ShowDialog();
            return _result;
        }
        public static MessageBoxResultCustom ShowDialog(string messageBoxText, string caption, PackIconKind icon)
        {
            return ShowDialog(messageBoxText, caption, null, null, icon);
        }
    }

    public enum MessageBoxResultCustom
    {
        Yes = MessageBoxResult.Yes,
        No = MessageBoxResult.No,
    }
}
