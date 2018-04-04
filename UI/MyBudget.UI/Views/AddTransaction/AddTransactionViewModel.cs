using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.UI.Common;

namespace MyBudget.UI.Views
{
    public class AddTransactionViewModel : NotifiableObject
    {
        private List<PieData> pieValues;
        public List<PieData> PieValues
        {
            get => pieValues;
            set => SetField(ref pieValues, value);
        }

        public AddTransactionViewModel()
        {
            PieValues = new List<PieData>
            {
                new PieData("Cat1", 90),
                new PieData("Cat2", 90),
                new PieData("Cat3", 90)
            };
        }
    }
}
