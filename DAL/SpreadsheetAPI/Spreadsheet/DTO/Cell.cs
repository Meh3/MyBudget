using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DotNetStandardExtensions.Arguments;

namespace MyBudget.Spreadsheet
{
    public class Cell
    {
        public string Column { get; }
        public int Row { get; }

        public Cell(string column, int row)
        {
            Column = column.ThrowIfNullOrUnmatched(nameof(column), @"^[a-zA-Z]+$");
            Row = row.ThrowIfLessOrEqualZero(nameof(row));
        }

        public override string ToString() => $"{Column}{Row}";
    }
}
