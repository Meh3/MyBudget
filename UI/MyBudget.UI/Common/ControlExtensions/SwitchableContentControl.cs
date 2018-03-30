using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Timers;
using MyBudget.UI.Views;
using System.Windows.Media.Animation;

namespace MyBudget.UI.Common
{
    [TemplatePart(Name = GridPartName, Type = typeof(Grid))]
    [TemplatePart(Name = Content1PartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = Content2PartName, Type = typeof(ContentControl))]
    public class SwitchableContentControl : Control
    {
        private const string GridPartName = "PART_Grid";
        private const string Content1PartName = "PART_Content1";
        private const string Content2PartName = "PART_Content2";

        private Grid TemplateGrid;
        private ContentControl TemplateContent1;
        private ContentControl TemplateContent2;

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(SwitchableContentControl), new PropertyMetadata(SwitchContentChangedCallback));
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty AnimationTimeMsProperty =
            DependencyProperty.Register("AnimationTimeMs", typeof(int), typeof(SwitchableContentControl), new PropertyMetadata(0));
        public int AnimationTimeMs
        {
            get => (int)GetValue(AnimationTimeMsProperty);
            set => SetValue(AnimationTimeMsProperty, value);
        }

        public static readonly DependencyProperty OldContentAnimationDirectionProperty =
            DependencyProperty.Register("OldContentAnimationDirection", typeof(Direction), typeof(SwitchableContentControl), new PropertyMetadata(Direction.Left));
        public Direction OldContentAnimationDirection
        {
            get => (Direction)GetValue(OldContentAnimationDirectionProperty);
            set => SetValue(OldContentAnimationDirectionProperty, value);
        }

        public static readonly DependencyProperty NewContentAnimationDirectionProperty =
            DependencyProperty.Register("NewContentAnimationDirection", typeof(Direction), typeof(SwitchableContentControl), new PropertyMetadata(Direction.Left));
        public Direction NewContentAnimationDirection
        {
            get => (Direction)GetValue(NewContentAnimationDirectionProperty);
            set => SetValue(NewContentAnimationDirectionProperty, value);
        }

        public SwitchableContentControl() => this.DefaultStyleKey = typeof(SwitchableContentControl);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TemplateGrid = Template.FindName(GridPartName, this) as Grid;
            TemplateContent1 = Template.FindName(Content1PartName, this) as ContentControl;
            TemplateContent2 = Template.FindName(Content2PartName, this) as ContentControl;
            TemplateGrid.Loaded += SwitchableContentControl_Loaded;
        }

        private void SwitchableContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Content != null)
            {
                SwitchContentChangedCallback(this, new DependencyPropertyChangedEventArgs());
            }
        }

        private static void SwitchContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ( !(d is SwitchableContentControl control) || control.TemplateGrid == null )
                return;

            var (newContent, oldContent) = GetClassifiedContents(control);
            var animationTime = new TimeSpan(0, 0, 0, 0, control.AnimationTimeMs);
            if (control.AnimationTimeMs <= 0)
            {
                oldContent.Content = control.Content;
                return;
            }
            newContent.Margin = GetStartMarginForNewContent(control.NewContentAnimationDirection, control);
            newContent.Content = control.Content;

            var zeroMargin = new Thickness(0, 0, 0, 0);
            var endMarginForOldContent = GetEndMarginForOldContent(control.OldContentAnimationDirection, control);
            var newContentAnimation = new ThicknessAnimation(zeroMargin, animationTime);
            var oldContentAnimation = new ThicknessAnimation(endMarginForOldContent, animationTime, FillBehavior.Stop);
            oldContentAnimation.Completed += (sender, args) =>
            {
                oldContent.Content = null;
                oldContent.Margin = zeroMargin;
            };
            newContent.BeginAnimation(ContentControl.MarginProperty, newContentAnimation);
            oldContent.BeginAnimation(ContentControl.MarginProperty, oldContentAnimation);
        }

        private static Thickness GetStartMarginForNewContent(Direction direction, SwitchableContentControl control)
        {
            var controlWidth = control.TemplateGrid.ActualWidth;
            var controlHeight = control.TemplateGrid.ActualHeight;

            switch (direction)
            {
                case Direction.Left:
                default:
                    return new Thickness(controlWidth, 0, 0, 0);

                case Direction.Right:
                    return new Thickness(-controlWidth, 0, 0, 0);

                case Direction.Up:
                    return new Thickness(0, controlHeight, 0, 0);

                case Direction.Down:
                    return new Thickness(0, -controlHeight, 0, 0);
            }
        }

        private static Thickness GetEndMarginForOldContent(Direction direction, SwitchableContentControl control)
        {
            var tempMargins = GetStartMarginForNewContent(direction, control);
            return new Thickness(-tempMargins.Left, -tempMargins.Top, 0, 0);
        }

        private static (ContentControl New, ContentControl Old) GetClassifiedContents(SwitchableContentControl control) =>
            control.TemplateContent1.Content != null
                ? (control.TemplateContent2, control.TemplateContent1)
                : (control.TemplateContent1, control.TemplateContent2);
    }
}
