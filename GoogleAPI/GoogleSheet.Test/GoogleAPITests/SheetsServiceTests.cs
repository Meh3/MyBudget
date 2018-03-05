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
    public class SheetsServiceTests
    {
        CredentialsTests credentialsTests = new CredentialsTests();
        private const string ApplicationName = "Google Sheets API";

        [TestMethod]
        public void CreateSheetsService_Test()
        {
            var credentials = credentialsTests.Connect_Test();
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName,
            });

            Assert.IsNotNull(service);
            Assert.AreEqual(ApplicationName, service.ApplicationName);
        }
    }
}
