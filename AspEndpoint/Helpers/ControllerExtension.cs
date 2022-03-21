using Microsoft.AspNetCore.Mvc;

namespace AspEndpoint.Helpers
{
    public static class ControllerExtension
    {
        public enum ResponseStatusCodes
        {
            Ok = 200,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403, 
            NotFound = 404,
            Conflict = 409,
            UnprocessableEntity = 422
        }
        public static JsonResult JsonBadRequest(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.BadRequest);
        }

        public static JsonResult JsonOk(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.Ok);
        }

        public static JsonResult JsonNotFound(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.NotFound);
        }

        public static JsonResult JsonUnauthorized(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.Unauthorized);
        }

        public static JsonResult JsonForbidden(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.Forbidden);
        }

        public static JsonResult JsonConflict(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.Conflict);
        }

        public static JsonResult JsonUnprocessableEntity(this ControllerBase controller, string message)
        {
            return CreateJson(message, ResponseStatusCodes.UnprocessableEntity);
        }

        private static JsonResult CreateJson(string message, ResponseStatusCodes statusCode)
        {
            var jsonResult = new JsonResult(new { Message = message });
            jsonResult.StatusCode = (int)statusCode;
            return jsonResult;
        }
    }
}
