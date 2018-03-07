using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyBudget.UI.ControlExtensions
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

        public static readonly DependencyProperty Foreground3Property =
            DependencyProperty.Register("Foreground3",
            typeof(SolidColorBrush),
            typeof(UIButton));

        public SolidColorBrush Foreground3
        {
            get { return (SolidColorBrush)GetValue(Foreground3Property); }
            set { SetValue(Foreground3Property, value); }
        }
    }
}
