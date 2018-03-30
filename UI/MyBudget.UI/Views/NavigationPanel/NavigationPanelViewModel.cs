using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.UI.Common;

namespace MyBudget.UI.Views
{
    public class ToggleItem : NotifiableObject
    {
        public string ButtonText { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                    return;
                SetField(ref isSelected, value);
                if (isSelected)
                    ActionWhenSelected(ButtonText);
            }
        }

        public static Action<string> ActionWhenSelected;
    }
    public class NavigationPanelViewModel : NotifiableObject
    {
        public List<ToggleItem> Views { get; set; }

        private Type selectedView;
        public Type SelectedView
        {
            get => selectedView;
            set => SetField(ref selectedView, value);
        }

        private Dictionary<string, Type> viewTypes = new Dictionary<string, Type>
        {
            { "Current Month", typeof(CurrentMonthView) },
            { "Add Transaction", typeof(AddTransactionView) },
        };

        public NavigationPanelViewModel()
        {
            ToggleItem.ActionWhenSelected += SwtichView;

            Views = new List<ToggleItem>
            {
                new ToggleItem() { ButtonText="Current Month" },
                new ToggleItem() { ButtonText="Add Transaction" }
            };
        }

        private void SwtichView(string viewButtonText)
        {
            SelectedView = viewTypes[viewButtonText];
        }
    }
}
