using Jobs.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityService.FunctionalTests
{
    public abstract class BaseTestEntity
    {
        protected TestServer Server; 
        protected HttpClient Client;

        private TestServer CreateTestServer()
        {
            var path = Assembly.GetAssembly(typeof(Startup)).Location;

            var hostBuilder = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(contextBuilder =>
                {
                    contextBuilder.AddJsonFile("appsettings.json", optional: false).AddEnvironmentVariables();
                })
                .UseStartup<Startup>();

            return new TestServer(hostBuilder);
        }

        [TestInitialize]
        public void SetUp()
        {
            Server = CreateTestServer();
            Client = Server.CreateClient();
        }

        [TestCleanup]
        public void TearDown()
        {
            Server?.Dispose();
            Client?.Dispose();
            Server = null;
            Client = null;
        }

        public string GenaretJson(object requestModel)
        {
            return RequestModel.GenaretJson(requestModel);
        }
    }

    public static class RequestModelExtensions
    {
        public static RequestModel RequestModelFromJson(this HttpResponseMessage httpResponse, string jsonData)
        {
            return JsonConvert.DeserializeObject<RequestModel>(jsonData);
        }

        public static async Task<RequestModel> GetResponseModel(this HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();

            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return httpResponse.RequestModelFromJson(responseString);
        }

        public static TResponseType GetResponseFromResult<TResponseType>(this RequestModel response)
        {
            return JsonConvert.DeserializeObject<TResponseType>(response.Result.ToString());
        }
    }
}
