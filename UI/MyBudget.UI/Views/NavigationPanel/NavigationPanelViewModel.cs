using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.UI.Common;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyBudget.UI.Views
{
    public class NavigationPanelViewModel : NotifiableObject
    {
        public ObservableCollection<UIDataItem<Type>> ViewItems { get; set; }

        private Type selectedViewItem;
        public Type SelectedViewItem
        {
            get => selectedViewItem;
            set => SetField(ref selectedViewItem, value);
        }

        ICommand selectViewCommand;
        public ICommand SelectViewCommand => selectViewCommand ?? (selectViewCommand = new RelayCommand(ExecuteSelectView));


        public NavigationPanelViewModel(IDictionary<string, Type> viewTypes) =>
            ViewItems = viewTypes
                .Select(x => new UIDataItem<Type> { PrimaryText = x.Key.GetResourceText(), Data = x.Value })
                .ToObservableCollection();

        private void ExecuteSelectView(object param)
        {
            if (!(param is Type type))
                return;
            SelectedViewItem = type;
        }
    }
}
