using ArcadiaWebForm.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{

    public interface ICallApi
    {
        Task<string> GetId();
        Task<HttpStatusCode> StoreObject(Opportunity obj);
    }

    public class ApiCaller : ICallApi
    {
        private readonly IAccessTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public ApiCaller(IConfiguration configuration, IAccessTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
            _configuration = configuration;
        }

        public async Task<string> GetId()
        {
            var accessToken = await _tokenHandler.AquireAccessTokenAsync();

            var baseUrl = _configuration["Settings:BaseUrl"];
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/id");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var bodyAsString = await response.Content.ReadAsStringAsync();
            var idModel = JsonConvert.DeserializeObject<IdModel>(bodyAsString);

            return idModel.results[0].ids[0];
        }

        public async Task<HttpStatusCode> StoreObject(Opportunity obj)
        {

            await Task.Delay(1);
            return HttpStatusCode.OK;
        }
    }
}