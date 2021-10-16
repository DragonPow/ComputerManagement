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

namespace ComputerProject.CustomControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ComputerProject.CustomControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ComputerProject.CustomControl;assembly=ComputerProject.CustomControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Textbox/>
    ///
    /// </summary>
    public class TextboxCustom : TextBox
    {
        static TextboxCustom()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextboxCustom), new FrameworkPropertyMetadata(typeof(TextboxCustom)));
        }

        public static readonly DependencyProperty TexthintProperty
            = DependencyProperty.Register(nameof(Texthint), typeof(string), typeof(TextboxCustom), new PropertyMetadata(string.Empty));
        public string Texthint
        {
            set { SetValue(TexthintProperty, value); }
            get { return (string)GetValue(TexthintProperty); }
        }

        public static readonly DependencyProperty TitleProperty
           = DependencyProperty.Register(nameof(Title), typeof(string), typeof(TextboxCustom), new PropertyMetadata(string.Empty));
        public string Title
        {
            set { SetValue(TitleProperty, value); }
            get { return (string)GetValue(TitleProperty); }
        }

        public static readonly DependencyProperty FontsizeTitleProperty
            = DependencyProperty.Register(nameof(FontsizeTitle), typeof(int), typeof(TextboxCustom), new PropertyMetadata(14));
        public int FontsizeTitle
        {
            set { SetValue(FontsizeTitleProperty, value); }
            get { return (int)GetValue(FontsizeTitleProperty); }
        }

        public static readonly DependencyProperty FontsizeProperty
            = DependencyProperty.Register(nameof(Fontsize), typeof(int), typeof(TextboxCustom), new PropertyMetadata(14));
        public int Fontsize
        {
            set { SetValue(FontsizeProperty, value); }
            get { return (int)GetValue(FontsizeProperty); }
        }

        public static readonly DependencyProperty VisibleTitleProperty
            = DependencyProperty.Register(nameof(VisibleTitle), typeof(bool), typeof(TextboxCustom), new PropertyMetadata(true));
        public bool VisibleTitle
        {
            set { SetValue(VisibleTitleProperty, value); }
            get { return (bool)GetValue(VisibleTitleProperty); }
        }

    }

    /*Example
     *      <local:TextboxCustom
                Texthint = "Lap top"
                Title="Tên danh mục"
                FontsizeTitle="19"
                Fontsize = "17"
             >
            </local:TextboxCustom>*/
}
