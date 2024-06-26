using Identity.IntegrationTests;
using Jobs.Service.Common;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Identity.IntegrationTests
{
    public abstract class BaseTestEntity
    {
        protected TestServer Server;
        protected HttpClient Client;

        [TestInitialize]
        public void SetUp()
        {
            Server = new ServerApiFactory().Server;
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
