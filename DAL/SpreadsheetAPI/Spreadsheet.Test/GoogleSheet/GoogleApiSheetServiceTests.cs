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
    public class GoogleApiSheetServiceTests : GoogleApiTestsBase
    {
        private UserCredential _credentials;
        private UserCredential credentials => _credentials = _credentials ?? GoogleApiCredentialTests.Connect(CredentialPath);

        [TestMethod]
        public void CreateSheetsService_Test()
        {
            var sheetsService = CreateSheetService(credentials);

            Assert.IsNotNull(sheetsService);
            Assert.AreEqual(ApplicationName, sheetsService.ApplicationName);
        }

        public static SheetsService CreateSheetService(UserCredential credentials)
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName,
            });
            return service;
        }
    }
}
