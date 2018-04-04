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
        public int Index { get; set; }

        public PieData(string name, double value)
        {
            Value = value;
            Name = name;
        }
    }

    public class ValueAndLocation
    {
        public string StringValue { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
    }

    public class PieDataVisual
    {
        public PathGeometry PieGeometry { get; private set; }
        public IEnumerable<ValueAndLocation> ValuesAndLocations { get; private set; }

        public PieDataVisual(PathGeometry pieGeometry, IEnumerable<ValueAndLocation> valuesAndLocations)
        {
            PieGeometry = pieGeometry;
            ValuesAndLocations = valuesAndLocations;
        }
    }

    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = PathCirclePartName, Type = typeof(Path))]
    public class PieChart : CircleChartBase<IEnumerable<PieData>, PieDataVisual>
    {
        private const string CanvasPartName = "PART_Canvas";
        private const string PathCirclePartName = "PART_CirclePath";
        private Canvas TemplateCanvas;
        private Path TemplatePathCircle;

        private double dataSum;
        private List<(double Percentage, double RadAngle)> relativeData;
        private double outerRadius;

        public PieChart() => this.DefaultStyleKey = typeof(PieChart);

        static PieChart() =>
            DataProperty.OverrideMetadata(typeof(PieChart), new PropertyMetadata(DataChangedCallback));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplateCanvas = Template.FindName(CanvasPartName, this) as Canvas;
            TemplatePathCircle = Template.FindName(PathCirclePartName, this) as Path;

            UpdateControlSize(this);
        }

        private static void DataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PieChart control))
                return;

            var data = control.Data;
            var dataSum = control.dataSum = data.Select(x => x.Value).Sum();
            control.relativeData = data.Select(x => (x.Value * 100 / dataSum, x.Value * deg360 / dataSum)).ToList();

            var i = 1;
            foreach (var dataValue in data)
            {
                dataValue.Index = i++;
            }

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

        private static Func<double, PieDataVisual> CreateAnimationFunction(PieChart control) =>
            part =>
            {
                var textHalfSize = 10;
                var outerRadius = control.outerRadius;
                var radius = control.Radius;
                var radiusForPercentageText = radius + textHalfSize + 5;
                var radiusForAliasText = radius / 2;
                var center = new Point(outerRadius, outerRadius);

                var actualAngles = control.relativeData.Select(x => x.RadAngle * part).ToList();
                var anglesFromBegining = CalculateAnglesFromBegining(actualAngles).ToList();

                if (part == 1.0)
                {
                    var lastIndex = anglesFromBegining.Count - 1;
                    anglesFromBegining[lastIndex] = (deg360, anglesFromBegining[lastIndex].PercentageAngle);
                }

                var pathGeometry = new PathGeometry();
                anglesFromBegining
                    .Select(x => new LineGeometry(center, CalculatePointOnCircle(center, radius, x.LineAngle)))
                    .ToList()
                    .ForEach(pathGeometry.AddGeometry);

                var i = 1;
                var aliasesAndLocations = anglesFromBegining
                    .Select(x => CalculatePointOnCircle(center, radiusForAliasText, x.PercentageAngle))
                    .Select(point => new ValueAndLocation { Left = point.X - textHalfSize, Top = point.Y - textHalfSize, StringValue = (i++).ToString() })
                    .ToList();

                var percentagesAndLocations = control.relativeData
                    .Select(x => Math.Round(x.Percentage * part).ToString() + "%")
                    .Zip(anglesFromBegining, (percentage, angle) => new { Percentage = percentage, Point = CalculatePointOnCircle(center, radiusForPercentageText, angle.PercentageAngle) })
                    .Select(x => new ValueAndLocation { Left = x.Point.X - textHalfSize, Top = x.Point.Y - textHalfSize, StringValue = x.Percentage })
                    .ToList();

                aliasesAndLocations.AddRange(percentagesAndLocations);

                return new PieDataVisual(pathGeometry, aliasesAndLocations);
            };

        private static IEnumerable<(double LineAngle, double PercentageAngle)> CalculateAnglesFromBegining(List<double> angles)
        {
            for (int i = 0; i < angles.Count ; ++i)
            {
                var lineAngle = 0.0;
                for (int j = 0; j <= i; ++j)
                {
                    lineAngle += angles[j];
                }
                yield return (lineAngle, lineAngle - angles[i] / 2);
            }
        }

        private static void UpdateControlSize(PieChart pieChart)
        {
            var ringForMarkersWidth = 30;
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
