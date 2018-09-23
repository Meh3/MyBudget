using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetStandardExtensions.ErrorsCheck;

namespace MyBudget.Spreadsheet
{
    internal abstract class SheetRange<T> : CellFullSignature
    {
        public T To { get; }

        protected SheetRange(CellFullSignature fromCell, T to)
            : base(fromCell.SpreadsheetId, fromCell.Sheet, fromCell.Column, fromCell.Row)
        {
            To = to;
        }

        public override string ToString() => $"{base.ToString()}:{To}";
    }

    internal class CellRange: SheetRange<Cell>
    {
        public CellRange(CellFullSignature fromCell, Cell toCell) :
            base(fromCell.ThrowIfNull(nameof(fromCell)), toCell.ThrowIfNull(nameof(toCell)))
        { }
    }

    internal class ColumnRange : SheetRange<string>
    {
        public ColumnRange(CellFullSignature fromCell, string column) :
            base(fromCell.ThrowIfNull(nameof(fromCell)), column.ThrowIfNullOrUnmatched(nameof(column), Cell.ColumnStringPattern))
        { }
    }

    internal class RowRange : SheetRange<int>
    {
        public RowRange(CellFullSignature fromCell, int row) :
            base(fromCell.ThrowIfNull(nameof(fromCell)), row.ThrowIfLessOrEqualZero(nameof(row)))
        { }
    }
}
