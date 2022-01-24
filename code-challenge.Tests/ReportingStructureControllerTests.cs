using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    class ReportingStructureControllerTests
    {

        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateReportingStructure_Returns_Created()
        {
            // Arrange
            // Get employee John Lennon
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var getResponse = getRequestTask.Result;
            var employee = getResponse.DeserializeContent<Employee>();

            var reportingStructure = new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = 4
            };

            var requestContent = new JsonSerialization().ToJson(reportingStructure);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/reportingstructure",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var postResponse = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            var newReportingStructure = postResponse.DeserializeContent<ReportingStructure>();
            Assert.IsNotNull(newReportingStructure.Employee);
            Assert.AreEqual(reportingStructure.Employee, newReportingStructure.Employee);
            Assert.AreEqual(reportingStructure.NumberOfReports, newReportingStructure.NumberOfReports);
        }
    }
}
