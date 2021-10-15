
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


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
    ///     <MyNamespace:ButtonIcon/>
    ///
    /// </summary>
    public class ButtonIcon : Button
    {
        static ButtonIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonIcon), new FrameworkPropertyMetadata(typeof(ButtonIcon)));
        }

        public static readonly DependencyProperty PathDataProperty
            = DependencyProperty.Register(nameof(PathData), typeof(Geometry), typeof(ButtonIcon), new PropertyMetadata(Geometry.Empty));
        public Geometry PathData
        {
            set { SetValue(PathDataProperty, value); }
            get { return (Geometry)GetValue(PathDataProperty); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                            typeof(ButtonIcon), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation),
            typeof(Orientation), typeof(ButtonIcon), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty TextVisibilityProperty =
            DependencyProperty.Register(nameof(TextVisibility),
             typeof(Visibility), typeof(ButtonIcon), new PropertyMetadata(default(Visibility)));

        public Visibility TextVisibility
        {
            get { return (Visibility)GetValue(TextVisibilityProperty); }
            set { SetValue(TextVisibilityProperty, value); }
        }

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(nameof(IconVisibility),
            typeof(Visibility), typeof(ButtonIcon), new PropertyMetadata(default(Visibility)));

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }


        /* Path of icon can get in https://materialdesignicons.com/
         * example: PlusIcon
              <local:ButtonIcon
             Height = "40"
             Width="105"
             Text="Thêm mới" 
             PathData="M20 14H14V20H10V14H4V10H10V4H14V10H20V14Z" 
             Background="{StaticResource MainColor}" Click="ButtonIcon_Click"  
             />*/

    }
}
