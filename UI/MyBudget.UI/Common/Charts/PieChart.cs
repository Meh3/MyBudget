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

        public PieData(double value, string name)
        {
            Value = value;
            Name = name;
        }
    }

    public class PieDataVisual
    {
        private readonly string format;
        private readonly NumberFormatInfo numberFormatInfo;

        public PathGeometry PieGeometry { get; private set; }
        public IEnumerable<string> StringValues { get; private set; }
        public IEnumerable<Point> MarkerPoints { get; private set; }

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
    public class PieChart : CircleChartBase<PieData, PieDataVisual>
    {
        private const string CanvasPartName = "PART_Canvas";
        private const string PathPartName = "PART_Path";
        private const string PathCirclePartName = "PART_CirclePath";
        private Canvas TemplateCanvas;
        private Path TemplatePath;
        private Path TemplatePathCircle;

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
            //if (!(d is PieChart control))
            //    return;

            //var animationTimeMs = control.AnimationTimeMs;
            //if (animationTimeMs > 0)
            //{

            //    control.BeginAnimation(PieChart.DataVisualRepresentationProperty, );
            //}
            //else
            //{
            //    control.DataVisualRepresentation = ;
            //}
        }

        private static void UpdateControlSize(PieChart pieChart)
        {
            var ringForMarkersWidth = 5;
            var radius = pieChart.Radius;
            var outerRadius = radius + ringForMarkersWidth;
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
