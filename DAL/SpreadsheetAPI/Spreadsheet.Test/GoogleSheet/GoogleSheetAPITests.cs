using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBudget.Spreadsheet.GoogleSheet;
using MyBudget.Spreadsheet;
using System.Threading.Tasks;

namespace MyBudget.GoogleSheet.Test
{
    [TestClass]
    public class GoogleSheetAPITests : GoogleApiTestsBase
    {
        private ISpreadsheetOperations _operations;
        private ISpreadsheetOperations operations => _operations = _operations ?? new GoogleSheetAPI(ClientIdFileName, CredentialPath, ApplicationName);
         
        [TestMethod]
        public void GetDataFromSingleCell_Test()
        {
            var expectedValue = "test";
            var cell = new CellFullSignature(SpreadsheetId, "Test", "A", 1);

            var actualValue = operations.GetData(cell);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataFromSingleCellThrowNullException_Test()
        {
            var expectedValue = "test";

            var actualValue = operations.GetData(null);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public async Task GetDataFromSingleCellAsync_Test()
        {
            var expectedValue = "test";
            var cell = new CellFullSignature(SpreadsheetId, "Test", "A", 1);

            var actualValue = await operations.GetDataAsync(cell);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetDataFromSingleCellAsyncThrowNullException_Test()
        {
            var expectedValue = "test";

            var actualValue = await operations.GetDataAsync(null);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetDataFromRange_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);
            var toCell = new Cell("C", 13);

            var actualValue = operations.GetData(fromCell, toCell);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        public void GetDataFromRevertedRange_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "C", 13);
            var toCell = new Cell("B", 1);

            var actualValue = operations.GetData(fromCell, toCell);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataFromRangeThrowNullException_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);

            var actualValue = operations.GetData(fromCell, null);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        public async Task GetDataFromRangeAsync_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);
            var toCell = new Cell("C", 13);

            var actualValue = await operations.GetDataAsync(fromCell, toCell);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetDataFromRangeAsyncThrowNullException_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);

            var actualValue = await operations.GetDataAsync(fromCell, null);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        public void GetDataToLastWrittenRow_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);
            var toColumn = "C";

            var actualValue = operations.GetDataToLastWrittenRow(fromCell, toColumn);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataToLastWrittenRowNullColumn_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);

            var actualValue = operations.GetDataToLastWrittenRow(fromCell, null);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataToLastWrittenRowInvalidColumn_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);
            var toColumn = "3";

            var actualValue = operations.GetDataToLastWrittenRow(fromCell, toColumn);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        public async Task GetDataToLastWrittenRowAsync_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);
            var toColumn = "C";

            var actualValue = await operations.GetDataToLastWrittenRowAsync(fromCell, toColumn);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetDataToLastWrittenRowAsyncInvalidColumn_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "B", 1);
            var toColumn = "2";

            var actualValue = await operations.GetDataToLastWrittenRowAsync(fromCell, toColumn);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        public void GetDataToLastWrittenColumn_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | 2 | 3 |  |  |  | a", "b | c");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "D", 18);
            var toRow = 19;

            var actualValue = operations.GetDataToLastWrittenColumn(fromCell, toRow);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDataToLastWrittenColumnInvalidRow_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | 2 | 3 |  |  |  | a", "b | c");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "D", 18);
            var toRow = 0;

            var actualValue = operations.GetDataToLastWrittenColumn(fromCell, toRow);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        public async Task GetDataToLastWrittenColumnAsync_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | 2 | 3 |  |  |  | a", "b | c");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "D", 18);
            var toRow = 19;

            var actualValue = await operations.GetDataToLastWrittenColumnAsync(fromCell, toRow);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetDataToLastWrittenColumnAsyncInvalidRow_Test()
        {
            var expectedValue = string.Join(Environment.NewLine, "1 | 2 | 3 |  |  |  | a", "b | c");
            var fromCell = new CellFullSignature(SpreadsheetId, "Test", "D", 18);
            var toRow = -1;

            var actualValue = await operations.GetDataToLastWrittenColumnAsync(fromCell, toRow);
            Assert.AreEqual(expectedValue, actualValue.ToSingleString());
        }
    }
}
