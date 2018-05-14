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
        string GetData(CellFullSignature cellID);
        Task<string> GetDataAsync(CellFullSignature cellID);
        ImmutableArray<ImmutableArray<string>> GetData(CellFullSignature cellID, Cell to);
        Task<ImmutableArray<ImmutableArray<string>>> GetDataAsync(CellFullSignature cellID, Cell to);
        ImmutableArray<ImmutableArray<string>> GetDataToLastWrittenRow(CellFullSignature cellID, string columnTo);
        Task<ImmutableArray<ImmutableArray<string>>> GetDataToLastWrittenRowAsync(CellFullSignature cellID, string columnTo);
        ImmutableArray<ImmutableArray<string>> GetDataToLastWrittenColumn(CellFullSignature cellID, int rowTo);
        Task<ImmutableArray<ImmutableArray<string>>> GetDataToLastWrittenColumnAsync(CellFullSignature cellID, int rowTo);
    }
}
