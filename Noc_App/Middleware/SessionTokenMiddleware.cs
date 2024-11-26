using Noc_App.UtilityService;

namespace Noc_App.Middleware
{
    public class SessionTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        public SessionTokenMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get token from session
            var token = context.Session.GetString("SessionToken");

            // Validate the token
            if (!string.IsNullOrEmpty(token) && _tokenService.ValidateToken(token))
            {
                // Token is valid, rotate the token
                _tokenService.RotateToken(context);
            }
            else
            {
                // Invalid or expired token, handle the session termination logic (redirect to login, etc.)
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }

}
