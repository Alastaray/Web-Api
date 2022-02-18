using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http.Json;

namespace Project
{
    public class ServerExpection : System.Exception
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
        private const string connStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Администратор\\source\\repos\\endpoint\\endpoint\\DB.mdf;Integrated Security=True";
        Database db;

        public Server(string host)
        {
            Host = host;
            listener = new HttpListener();
            db = new Database(connStr);
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
                    ServerCommand command = new ServerCommand(db, context);
                    json = ExecuteRequest(command);
                    if (json == null) break;
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

        public JsonContent ExecuteRequest(ServerCommand command)
        {
            switch (CheckRequest(command.GetHttpListenerContext().Request))
            {
                case "upload-by-url":
                    return command.DownloadPicture(Host);
                case "get-url":
                    return command.GetPathPicture(Host, false);
                case "get-new-url":
                    return command.GetPathPicture(Host, true);
                case "remove":
                    return command.RemovePicture();
                default:
                    return null;
            }
        }

        public string CheckRequest(HttpListenerRequest request)
        {
            string[] str_request = request.RawUrl.Replace('?', '/').Split("/");
            string end = "end";
            string[] possible_requests = { "upload-by-url", "get-url", "get-new-url", "remove" };
            if (str_request.Length != 0)
            {
                if (str_request[1].Equals(end)) return end;
                if (str_request[1].Equals("api"))
                {
                    foreach (var item in possible_requests)
                    {
                        if (str_request[2].Equals(item)) return item;
                    }
                }
            }
            throw new ServerExpection("Incorrect request!", 400);
        }
        
    }
}
