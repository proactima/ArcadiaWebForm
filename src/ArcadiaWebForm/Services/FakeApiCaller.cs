using ArcadiaWebForm.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public class FakeApiCaller : ICallApi
    {
        public Task<string> GetId()
        {
            return Task.FromResult("aa");
        }

        public Task<IEnumerable<T>> LoadEntities<T>() where T : Entity, new()
        {
            var theList = new List<T>();
            if (typeof(T) == typeof(Organisation))
            {
                theList.Add(new Organisation { Id = "cid1", Name = "Contoso" } as T);
                theList.Add(new Organisation { Id = "cid2", Name = "Fabrikam" } as T);
            }

            if (typeof(T) == typeof(CrmStatus))
            {
                theList.Add(new CrmStatus { Id = "cs1", Name = "Draft", InDraft = true } as T);
                theList.Add(new CrmStatus { Id = "cs2", Name = "Open", InDraft = false } as T);
                theList.Add(new CrmStatus { Id = "cs3", Name = "Closed", InDraft = false } as T);
            }

            return Task.FromResult((IEnumerable<T>)theList);
        }

        public Task<HttpResponseMessage> StoreArticleAsync(Article obj)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            return Task.FromResult(response);
        }
    }
}