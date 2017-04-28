using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public interface ICallApi
    {
        Task<string> GetId();
        Task<UserProfile> GetUserProfile();
        Task<T> StoreArticleAsync<T>(T obj) where T : Article;
        Task<IEnumerable<T>> LoadEntities<T>() where T : Entity, new();
        Task<IEnumerable<AnyEntity>> LoadAllEntities(string objectname);
    }
}