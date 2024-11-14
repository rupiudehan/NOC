namespace Noc_App.Middleware
{
    public class IpBindingMiddleware
    {
        private readonly RequestDelegate _next;

        public IpBindingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sessionIp = context.Session.GetString("UserIp");
            var currentIp = context.Connection.RemoteIpAddress?.ToString();

            if (!string.IsNullOrEmpty(sessionIp) && sessionIp != currentIp)
            {
                // IP address mismatch: clear the session and redirect to login
                context.Session.Clear();
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    var sessionIp = context.Session.GetString("UserIp");
        //    var currentIp = context.Connection.RemoteIpAddress?.ToString();
        //    var ipBindingExempt = context.Session.GetString("IpBindingExempt"); // Exemption flag

        //    if (ipBindingExempt != "true" && !string.IsNullOrEmpty(sessionIp) && sessionIp != currentIp)
        //    {
        //        context.Session.Clear();
        //        context.Response.Redirect("/Account/Login");
        //        return;
        //    }

        //    await _next(context);
        //}
    }

}
