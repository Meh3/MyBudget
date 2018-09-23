using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DotNetStandardExtensions.ErrorsCheck;

namespace MyBudget.Spreadsheet
{
    /// <summary>
    /// Cell identyfication class. Contains row and column ids.  
    /// </summary>
    public class Cell
    {
        public const string ColumnStringPattern = @"^[a-zA-Z]+$";
        private const int numberOfLettersInAlphabet = 26;
        private const int characterAPosition = 65;

        /// <summary>
        /// Column value.
        /// </summary>
        public string Column { get; }
        /// <summary>
        /// Row value.
        /// </summary>
        public int Row { get; }

        public Cell(string column, int row)
        {
            Column = column.ThrowIfNullOrUnmatched(nameof(column), ColumnStringPattern).ToUpper();
            Row = row.ThrowIfLessOrEqualZero(nameof(row));
        }

        public CellFullSignature ToFullSignature(string spreadsheetId, string sheet) => 
            new CellFullSignature(spreadsheetId, sheet, this);

        public Cell ShiftRow(int numberOfRows) => new Cell(Column, Row + numberOfRows);

        public Cell ShiftColumn(int numberOfColumns)
        {
            var alphabetPosition = AlphabetPosition(Column) + numberOfColumns;
            var column = ColumnNameFromPosition(alphabetPosition);
            return new Cell(column, Row);
        }

        /// <summary>
        /// Returns string representation.
        /// </summary>
        /// <returns>string in format "{Column}{Row}" e.g. "A1".</returns>
        public override string ToString() => $"{Column}{Row}";

        private static int AlphabetPosition(string column)
        {
            int power = column.Count() - 1;
            int position = 0;
            foreach (var letter in column.ToUpper())
            {
                var letterPosition = letter - characterAPosition + 1;
                position += letterPosition * (int)Math.Pow(numberOfLettersInAlphabet, power--);
            }
            return position;
        }

        private static string ColumnNameFromPosition(int alphabetPosition)
        {
            var characters = new List<char>();
            int remainedPosition = alphabetPosition;
            do
            {
                var letterPosition = remainedPosition % numberOfLettersInAlphabet;
                characters.Add((char)(letterPosition + characterAPosition - 1));
                remainedPosition = (remainedPosition - letterPosition) / numberOfLettersInAlphabet;

            } while (remainedPosition > 0);
            characters.Reverse();
            return new string(characters.ToArray());
        }
    }
}
