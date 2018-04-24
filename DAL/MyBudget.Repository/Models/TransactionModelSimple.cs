using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public class TransactionModelSimple
    {
        public DateTime Date { get; private set; }
        public double Amount { get; private set; }
        public string Description { get; private set; }

        public TransactionModelSimple(DateTime date = default(DateTime), double amount = 0.0, string description = null)
        {
            Date = date;
            Amount = amount;
            Description = description;
        }
    }
}
