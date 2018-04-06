using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;

namespace MyBudget.UI.Common
{
    public abstract class ChartBase : Control
    {
        protected readonly NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(ChartBase), new PropertyMetadata(string.Empty));
        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register("ValueFormat", typeof(string), typeof(ChartBase), new PropertyMetadata(null));
        public string ValueFormat
        {
            get => (string)GetValue(ValueFormatProperty);
            set => SetValue(ValueFormatProperty, value);
        }

        public static readonly DependencyProperty ThousandsSeparatorProperty =
            DependencyProperty.Register("ThousandsSeparator", typeof(string), typeof(ChartBase), new PropertyMetadata(string.Empty, SeparatorChangedCallback));
        public string ThousandsSeparator
        {
            get => (string)GetValue(ThousandsSeparatorProperty);
            set => SetValue(ThousandsSeparatorProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ChartBase), new PropertyMetadata(string.Empty));
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty AnimationTimeMsProperty =
            DependencyProperty.Register("AnimationTimeMs", typeof(int), typeof(ChartBase), new PropertyMetadata(0));
        public int AnimationTimeMs
        {
            get => (int)GetValue(AnimationTimeMsProperty);
            set => SetValue(AnimationTimeMsProperty, value);
        }

        public static readonly DependencyProperty PathStyleProperty =
            DependencyProperty.Register("PathStyle", typeof(Style), typeof(ChartBase));
        public Style PathStyle
        {
            get => (Style)GetValue(PathStyleProperty);
            set => SetValue(PathStyleProperty, value);
        }

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register("TextStyle", typeof(Style), typeof(ChartBase));
        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty);
            set => SetValue(TextStyleProperty, value);
        }

        private static void SeparatorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ChartBase control))
                return;
            control.numberFormatInfo.NumberGroupSeparator = control.ThousandsSeparator;
        }
    }

    public abstract class ChartBase<TData, TDataForChart> : ChartBase
        where TData : class
        where TDataForChart : class
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(TData), typeof(ChartBase<TData, TDataForChart>));
        public TData Data
        {
            get => (TData)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataVisualRepresentationProperty =
            DependencyProperty.Register("DataVisualRepresentation", typeof(TDataForChart), typeof(ChartBase<TData, TDataForChart>));
        public TDataForChart DataVisualRepresentation
        {
            get => (TDataForChart)GetValue(DataVisualRepresentationProperty);
            set => SetValue(DataVisualRepresentationProperty, value);
        }
    }
}
