using ArcadiaWebForm.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{

    public interface ICallApi
    {
        Task<string> GetId();
        Task<HttpResponseMessage> StoreArticleAsync(BaseModel obj);
    }

    public class ApiCaller : ICallApi
    {
        private readonly IAccessTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client = new HttpClient();

        public ApiCaller(IConfiguration configuration, IAccessTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
            _configuration = configuration;
        }

        public async Task<string> GetId()
        {
            var response = await ExecuteAsync(HttpMethod.Get, "/id");

            var bodyAsString = await response.Content.ReadAsStringAsync();
            var idModel = JsonConvert.DeserializeObject<IdModel>(bodyAsString);

            return idModel.Results[0].Ids[0];
        }

        private async Task<HttpResponseMessage> ExecuteAsync(HttpMethod method, string path, HttpContent content = null)
        {
            var baseUrl = _configuration["Settings:BaseUrl"];
            var accessToken = await _tokenHandler.AquireAccessTokenAsync();

            var request = new HttpRequestMessage(method, baseUrl + path);
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> StoreArticleAsync(BaseModel obj)
        {
            var objAsString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(objAsString, Encoding.UTF8, "application/json");
            var response = await ExecuteAsync(HttpMethod.Put, $"/article/{obj.Objectname}/{obj.Id}", content);

            return response;
        }
    }
}