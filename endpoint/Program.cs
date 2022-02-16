using System;
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
            HttpListener listener = new HttpListener();
            string host = "http://localhost:8888/";
            listener.Prefixes.Add(host);
            listener.Start();
            Console.WriteLine("Waiting for connection!");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                Picture picture = new Picture();
                JsonContent json;
                try
                {
                    picture.DownloudPicture(request);
                    json = JsonContent.Create(new Link(host+picture.Path));
                }
                catch (PictureExpection er)
                {
                    response.StatusCode = er.StatusCode;
                    json = JsonContent.Create(new ErrorMessage(er.Message));
                }
                catch (Exception er)
                {
                    response.StatusCode = 400;
                    json = JsonContent.Create(new ErrorMessage(er.Message));
                }                     
                Stream output = response.OutputStream;
                json.CopyToAsync(output);
                output.Close();
            }
            listener.Stop();
        }
    }
}
