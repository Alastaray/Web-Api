using AspEndpoint.Helpers;
using AspEndpoint.Requests;
using AspEndpoint.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace AspEndpoint.Controllers
{
    public class UserController : Controller
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("api/authorization")]
        public async Task<IActionResult> Authorizate([FromBody] AuthorizationRequest requestUser)
        {
            try
            {
                return this.JsonOk(await _userService.Authorizate(requestUser, HttpContext));
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

        [HttpPost]
        [Route("api/registration")]
        public async Task<IActionResult> Registrate([FromBody] RegistrationRequest requestUser)
        {
            try
            {
                return this.JsonOk(await _userService.Registrate(requestUser, HttpContext));
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
