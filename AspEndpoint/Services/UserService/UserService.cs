using AspEndpoint.Helpers;
using AspEndpoint.Models;
using AspEndpoint.Requests;
using AspEndpoint.Services.AuthService;
using FileManagerLibrary;

namespace AspEndpoint.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;

        public UserService(DataContext context, IAuthService authService)
        {
            _dataContext = context;
            _authService = authService;
        }

        public async Task<string> Authorizate(AuthorizationRequest requestUser, HttpContext httpContext)
        {
            if(await IsUserAuthorized(_authService.GetRefreshToken(httpContext)))
                throw new ControllerExpection("You have already authorizated!", ResponseStatusCode.BadRequest);
            var user = await GetUser(requestUser.Login);
            if (!await IsUser(user, requestUser.Password))
                throw new ControllerExpection("Invalid username or password!", ResponseStatusCode.UnprocessableEntity);
            return await _authService.CreateRefreshSession(user, httpContext);
        }

        public async Task<string> Registrate(RegistrationRequest requestUser, HttpContext httpContext)
        {
            if (await IsUserExist(requestUser.Login))
                throw new ControllerExpection("This login has already taken!", ResponseStatusCode.UnprocessableEntity);
            UserModel user = CreateUser(requestUser);          
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            //return "You successfully registered!";
            return await _authService.CreateRefreshSession(user, httpContext);
        }

        public async Task<bool> IsUser(UserModel? user, string password)
        {
            if (user == null || !CheckUserPassword(user, password))
                return false;
            return true;
        }

        public async Task<bool> IsUserAuthorized(string? refreshToken)
        {
            var lastRefreshSession = await _authService.GetLastRefreshSession();
            return refreshToken?.Equals(lastRefreshSession?.RefreshToken) ?? false;
        }

        public UserModel CreateUser(RegistrationRequest requestUser)
        {
            UserModel user = new UserModel();
            user.Name = requestUser.Name;
            user.Surname = requestUser.Surname;
            user.Login = requestUser.Login;
            user.Password = CreateHashPassword(requestUser.Password, user.CreatedAt);
            return user;
        }

        private string CreateHashPassword(string password, DateTime createdAt)
        {
            return Hasher.CreateHashStr(password, createdAt.Day, createdAt);
        }

        private bool CheckUserPassword(UserModel user, string password)
        {
            string hashPassword = Hasher.CreateHashStr(password, user.CreatedAt.Day, user.CreatedAt);
            return hashPassword.Equals(user.Password);
        }

        private async Task<UserModel?> GetUser(string login)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        private async Task<bool> IsUserExist(string login)
        {            
            if (await GetUser(login) == null) return false;
            return true;
        }
    }
}
