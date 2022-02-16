using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Picture
    {
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
            return false;
        }
        public bool DownloudPicture(HttpListenerRequest request)
        {
            if (Url == null)
            {   
                if(!SetUrl(request))return false;    
            };
            return true;
        }
        public string Url { get; set; }
        public string Path { get; set; }
    }
}
