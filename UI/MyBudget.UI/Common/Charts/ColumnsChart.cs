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
    public class ColumnData
    {
        public string Name { get; private set; }
        public double Value { get; private set; }

        public ColumnData(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ColumnDataVisual : NotifiableObject
    {
        private readonly double heightToValueRatio;
        private readonly string format;
        private readonly NumberFormatInfo numberFormatInfo;

        public string StringValue { get; private set; }
        public double Height { get; private set; }

        private double numberValue;
        public double NumberValue
        {
            get => numberValue;
            set
            {
                SetField(ref numberValue, value);
                Height = numberValue * heightToValueRatio;
                StringValue = numberValue.ToString(format, numberFormatInfo);
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(StringValue));
            }
        }

        public ColumnDataVisual(double value, double heightToValueRatio, string format = null, NumberFormatInfo nfi = null)
        {
            NumberValue = value;
            this.heightToValueRatio = heightToValueRatio;
            this.format = format;
            numberFormatInfo = nfi;
        }
    }

    [TemplatePart(Name = PanelPartName, Type = typeof(Grid))]
    public class ColumnsChart : ChartBase<IEnumerable<ColumnData>, IEnumerable<ColumnDataVisual>>
    {
        private double panelHeight = 100;
        private const string PanelPartName = "PART_ColumnsPanel";
        private Grid TemplatePanel;


        public ColumnsChart() => this.DefaultStyleKey = typeof(ColumnsChart);

        static ColumnsChart() =>
            DataProperty.OverrideMetadata(typeof(ColumnsChart), new PropertyMetadata(null, DataChangedCallback));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplatePanel = Template.FindName(PanelPartName, this) as Grid;
            TemplatePanel.Loaded += TemplatePanel_Loaded;
        }

        private void TemplatePanel_Loaded(object sender, RoutedEventArgs e)
        {
            panelHeight = TemplatePanel.ActualHeight - 20;
            DataChangedCallback(this, new DependencyPropertyChangedEventArgs());
        }

        private static void DataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ColumnsChart control) || control.TemplatePanel == null)
                return;

            var columns = ConvertData(control.Data, control.panelHeight, control.ValueFormat, control.numberFormatInfo);
            var animationTimeMs = control.AnimationTimeMs;
            if (animationTimeMs > 0)
            {
                var animationTime = new TimeSpan(0, 0, 0, 0, animationTimeMs);
                var columnsAnimation = new ColumnDataVisualAnimation { To = columns, Duration = animationTime };
                control.BeginAnimation(ColumnsChart.DataVisualRepresentationProperty, columnsAnimation);
            }
            else
            {
                control.DataVisualRepresentation = columns;
            }
        }

        private static IEnumerable<ColumnDataVisual> ConvertData(IEnumerable<ColumnData> columnData, double maxHeight, string format, NumberFormatInfo nfi)
        {
            var maxValue = columnData.Select(x => x.Value).Max();
            var ratio = maxHeight / maxValue;
            return columnData
                .Select(x => new ColumnDataVisual(x.Value, ratio, format, nfi))
                .ToList();
        }
    }
}
