using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerProject.ApplicationWorkspace
{
    public interface ITabView
    {
        string ViewName { get; }
        PackIconKind ViewIcon { get; }
        void LoadDataAsync();

        bool AllowChangeTab();
    }
}
