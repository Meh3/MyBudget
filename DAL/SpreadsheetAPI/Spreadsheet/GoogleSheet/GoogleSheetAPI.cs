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
using System.Text.RegularExpressions;
using DotNetStandardExtensions.Arguments;

namespace MyBudget.Spreadsheet.GoogleSheet
{
    public class GoogleSheetAPI : ISpreadsheetOperations
    {
        private const string credentialFilename = ".credentials/sheets.googleapis.com-dotnet-MyBydget.json";
        private const string ApplicationName = "Google Sheets API";

        private readonly string[] credentialScope = { SheetsService.Scope.Spreadsheets };
        private readonly Task<SheetsService> sheetServiceTask;

        /// <summary>
        /// Create googlesheet api for basic sheet operations.
        /// </summary>
        /// <param name="clientIdFile">Path where clien id file is located.</param>
        /// <param name="credentialFileName">Location where created credential file will be stored.</param>
        /// <param name="applicationName">Google application name</param>
        public GoogleSheetAPI(string clientIdFile, string credentialFileName, string applicationName) =>
            sheetServiceTask = InitializeSheetServiceAsync(clientIdFile, credentialFileName, applicationName, credentialScope);

        public string GetData(CellFullSignature cellID) =>
            GetData<string, Cell>(sheetServiceTask, cellID, null, ConvertToSingleValue);

        public async Task<string> GetDataAsync(CellFullSignature cellID) =>
            await GetDataAsync<string, Cell>(sheetServiceTask, cellID, null, ConvertToSingleValue);

        public ImmutableArray<ImmutableArray<string>> GetData(CellFullSignature cellID, Cell to) =>
            GetData(sheetServiceTask, cellID, to, ConvertToArrays, x => x.ThrowIfNull(nameof(to)));

        public async Task<ImmutableArray<ImmutableArray<string>>> GetDataAsync(CellFullSignature cellID, Cell to) =>
            await GetDataAsync(sheetServiceTask, cellID, to, ConvertToArrays, x => x.ThrowIfNull(nameof(to)));

        public ImmutableArray<ImmutableArray<string>> GetDataToLastWrittenRow(CellFullSignature cellID, string columnTo) =>
            GetData(sheetServiceTask, cellID, columnTo, ConvertToArrays, x => x.ThrowIfNullOrUnmatched(nameof(columnTo), @"^[a-zA-Z]+$"));

        public async Task<ImmutableArray<ImmutableArray<string>>> GetDataToLastWrittenRowAsync(CellFullSignature cellID, string columnTo) =>
            await GetDataAsync(sheetServiceTask, cellID, columnTo, ConvertToArrays, x => x.ThrowIfNullOrUnmatched(nameof(columnTo), @"^[a-zA-Z]+$"));

        public ImmutableArray<ImmutableArray<string>> GetDataToLastWrittenColumn(CellFullSignature cellID, int rowTo) =>
            GetData(sheetServiceTask, cellID, rowTo, ConvertToArrays, x => x.ThrowIfLessOrEqualZero(nameof(rowTo)));

        public async Task<ImmutableArray<ImmutableArray<string>>> GetDataToLastWrittenColumnAsync(CellFullSignature cellID, int rowTo) =>
            await GetDataAsync(sheetServiceTask, cellID, rowTo, ConvertToArrays, x => x.ThrowIfLessOrEqualZero(nameof(rowTo)));

        private static async Task<SheetsService> InitializeSheetServiceAsync(string clientIdFile, string credentialFileName, string applicationName, string[] scopes)
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

        private static TResult GetData<TResult, Tto>(Task<SheetsService> service, CellFullSignature cellID, Tto to, Func<ValueRange, TResult> convertResult, Func<Tto, Tto> checkToArg = null)
        {
            cellID.ThrowIfNull(nameof(cellID));
            checkToArg?.Invoke(to);
            return ReadDataFromService(service, cellID, to, convertResult);
        }

        private static async Task<TResult> GetDataAsync<TResult, Tto>(Task<SheetsService> service, CellFullSignature cellID, Tto to, Func<ValueRange, TResult> convertResult, Func<Tto, Tto> checkTo = null)
        {
            cellID.ThrowIfNull(nameof(cellID));
            checkTo?.Invoke(to);
            return await ReadDataFromServiceAsync(service, cellID, to, convertResult);
        }

        private static TResult ReadDataFromService<TResult>(Task<SheetsService> serviceTask, CellFullSignature cellId, object to, Func<ValueRange, TResult> convertResult)
        {
            var rangeString = to == null ? cellId.ToString() : cellId.ToString(to);
            var valueRange = serviceTask.WaitAndUnwrapException()
                .Spreadsheets.Values.Get(cellId.SpreadsheetId, rangeString).Execute();
            return convertResult(valueRange);
        }

        private static async Task<TResult> ReadDataFromServiceAsync<TResult>(Task<SheetsService> serviceTask, CellFullSignature cellId, object to, Func<ValueRange, TResult> convertResult)
        {
            var rangeString = to == null ? cellId.ToString() : cellId.ToString(to);
            var sheetService = await serviceTask;
            var valueRange = await sheetService.Spreadsheets.Values.Get(cellId.SpreadsheetId, rangeString).ExecuteAsync();
            return convertResult(valueRange);
        }

        private static string ConvertToSingleValue(ValueRange valueRange) =>
            valueRange.Values[0][0].ToString();

        private static ImmutableArray<ImmutableArray<string>> ConvertToArrays(ValueRange valueRange) =>
            valueRange.Values
                .Select(row => row.Cast<string>().ToImmutableArray())
                .ToImmutableArray();

        private static int AlphabetPosition(string characters)
        {
            var numberOfLettersInAlphabet = 26;
            var characterAValue = 65;

            var offset = (characters.Count() - 1) * numberOfLettersInAlphabet;
            var lastCharacterAlphabetPosition = char.ToUpper(characters.Last()) - characterAValue - 1;
            return offset + lastCharacterAlphabetPosition;
        }
    }
}
