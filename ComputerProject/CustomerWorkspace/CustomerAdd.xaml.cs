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

namespace ComputerProject.CustomerWorkspace
{
    /// <summary>
    /// Interaction logic for CustomerAdd.xaml
    /// </summary>
    public partial class CustomerAdd : UserControl
    {
        public CustomerViewModel ViewModel => this.DataContext as CustomerViewModel;

        public CustomerAdd()
        {
            InitializeComponent();
            datetimepicker.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.GetCultureInfo("vi").IetfLanguageTag);

            BtnBack.Click += (s, e) => Closed_NotSave?.Invoke(this, e);
            btnCancel.Click += (s, e) => Closed_NotSave?.Invoke(this, e);

            btnSave.Click += (s, e) =>
            {
                Save();
            };
        }

        public event EventHandler Closed_NotSave;
        public event EventHandler SaveOK;

        public void Save()
        {
            ViewModel.Insert(() => SaveOK?.Invoke(this, null));
        }
    }
}
