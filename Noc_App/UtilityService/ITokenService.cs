namespace Noc_App.UtilityService
{
    public interface ITokenService
    {
        string GenerateToken();
        bool ValidateToken(string token);
        void RotateToken(HttpContext context);
        void InvalidateToken(string token);
    }
}
