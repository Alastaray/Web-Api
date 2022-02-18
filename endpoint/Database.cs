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
        public int ExecuteInsertIntoDB(string path, string new_path)
        {
            const string sql_insert = "INSERT INTO [Pictures] (path, new_path) values ";
            string sql_comm = sql_insert + "(N'" + path + "'," + "N'" + new_path + "')";
            SqlCommand command = new SqlCommand(sql_comm, sqlConn);
            return command.ExecuteNonQuery();
        }

        public int ExecuteDeleteFromDB(string id)
        {
            const string sql_comm = "DELETE from [Pictures] where (id) = ";
            SqlCommand command = new SqlCommand(sql_comm + id, sqlConn);
            return command.ExecuteNonQuery();
        }

        public string ExecuteSelectFromDB(string id, bool _new)
        {
            try
            {
                string path;
                const string sql_comm = "SELECT * from [Pictures] where (id) = ";
                SqlCommand command = new SqlCommand(sql_comm + id, sqlConn);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                sqlDataReader.Read();
                if (_new) path = sqlDataReader["new_path"].ToString();
                else path = sqlDataReader["path"].ToString();
                sqlDataReader.Close();
                return path;
            }
            catch (Exception)
            {
                throw new ServerExpection("Record doesnot found!", 404);
            }
        }
    }
}
