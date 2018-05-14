using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetStandardExtensions.Arguments;

namespace MyBudget.Spreadsheet
{
    public class CellFullSignature : Cell
    {
        public string SpreadsheetId { get; }
        public string Sheet { get; }

        public CellFullSignature(string spreadsheetId, string sheet, string column, int row) : base(column, row)
        {
            SpreadsheetId = spreadsheetId.ThrowIfNullOrWhite(nameof(spreadsheetId));
            Sheet = sheet.ThrowIfNullOrEmpty(nameof(sheet));
        }

        public CellFullSignature(string spreadsheetId, string sheet, Cell cell) : 
            this(spreadsheetId, sheet, cell.ThrowIfNull(nameof(cell)).Column, cell.Row) { }

        public string ToString(object to) => $"{ToString()}:{to.ThrowIfNull(nameof(to)).ToString()}";
        public override string ToString() => $"{Sheet}!{Column}{Row}";
    }
}
