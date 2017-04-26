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
    public class AccessTokenHandler : IAccessTokenHandler
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly string _appKey;
        private readonly string _clientId;
        private readonly string _resourceId;
        private readonly string _authority;

        public AccessTokenHandler(IConfiguration configuration, IHttpContextAccessor contextAccessor, IMemoryCache cache)
        {
            _contextAccessor = contextAccessor;
            _cache = cache;

            _appKey = configuration["Authentication:AzureAd:SecretKey"];
            _clientId = configuration["Authentication:AzureAd:ClientId"];
            _resourceId = configuration["Authentication:AzureAd:ResourceId"];
            _authority = $"{configuration["Authentication:AzureAd:AADInstance"]}{configuration["Authentication:AzureAd:TenantId"]}";
        }

        public async Task<string> AquireAccessTokenAsync()
        {
            string userObjectID = _contextAccessor.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var credential = new ClientCredential(_clientId, _appKey);
            var authContext = new AuthenticationContext(_authority, new InMemoryTokenCache(_cache, userObjectID));

            var result = await authContext.AcquireTokenAsync(_resourceId, credential);
            return result.AccessToken;
        }
    }
}
