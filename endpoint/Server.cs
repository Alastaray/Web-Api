using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http.Json;

namespace Project
{
    class Server
    {
        public string Host { get;}
        private HttpListener listener = null;
        SqlConnection sqlConn = null;
        private const string connStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Администратор\\source\\repos\\endpoint\\endpoint\\DB.mdf;Integrated Security=True";
        private const string requestSelect = "SELECT * from [Pictures] where (id) = 3";
        private const string requestInsert = "INSERT INTO [Pictures] (path, new_path) values ('ytryrt', 'ytryrt')";
        private const string requestDelete = "DELETE from [Pictures] where (id) = 3";

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
                JsonContent json = RequestDownload(context);
                
                Stream output = context.Response.OutputStream;
                json.CopyToAsync(output);
                output.Close();
            }            
        }
        public JsonContent RequestDownload(HttpListenerContext context)
        {
            Picture picture = new Picture();
            try
            {
                picture.Downloud(context.Request);
                return JsonContent.Create(new Link(Host + picture.NewPath));
            }
            catch (PictureExpection er)
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
    }
}
