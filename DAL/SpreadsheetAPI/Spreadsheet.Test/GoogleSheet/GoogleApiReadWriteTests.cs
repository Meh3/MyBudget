using System;
using System.Linq;
using System.Text;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBudget.GoogleSheet.Test
{
    public static class TestExtensions
    {
        public static string ToSingleString(this IList<IList<Object>> list) =>
            string.Join(Environment.NewLine, list.Select(row => string.Join(" | ", row.Select(cell => cell.ToString()))));

        public static string ToSingleString(this ImmutableArray<ImmutableArray<string>> array) =>
            string.Join(Environment.NewLine, array.Select(row => string.Join(" | ", row.Select(cell => cell.ToString()))));

        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int millisecondsTimeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
                await task;

            throw new TimeoutException();
        }
    }

    [TestClass]
    public class GoogleApiReadWriteTests : GoogleApiTestsBase
    {
        private SheetsService _service;
        private SheetsService service => _service = _service ?? GoogleApiSheetServiceTests.CreateSheetService(GoogleApiCredentialTests.Connect(CredentialPath));

        [TestMethod]
        public void ReadValueFromCell_Test()
        {
            var range = "Test!A1";
            var expectedValue = "test";

            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var actualValue = request.Execute().Values[0][0];

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ReadColumnValuesToLastWrittenRow_Test()
        {
            var range = "Test!B1:C";
            var expectedValue = string.Join(Environment.NewLine, "1 | k", " | 2", "b", "c | l", "", "d", "", "e", "f", "g", "h", "i", "j");

            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var actualValue = request.Execute().Values.ToSingleString();

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ReadRowsValuesToLastWrittenColmun_Test()
        {
            var range = "Test!D18:19";
            var expectedValue = string.Join(Environment.NewLine, "1 | 2 | 3 |  |  |  | a", "b | c");

            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var actualValue = request.Execute().Values.ToSingleString();

            Assert.AreEqual(actualValue, expectedValue);
        }
    }
}
