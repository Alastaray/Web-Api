using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using Endpoint.Models;

namespace Endpoint.Controllers.Requests
{
    internal class RequestRemoveImage : ServerController
    {
        public RequestRemoveImage(DatabaseController _database, HttpListenerContext _context) : base(_database, _context)
        {
        }
        public override JsonContent ExecuteRequest(string host)
        {
            string id = IsKey("id");
            if (id == null)
                throw new ServerExpection("Id is empty!", 422);
            try
            {
                string path = Database.ExecuteSelectFromDB(id),
                    name = Database.ExecuteSelectFromDB(id, true);
                File.Delete(path + name);
                File.Delete(path + "new_100_" + name);
                File.Delete(path + "new_300_" + name);
                Database.ExecuteDeleteFromDB(id);
                return JsonContent.Create(new Message("Successfully deleting!"));
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
