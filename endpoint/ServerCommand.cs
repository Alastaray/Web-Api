using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;

namespace Project
{
    class ServerCommand
    {
        Database database;
        HttpListenerContext context;

        public ServerCommand(Database _database, HttpListenerContext _context)
        {
            database = _database;
            context = _context;
        }

        public HttpListenerContext GetHttpListenerContext() { return context; }

        public JsonContent DownloadPicture(string host)
        {
            try
            {
                string url = CheckUrl();
                Image image = new Image();
                if(!image.Download(url)) throw new ServerExpection("Image already exists!", 400);
                image.Cut(100);
                image.Cut(300);
                if (database.ExecuteInsertIntoDB(image.Path, image.Name) != 1)
                    throw new ServerExpection("Bad request to database!", 409);
                return JsonContent.Create(new Link(host + image.Path + image.Name));
            }
            catch (ServerExpection er)
            {
                context.Response.StatusCode = er.StatusCode;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
            catch (Exception er)
            {
                context.Response.StatusCode = 400;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
        }

        public JsonContent RemovePicture()
        {
            string id = IsKey("id");
            if (id == null)
                throw new ServerExpection("Id is empty!", 422);
            try
            {
                string path = database.ExecuteSelectFromDB(id),
                    name = database.ExecuteSelectFromDB(id,true);
                File.Delete(path + name);
                File.Delete(path + "new_100_" + name);
                File.Delete(path + "new_300_" + name);
                database.ExecuteDeleteFromDB(id);
                return JsonContent.Create(new Message("Successfully deleting!"));
            }
            catch (ServerExpection er)
            {
                context.Response.StatusCode = er.StatusCode;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
            catch (Exception er)
            {
                context.Response.StatusCode = 400;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }

        }

        public JsonContent GetPathPicture(string host)
        {
            string id = IsKey("id");
            if (id == null)
                throw new ServerExpection("Id is empty!", 422);
            try
            {
                string path = database.ExecuteSelectFromDB(id);
                string name = database.ExecuteSelectFromDB(id, true);
                return JsonContent.Create(new Link(host + path + name));
            }
            catch (ServerExpection er)
            {
                context.Response.StatusCode = er.StatusCode;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
            catch (Exception er)
            {
                context.Response.StatusCode = 400;
                return JsonContent.Create(new ErrorMessage(er.Message));
            }
        }

        public double GetPictureSize(string Url)
        {
            HttpClient webRequest = new HttpClient();
            using (var webResponse = webRequest.GetAsync(Url))
            {
                string[] fileSizeBytes = (string[])webResponse.Result.Content.Headers.GetValues("Content-Length");
                return Math.Round(Convert.ToDouble(fileSizeBytes[0]) / 1024.0 / 1024.0, 2);
            }
        }

        public string IsKey(string key)
        {
            if (context.Request.QueryString.Count != 0)
            {
                foreach (string item in context.Request.QueryString.Keys)
                {
                    if (item!=null && item.Equals(key))
                    {
                        return context.Request.QueryString.Get(item);
                    }
                }
            }
            return null;
        }

        public string CheckUrl()
        {
            string url = IsKey("url");
            if (url == null)
                throw new ServerExpection("Url is empty!", 422);
            if (GetPictureSize(url) > 5)
                throw new ServerExpection("Image has size than more 5MB!", 400);
            return url;
        }
    }
}
