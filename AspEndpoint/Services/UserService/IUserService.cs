using AspEndpoint.Models;
using AspEndpoint.Requests;
using System.Security.Claims;

namespace AspEndpoint.Services.UserService
{
    public interface IUserService
    {
        public Task<string> Authorizate(AuthorizationRequest requestUser, HttpContext httpContext);
        public Task<string> Registrate(RegistrationRequest requestUser, HttpContext httpContext);
        public Task<bool> IsUser(UserModel? user, string password);
        public UserModel CreateUser(RegistrationRequest requestUser);
        public Task<bool> IsUserAuthorized(string? refreshToken); 
    }
}
