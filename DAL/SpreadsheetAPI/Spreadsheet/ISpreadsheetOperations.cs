using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace MyBudget.Spreadsheet
{
    public interface ISpreadsheetOperations
    {
        string GetData(CellFullSignature cellID, Cell cellTo = null);
        Task<string> GetDataAsync(CellFullSignature cellID, Cell cellTo = null);
        ImmutableArray<ImmutableArray<string>> GetDataToLastWrittenRow(CellFullSignature cellID, string columnTo);
        Task<ImmutableArray<ImmutableArray<string>>> GetDataToLastWrittenRowAsync(CellFullSignature cellID, string columnTo);
        ImmutableArray<ImmutableArray<string>> GetDataToLastWrittenColumn(CellFullSignature cellID, int rowTo);
        Task<ImmutableArray<ImmutableArray<string>>> GetDataToLastWrittenColumnAsync(CellFullSignature cellID, int rowTo);
    }
}
