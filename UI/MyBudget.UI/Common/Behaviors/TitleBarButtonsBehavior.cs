using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyBudget.UI.Common
{
    public enum WindowBehavior { Maximize, Minimize, Close }

    internal static class WindowExtensions
    {
        public static void SetState(this MainWindow window, WindowBehavior behavior)
        {
            if (behavior == WindowBehavior.Close)
                return;

            window.WindowState = behavior == WindowBehavior.Maximize
                ? WindowState.Maximized
                : WindowState.Minimized;
        }
    }

    public class TitleBarButtonsBehavior : BehaviorBase<Button>
    {
        public static readonly DependencyProperty WindowBehaviorProperty = DependencyProperty.Register(
            "WindowBehavior", typeof(WindowBehavior), typeof(TitleBarButtonsBehavior), new FrameworkPropertyMetadata(WindowBehavior.Close));

        public WindowBehavior WindowBehavior
        {
            get { return (WindowBehavior)GetValue(WindowBehaviorProperty); }
            set { SetValue(WindowBehaviorProperty, value); }
        }

        protected override void OnSetup()
        {
            AssociatedObject.Click += AssociatedObject_Click; ;
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Command?.Execute(AssociatedObject.CommandParameter);
            var window = Window.GetWindow(AssociatedObject) as MainWindow;
            var behavior = WindowBehavior;
            var state = window.WindowState;

            if (behavior == WindowBehavior.Close)
            {
                window.Close();
                return;
            }

            if (state == WindowState.Normal)
            {
                window.SetState(behavior);
            }
            else // only maximized possible
            {
                if (behavior == WindowBehavior.Maximize)
                    window.WindowState = WindowState.Normal;
                else
                    window.WindowState = WindowState.Minimized;
            }
        }

        protected override void OnCleanup()
        {
            AssociatedObject.Click -= AssociatedObject_Click;
        }
    }
}
