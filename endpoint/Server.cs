using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http.Json;

namespace Project
{
    public class ServerExpection : System.Exception
    {
        public ServerExpection(string message, int status_code) : base(message)
        {
            StatusCode = status_code;
        }
        public int StatusCode { get; set; }
    }

    class Server
    {
        public string Host { get;}
        private HttpListener listener = null;
        SqlConnection sqlConn = null;       
        private const string connStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Администратор\\source\\repos\\endpoint\\endpoint\\DB.mdf;Integrated Security=True";

        public Server(string host)
        {
            Host = host;
            listener = new HttpListener();
            sqlConn = new SqlConnection(connStr);         
            listener.Prefixes.Add(Host);
            listener.Start();
            sqlConn.Open();
        }
        ~Server()
        {
            listener.Stop();
            sqlConn.Close();
        }

        public void Start()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                JsonContent json;
                try
                {
                    json = ExecuteRequest(context);
                    if (json == null) break;
                }
                catch (ServerExpection er)
                {
                    context.Response.StatusCode = er.StatusCode;
                    json = JsonContent.Create(new ErrorMessage(er.Message));
                }             
                Stream output = context.Response.OutputStream;
                json.CopyToAsync(output);
                output.Close();
            }            
        }

        public JsonContent ExecuteRequest(HttpListenerContext context)
        {
            switch (CheckRequest(context.Request))
            {
                case "upload-by-url":
                    return DownloadPicture(context);
                case "get-url":
                    return DownloadPicture(context);
                case "get-new-url":
                    return DownloadPicture(context);
                case "remove":
                    return DownloadPicture(context);
                default:
                    return null;
            }
        }

        public string CheckRequest(HttpListenerRequest request)
        {
            string[] str_request = request.RawUrl.Replace('?', '/').Split("/");
            string end = "end";
            string[] possible_requests = { "upload-by-url", "get-url", "get-new-url", "remove" };
            if (str_request.Length != 0)
            {
                if (str_request[1].Equals(end)) return end;
                if (str_request[1].Equals("api"))
                {
                    foreach (var item in possible_requests)
                    {
                        if (str_request[2].Equals(item)) return item;
                    }
                }
            }               
            throw new ServerExpection("Incorrect request!", 400);
        }

        public int ExecuteInsertIntoDB(string path, string new_path)
        {
            const string sql_insert = "INSERT INTO [Pictures] (path, new_path) values ";
            string sql_comm = sql_insert + "(N'" + path + "'," + "N'" + new_path + "')";
            SqlCommand command = new SqlCommand(sql_comm, sqlConn);
            return command.ExecuteNonQuery();
        }

        public int ExecuteDeleteFromDB(int id)
        {
            const string sql_comm = "DELETE from [Pictures] where (id) = ";
            SqlCommand command = new SqlCommand(sql_comm + id, sqlConn);
            return command.ExecuteNonQuery();
        }

        public string ExecuteSelectFromDB(int id, bool _new)
        {
            const string sql_comm = "SELECT * from [Pictures] where (id) = ";
            SqlCommand command = new SqlCommand(sql_comm + id, sqlConn);
            SqlDataReader sqlDataReader = command.ExecuteReader();
            sqlDataReader.Read();
            if (_new) return sqlDataReader["path"].ToString();
            else return sqlDataReader["new_path"].ToString();
        }

        public JsonContent DownloadPicture(HttpListenerContext context)
        {           
            try
            {
                string url = CheckUrl(context.Request);
                Picture picture = new Picture();
                picture.Download(url);
                picture.Cut();
                if (ExecuteInsertIntoDB(picture.Path, picture.NewPath) != 1) 
                        throw new ServerExpection("Bad request to database!", 409);
                return JsonContent.Create(new Link(Host + picture.NewPath));
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
            var webRequest = HttpWebRequest.Create(Url);
            webRequest.Method = "HEAD";
            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                return Math.Round(Convert.ToDouble(fileSize) / 1024.0 / 1024.0, 2);
            }
        }

        public bool IsUrl(HttpListenerRequest request, ref string url)
        {
            if (request.QueryString.Count != 0)
            {
                foreach (string key in request.QueryString.Keys)
                {
                    if (key.Equals("url"))
                    {
                        url = request.QueryString.Get(key);
                        return true;
                    }
                }
            }
            return false;
        }

        public string CheckUrl(HttpListenerRequest request)
        {
            string url = String.Empty;
            if (!IsUrl(request, ref url))
                throw new ServerExpection("Incorrect url!", 422);
            if (GetPictureSize(url) > 5)
                throw new ServerExpection("Picture has size than more 5MB!", 400);
            return url;
        }

        /*public JsonContent DeletePicture(HttpListenerContext context)
        {
            //File.Delete(String);
        }*/
        /* public JsonContent GetPathPicture(HttpListenerContext context)
         {

         }
         public JsonContent GetNewPathPicture(HttpListenerContext context)
         {

         }*/
    }
}
