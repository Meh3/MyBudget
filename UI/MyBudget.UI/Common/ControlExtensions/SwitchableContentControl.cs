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
    [TemplatePart(Name = Content1PartName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = Content2PartName, Type = typeof(ContentPresenter))]
    public class SwitchableContentControl : Control
    {
        private const string GridPartName = "PART_Grid";
        private const string Content1PartName = "PART_Content1";
        private const string Content2PartName = "PART_Content2";

        private Grid TemplateGrid;
        private ContentPresenter TemplateContent1;
        private ContentPresenter TemplateContent2;

        public static readonly DependencyProperty Content1Property =
            DependencyProperty.Register("Content1", typeof(object), typeof(SwitchableContentControl), null);

        public object Content1
        {
            get { return GetValue(Content1Property); }
            set { SetValue(Content1Property, value); }
        }

        public static readonly DependencyProperty Content2Property =
            DependencyProperty.Register("Content2", typeof(object), typeof(SwitchableContentControl), null);

        public object Content2
        {
            get { return GetValue(Content2Property); }
            set { SetValue(Content2Property, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(SwitchableContentControl), new PropertyMetadata(SwitchContentChangedCallback));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeMsProperty =
            DependencyProperty.Register("AnimationTimeMs", typeof(int), typeof(SwitchableContentControl), new PropertyMetadata(0));

        public int AnimationTimeMs
        {
            get { return (int)GetValue(AnimationTimeMsProperty); }
            set { SetValue(AnimationTimeMsProperty, value); }
        }

        public static readonly DependencyProperty OldContentAnimationDirectionProperty =
            DependencyProperty.Register("OldContentAnimationDirection", typeof(Direction), typeof(SwitchableContentControl), new PropertyMetadata(Direction.Left));

        public Direction OldContentAnimationDirection
        {
            get { return (Direction)GetValue(OldContentAnimationDirectionProperty); }
            set { SetValue(OldContentAnimationDirectionProperty, value); }
        }

        public static readonly DependencyProperty NewContentAnimationDirectionProperty =
            DependencyProperty.Register("NewContentAnimationDirection", typeof(Direction), typeof(SwitchableContentControl), new PropertyMetadata(Direction.Left));

        public Direction NewContentAnimationDirection
        {
            get { return (Direction)GetValue(NewContentAnimationDirectionProperty); }
            set { SetValue(NewContentAnimationDirectionProperty, value); }
        }

        public SwitchableContentControl()
        {
            this.DefaultStyleKey = typeof(SwitchableContentControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TemplateGrid = Template.FindName(GridPartName, this) as Grid;
            TemplateContent1 = Template.FindName(Content1PartName, this) as ContentPresenter;
            TemplateContent2 = Template.FindName(Content2PartName, this) as ContentPresenter;

            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => 
            {
                Content = isTrue ? new CurrentMonthView() as object : new AddTransactionView() as object;
                isTrue = !isTrue;
            });
        }

        Timer timer = new Timer(5000);
        bool isTrue = false;

        private static void SwitchContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SwitchableContentControl;

            if (control == null)
                return;

            if (control.AnimationTimeMs == 0)
            {
                SwitchContentWithoutAnimation(control);
                return;
            }

            var animationTime = new TimeSpan(0, 0, 0, 0, control.AnimationTimeMs);
            var startMarginForNewContent = GetStartMarginForNewContent(control.NewContentAnimationDirection, control);
            var endMarginForOldContent = GetEndMarginForNewContent(control.OldContentAnimationDirection, control);
            ContentPresenter newContentTemplate;
            ContentPresenter oldContentTemplate;
            Action setOldContentToNull;

            if (control.Content1 != null)
            {
                oldContentTemplate = control.TemplateContent1;
                newContentTemplate = control.TemplateContent2;
                newContentTemplate.Margin = startMarginForNewContent;
                control.Content2 = control.Content;
                setOldContentToNull = () => control.Content1 = null;
            }
            else
            {
                oldContentTemplate = control.TemplateContent2;
                newContentTemplate = control.TemplateContent1;
                newContentTemplate.Margin = startMarginForNewContent;
                control.Content1 = control.Content;
                setOldContentToNull = () => control.Content2 = null;
            }

            var zeroMargin = new Thickness(0, 0, 0, 0);
            var newContentAnimation = new ThicknessAnimation(zeroMargin, animationTime);
            var oldContentAnimation = new ThicknessAnimation(endMarginForOldContent, animationTime, FillBehavior.Stop);

            oldContentAnimation.Completed += (sender, args) =>
            {
                setOldContentToNull();
                oldContentTemplate.Margin = zeroMargin;
            };

            newContentTemplate.BeginAnimation(ContentPresenter.MarginProperty, newContentAnimation);
            oldContentTemplate.BeginAnimation(ContentPresenter.MarginProperty, oldContentAnimation);
        }

        private static void SwitchContentWithoutAnimation(SwitchableContentControl control)
        {
            if (control.Content1 != null || control.Content2 == null)
                control.Content1 = control.Content;
            else
                control.Content2 = control.Content;
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

        private static Thickness GetEndMarginForNewContent(Direction direction, SwitchableContentControl control)
        {
            var tempMargins = GetStartMarginForNewContent(direction, control);
            return new Thickness(-tempMargins.Left, -tempMargins.Top, 0, 0);
        }
    }
}
