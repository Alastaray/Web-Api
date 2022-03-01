using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Endpoint.Models;

namespace Endpoint.Controllers.Requests
{
    internal class RequestUploadImage : ServerController
    {
        public RequestUploadImage(DatabaseController _database, HttpListenerContext _context) : base(_database, _context)
        {
        }
        public override JsonContent ExecuteRequest(string host)
        {
            try
            {
                string url = CheckUrl();
                ImageController imageController = new ImageController();
                if (!imageController.Download(url)) throw new ServerExpection("Image already exists!", 400);
                imageController.Cut(100);
                imageController.Cut(300);

                string image_name = imageController.imageModel.Name,
                    image_path = imageController.imageModel.Path;

                if (Database.ExecuteInsertIntoDB(image_name, image_name) != 1)
                    throw new ServerExpection("Bad request to database!", 409);
                return JsonContent.Create(new LinkModel(host + image_name + image_name));
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

        public double GetPictureSize(string Url)
        {
            HttpClient webRequest = new HttpClient();
            using (var webResponse = webRequest.GetAsync(Url))
            {
                string[] fileSizeBytes = (string[])webResponse.Result.Content.Headers.GetValues("Content-Length");
                return Math.Round(Convert.ToDouble(fileSizeBytes[0]) / 1024.0 / 1024.0, 2);
            }
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
