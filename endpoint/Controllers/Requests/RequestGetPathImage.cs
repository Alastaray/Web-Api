using Endpoint.Models;
using System;
using System.Net;
using System.Net.Http.Json;


namespace Endpoint.Controllers.Requests
{
    internal class RequestGetPathImage : ServerController
    {
        public RequestGetPathImage(DatabaseController _database, HttpListenerContext _context) : base(_database, _context)
        {
        }
        public override JsonContent ExecuteRequest(string host)
        {
            string id = IsKey("id");
            if (id == null)
                throw new ServerExpection("Id is empty!", 422);
            try
            {
                string path = Database.ExecuteSelectFromDB(id);
                string name = Database.ExecuteSelectFromDB(id, true);
                return JsonContent.Create(new LinkModel(host + path + name));
            }
            catch (ServerExpection er)
            {
                Context.Response.StatusCode = er.StatusCode;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
            catch (Exception er)
            {
                Context.Response.StatusCode = 400;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
        }
    }
}
