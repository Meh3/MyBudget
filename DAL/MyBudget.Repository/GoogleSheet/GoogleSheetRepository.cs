using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    internal class GoogleSheetRepository : IBudgetRepository
    {
        private readonly StorageSchema schema;

        public GoogleSheetRepository(StorageSchema schema)
        {
            this.schema = schema;
        }

        public IEnumerable<TransactionModel> GetMonthTransactions(Month month, int year)
        {
            throw new NotImplementedException();
        }
    }
}
