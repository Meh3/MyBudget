using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyBudget.UI.Behaviors
{
    public class ControlDragsWindow : BehaviorBase<FrameworkElement>
    {
        protected override void OnSetup()
        {
            AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var parentWindow = Window.GetWindow(AssociatedObject);
            parentWindow.DragMove();
        }

        protected override void OnCleanup()
        {
            AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
        }
    }
}
