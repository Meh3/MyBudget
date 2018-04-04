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
    public class FunctionAnimation<TVisualData> : AnimationTimeline
        where TVisualData : class
    {
        public Func<double, TVisualData> AnimationFunction { get; set; }

        public override Type TargetPropertyType => typeof(TVisualData);

        protected override Freezable CreateInstanceCore() => new FunctionAnimation<TVisualData>() { AnimationFunction = this.AnimationFunction };

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock) =>
            UdateValue(animationClock);

        public TVisualData UdateValue(AnimationClock animationClock) =>
            AnimationFunction == null
                ? null
                : animationClock.CurrentProgress.HasValue
                    ? AnimationFunction(animationClock.CurrentProgress.Value)
                    : AnimationFunction(0);
    }
}
