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
        protected const double deg90 = Math.PI / 2;
        protected const double deg180 = Math.PI;
        protected const double deg360 = 2 * Math.PI;

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

        protected static Point CalculatePointOnCircle(Point center, double radius, double angle) =>
            new Point()
            {
                // calculated point has 90 deg offset
                X = center.X + radius * Math.Cos(angle + deg90),
                Y = center.Y - radius * Math.Sin(angle + deg90)
            };
    }
}
