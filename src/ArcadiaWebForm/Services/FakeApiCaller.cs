using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public class FakeApiCaller : ICallApi
    {
        public Task<string> GetId()
        {
            return Task.FromResult("aa");
        }

        public Task<UserProfile> GetUserProfile()
        {
            return Task.FromResult(new UserProfile
            {
                Id = "id",
                ShareduserId = "shareduserid",
                MailAddress = "user@users.org",
                CurrentOrganization = new OrganizationInfo
                {
                    Prefix = "a1234567",
                    Name = "Current Organization"
                },
                AccessOrganizations = new List<OrganizationInfo>
                {
                    new OrganizationInfo
                    {
                        Prefix = "a1234567",
                        Name = "Current Organization"
                    },
                    new OrganizationInfo
                    {
                        Prefix = "orgprefix",
                        Name = "Some Other Organization"
                    }
                }
            });
        }

        public Task<IEnumerable<T>> LoadEntities<T>() where T : Entity, new()
        {
            var theList = new List<T>();

            if (typeof(T) == typeof(CrmStatus))
            {
                theList.Add(new CrmStatus { Id = "cs1", Name = "Draft", InDraft = true } as T);
                theList.Add(new CrmStatus { Id = "cs2", Name = "Open", InDraft = false } as T);
                theList.Add(new CrmStatus { Id = "cs3", Name = "Closed", InDraft = false } as T);
            }

            return Task.FromResult((IEnumerable<T>)theList);
        }

        public Task<IEnumerable<AnyEntity>> LoadAllEntities(string objectname)
        {
            var theList = new List<AnyEntity>
            {
                new AnyEntity { Id = "cid1", Name = "Contoso" },
                new AnyEntity { Id = "cid2", Name = "Fabrikam" }
            };
            return Task.FromResult((IEnumerable<AnyEntity>)theList);
        }

        public Task<T> StoreArticleAsync<T>(T obj) where T : Article
        {
            return Task.FromResult(obj);
        }
    }
}