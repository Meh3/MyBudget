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
    public class SheetsServiceIntegrationTests
    {
        private const string ApplicationName = "Google Sheets API";

        [TestMethod]
        public void CreateSheetsService_Test()
        {
            var sheetsService = CreateSheetService();

            Assert.IsNotNull(sheetsService);
            Assert.AreEqual(ApplicationName, sheetsService.ApplicationName);
        }

        public static SheetsService CreateSheetService()
        {
            var credentials = CredentialsIntegrationTests.Connect();
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName,
            });
            return service;
        }
    }
}
