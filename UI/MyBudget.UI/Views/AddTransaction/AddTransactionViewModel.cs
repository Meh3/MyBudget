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
                new PieData("Categor1", 11),
                new PieData("Categor2", 56),
                new PieData("Categor3", 114),
                new PieData("Categor4", 30),
            };
        }
    }
}
