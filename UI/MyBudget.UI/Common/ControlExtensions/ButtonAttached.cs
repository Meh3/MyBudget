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
    public static class ButtonAttached
    {
        public static readonly DependencyProperty Foreground2Property =
            DependencyProperty.RegisterAttached(
            "Foreground2",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForeground2(DependencyObject target)
        {
            return (Brush)target.GetValue(Foreground2Property);
        }

        public static void SetForeground2(DependencyObject target, Brush value)
        {
            target.SetValue(Foreground2Property, value);
        }

        public static readonly DependencyProperty ForegroundMouseOverProperty =
            DependencyProperty.RegisterAttached(
            "ForegroundMouseOver",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForegroundMouseOver(DependencyObject target)
        {
            return (Brush)target.GetValue(ForegroundMouseOverProperty);
        }

        public static void SetForegroundMouseOver(DependencyObject target, Brush value)
        {
            target.SetValue(ForegroundMouseOverProperty, value);
        }

        public static readonly DependencyProperty Foreground2MouseOverProperty =
            DependencyProperty.RegisterAttached(
            "Foreground2MouseOver",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForeground2MouseOver(DependencyObject target)
        {
            return (Brush)target.GetValue(Foreground2MouseOverProperty);
        }

        public static void SetForeground2MouseOver(DependencyObject target, Brush value)
        {
            target.SetValue(Foreground2MouseOverProperty, value);
        }

        public static readonly DependencyProperty ForegroundPressedProperty =
            DependencyProperty.RegisterAttached(
            "ForegroundPressed",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForegroundPressed(DependencyObject target)
        {
            return (Brush)target.GetValue(ForegroundPressedProperty);
        }

        public static void SetForegroundPressed(DependencyObject target, Brush value)
        {
            target.SetValue(ForegroundPressedProperty, value);
        }

        public static readonly DependencyProperty Foreground2PressedProperty =
            DependencyProperty.RegisterAttached(
            "Foreground2Pressed",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForeground2Pressed(DependencyObject target)
        {
            return (Brush)target.GetValue(Foreground2PressedProperty);
        }

        public static void SetForeground2Pressed(DependencyObject target, Brush value)
        {
            target.SetValue(Foreground2PressedProperty, value);
        }

        public static readonly DependencyProperty ForegroundSelectedProperty =
            DependencyProperty.RegisterAttached(
            "ForegroundSelected",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForegroundSelected(DependencyObject target)
        {
            return (Brush)target.GetValue(ForegroundSelectedProperty);
        }

        public static void SetForegroundSelected(DependencyObject target, Brush value)
        {
            target.SetValue(ForegroundSelectedProperty, value);
        }

        public static readonly DependencyProperty Foreground2SelectedProperty =
            DependencyProperty.RegisterAttached(
            "Foreground2Selected",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetForeground2Selected(DependencyObject target)
        {
            return (Brush)target.GetValue(Foreground2SelectedProperty);
        }

        public static void SetForeground2Selected(DependencyObject target, Brush value)
        {
            target.SetValue(Foreground2SelectedProperty, value);
        }

        public static readonly DependencyProperty BackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached(
            "BackgroundMouseOver",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetBackgroundMouseOver(DependencyObject target)
        {
            return (Brush)target.GetValue(BackgroundMouseOverProperty);
        }

        public static void SetBackgroundMouseOver(DependencyObject target, Brush value)
        {
            target.SetValue(BackgroundMouseOverProperty, value);
        }

        public static readonly DependencyProperty BackgroundPressedProperty =
            DependencyProperty.RegisterAttached(
            "BackgroundPressed",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetBackgroundPressed(DependencyObject target)
        {
            return (Brush)target.GetValue(BackgroundPressedProperty);
        }

        public static void SetBackgroundPressed(DependencyObject target, Brush value)
        {
            target.SetValue(BackgroundPressedProperty, value);
        }

        public static readonly DependencyProperty BackgroundSelectedProperty =
            DependencyProperty.RegisterAttached(
            "BackgroundSelected",
            typeof(Brush),
            typeof(ButtonAttached));

        public static Brush GetBackgroundSelected(DependencyObject target)
        {
            return (Brush)target.GetValue(BackgroundSelectedProperty);
        }

        public static void SetBackgroundSelected(DependencyObject target, Brush value)
        {
            target.SetValue(BackgroundSelectedProperty, value);
        }
    }
}
