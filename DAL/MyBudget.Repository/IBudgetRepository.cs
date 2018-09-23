using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace MyBudget.Repository
{
    public interface IBudgetRepository
    {
        ImmutableArray<TransactionModel> GetMonthTransactions(Month month, int year);
    }
}
