using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http.Json;

namespace Project
{
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
