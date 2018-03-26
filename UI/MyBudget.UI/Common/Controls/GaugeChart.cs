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
    [TemplatePart(Name = PathPartName, Type = typeof(Path))]
    [TemplatePart(Name = TextPartName, Type = typeof(TextBlock))]
    public class GaugeChart : Control
    {
        private const double deg90 = Math.PI / 2;
        private const double deg180 = Math.PI;
        private const double deg360 = 2 * Math.PI;
        private const string PathPartName = "PART_Path";
        private const string TextPartName = "PART_Text";

        private Path TemplatePath;
        private TextBlock TemplateText;

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(GaugeChart), new PropertyMetadata("", TextChangedCallback));
        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register("ValueFormat", typeof(string), typeof(GaugeChart), new PropertyMetadata(TextChangedCallback));
        public string ValueFormat
        {
            get => (string)GetValue(ValueFormatProperty);
            set => SetValue(ValueFormatProperty, value);
        }

        public static readonly DependencyProperty ThousandsSeparatorProperty =
            DependencyProperty.Register("ThousandsSeparator", typeof(string), typeof(GaugeChart), new PropertyMetadata("", TextChangedCallback));
        public string ThousandsSeparator
        {
            get => (string)GetValue(ThousandsSeparatorProperty);
            set => SetValue(ThousandsSeparatorProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GaugeChart));
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(GaugeChart), new PropertyMetadata(50.0));
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(GaugeChart), new PropertyMetadata(0.0, ValueChangedCallback));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(GaugeChart), new PropertyMetadata(1.0, ValueChangedCallback));
        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly DependencyProperty RingWidthRelativeToRadiusProperty =
            DependencyProperty.Register("RingWidthRelativeToRadius", typeof(double), typeof(GaugeChart), new PropertyMetadata(0.1));
        public double RingWidthRelativeToRadius
        {
            get => (double)GetValue(RingWidthRelativeToRadiusProperty);
            set => SetValue(RingWidthRelativeToRadiusProperty, value);
        }

        public static readonly DependencyProperty PathStyleProperty =
            DependencyProperty.Register("PathStyle", typeof(Style), typeof(GaugeChart));
        public Style PathStyle
        {
            get => (Style)GetValue(PathStyleProperty);
            set => SetValue(PathStyleProperty, value);
        }

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register("TextStyle", typeof(Style), typeof(GaugeChart));
        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty);
            set => SetValue(TextStyleProperty, value);
        }

        public GaugeChart() => this.DefaultStyleKey = typeof(GaugeChart);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplatePath = Template.FindName(PathPartName, this) as Path;
            TemplateText = Template.FindName(TextPartName, this) as TextBlock;

            // animation when template is loaded 
            var animationTime = new TimeSpan(0, 0, 0, 2);
            var valueAnimation = new DoubleAnimation(0, Value, animationTime);
            BeginAnimation(GaugeChart.ValueProperty, valueAnimation);
        }

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ( !(d is GaugeChart control) || control.TemplateText == null )
                return;

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = control.ThousandsSeparator;

            var (value, maxValue) = string.IsNullOrEmpty(control.ValueFormat)
                ? (control.Value.ToString(), 
                   control.MaxValue.ToString())
                : (control.Value.ToString(control.ValueFormat, nfi), 
                   control.MaxValue.ToString(control.ValueFormat, nfi));

            control.TemplateText.Text = $"{value} {control.Unit}{Environment.NewLine}/ {maxValue} {control.Unit}";
        }

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is GaugeChart control) || control.TemplatePath == null)
                return;

            const double markerLineLength = 4;
            const double markerRadius = 2;
            const double outerRadius = 50.0 - markerLineLength / 2;
            var innerRadius = outerRadius - control.RingWidthRelativeToRadius * outerRadius;
            var center = new Point(outerRadius, outerRadius + markerLineLength);
            var angle = CalculateAngle(control.Value, control.MaxValue);

            var (figure, start, end) = CreateGaugeProgress(center, outerRadius, innerRadius, markerLineLength, angle);
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(figure); // gauge progress
            pathGeometry.AddGeometry(CreateCircleGeometry(start, markerRadius)); // gauge start marker
            pathGeometry.AddGeometry(CreateCircleGeometry(end, markerRadius)); // gauge end marker
            pathGeometry.AddGeometry(CreateCircleGeometry(center, outerRadius)); // outer circle

            control.TemplatePath.Data = pathGeometry;
            TextChangedCallback(d, e);
        }

        private static double CalculateAngle(double value, double maxValue) =>
            value < maxValue 
                ? deg360 * value / maxValue 
                : deg360;

        private static Point CalculatePointOnCircle(Point center, double radius, double angle) =>
            new Point()
            {
                // calculated point has 90 deg offset
                X = center.X + radius * Math.Cos(angle + deg90),
                Y = center.Y - radius * Math.Sin(angle + deg90)
            };

        private static EllipseGeometry CreateCircleGeometry(Point center, double radius) =>
            new EllipseGeometry
            {
                Center = center,
                RadiusX = radius,
                RadiusY = radius,
            };

        private static (PathFigure Figure, Point Start, Point End) CreateGaugeProgress(
            Point center, double outerRadius, double innerRadius, double markerLength, double angle)
        {
            var startPoint = new Point(center.X, center.Y - outerRadius - markerLength);
            var endPoint = CalculatePointOnCircle(center, innerRadius - markerLength, angle);
            var figure = new PathFigure()
            {
                StartPoint = startPoint,
                IsClosed = false
            };
            
            var line1 = new LineSegment(new Point(center.X, center.Y - innerRadius), true);
            var arc = CreateArcFromStart(center, innerRadius, angle);
            var line2 = new LineSegment(CalculatePointOnCircle(center, outerRadius, angle), true);
            var line3 = new LineSegment(endPoint, true);
            figure.Segments.Add(line1);
            figure.Segments.Add(arc);
            figure.Segments.Add(line2);
            figure.Segments.Add(line3);

            return (figure, startPoint, endPoint);
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
