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

        private readonly string _callback;
        private readonly string _appKey;
        private readonly string _clientId;
        private readonly string _resourceId;
        private readonly string _authority;

        public AccessTokenHandler(IConfiguration configuration, IHttpContextAccessor contextAccessor, IMemoryCache cache)
        {
            _contextAccessor = contextAccessor;
            _cache = cache;

            _callback = configuration["Authentication:AzureAd:CallbackPath"];
            _appKey = configuration["Authentication:AzureAd:SecretKey"];
            _clientId = configuration["Authentication:AzureAd:ClientId"];
            _resourceId = configuration["Authentication:AzureAd:ResourceId"];
            _authority = $"{configuration["Authentication:AzureAd:AADInstance"]}{configuration["Authentication:AzureAd:TenantId"]}";
        }

        public object OpenIdConnectAuthenticationDefaults { get; private set; }

        public async Task<string> AquireAccessTokenAsync()
        {

            var userObjectID = (_contextAccessor.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            var authContext = new AuthenticationContext(Startup.Authority, new InMemoryTokenCache(_cache, userObjectID));
            var credential = new ClientCredential(Startup.ClientId, Startup.ClientSecret);
            var result = await authContext.AcquireTokenSilentAsync(Startup.ResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

            return result.AccessToken;
        }
    }
}
