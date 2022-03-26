using AspEndpoint.Controllers;
using AspEndpoint.Models;
using AspEndpoint.Requests;
using FileManagerLibrary;
using System.Security.Claims;
namespace AspEndpoint.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        public UserService(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<string> Authorizate(AuthorizationRequest requestUser)
        {
            var identity = await GetIdentity(requestUser.Login, requestUser.Password);
            if (identity == null)
                throw new ControllerExpection("Invalid username or password!", ResponseStatusCode.UnprocessableEntity);

            return Jwt.Create(identity);
        }

        public async Task<ClaimsIdentity?> GetIdentity(string login, string password)
        {
            var user = await GetUser(login);
            if (user == null) return null;
            if(!CheckUserPassword(password, user))return null;
            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name),
                    new Claim("Surname", user.Surname)
                };
            return new ClaimsIdentity(claims);
        }

        public async Task<string> Registrate(RegistrationRequest requestUser)
        {
            if (await IsUserExist(requestUser.Login))
                throw new ControllerExpection("This login has already taken!", ResponseStatusCode.UnprocessableEntity);
            UserModel user = await CreateUser(requestUser);          
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return "You successfully registered!";
        }

        public async Task<UserModel> CreateUser(RegistrationRequest requestUser)
        {
            UserModel user = new UserModel();
            user.Name = requestUser.Name;
            user.Surname = requestUser.Surname;
            user.Login = requestUser.Login;
            user.CreatedAt = DateTime.UtcNow;
            user.Password = CreateHashPassword(requestUser.Password, user.CreatedAt);
            return user;
        }

        private string CreateHashPassword(string password, DateTime? createdAt)
        {
            return Hasher.CreateHashStr(password, createdAt.Value.Day, createdAt);
        }

        private bool CheckUserPassword(string password, UserModel user)
        {
            string hashPassword = Hasher.CreateHashStr(password, user.CreatedAt.Value.Day, user.CreatedAt);
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
