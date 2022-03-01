using System;

namespace Project
{
    public class Message
    {
        public Message(string _message) { message = _message; }
        public string message { get; set; }
    }
    public class ErrorMessage
    {
        public ErrorMessage(string message) { Error = message; }
        public string Error { get; set; }
    }
    public class Link
    {
        public Link(string url) { Url = url; }
        public string Url { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            const string host = "http://localhost:8888/";
            Server server = new Server(host);
            Console.WriteLine("Waiting for connection!");
            server.Start();
            
        }
    }
}
