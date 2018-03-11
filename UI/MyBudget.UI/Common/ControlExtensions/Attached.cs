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
    public static class Attached
    {
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.RegisterAttached(
            "Color",
            typeof(Color),
            typeof(Attached));

        public static Color GeColor(DependencyObject target)
        {
            return (Color)target.GetValue(ColorProperty);
        }

        public static void SetColor(DependencyObject target, Color value)
        {
            target.SetValue(ColorProperty, value);
        }
    }
}
