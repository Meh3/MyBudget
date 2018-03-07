using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyBudget.UI.ControlExtensions
{
    public static class PathExtension
    {
        public static readonly DependencyProperty HelperColorProperty =
            DependencyProperty.RegisterAttached(
            "HelperColor",
            typeof(Color),
            typeof(PathExtension),
            new PropertyMetadata(Colors.Transparent));

        public static Color GetHelperColor(DependencyObject target)
        {
            return (Color)target.GetValue(HelperColorProperty);
        }

        public static void SetHelperColor(DependencyObject target, Color value)
        {
            target.SetValue(HelperColorProperty, value);
        }
    }
}
