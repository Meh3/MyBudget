using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.Spreadsheet.GoogleSheet;

namespace MyBudget.Spreadsheet
{
    public static class SpreadsheetFactory
    {
        public static ISpreadsheetOperations CreateGoogleSheetAPI(string clientIdFile, string credentialFileName, string applicationName) =>
            new GoogleSheetAPI(clientIdFile, credentialFileName, applicationName);
    }
}
