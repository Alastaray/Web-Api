using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;

namespace Project
{

    public class Error
    {
        public Error(string what)
        {
            error = what;
        }
        public string error { get; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/");
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerResponse response = context.Response;
                Picture picture = new Picture();
                JsonContent json;
                if (picture.SetUrl(context.Request))
                {
                    json = JsonContent.Create(picture);
                }
                else
                {
                    response.StatusCode = 400;
                    json = JsonContent.Create(new Error("Incorrect url!"));
                }            
                Stream output = response.OutputStream;
                json.CopyToAsync(output);
                output.Close();
            }
            listener.Stop();
        }
    }
}
