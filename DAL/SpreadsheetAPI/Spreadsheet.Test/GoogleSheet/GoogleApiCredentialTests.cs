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
    public class GoogleApiTestsBase
    {
        protected const string ClientIdFileName = "GoogleSheet/client_id.json";
        protected const string ApplicationName = "Google Sheets API";
        protected const string SpreadsheetId = "1-YiEKit8Y1e2vnnj1NxnKGHDKNGYZWjrGo9_LD_z-uw";

        private string credentialPath;
        protected string CredentialPath => credentialPath = credentialPath ?? Path.Combine(TestContext.TestDir, "../", ".credentials/sheets.googleapis.com-dotnet-test.json");

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get => testContextInstance;
            set => testContextInstance = value;
        }
    }

    [TestClass]
    public class GoogleApiCredentialTests : GoogleApiTestsBase
    {
        [TestMethod]
        public void Connect_Test()
        {
            var credential = Connect(CredentialPath);
            Assert.AreNotEqual(credential.Token, null);
        }

        public static UserCredential Connect(string credentialPath)
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
                    new FileDataStore(credentialPath, true)).Result;
            }
            return userCredential;
        }
    }
}
