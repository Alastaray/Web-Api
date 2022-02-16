﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class PictureExpection : System.Exception
    {
        public PictureExpection(string message, int status_code) : base(message)
        {
            StatusCode = status_code;
        }
        public int StatusCode { get; set; }
    }

    public class Picture
    {
        public string Url { get; set; }
        public string Path { get; set; }

        public bool SetUrl(HttpListenerRequest request)
        {
            if (request.QueryString.Count != 0)
            {
                foreach (string key in request.QueryString.Keys)
                {
                    if (key == "url")
                    {
                        Url = request.QueryString.Get(key);
                        return true;
                    }
                }
            }
            throw new PictureExpection("Incorrect url!", 400);
        }
        public bool DownloudPicture(HttpListenerRequest request)
        {
            if (Url == null)
            {   
                if(!SetUrl(request)) throw new PictureExpection("Incorrect url!", 400);    
            };
            try
            {
                WebClient client = new WebClient();
                string[] names = Url.Split(new char[] { '/' });//"Images/" +
                string filename = names[names.Length - 1];
                try
                {                                   
                    Path = "Images//" + filename;
                    client.DownloadFile(Url, Path);
                }
                catch (WebException)
                {
                    Path = filename;
                    client.DownloadFile(Url, Path);
                }               
            }
            catch (Exception e)
            {
                throw new PictureExpection(Path, 400);
            }
            return true;
        }
        
    }
}
