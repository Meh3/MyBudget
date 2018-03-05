using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBudget.GoogleSheet.Test
{
    [TestClass]
    public class CredentialsTests
    {
        private static string CredentialPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            , ".credentials/sheets.googleapis.com-dotnet-test.json");

        private const string ClientIdFileName = "client_id.json";

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(CredentialPath))
                File.Delete(CredentialPath);
        }

        [TestMethod]
        public void Connect_Test()
        {
            var credential = Connect();
            Assert.AreNotEqual(credential.Token, null);
        }

        public static UserCredential Connect()
        {
            string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            UserCredential userCredential;

            using (var stream =
                new FileStream(ClientIdFileName, FileMode.Open, FileAccess.Read))
            {
                userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(CredentialPath, true)).Result;
            }
            return userCredential;
        }


        //[TestMethod]
        //public void ReadValueFromCell_Test()
        //{
        //    var credential = ConnectTest();
        //    var sheetId = "1-YiEKit8Y1e2vnnj1NxnKGHDKNGYZWjrGo9_LD_z-uw";
        //    var range = "Test!A1";
        //    var expectedValue = "test";

        //    var service = new SheetsService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });
        //    var request = service.Spreadsheets.Values.Get(sheetId, range);
        //    var actualValue = request.Execute().Values[0][0] as string;

        //    Assert.AreEqual(actualValue, expectedValue);
        //}
    }
}
