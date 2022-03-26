using AspEndpoint.Models;
using AspEndpoint.Requests;
using System.Security.Claims;

namespace AspEndpoint.Services.UserService
{
    public interface IUserService
    {
        public Task<string> Authorizate(AuthorizationRequest requestUser);
        public Task<string> Registrate(RegistrationRequest requestUser);
        public Task<ClaimsIdentity?> GetIdentity(string login, string password);
        public Task<UserModel> CreateUser(RegistrationRequest requestUser);
    }
}
