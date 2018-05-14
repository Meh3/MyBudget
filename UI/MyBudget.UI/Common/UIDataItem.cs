using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Common
{
    public class UIDataItem : NotifiableObject
    {
        private string primaryText;
        public string PrimaryText
        {
            get => primaryText;
            set => SetField(ref primaryText, value);
        }

        private string secondaryText;
        public string SecondaryText
        {
            get => secondaryText;
            set => SetField(ref secondaryText, value);
        }
    }

    public class UIDataItem<T> : UIDataItem
    {
        private T data;
        public T Data
        {
            get => data;
            set => SetField(ref data, value);
        }
    }
}
