using Microsoft.AspNetCore.Mvc;

namespace AspEndpoint.Controllers
{
    public enum ResponseStatusCode
    {
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        UnprocessableEntity = 422
    }
    public static class ControllerExtension
    {       
        public static JsonResult JsonBadRequest(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.BadRequest);
        }

        public static JsonResult JsonOk(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.Ok);
        }

        public static JsonResult JsonNotFound(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.NotFound);
        }

        public static JsonResult JsonUnauthorized(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.Unauthorized);
        }

        public static JsonResult JsonForbidden(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.Forbidden);
        }

        public static JsonResult JsonConflict(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.Conflict);
        }

        public static JsonResult JsonUnprocessableEntity(this ControllerBase controller, string message)
        {
            return controller.CreateJson(message, ResponseStatusCode.UnprocessableEntity);
        }

        public static JsonResult CreateJson(this ControllerBase controller, string message, ResponseStatusCode statusCode)
        {
            var jsonResult = new JsonResult(new { Message = message });
            jsonResult.StatusCode = (int)statusCode;
            return jsonResult;
        }
    }
}
