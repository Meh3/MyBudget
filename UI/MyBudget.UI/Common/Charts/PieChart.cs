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
    public class PieData
    {
        public double Value { get; private set; }
        public string Name { get; private set; }

        public PieData(string name, double value)
        {
            Value = value;
            Name = name;
        }
    }

    public class PieDataVisual
    {
        public PathGeometry PieGeometry { get; private set; }
        public IEnumerable<string> StringValues { get; private set; }
        public IEnumerable<Point> MarkerPoints { get; private set; }

        public PieDataVisual(PathGeometry pieGeometry)
        {
            PieGeometry = pieGeometry;
        }

        public PieDataVisual(PathGeometry pieGeometry, IEnumerable<double> values, IEnumerable<Point> markerPoints, string format, NumberFormatInfo nfi)
        {
            PieGeometry = pieGeometry;
            StringValues = values.Select(x => x.ToString(format, nfi)).ToList();
            MarkerPoints = markerPoints;
        }
    }

    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = PathPartName, Type = typeof(Path))]
    [TemplatePart(Name = PathCirclePartName, Type = typeof(Path))]
    public class PieChart : CircleChartBase<IEnumerable<PieData>, PieDataVisual>
    {
        private const string CanvasPartName = "PART_Canvas";
        private const string PathPartName = "PART_Path";
        private const string PathCirclePartName = "PART_CirclePath";
        private Canvas TemplateCanvas;
        private Path TemplatePath;
        private Path TemplatePathCircle;

        private double dataSum;
        private List<(string Name, double Value, double Proportion, double Percentage, double RadAngle)> relativeData;
        private double outerRadius;

        public PieChart() => this.DefaultStyleKey = typeof(PieChart);

        static PieChart() =>
            DataProperty.OverrideMetadata(typeof(PieChart), new PropertyMetadata(DataChangedCallback));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplateCanvas = Template.FindName(CanvasPartName, this) as Canvas;
            TemplatePath = Template.FindName(PathPartName, this) as Path;
            TemplatePathCircle = Template.FindName(PathCirclePartName, this) as Path;

            UpdateControlSize(this);
        }

        private static void DataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PieChart control))
                return;

            var data = control.Data;
            control.dataSum = data.Select(x => x.Value).Sum();
            var dataSum = control.dataSum;
            control.relativeData = data.Select(x => (x.Name, x.Value, x.Value / dataSum, x.Value * 100 / dataSum, x.Value * deg360 / dataSum)).ToList();

            var animationTimeMs = control.AnimationTimeMs;
            var animateFunction = CreateAnimationFunction(control);
            if (animationTimeMs > 0)
            {
                var animationTime = new TimeSpan(0, 0, 0, 0, animationTimeMs);
                var pieAnimation = new FunctionAnimation<PieDataVisual>
                {
                    AnimationFunction = animateFunction,
                    Duration = animationTime
                };
                control.BeginAnimation(PieChart.DataVisualRepresentationProperty, pieAnimation);
            }
            else
            {
                control.DataVisualRepresentation = animateFunction(1);
            }
        }

        private static Func<double, PieDataVisual> CreateAnimationFunction(PieChart control)
            =>
            part =>
            {
                var outerRadius = control.outerRadius;
                var radius = control.Radius;
                var center = new Point(outerRadius, outerRadius);

                var actualAngles = control.relativeData.Select(x => x.RadAngle * part).ToList();
                var anglesFromBegining = CalculateAnglesFromBegining(actualAngles).ToList();

                if (part == 1.0)
                {
                    var lastIndex = anglesFromBegining.Count - 1;
                    anglesFromBegining[lastIndex] = deg360;
                }

                var pathGeometry = new PathGeometry();
                var lines = anglesFromBegining.Select(x => new LineGeometry(center, CalculatePointOnCircle(center, radius, x))).ToList();
                lines.ForEach(pathGeometry.AddGeometry);

                return new PieDataVisual(pathGeometry);
            };

        private static IEnumerable<double> CalculateAnglesFromBegining(List<double> angles)
        {
            for (int i = 0; i < angles.Count ; ++i)
            {
                var angleFromBegining = 0.0;
                for (int j = 0; j <= i; ++j)
                {
                    angleFromBegining += angles[j];
                }
                yield return angleFromBegining;
            }
        }

        private static void UpdateControlSize(PieChart pieChart)
        {
            var ringForMarkersWidth = 5;
            var radius = pieChart.Radius;
            var outerRadius = pieChart.outerRadius = radius + ringForMarkersWidth;
            var height = outerRadius * 2;
            var width = outerRadius * 2;
            pieChart.TemplateCanvas.Height = height;
            pieChart.TemplateCanvas.Width = width;

            var pathGeometry = new PathGeometry();
            pieChart.TemplatePathCircle.Data = pathGeometry;
            AddCircleGeometry(new Point(outerRadius, outerRadius), radius, pathGeometry);
        }
    }
}
