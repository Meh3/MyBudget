using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;

namespace MyBudget.UI.Common
{
    public class GaugeData
    {
        public double MaxValue { get; private set; }
        public double Value { get; private set; }

        public GaugeData(double value, double maxValue)
        {
            Value = value;
            MaxValue = maxValue;
        }
    }

    public class GaugeDataVisual
    {
        public PathGeometry GaugeProgress { get; private set; }
        public string StringValue { get; private set; }
        public string StringMaxValue { get; private set; }

        public GaugeDataVisual(PathGeometry gaugeProgress, double value, double maxValue, string format = null, NumberFormatInfo nfi = null)
        {
            GaugeProgress = gaugeProgress;
            StringValue = value.ToString(format, nfi);
            StringMaxValue = maxValue.ToString(format, nfi);
        }
    }

    public class GaugeChart : CircleChartBase<GaugeData, GaugeDataVisual>
    {
        public static readonly DependencyProperty RingWidthRelativeToRadiusProperty =
            DependencyProperty.Register("RingWidthRelativeToRadius", typeof(double), typeof(GaugeChart), new PropertyMetadata(0.1));
        public double RingWidthRelativeToRadius
        {
            get => (double)GetValue(RingWidthRelativeToRadiusProperty);
            set => SetValue(RingWidthRelativeToRadiusProperty, value);
        }

        public GaugeChart() => this.DefaultStyleKey = typeof(GaugeChart);

        static GaugeChart()=>
            DataProperty.OverrideMetadata(typeof(GaugeChart), new PropertyMetadata(DataChangedCallback));

        private static void DataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is GaugeChart control))
                return;

            var animationTimeMs = control.AnimationTimeMs;
            var animateFunc = CreateAnimationFunction(control);
            if (animationTimeMs > 0)
            {
                var animationTime = new TimeSpan(0, 0, 0, 0, animationTimeMs);
                var gaugeAnimation = new FunctionAnimation<GaugeDataVisual>
                {
                    AnimationFunction = animateFunc,
                    Duration = animationTime
                };
                control.BeginAnimation(GaugeChart.DataVisualRepresentationProperty, gaugeAnimation);
            }
            else
            {
                control.DataVisualRepresentation = animateFunc(1);
            }
        }

        private static Func<double, GaugeDataVisual> CreateAnimationFunction(GaugeChart control) =>
            part =>
            {
                const double markerLineLength = 4;
                const double markerRadius = 2;
                const double outerRadius = 50.0 - markerLineLength / 2;
                var actualValue = control.Data.Value * part;
                var maxValue = control.Data.MaxValue;
                var innerRadius = outerRadius - control.RingWidthRelativeToRadius * outerRadius;
                var center = new Point(outerRadius, outerRadius + markerLineLength);
                var angle = CalculateAngle(actualValue, maxValue);

                var pathGeometry = new PathGeometry();
                var (start, end) = CreateGaugeProgress(center, outerRadius, innerRadius, markerLineLength, angle, pathGeometry); // add gauge progress
                AddCircleGeometry(start, markerRadius, pathGeometry); // add gauge start marker
                AddCircleGeometry(end, markerRadius, pathGeometry); // add gauge end marker
                AddCircleGeometry(center, outerRadius, pathGeometry); // add outer circle
                return new GaugeDataVisual(pathGeometry, actualValue, maxValue, control.ValueFormat, control.numberFormatInfo);
            };

        private static double CalculateAngle(double value, double maxValue) =>
            value < maxValue 
                ? deg360 * value / maxValue 
                : deg360;

        private static (Point Start, Point End) CreateGaugeProgress(
            Point center, double outerRadius, double innerRadius, double markerLength, double angle, PathGeometry pathGeometry)
        {
            var startPoint = new Point(center.X, center.Y - outerRadius - markerLength);
            var endPoint = CalculatePointOnCircle(center, innerRadius - markerLength, angle);
            var figure = new PathFigure()
            {
                StartPoint = startPoint,
                IsClosed = false
            };
            pathGeometry.Figures.Add(figure);

            var line1 = new LineSegment(new Point(center.X, center.Y - innerRadius), true);
            var arc = CreateArcFromStart(center, innerRadius, angle);
            var line2 = new LineSegment(CalculatePointOnCircle(center, outerRadius, angle), true);
            var line3 = new LineSegment(endPoint, true);
            figure.Segments.Add(line1);
            figure.Segments.Add(arc);
            figure.Segments.Add(line2);
            figure.Segments.Add(line3);

            return (startPoint, endPoint);
        }

        private static ArcSegment CreateArcFromStart(Point center, double radius, double angle) =>
            new ArcSegment
            {
                IsLargeArc = angle > deg180,
                SweepDirection = angle > 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise,
                RotationAngle = 0,
                Size = new Size(radius, radius),
                Point = CalculatePointOnCircle(center, radius, angle)
            };
    }
}
