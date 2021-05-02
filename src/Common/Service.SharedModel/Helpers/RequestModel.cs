using Jobs.SharedModel.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace Service.SharedModel.Helpers
{
    public class RequestModel
    {
        public int ErrorId { get; set; }
        public bool ShouldSerializeErrorId() => ErrorId > 0;

        [JsonProperty("Description")]
        public string Error { get; set; } = string.Empty;
        public bool ShouldSerializeError() => !Error.IsNullOrEmpty();

        public object Result { get; set; }
        public bool ShouldSerializeResult() => Result != null;

        public static async Task<RequestModel> ErrorRequestAsync(string error = "error", int errorId = 400)
        {
            return await Task.Run(() => new RequestModel() { Error = error, ErrorId = errorId });
        }

        public static async Task<RequestModel> SuccessAsync(object result = null)
        {
            return await Task.Run(() =>
            {
                if (result == null) result = new { };

                return new RequestModel() { Result = result };
            });
        }

        public static async Task<RequestModel> NotAccessAsync(string error = "You haven't access to API!", int errorId = 400)
        {
            return await ErrorRequestAsync(error, errorId);
        }

        public static async Task<RequestModel> NotFoundAsync(string error = "Not found", int errorId = 404)
        {
            return await ErrorRequestAsync(error, errorId);
        }

        public static string GenaretJson(object textJson)
        {
            return JsonConvert.SerializeObject(textJson, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }

    public static class RequestModelExtensions
    {
        public static string GenaretJson(this RequestModel requestModel)
        {
            return RequestModel.GenaretJson(requestModel);
        }
    }
}