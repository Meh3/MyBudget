using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyBudget.UI.Common
{
    public class ControlDragsWindowBehavior : BehaviorBase<FrameworkElement>
    {
        protected override void OnSetup() => AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        protected override void OnCleanup() => AssociatedObject.MouseDown -= AssociatedObject_MouseDown;

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;
            var parentWindow = Window.GetWindow(AssociatedObject);
            parentWindow.DragMove();
        }
    }
}
