using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.Spreadsheet;
using System.Collections.Immutable;

namespace MyBudget.Repository
{
    internal class SpreadsheetRepository : IBudgetRepository
    {
        private const string VERTICAL = "Vertical";
        private const string TRANSACTIONS = "Transactions";
        private const string CATEGORIES = "Categories";
        private const string INITIALSTATE = "InitialState";

        private readonly ISpreadsheetOperations spreadSheetApi;
        private readonly StorageSchema schema;
        private readonly string spreadsheetId;
        private readonly Table transactionSchema;

        public SpreadsheetRepository(ISpreadsheetOperations spreadSheetApi, StorageSchema schema)
        {
            this.spreadSheetApi = spreadSheetApi;
            this.schema = schema;
            spreadsheetId = schema.Spreadsheet;
            transactionSchema = schema.GetTable(TRANSACTIONS);
        }

        public ImmutableArray<TransactionModel> GetMonthTransactions(Month month, int year)
        {
            var fromCell = transactionSchema.Headers.First().Cell
                .ToFullSignature(spreadsheetId, transactionSchema.Sheet);
            var toColumn = transactionSchema.Headers.Last().Column;
            var 

        }

        private DateTime[] GetAllTransactionsDates()
        {
            var transactionModel = new TransactionModel();
            var dateHeaderName = nameof(transactionModel.Date);
            var sheet = transactionSchema.Sheet;
            var fromCell = transactionSchema.GetHeader(dateHeaderName).Cell.ToFullSignature(spreadsheetId, sheet);
            return GetDataToLastWritten(fromCell, transactionSchema.Orientation, DateTime.Parse);
        }

        private static CellFullSignature GetFirstTransactionDateCell()
        {
            var transactionModel = new TransactionModel();
            var dateHeaderName = nameof(transactionModel.Date);
            var cell = transactionSchema.GetHeader(dateHeaderName).Cell;

            transactionSchema.Orientation == VERTICAL
                    ? cell.Change(row: cell.Row + 1)
                    : cell.Change(column cell.Column)


        }

        private static TResult[] GetDataToLastWritten<TResult>(
            ISpreadsheetOperations operations,
            CellFullSignature cellId, 
            string orientation, 
            Func<string, TResult> parse)
        {
            var stringResults = orientation == VERTICAL
                ? operations.GetDataToLastWrittenRow(cellId, cellId.Column)
                : operations.GetDataToLastWrittenColumn(cellId, cellId.Row);
            return ConvertToOneDimension(stringResults, orientation, parse);
        }

        private static async Task<TResult[]> GetDataToLastWrittenAsync<TResult>(
            ISpreadsheetOperations operations, 
            CellFullSignature cellId, 
            string orientation, 
            Func<string, TResult> parse)
        {
            var stringResults = await (orientation == VERTICAL
                ? operations.GetDataToLastWrittenRowAsync(cellId, cellId.Column)
                : operations.GetDataToLastWrittenColumnAsync(cellId, cellId.Row));

            return ConvertToOneDimension(stringResults, orientation, parse);
        }

        private static TResult[] ConvertToOneDimension<TResult>(
            ImmutableArray<ImmutableArray<string>> stringResults, 
            string  orientation, 
            Func<string, TResult> parse) =>
                (orientation == VERTICAL
                    ? stringResults.Select(row => parse(row.First()))
                    : stringResults.First().Select(x => parse(x)))
                    .ToArray();
    }
}
