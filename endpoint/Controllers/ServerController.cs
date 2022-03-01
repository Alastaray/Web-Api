using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using Endpoint.Controllers.Requests;

namespace Endpoint.Controllers
{
    abstract class ServerController
    {
        protected DatabaseController Database { get; set; }
        protected HttpListenerContext Context { get; set; }

        public ServerController(DatabaseController database, HttpListenerContext context)
        {
            Database = database;
            Context = context;
        }

        public string IsKey(string key)
        {
            if (Context.Request.QueryString.Count != 0)
            {
                foreach (string item in Context.Request.QueryString.Keys)
                {
                    if (item != null && item.Equals(key))
                    {
                        return Context.Request.QueryString.Get(item);
                    }
                }
            }
            return null;
        }
        public abstract JsonContent ExecuteRequest(string host);
        static public ServerController GetRequest(DatabaseController db, HttpListenerContext context)
        {
            switch (CheckRequest(context.Request))
            {
                case "upload-by-url":
                    return new RequestUploadImage(db, context);
                case "get-url":
                    return new RequestGetPathImage(db, context);
                case "remove":
                    return new RequestRemoveImage(db, context);
                default:
                    return null;
            }
        }

        static public string CheckRequest(HttpListenerRequest request)
        {
            string[] str_request = request.RawUrl.Replace('?', '/').Split("/");
            string end = "end";
            string[] possible_requests = { "upload-by-url", "get-url", "remove" };
            if (str_request.Length != 0)
            {
                if (str_request[1].Equals("api"))
                {
                    if (str_request[2].Equals(end)) return end;
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
