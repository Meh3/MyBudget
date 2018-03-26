using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace MyBudget.UI.Common
{
    /// <summary>
    /// Base class for attached behavior preventing memory leaks.
    /// </summary>
    /// <typeparam name="T">FramewrokElement type</typeparam>
    public abstract class BehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        private bool isSetup = false;
        private bool isHookedUp;
        private WeakReference weakTarget;

        protected virtual void OnSetup() { }
        protected virtual void OnCleanup() { }
        protected override void OnChanged()
        {
            if (AssociatedObject != null)
            {
                HookupBehavior(AssociatedObject);
            }
            else
            {
                UnHookupBehavior();
            }
        }

        private void OnTarget_Loaded(object sender, RoutedEventArgs e) => SetupBehavior();

        private void OnTarget_Unloaded(object sender, RoutedEventArgs e) => CleanupBehavior();

        private void HookupBehavior(T target)
        {
            if (isHookedUp)
                return;
            weakTarget = new WeakReference(target);
            isHookedUp = true;
            target.Unloaded += OnTarget_Unloaded;
            target.Loaded += OnTarget_Loaded;
            SetupBehavior();
        }

        private void UnHookupBehavior()
        {
            if (!isHookedUp)
                return;
            isHookedUp = false;
            var target = AssociatedObject ?? (T)weakTarget.Target;
            if (target != null)
            {
                target.Unloaded -= OnTarget_Unloaded;
                target.Loaded -= OnTarget_Loaded;
            }
            CleanupBehavior();
        }

        private void SetupBehavior()
        {
            if (isSetup)
                return;
            isSetup = true;
            OnSetup();
        }

        private void CleanupBehavior()
        {
            if (!isSetup)
                return;
            isSetup = false;
            OnCleanup();
        }
    }
}
