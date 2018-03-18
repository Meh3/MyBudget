using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyBudget.UI.Common
{
    public class UIButton : Button
    {
        public static readonly DependencyProperty Foreground2Property =
            DependencyProperty.Register("Foreground2", 
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush Foreground2
        { 
            get { return (SolidColorBrush)GetValue(Foreground2Property); }
            set { SetValue(Foreground2Property, value); }
        }


        public static readonly DependencyProperty ForegroundMouseOverProperty =
            DependencyProperty.Register("ForegroundMouseOver",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush ForegroundMouseOver
        {
            get { return (SolidColorBrush)GetValue(ForegroundMouseOverProperty); }
            set { SetValue(ForegroundMouseOverProperty, value); }
        }


        public static readonly DependencyProperty Foreground2MouseOverProperty =
            DependencyProperty.Register("Foreground2MouseOver",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush Foreground2MouseOver
        {
            get { return (SolidColorBrush)GetValue(Foreground2MouseOverProperty); }
            set { SetValue(Foreground2MouseOverProperty, value); }
        }


        public static readonly DependencyProperty ForegroundPressedProperty =
            DependencyProperty.Register("ForegroundPressed",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush ForegroundPressed
        {
            get { return (SolidColorBrush)GetValue(ForegroundPressedProperty); }
            set { SetValue(ForegroundPressedProperty, value); }
        }


        public static readonly DependencyProperty Foreground2PressedProperty =
            DependencyProperty.Register("Foreground2Pressed",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush Foreground2Pressed
        {
            get { return (SolidColorBrush)GetValue(Foreground2PressedProperty); }
            set { SetValue(Foreground2PressedProperty, value); }
        }


        public static readonly DependencyProperty BackgroundMouseOverProperty =
            DependencyProperty.Register("BackgroundMouseOver",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush BackgroundMouseOver
        {
            get { return (SolidColorBrush)GetValue(BackgroundMouseOverProperty); }
            set { SetValue(BackgroundMouseOverProperty, value); }
        }


        public static readonly DependencyProperty BackgroundPressedProperty =
            DependencyProperty.Register("BackgroundPressed",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush BackgroundPressed
        {
            get { return (SolidColorBrush)GetValue(BackgroundPressedProperty); }
            set { SetValue(BackgroundPressedProperty, value); }
        }

        public UIButton()
        {
            DefaultStyleKey = typeof(UIButton);
        }
    }
}
