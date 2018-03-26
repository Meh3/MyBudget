using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyBudget.UI.Common
{
    public class ThumbResizesWindowBehavior : BehaviorBase<Thumb>
    {
        protected override void OnSetup()
        {
            AssociatedObject.DragDelta += AssociatedObject_DragDelta;
            AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.DragDelta -= AssociatedObject_DragDelta;
            AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
        }

        private void AssociatedObject_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var deltaX = e.HorizontalChange;
            var deltaY = e.VerticalChange;
            var parentWindow = Window.GetWindow(AssociatedObject);

            if (parentWindow.Width + deltaX >= parentWindow.MinWidth)
                parentWindow.Width += deltaX;
            if (parentWindow.Height + deltaY >= parentWindow.MinHeight)
                parentWindow.Height += deltaY;
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e) => 
            Mouse.OverrideCursor = null;

        private void AssociatedObject_MouseEnter(object sender, MouseEventArgs e) => 
            Mouse.OverrideCursor = Cursors.SizeNWSE;
    }
}
