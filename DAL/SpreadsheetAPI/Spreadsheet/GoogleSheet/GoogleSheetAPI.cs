using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Nito.AsyncEx.Synchronous;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Collections.Immutable;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using DotNetStandardExtensions.ErrorsCheck;
using Range = System.Collections.Immutable.ImmutableArray<System.Collections.Immutable.ImmutableArray<string>>;

namespace MyBudget.Spreadsheet.GoogleSheet
{
    internal class GoogleSheetAPI : ISpreadsheetOperations
    {
        private const string credentialFilebbbname = ".credentials/sheets.googleapis.com-dotnet-MyBydget.json";
        private const string ApplicationnnnName = "Google Sheets API";

        private readonly string[] credentialScope = { SheetsService.Scope.Spreadsheets };
        private readonly Task<SheetsService> sheetServiceTask;

        /// <summary>
        /// Create googlesheet api for basic sheet operations.
        /// </summary>
        /// <param name="clientIdFile">Path where clien id file for authorization is located.</param>
        /// <param name="credentialFileName">Location where created credential file will be stored.</param>
        /// <param name="applicationName">Google application name</param>
        public GoogleSheetAPI(string clientIdFile, string credentialFileName, string applicationName)
        {
            clientIdFile.ThrowIfFileNotExist(nameof(clientIdFile));
            credentialFileName.ThrowIfNotFilePath(nameof(credentialFileName));
            applicationName.ThrowIfNullOrEmpty(nameof(applicationName));

            sheetServiceTask = InitializeSheetServiceAsync(clientIdFile, credentialFileName, applicationName, credentialScope);
        }

        public string GetData(CellFullSignature cellID, Cell cellTo = null)
        {
            cellID.ThrowIfNull(nameof(cellID));
            var cellsToRead = cellTo == null ? cellID : new CellRange(cellID, cellTo);
            var rangeValue = ReadDataFromService(cellsToRead);
            return ConvertToSingleValue(rangeValue);
        }

        public async Task<string> GetDataAsync(CellFullSignature cellID, Cell cellTo = null)
        {
            cellID.ThrowIfNull(nameof(cellID));
            var cellsToRead = cellTo == null ? cellID : new CellRange(cellID, cellTo);
            var rangeValue = await ReadDataFromServiceAsync(cellsToRead);
            return ConvertToSingleValue(rangeValue);
        }

        public Range GetDataToLastWrittenRow(CellFullSignature cellID, string columnTo)
        {
            cellID.ThrowIfNull(nameof(cellID));
            var cellRange = new ColumnRange(cellID, columnTo);
            var rangeValue = ReadDataFromService(cellRange);
            return ConvertToArrays(rangeValue);
        }

        public async Task<Range> GetDataToLastWrittenRowAsync(CellFullSignature cellID, string columnTo)
        {
            cellID.ThrowIfNull(nameof(cellID));
            var cellRange = new ColumnRange(cellID, columnTo);
            var rangeValue = await ReadDataFromServiceAsync(cellRange);
            return ConvertToArrays(rangeValue);
        }

        public Range GetDataToLastWrittenColumn(CellFullSignature cellID, int rowTo)
        {
            cellID.ThrowIfNull(nameof(cellID));
            var cellRange = new RowRange(cellID, rowTo);
            var rangeValue = ReadDataFromService(cellRange);
            return ConvertToArrays(rangeValue);
        }

        public async Task<Range> GetDataToLastWrittenColumnAsync(CellFullSignature cellID, int rowTo)
        {
            cellID.ThrowIfNull(nameof(cellID));
            var cellRange = new RowRange(cellID, rowTo);
            var rangeValue = await ReadDataFromServiceAsync(cellRange);
            return ConvertToArrays(rangeValue);
        }

        private static async Task<SheetsService> InitializeSheetServiceAsync(string clientIdFile, string credentialFileName, 
            string applicationName, string[] scopes)
        {
            UserCredential userCredential;
            using (var stream = new FileStream(clientIdFile, FileMode.Open, FileAccess.Read))
            {
                userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialFileName, true));
            }

            return new SheetsService(new BaseClientService.Initializer()  
            {
                HttpClientInitializer = userCredential,
                ApplicationName = applicationName,
            });
        }

        private ValueRange ReadDataFromService(CellFullSignature cellId) =>
            sheetServiceTask.WaitAndUnwrapException()
                .Spreadsheets.Values.Get(cellId.SpreadsheetId, cellId.ToString())
                .Execute();

        private async Task<ValueRange> ReadDataFromServiceAsync(CellFullSignature cellId)
        {
            var sheetService = await sheetServiceTask;
            return await sheetService.Spreadsheets.Values
                .Get(cellId.SpreadsheetId, cellId.ToString())
                .ExecuteAsync();
        }

        private static string ConvertToSingleValue(ValueRange valueRange) =>
            valueRange.Values[0][0].ToString();

        private static Range ConvertToArrays(ValueRange valueRange) =>
            valueRange.Values
                .Select(row => row.Cast<string>().ToImmutableArray())
                .ToImmutableArray();       
    }
}
