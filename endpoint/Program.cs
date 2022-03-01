using System;

namespace Endpoint
{
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
