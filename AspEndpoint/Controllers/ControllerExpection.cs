namespace AspEndpoint.Controllers
{
    public class ControllerExpection : Exception
    {
        public ResponseStatusCode StatusCode { get; set; }
        public ControllerExpection(string? message, ResponseStatusCode statusCode) :base(message)
        {
            StatusCode = statusCode;
        }
    }
}
