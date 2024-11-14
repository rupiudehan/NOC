using Noc_App.Helpers;

namespace Noc_App.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Session.GetString("DeviceId") == null)
            {
                // Set a unique device ID for the session on initial login
                context.Session.SetString("DeviceId", DeviceIdHelper.GenerateDeviceId(context));
                context.Session.SetString("UserIp", context.Connection.RemoteIpAddress?.ToString());
            }
            else
            {
                // Validate Device ID and IP Address
                var deviceId = context.Session.GetString("DeviceId");
                var userIp = context.Session.GetString("UserIp");

                if (deviceId != DeviceIdHelper.GenerateDeviceId(context) || userIp != context.Connection.RemoteIpAddress?.ToString())
                {
                    // Invalidate the session if device or IP doesn't match
                    context.Session.Clear();
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(context);
        }

        //private string GenerateDeviceId(HttpContext context)
        //{
        //    // Generate a device identifier based on browser, OS, and other headers
        //    var userAgent = context.Request.Headers["User-Agent"].ToString();
        //    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userAgent));
        //}
    }

}
