using Endpoint.Controllers;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using Endpoint.Models;

namespace Endpoint
{
    public class ServerExpection : Exception
    {
        public ServerExpection(string message, int status_code) : base(message)
        {
            StatusCode = status_code;
        }
        public int StatusCode { get; set; }
    }

    class Server
    {
        public string Host { get;}
        private HttpListener listener;
        private string connStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=";
        DatabaseController db;

        public Server(string host)
        {
            Host = host;
            connStr += Directory.GetCurrentDirectory() + "\\DB.mdf; Integrated Security = True";
            listener = new HttpListener();
            db = new DatabaseController(connStr);
            listener.Prefixes.Add(Host);
            listener.Start();            
        }

        ~Server()
        {
            listener.Stop();            
        }

        public void Start()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                JsonContent json;
                try
                {

                    ServerController serverController = ServerController.GetRequest(db, context);
                    if (serverController == null) break;
                    json = serverController.ExecuteRequest(Host);                   
                }
                catch (ServerExpection er)
                {
                    context.Response.StatusCode = er.StatusCode;
                    json = JsonContent.Create(new ErrorMessage(er.Message));
                }
                Stream output = context.Response.OutputStream;
                json.CopyToAsync(output);
                output.Close();
            }            
        }      
    }
}
