using AspEndpoint.Helpers;
using AspEndpoint.Models;
using FileManagerLibrary;
using System.Security.Claims;

namespace AspEndpoint.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private const string RefreshTokenKey = "RefreshToken";
        private static int RefreshTokenTimeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RefreshTokenLifeTime"] ?? "14");
        private static int CountRefreshSession = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CountRefreshSession"] ?? "1");        
        public AuthService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<string> CreateRefreshSession(UserModel user, HttpContext httpContext)
        {
            string randomStr = Hasher.CreateRandomHashStr();
            RefreshSessionModel refreshSession = new RefreshSessionModel();
            refreshSession.UserId = user.Id;
            refreshSession.User = user;
            refreshSession.RefreshToken = randomStr;
            await AddRefreshSession(refreshSession);
            SetRefreshToken(randomStr, httpContext);
            return Jwt.Create(GetUserClaims(user));
        }

        public async Task AddRefreshSession(RefreshSessionModel refreshSession)
        {
            if(await GetCountRefreshSession(refreshSession.UserId) >= CountRefreshSession)
            {
                _dataContext.RefreshSessions.Remove(await GetRefreshSession(refreshSession.UserId));
            }
            await _dataContext.RefreshSessions.AddAsync(refreshSession);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<string> UpdateRefreshSession(HttpContext httpContext)
        {
            var refreshSession = await GetRefreshSession(GetRefreshToken(httpContext));
            if (refreshSession == null)
                throw new ControllerExpection("You need to authorize!", ResponseStatusCode.Unauthorized);
            var user = await _dataContext.Users.FindNotDeletedAsync(refreshSession.UserId);
            _dataContext.RefreshSessions.Remove(refreshSession);
            return await CreateRefreshSession(user, httpContext);
        }

        public void SetRefreshToken(string refreshToken, HttpContext httpContext)
        {
            httpContext.Response.Cookies.Append(RefreshTokenKey, refreshToken,
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(RefreshTokenTimeout),
                    HttpOnly = true,
                    Secure = true,
                });
        }

        public string? GetRefreshToken(HttpContext httpContext)
        {
            return httpContext.Request.Cookies[RefreshTokenKey];
        }

        public async Task<RefreshSessionModel?> GetRefreshSession(int userId)
        {
            return await _dataContext.RefreshSessions.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<RefreshSessionModel?> GetRefreshSession(string? refreshToken)
        {
            return refreshToken == null ? null : 
                await _dataContext.RefreshSessions.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }

        public async Task<RefreshSessionModel?> GetLastRefreshSession()
        {
            return await _dataContext.RefreshSessions.OrderBy(x => x.Id).LastOrDefaultAsync();
        }

        public async Task<int> GetCountRefreshSession(int userId)
        {
            return await _dataContext.RefreshSessions.CountAsync(x => x.UserId == userId);
        }

        public ClaimsIdentity GetUserClaims(UserModel user)
        {
            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name),
                    new Claim("Surname", user.Surname)
                };
            return new ClaimsIdentity(claims);
        }
    }
}
