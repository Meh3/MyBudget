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
    public class ColumnDataVisualAnimation : AnimationTimeline
    {
        private List<double> values;

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(IEnumerable<ColumnDataVisual>), typeof(ColumnDataVisualAnimation));
        public IEnumerable<ColumnDataVisual> To
        {
            get => (IEnumerable<ColumnDataVisual>)GetValue(ToProperty);
            set
            {
                values = value.Select(x => x.NumberValue).ToList();
                SetValue(ToProperty, value);
            }
        }

        public override Type TargetPropertyType => typeof(IEnumerable<ColumnDataVisual>);

        protected override Freezable CreateInstanceCore() => new ColumnDataVisualAnimation() { values = this.values };

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

        private void UpdateHeights(IEnumerable<ColumnDataVisual> columns, double timeRatio)
        {
            var i = 0;
            foreach (var column in columns.ToList())
                column.NumberValue = values[i++] * timeRatio;
        }
    }
}
