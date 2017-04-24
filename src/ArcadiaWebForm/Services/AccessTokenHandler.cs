using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public interface IAccessTokenHandler
    {
        Task<string> AquireAccessTokenAsync();
    }
    public class AccessTokenHandler: IAccessTokenHandler
    {
        private readonly IMemoryCache _cache;
        
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public AccessTokenHandler(IConfiguration configuration, IHttpContextAccessor contextAccessor, IMemoryCache cache)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _cache = cache;
        }

        public async Task<string> AquireAccessTokenAsync()
        {
            var instance = _configuration["Authentication:AzureAd:AADInstance"];
            var tenantId = _configuration["Authentication:AzureAd:TenantId"];
            var appKey = _configuration["Authentication:AzureAd:SecretKey"];
            var clientId = _configuration["Authentication:AzureAd:ClientId"];
            var todoListResourceId = _configuration["Authentication:AzureAd:ResourceId"];

            var authority = $"{instance}{tenantId}";
            string userObjectID = _contextAccessor.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var credential = new ClientCredential(clientId, appKey);
            var authContext = new AuthenticationContext(authority, new InMemoryTokenCache(_cache, userObjectID));

            var result = await authContext.AcquireTokenAsync(todoListResourceId, credential);
            return result.AccessToken;
        }
    }
}
