﻿using System;
using System.Drawing;
using System.IO;
using System.Net;



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
        public string Url { get; private set; }
        public string Path { get; private set; }
        public string NewPath { get; private set; }
        public string Name { get; private set; }

        public bool SetUrl(HttpListenerRequest request)
        {
            if (request.QueryString.Count != 0)
            {
                foreach (string key in request.QueryString.Keys)
                {
                    if (key.Equals("url"))
                    {
                        Url = request.QueryString.Get(key);
                        return true;
                    }
                }
            }
            throw new PictureExpection("Incorrect url!", 400);
        }

        public void Downloud(HttpListenerRequest request)
        {
            if (Url == null)
            {   
                if(!SetUrl(request)) 
                    throw new PictureExpection("Incorrect url!", 400);    
            };

            if(GetPictureSize(Url) > 5) 
                throw new PictureExpection("Picture has size than more 5MB!", 400);

            try
            {
                WebClient client = new WebClient();
                string[] names = Url.Split(new char[] { '/' });
                Name = names[names.Length - 1];
                try
                {                                   
                    Path = Directory.GetCurrentDirectory()+"\\Images\\" + Name;
                    client.DownloadFile(Url, Path);
                }
                catch (WebException)
                {
                    Path = Directory.GetCurrentDirectory()+"\\"+Name;
                    client.DownloadFile(Url, Path);
                }
                Cut();
            }
            catch (WebException e)
            {
                throw new PictureExpection(e.Message, 400);
            }
            
        }

        public double GetPictureSize(string Url)
        {
            var webRequest = HttpWebRequest.Create(Url);
            webRequest.Method = "HEAD";
            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                return Math.Round(Convert.ToDouble(fileSize) / 1024.0 / 1024.0, 2);
            }
        }

        public Size GetNewPictureSize(Size size)
        {
            int width = size.Width,
               height = size.Height,
               width_modifier = width / 100,
               height_modifier = height / 100,
               modifier = width_modifier.Equals(height_modifier) ? width_modifier : height_modifier;
            return new Size(width / modifier, height / modifier);
        }

        public void Cut()
        {
            using (Bitmap bitmap = new Bitmap(Path))
            {
                Size size = GetNewPictureSize(bitmap.Size);
                using (Bitmap newBitmap = new Bitmap(bitmap, size))
                {
                    try
                    {
                        NewPath = Directory.GetCurrentDirectory() + "\\Images\\new_" + Name;
                        newBitmap.Save(NewPath);
                    }
                    catch (Exception)
                    {
                        NewPath = Directory.GetCurrentDirectory() + "\\new_" + Name;
                        newBitmap.Save(NewPath);
                    }
                    
                }
            }
        }
    }
}
