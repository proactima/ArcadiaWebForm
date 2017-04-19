using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Services
{
    public interface IAccessTokenHandler
    {
        Task<string> AquireAccessTokenAsync(HttpContext context);
    }
    public class AccessTokenHandler: IAccessTokenHandler
    {
        private readonly IConfiguration _configuration;
        private string _currentAccessToken;
        public AccessTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AquireAccessTokenAsync(HttpContext context)
        {
            if (!string.IsNullOrEmpty(_currentAccessToken)) return _currentAccessToken;

            var instance = _configuration["Authentication:AzureAd:AADInstance"];
            var tenantId = _configuration["Authentication:AzureAd:TenantId"];
            var appKey = _configuration["Authentication:AzureAd:SecretKey"];
            var clientId = _configuration["Authentication:AzureAd:ClientId"];
            var todoListResourceId = _configuration["Authentication:AzureAd:ResourceId"];

            var authority = $"{instance}{tenantId}";
            string userObjectID = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var credential = new ClientCredential(clientId, appKey);
            var authContext = new AuthenticationContext(authority);

            var result = await authContext.AcquireTokenAsync(todoListResourceId, credential);
            _currentAccessToken = result.AccessToken;
            return _currentAccessToken;
        }
    }
}
