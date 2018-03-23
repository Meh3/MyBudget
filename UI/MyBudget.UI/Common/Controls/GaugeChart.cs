using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;

namespace MyBudget.UI.Common
{
    [TemplatePart(Name = PathPartName, Type = typeof(Path))]
    [TemplatePart(Name = TextPartName, Type = typeof(TextBlock))]
    public class GaugeChart : Control
    {
        private const string PathPartName = "PART_Path";
        private const string TextPartName = "PART_Text";
        private Path TemplatePath;
        private TextBlock TemplateText;

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(GaugeChart), new PropertyMetadata("", TextChangedCallback));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register("ValueFormat", typeof(string), typeof(GaugeChart), new PropertyMetadata(TextChangedCallback));

        public string ValueFormat
        {
            get { return (string)GetValue(ValueFormatProperty); }
            set { SetValue(ValueFormatProperty, value); }
        }

        public static readonly DependencyProperty ThousandsSeparatorProperty =
            DependencyProperty.Register("ThousandsSeparator", typeof(string), typeof(GaugeChart), new PropertyMetadata("", TextChangedCallback));

        public string ThousandsSeparator
        {
            get { return (string)GetValue(ThousandsSeparatorProperty); }
            set { SetValue(ThousandsSeparatorProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GaugeChart));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(GaugeChart), new PropertyMetadata(50.0));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(GaugeChart), new PropertyMetadata(0.0, ValueChangedCallback));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(GaugeChart), new PropertyMetadata(1.0));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty RingWidthRelativeToRadiusProperty =
            DependencyProperty.Register("RingWidthRelativeToRadius", typeof(double), typeof(GaugeChart), new PropertyMetadata(0.1));

        public double RingWidthRelativeToRadius
        {
            get { return (double)GetValue(RingWidthRelativeToRadiusProperty); }
            set { SetValue(RingWidthRelativeToRadiusProperty, value); }
        }

        public static readonly DependencyProperty PathStyleProperty =
            DependencyProperty.Register("PathStyle", typeof(Style), typeof(GaugeChart));

        public Style PathStyle
        {
            get { return (Style)GetValue(PathStyleProperty); }
            set { SetValue(PathStyleProperty, value); }
        }

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register("TextStyle", typeof(Style), typeof(GaugeChart));

        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public GaugeChart()
        {
            this.DefaultStyleKey = typeof(GaugeChart);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplatePath = Template.FindName(PathPartName, this) as Path;
            TemplateText = Template.FindName(TextPartName, this) as TextBlock;

            var animationTime = new TimeSpan(0, 0, 0, 2);
            var valueAnimation = new DoubleAnimation(0, Value, animationTime);
            BeginAnimation(GaugeChart.ValueProperty, valueAnimation);
        }

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GaugeChart;
            if (control?.TemplateText == null)
                return;

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = control.ThousandsSeparator;
            string value;
            string maxValue;

            if (!string.IsNullOrEmpty(control.ValueFormat))
            {
                value = control.Value.ToString(control.ValueFormat, nfi);
                maxValue = control.MaxValue.ToString(control.ValueFormat, nfi);
            }                        
            else
            {
                value = control.Value.ToString();
                maxValue = control.MaxValue.ToString();
            }
            control.TemplateText.Text = $"{value} {control.Unit}{Environment.NewLine}/ {maxValue} {control.Unit}";
        }

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GaugeChart;
            if (control?.TemplatePath == null)
                return;

            var titleLineHeight = 5;
            var outerRadius = 50 - titleLineHeight / 2;
            var center = new Point(outerRadius, outerRadius + titleLineHeight);
            var innerRadius = outerRadius - control.RingWidthRelativeToRadius * outerRadius;
            var isLargeAngle = control.Value > control.MaxValue / 2;
            var angle = CalculateAngle(control.Value, control.MaxValue);

            var pathGeo = new PathGeometry();
            pathGeo.Figures.Add(CreateCircleFigure(center, outerRadius));

            var pathFig = new PathFigure();
            pathFig.StartPoint = new Point(center.X, center.Y - outerRadius);
            pathFig.IsClosed = true;
            pathGeo.Figures.Add(pathFig);

            var outerArc = new ArcSegment();
            outerArc.IsLargeArc = isLargeAngle;
            outerArc.SweepDirection = SweepDirection.Counterclockwise;
            outerArc.RotationAngle = 0;
            outerArc.Size = new Size(outerRadius, outerRadius);
            outerArc.Point = CalculatePointOnCircle(center, outerRadius, angle);
            pathFig.Segments.Add(outerArc);

            var line = new LineSegment();
            line.Point = CalculatePointOnCircle(center, innerRadius, angle);
            pathFig.Segments.Add(line);

            var innerArc = new ArcSegment();
            innerArc.IsLargeArc = isLargeAngle;
            innerArc.SweepDirection = SweepDirection.Clockwise;
            innerArc.RotationAngle = 0;
            innerArc.Size = new Size(innerRadius, innerRadius);
            innerArc.Point = new Point(center.X, center.Y - innerRadius);
            pathFig.Segments.Add(innerArc);

            var titleLineFig = new PathFigure();
            titleLineFig.StartPoint = new Point(center.X, center.Y - outerRadius);
            titleLineFig.IsClosed = true;
            pathGeo.Figures.Add(titleLineFig);

            var titleLine = new LineSegment();
            titleLine.Point = new Point(center.X, 0);
            titleLineFig.Segments.Add(titleLine);

            pathGeo.Figures.Add(CreateCircleFigure(new Point(center.X, 0), 2));

            control.TemplatePath.Data = pathGeo;
            TextChangedCallback(d, e);
        }

        private static double CalculateAngle(double value, double maxValue) =>
            value < maxValue ? 2 * Math.PI * value / maxValue : 6.28;

        private static Point CalculatePointOnCircle(Point center, double radius, double angle)
        {
            var initialRotateAngel = Math.PI / 2;
            var x = center.X + radius * Math.Cos(angle + initialRotateAngel);
            var y = center.Y - radius * Math.Sin(angle + initialRotateAngel);
            return new Point(x, y);
        }

        private static PathFigure CreateCircleFigure(Point center, double radius)
        {
            var circleFig = new PathFigure();
            circleFig.StartPoint = new Point(center.X, center.Y - radius);
            circleFig.IsClosed = false;
            circleFig.IsFilled = false;

            var circle = new ArcSegment();
            circle.IsLargeArc = true;
            circle.SweepDirection = SweepDirection.Counterclockwise;
            circle.RotationAngle = 0;
            circle.Size = new Size(radius, radius);
            circle.Point = CalculatePointOnCircle(center, radius, 6.28);
            circleFig.Segments.Add(circle);

            return circleFig;
        }
    }
}
