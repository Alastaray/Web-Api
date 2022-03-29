using AspEndpoint.Models;
using System.Security.Claims;

namespace AspEndpoint.Services.AuthService
{
    public interface IAuthService
    {
        public Task<string> CreateRefreshSession(UserModel user, HttpContext httpContext);
        public Task AddRefreshSession(RefreshSessionModel refreshSession);
        public Task<string> UpdateRefreshSession(HttpContext httpContext);
        public void SetRefreshToken(string refreshToken, HttpContext httpContext);
        public string? GetRefreshToken(HttpContext httpContext);
        public Task<RefreshSessionModel?> GetRefreshSession(int userId);
        public Task<RefreshSessionModel?> GetRefreshSession(string? refreshToken);
        public Task<RefreshSessionModel?> GetLastRefreshSession();
        public Task<int> GetCountRefreshSession(int userId);
        public ClaimsIdentity GetUserClaims(UserModel user);
    }
}
