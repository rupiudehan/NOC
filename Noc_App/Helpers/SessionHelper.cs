using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Noc_App.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectasJson(this ISession session, string Key, object value)
        {
            session.SetString(Key, JsonConvert.SerializeObject(value));
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static string GenerateSessionId(HttpContext context)
        {
            // Get user's IP address and User-Agent for uniqueness
            string userIp = context.Connection.RemoteIpAddress?.ToString();
            string userAgent = context.Request.Headers["User-Agent"].ToString();

            // Combine the IP address and User-Agent to create a unique session ID
            string combinedInfo = $"{userIp}-{userAgent}-{DateTime.UtcNow.Ticks}";
            //var combinedInfo = $"{userAgent}-{userIp}-{GenerateRandomString()}";

            // Create a hash from the combined string to generate a fixed length session ID
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedInfo));
                return Convert.ToBase64String(hashBytes);
            }
            //using (var sha256 = SHA256.Create())
            //{
            //    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedInfo));
            //    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            //}
        }

        private static string GenerateRandomString(int length = 16)
        {
            // Generate a random string to add to the uniqueness of the session ID
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
