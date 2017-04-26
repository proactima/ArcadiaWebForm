using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Entity;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public interface ICallApi
    {
        Task<string> GetId();
        Task<HttpResponseMessage> StoreArticleAsync(Article obj);
        Task<IEnumerable<T>> LoadEntities<T>() where T : Entity, new();
    }
}