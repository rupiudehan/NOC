using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

namespace Noc_App.UtilityService
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;
        private const string SessionTokenKey = "SessionToken";

        public TokenService(IMemoryCache cache)
        {
            _cache = cache;
        }

        [Obsolete]
        // Generate a new session token and store it with an expiration time
        public string GenerateToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenBytes = new byte[32]; // 256-bit token
                rng.GetBytes(tokenBytes);
                var token = Convert.ToBase64String(tokenBytes);
                // Store token in memory cache with an expiration time of 15 minutes
                _cache.Set(token, DateTime.UtcNow.AddMinutes(15), new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                });

                return token;
            }
        }

        // Validate the session token
        public bool ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            // Check if the token exists and is still valid
            if (_cache.TryGetValue(token, out DateTime? expirationTime) && expirationTime.HasValue)
            {
                return expirationTime.Value > DateTime.UtcNow;
            }

            return false;
        }

        // Rotate the session token after each request
        [Obsolete]
        public void RotateToken(HttpContext context)
        {
            // Remove the old token if it exists
            var oldToken = context.Session.GetString(SessionTokenKey);
            if (!string.IsNullOrEmpty(oldToken))
            {
                _cache.Remove(oldToken);
            }

            // Generate and set a new token in the session
            var newToken = GenerateToken();
            context.Session.SetString(SessionTokenKey, newToken);
        }

        // Invalidate a specific token (logout functionality)
        public void InvalidateToken(string token)
        {
            _cache.Remove(token);
        }
    }
}
