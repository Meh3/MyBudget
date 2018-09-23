using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetStandardExtensions.ErrorsCheck;

namespace MyBudget.Spreadsheet
{
    /// <summary>
    /// Cell full identyfication class. Contains: row, column, sheet and spreadsheet ids.
    /// </summary>
    public class CellFullSignature : Cell
    {
        /// <summary>
        /// Spreadsheet id value.
        /// </summary>
        public string SpreadsheetId { get; }
        /// <summary>
        /// Sheet value.
        /// </summary>
        public string Sheet { get; }

        public CellFullSignature(string spreadsheetId, string sheet, string column, int row) : base(column, row)
        {
            SpreadsheetId = spreadsheetId.ThrowIfNullOrWhite(nameof(spreadsheetId));
            Sheet = sheet.ThrowIfNullOrEmpty(nameof(sheet));
        }

        public CellFullSignature(string spreadsheetId, string sheet, Cell cell) : 
            this(spreadsheetId, sheet, cell.ThrowIfNull(nameof(cell)).Column, cell.Row) { }
            
        /// <summary>
        /// Returns string representation.
        /// </summary>
        /// <returns>string in format "{Sheet}!{Column}{Row}" e.g. "DataSheet!A1".</returns>
        public override string ToString() => $"{Sheet}!{Column}{Row}";
    }
}
