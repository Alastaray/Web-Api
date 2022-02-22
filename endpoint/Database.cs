using System;
using System.Data.SqlClient;


namespace Project
{
    class Database
    {
        SqlConnection sqlConn = null;
        public Database(string connectionStr)
        {
            sqlConn = new SqlConnection(connectionStr);
            sqlConn.Open();
        }
        ~Database()
        {
            sqlConn.Close();
        }
        public int ExecuteInsertIntoDB(string path, string name)
        {
            const string sql_insert = "INSERT INTO [Pictures] (path, name) values ";
            string sql_comm = sql_insert + "(N'" + path + "', '"+name+"')";
            SqlCommand command = new SqlCommand(sql_comm, sqlConn);
            return command.ExecuteNonQuery();
        }

        public int ExecuteDeleteFromDB(string id)
        {
            const string sql_comm = "DELETE from [Pictures] where (id) = ";
            SqlCommand command = new SqlCommand(sql_comm + id, sqlConn);
            return command.ExecuteNonQuery();
        }

        public string ExecuteSelectFromDB(string id, bool name = false)
        {
            string path;
            const string sql_comm = "SELECT * from [Pictures] where (id) = ";
            SqlCommand command = new SqlCommand(sql_comm + id, sqlConn);
            SqlDataReader sqlDataReader = command.ExecuteReader();
            try
            {
                sqlDataReader.Read();
                if(name) path = sqlDataReader["name"].ToString();
                else path = sqlDataReader["path"].ToString();
                sqlDataReader.Close();
                return path;
            }
            catch (Exception)
            {
                sqlDataReader.Close();
                throw new ServerExpection("Record doesnot found!", 404);
            }
        }
    }
}
