using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.UI.Common;

namespace MyBudget.UI.Views
{
    public class StatisticsViewModel : NotifiableObject
    {
        private GaugeData gaugeValues;
        public GaugeData GaugeValues
        {
            get => gaugeValues;
            set => SetField(ref gaugeValues, value);
        }

        public StatisticsViewModel()
        {
            GaugeValues = new GaugeData(3167.45, 4000);
        }
    }
}
