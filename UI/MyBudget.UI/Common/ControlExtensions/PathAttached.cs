using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyBudget.UI.Common
{
    public static class PathAttached
    {
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.RegisterAttached(
            "ShadowColor",
            typeof(Color),
            typeof(PathAttached),
            new PropertyMetadata(Colors.Transparent));

        public static Color GetShadowColor(DependencyObject target)
        {
            return (Color)target.GetValue(ShadowColorProperty);
        }

        public static void SetShadowColor(DependencyObject target, Color value)
        {
            target.SetValue(ShadowColorProperty, value);
        }
    }
}
