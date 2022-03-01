
namespace Endpoint.Models
{
    internal class Message
    {
        public Message(string _message) { message = _message; }
        public string message { get; set; }
    }
    internal class ErrorMessage
    {
        public ErrorMessage(string message) { Error = message; }
        public string Error { get; set; }
    }
}
