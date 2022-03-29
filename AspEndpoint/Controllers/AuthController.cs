using AspEndpoint.Helpers;
using AspEndpoint.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace AspEndpoint.Controllers
{
    public class AuthController : Controller
    {
        IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("api/auth/refresh-token")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                return this.JsonOk(await _authService.UpdateRefreshSession(HttpContext));
            }
            catch (ControllerExpection er)
            {
                return this.CreateJson(er.Message, er.StatusCode);
            }
            catch (Exception er)
            {
                return this.JsonBadRequest(er.Message);
            }
        }
    }
}
