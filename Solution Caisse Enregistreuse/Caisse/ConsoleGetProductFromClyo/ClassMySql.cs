using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGetProductFromClyo
{
    public class ClassMySqlDb
    {

        private string cs = "";
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public ClassMySqlDb(string s)
        {

            server = "127.0.0.1";
            database = "dbclyo";
            uid = "root";
            password = "123456";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            if (s == null)
                s = connectionString;

            connection = new MySqlConnection(s);

        }

        public string error = "";

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        error += ("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        error += ("Invalid username/password, please try again");
                        break;

                    default: error += ex.Message;
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                error += (ex.Message);
                return false;
            }
        }

        public List<object[]> QueryResponse(string query)
        {
            List<object[]> res = new List<object[]>();



            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        object[] o = new object[dr.FieldCount];

                        dr.GetValues(o);

                        res.Add(o);
                    }
                    dr.Close();
                    this.CloseConnection();
                    return res;
                }

                catch (MySqlException ex)
                {
                    error += ex.Message;

                    return null;
                }
            }
            else
            {
                return res;
            }
        }

    }
}
