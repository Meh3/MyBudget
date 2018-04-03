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

        private GaugeData gaugeValues;
        public GaugeData GaugeValues
        {
            get => gaugeValues;
            set => SetField(ref gaugeValues, value);
        }

        public CurrentMonthViewModel()
        {
            Columns = new List<ColumnData>
            {
                new ColumnData("Cat 1", 20),
                new ColumnData("Cat 2", 40)
            };

            GaugeValues = new GaugeData(3000, 4000);
        }
    }
}
