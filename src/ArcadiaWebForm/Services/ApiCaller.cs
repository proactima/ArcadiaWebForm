using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Entity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
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
            var response = await ExecuteAsync<IdModel>(HttpMethod.Get, "/id");
            return response.Results[0].Ids[0];
        }

        public async Task<IEnumerable<AnyEntity>> LoadAllEntities(string objectname)
        {
            var theList = new List<AnyEntity>();
            var path = $"/entity/{objectname}?sortfield=name";
            do
            {
                var response = await ExecuteAsync(HttpMethod.Get, path);
                var bodyAsString = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<ResponseResultModel<AnyEntity>>(bodyAsString);
                theList.AddRange(pagedResult.Results);

                if (!response.Headers.Contains("Link"))
                    path = string.Empty;
                path = response.Headers.GetValues("Link").First();
            } while (!string.IsNullOrEmpty(path));

            return theList.OrderBy(e => e.Name);
        }

        public async Task<IEnumerable<T>> LoadEntities<T>() where T : Entity, new()
        {
            var instance = new T();
            var response = await ExecuteAsync<T>(HttpMethod.Get, $"/entity/{instance.Objectname}");
            return response.Results;
        }

        private async Task<HttpResponseMessage> ExecuteAsync(HttpMethod method, string path, HttpContent content = null)
        {
            var baseUrl = _configuration["Settings:BaseUrl"];
            var accessToken = await _tokenHandler.AquireAccessTokenAsync();

            var request = new HttpRequestMessage(method, baseUrl + path);
            if (content != null)
                request.Content = content;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<ResponseResultModel<T>> ExecuteAsync<T>(HttpMethod method, string path, HttpContent content = null)
        {
            var response = await ExecuteAsync(method, path, content);
            var bodyAsString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ResponseResultModel<T>>(bodyAsString);

            return model;
        }

        public async Task<T> StoreArticleAsync<T>(T obj) where T : Article
        {
            var objAsString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(objAsString, Encoding.UTF8, "application/json");
            var response = await ExecuteAsync(HttpMethod.Put, $"/article/{obj.Objectname}/{obj.Id}", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<ResponseResultModel<T>>(responseContent);
            return deserialized.Results[0];
        }

        public async Task<UserProfile> GetUserProfile()
        {
            var response = await ExecuteAsync<UserProfile>(HttpMethod.Get, "/profile/user");
            return response.Results[0];
        }
    }
}