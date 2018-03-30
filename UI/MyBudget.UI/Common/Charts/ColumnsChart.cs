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
        public string Name { get; set; }
        public double Value { get; set; }

        public ColumnData(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ColumnDescription : NotifiableObject
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
                Height = value * heightToValueRatio;
                StringValue = value.ToString(format, numberFormatInfo);
                SetField(ref numberValue, value);
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(StringValue));
            }
        }

        public ColumnDescription(double value, double heightToValueRatio, string format = null, NumberFormatInfo nfi = null)
        {
            NumberValue = value;
            this.heightToValueRatio = heightToValueRatio;
            this.format = format;
            numberFormatInfo = nfi;
        }
    }

    public class ColumnsDescriptionAnimation : AnimationTimeline
    {
        private List<double> values;

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(IEnumerable<ColumnDescription>), typeof(ColumnsDescriptionAnimation));
        public IEnumerable<ColumnDescription> To
        {
            get => (IEnumerable<ColumnDescription>)GetValue(ToProperty);
            set
            {
                values = value.Select(x => x.NumberValue).ToList();
                SetValue(ToProperty, value);
            }
        }

        public override Type TargetPropertyType => typeof(IEnumerable<ColumnDescription>);

        protected override Freezable CreateInstanceCore() => new ColumnsDescriptionAnimation() { values = this.values};

        public override object GetCurrentValue(object defaultOriginValue,
                                       object defaultDestinationValue,
                                       AnimationClock animationClock)
        {
            UdateValue(animationClock);
            return To;
        }

        public void UdateValue(AnimationClock animationClock)
        {
            if (To == null)
                return;

            if (!animationClock.CurrentProgress.HasValue)
                UpdateHeights(To, 0);

            UpdateHeights(To, animationClock.CurrentProgress.Value);
        }

        private void UpdateHeights(IEnumerable<ColumnDescription> columns, double timeRatio)
        {
            var i = 0;
            foreach (var column in columns.ToList())
            {
                column.NumberValue = values[i] * timeRatio;
                ++i;
            }
        }
    }

    [TemplatePart(Name = PanelPartName, Type = typeof(Grid))]
    public class ColumnsChart : ChartBase
    {
        private const string PanelPartName = "PART_ColumnsPanel";
        private double panelHeight = 100;
        private Grid TemplatePanel;

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(IEnumerable<ColumnData>), typeof(ColumnsChart), new PropertyMetadata(null, SeparatorChangedCallback));
        public IEnumerable<ColumnData> Data
        {
            get => (IEnumerable<ColumnData>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty ColumnDescriptionProperty =
            DependencyProperty.Register("ColumnDescription", typeof(IEnumerable<ColumnDescription>), typeof(ColumnsChart));
        public IEnumerable<ColumnDescription> ColumnDescription
        {
            get => (IEnumerable<ColumnDescription>)GetValue(ColumnDescriptionProperty);
            set => SetValue(ColumnDescriptionProperty, value);
        }


        public ColumnsChart() => this.DefaultStyleKey = typeof(ColumnsChart);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplatePanel = Template.FindName(PanelPartName, this) as Grid;
            TemplatePanel.Loaded += TemplatePanel_Loaded;
        }

        private void TemplatePanel_Loaded(object sender, RoutedEventArgs e)
        {
            panelHeight = TemplatePanel.ActualHeight - 20;
            SeparatorChangedCallback(this, new DependencyPropertyChangedEventArgs());
        }

        private static void SeparatorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ColumnsChart control) || control.TemplatePanel == null)
                return;

            var columns = ConvertData(control.Data, control.panelHeight, control.ValueFormat, control.numberFormatInfo);
            var animationTime = new TimeSpan(0, 0, 0, 2);
            var columnsAnimation = new ColumnsDescriptionAnimation { To = columns, Duration = animationTime };
            control.BeginAnimation(ColumnsChart.ColumnDescriptionProperty, columnsAnimation);
        }

        private static IEnumerable<ColumnDescription> ConvertData(IEnumerable<ColumnData> columnData, double maxHeight, string format, NumberFormatInfo nfi)
        {
            var maxValue = columnData.Select(x => x.Value).Max();
            var ratio = maxHeight / maxValue;
            return columnData
                .Select(x => new ColumnDescription(x.Value, ratio, format, nfi))
                .ToList();
        }
    }
}
