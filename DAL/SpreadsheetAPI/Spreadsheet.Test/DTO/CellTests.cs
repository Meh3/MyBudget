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
    public class CellTests
    {
        [TestMethod]
        public void CreateCell_Test()
        {
            var column = "A";
            var row = 1;
            var cell = new Cell(column, row);

            Assert.AreEqual(column, cell.Column);
            Assert.AreEqual(row, cell.Row);
            Assert.AreEqual($"{column}{row}", cell.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCellNullColumn_Test()
        {
            var row = 1;
            var cell = new Cell(null, row);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCellEmptyColumn_Test()
        {
            var column = string.Empty;
            var row = 1;
            var cell = new Cell(column, row);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCellInvalidColumn_Test()
        {
            var column = "1";
            var row = 1;
            var cell = new Cell(column, row);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCellZeroRow_Test()
        {
            var column = "A";
            var row = 0;
            var cell = new Cell(column, row);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCellRowLessThanZero_Test()
        {
            var column = "A";
            var row = -1;
            var cell = new Cell(column, row);
        }
    }
}
