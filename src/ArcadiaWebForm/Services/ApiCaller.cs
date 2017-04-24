using ArcadiaWebForm.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public interface ICallApi
    {
        Task<string> GetId();
        Task<HttpResponseMessage> StoreArticleAsync(Article obj);
        Task<IEnumerable<T>> LoadEntities<T>(string objectname) where T : Entity;
    }

    public class FakeApiCaller : ICallApi
    {
        public Task<string> GetId()
        {
            return Task.FromResult("aa");
        }

        public Task<IEnumerable<T>> LoadEntities<T>(string objectname) where T : Entity
        {
            var theList = new List<T>();
            if (typeof(T) == typeof(Client))
            {
                theList.Add(new Client { Id = "cid1", Name = "Contoso", Objectname = "client" } as T);
                theList.Add(new Client { Id = "cid2", Name = "Fabrikam", Objectname = "client" } as T);
            }

            if (typeof(T) == typeof(CrmStatus))
            {
                theList.Add(new CrmStatus { Id = "cs1", Name = "Draft", IsDraft = true, Objectname = "crmstatus" } as T);
                theList.Add(new CrmStatus { Id = "cs2", Name = "Open", IsDraft = false, Objectname = "crmstatus" } as T);
                theList.Add(new CrmStatus { Id = "cs3", Name = "Closed", IsDraft = false, Objectname = "crmstatus" } as T);
            }

            return Task.FromResult((IEnumerable<T>)theList);
        }

        public Task<HttpResponseMessage> StoreArticleAsync(Article obj)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            return Task.FromResult(response);
        }
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
            var response = await ExecuteAsync<IdModel>(HttpMethod.Get, "/id");
            return response.Results[0].Ids[0];
        }

        public async Task<IEnumerable<T>> LoadEntities<T>(string objectname) where T : Entity
        {
            var response = await ExecuteAsync<T>(HttpMethod.Get, $"/entity/{objectname}");
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

        private async Task<ResponseModel<T>> ExecuteAsync<T>(HttpMethod method, string path, HttpContent content = null)
        {
            var response = await ExecuteAsync(method, path, content);
            var bodyAsString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ResponseModel<T>>(bodyAsString);

            return model;
        }

        public async Task<HttpResponseMessage> StoreArticleAsync(Article obj)
        {
            var objAsString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(objAsString, Encoding.UTF8, "application/json");
            var response = await ExecuteAsync(HttpMethod.Put, $"/article/{obj.Objectname}/{obj.Id}", content);

            return response;
        }
    }
}