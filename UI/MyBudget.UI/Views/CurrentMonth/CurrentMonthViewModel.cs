using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.UI.Common;

namespace MyBudget.UI.Views
{
    public class CurrentMonthViewModel : NotifiableObject
    {
        private List<ColumnData> columns;
        public List<ColumnData> Columns
        {
            get => columns;
            set => SetField(ref columns, value);
        }

        public CurrentMonthViewModel()
        {
            Columns = new List<ColumnData>
            {
                new ColumnData("Category1", 500),
                new ColumnData("Category2", 2000),
                new ColumnData("Category3", 3000),
                new ColumnData("Category4", 1000),
                new ColumnData("Category5", 2500)
            };
        }
    }
}
