using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.Spreadsheet;

namespace MyBudget.Repository
{
    internal class SpreadsheetRepository : IBudgetRepository
    {
        private readonly ISpreadsheetOperations googleSheet;
        private readonly StorageSchema schema;

        public SpreadsheetRepository(ISpreadsheetOperations googleSheet, StorageSchema schema)
        {
            this.googleSheet = googleSheet;
            this.schema = schema;
        }

        public IEnumerable<TransactionModel> GetMonthTransactions(Month month, int year)
        {
            throw new NotImplementedException();
        }
    }
}
