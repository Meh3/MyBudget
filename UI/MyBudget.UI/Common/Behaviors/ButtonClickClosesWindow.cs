using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyBudget.UI.Behaviors
{
    public class ButtonClickClosesWindow : BehaviorBase<Button>
    {
        protected override void OnSetup()
        {
            AssociatedObject.Click += AssociatedObject_Click; ;
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Command?.Execute(AssociatedObject.CommandParameter);
            var parentWindow = Window.GetWindow(AssociatedObject);
            parentWindow.Close();
        }

        protected override void OnCleanup()
        {
            AssociatedObject.Click -= AssociatedObject_Click;
        }
    }
}
