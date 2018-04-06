using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Common
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col) =>
            new ObservableCollection<T>(col);

        public static string GetResourceText(this string key) => StringResource.GetString(key);
    }
}
