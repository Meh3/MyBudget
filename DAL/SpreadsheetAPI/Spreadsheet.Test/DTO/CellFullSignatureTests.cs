using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBudget.Spreadsheet;

namespace MyBudget.Spreadsheet.Test
{
    [TestClass]
    public class CellFullSignatureTests
    {
        [TestMethod]
        public void CreateCellFullSignature_Test()
        {
            var spreadsheetId = "spreadsheetId";
            var sheet = "sheet";
            var column = "A";
            var row = 1;
            var cell = new Cell("B", 1);

            var fullCell = new CellFullSignature(spreadsheetId, sheet, column, row);

            Assert.AreEqual(spreadsheetId, fullCell.SpreadsheetId);
            Assert.AreEqual(sheet, fullCell.Sheet);
            Assert.AreEqual(column, fullCell.Column);
            Assert.AreEqual(row, fullCell.Row);
            Assert.AreEqual($"{sheet}!{column}{row}", fullCell.ToString());
            Assert.AreEqual($"{sheet}!{column}{row}:{cell.Column}{cell.Row}", fullCell.ToString(cell));

            fullCell = new CellFullSignature(spreadsheetId, sheet, cell);
            Assert.AreEqual(spreadsheetId, fullCell.SpreadsheetId);
            Assert.AreEqual(sheet, fullCell.Sheet);
            Assert.AreEqual(cell.Column, fullCell.Column);
            Assert.AreEqual(cell.Row, fullCell.Row);
        }
    }
}
