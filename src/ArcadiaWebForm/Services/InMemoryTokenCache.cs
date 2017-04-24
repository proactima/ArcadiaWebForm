using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace ArcadiaWebForm.Services
{
    // Only usable for single web-server implementations, use a distributed cache for web farms
    internal class InMemoryTokenCache : TokenCache
    {
        private readonly IMemoryCache _cache;
        private readonly string _userId;

        public InMemoryTokenCache(IMemoryCache cache, string userId)
        {
            _cache = cache;
            _userId = userId;

            BeforeAccess = OnBeforeAccess;
            AfterAccess = OnAfterAccess;
        }

        private void OnBeforeAccess(TokenCacheNotificationArgs args)
        {
            var userTokenCachePayload = _cache.Get<byte[]>(CacheKey);
            if (userTokenCachePayload != null)
            {
                Deserialize(userTokenCachePayload);
            }
        }

        private void OnAfterAccess(TokenCacheNotificationArgs args)
        {
            if (HasStateChanged)
            {
                _cache.Set(CacheKey, Serialize(), TimeSpan.FromDays(14));
                HasStateChanged = false;
            }
        }

        private string CacheKey => $"TokenCache_{_userId}";
    }
}
