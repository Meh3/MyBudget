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
    public class GaugeDataVisualAnimation : AnimationTimeline
    {
        public Func<double, GaugeDataVisual> AnimationFunction { get; set; }

        public override Type TargetPropertyType => typeof(GaugeDataVisual);

        protected override Freezable CreateInstanceCore() => new GaugeDataVisualAnimation() { AnimationFunction = this.AnimationFunction };

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock) =>
            UdateValue(animationClock);

        public GaugeDataVisual UdateValue(AnimationClock animationClock) =>
            AnimationFunction == null
                ? null
                : animationClock.CurrentProgress.HasValue
                    ? AnimationFunction(animationClock.CurrentProgress.Value)
                    : AnimationFunction(0);
    }
}
