using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public interface IBudgetRepository
    {
        IEnumerable<TransactionModel> GetMonthTransactions(Month month, int year);
    }
}
