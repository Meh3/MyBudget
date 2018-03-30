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
        public string ValueString { get; set; }

        private double height;
        public double Height
        {
            get => height;
            set => SetField(ref height, value);
        }

        public ColumnDescription(string valueString, double height)
        {
            ValueString = valueString;
            Height = height;
        }

        public ColumnDescription(double value, double MaxValue, double panelHeight, string format = null, NumberFormatInfo nfi = null)
        {
            ValueString = ChartBase.ConvertValue(value, format, nfi);
            Height = value * panelHeight / MaxValue;
        }
    }

    public class ColumnsDescriptionAnimation : AnimationTimeline
    {
        private List<double> heights;

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(IEnumerable<ColumnDescription>), typeof(ColumnsDescriptionAnimation));
        public IEnumerable<ColumnDescription> To
        {
            get => (IEnumerable<ColumnDescription>)GetValue(ToProperty);
            set
            {
                heights = value.Select(x => x.Height).ToList();
                SetValue(ToProperty, value);
            }
        }

        public override Type TargetPropertyType => typeof(IEnumerable<ColumnDescription>);

        protected override Freezable CreateInstanceCore() => new ColumnsDescriptionAnimation() { heights = this.heights };

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

        private void UpdateHeights(IEnumerable<ColumnDescription> columns, double ratio)
        {
            var i = 0;
            foreach (var column in columns.ToList())
            {
                column.Height = heights[i] * ratio;
                ++i;
            }
        }
    }

    public class ColumnsChart : ChartBase
    {
        private double panelHeight = 100;

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

        private static void SeparatorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ColumnsChart control))
                return;

            var data = control.Data as IEnumerable<ColumnData>;
            var maxValue = data.Select(x => x.Value).Max();
            var columns = data.Select(x => new ColumnDescription(x.Value, maxValue, 100, control.ValueFormat, control.numberFormatInfo)).ToList();

            var animationTime = new TimeSpan(0, 0, 0, 2);
            var columnsAnimation = new ColumnsDescriptionAnimation { To = columns, Duration = animationTime };
            control.BeginAnimation(ColumnsChart.ColumnDescriptionProperty, columnsAnimation);
        }
    }
}
