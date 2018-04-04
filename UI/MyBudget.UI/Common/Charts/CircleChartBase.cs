using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MyBudget.UI.Common
{
    public class CircleChartBase<TData, TDataForChart> : ChartBase<TData, TDataForChart>
        where TData : class
        where TDataForChart : class
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircleChartBase<TData, TDataForChart>), new PropertyMetadata(50.0));
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        protected static void AddCircleGeometry(Point center, double radius, PathGeometry pathGeometry) =>
            pathGeometry.AddGeometry(new EllipseGeometry
            {
                Center = center,
                RadiusX = radius,
                RadiusY = radius,
            });
    }
}
