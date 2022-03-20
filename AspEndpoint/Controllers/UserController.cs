using AspEndpoint.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspEndpoint.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        public UserController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpPost]
        [Route("api/authorization")]
        public async Task<IActionResult> Authorization([FromBody] UserRequest user)
        {
            var identity = await GetIdentity(user.Login, user.Password);
            if (identity == null)
                return Unauthorized(new { error = "Invalid username or password!" });
            
            return Json(new { access_token = Jwt.Create(identity) });
        }

        private async Task<ClaimsIdentity?> GetIdentity(string login, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (user == null) return null; 
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
