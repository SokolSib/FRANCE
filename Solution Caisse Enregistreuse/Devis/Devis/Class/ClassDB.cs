using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace Devis.Class
{
    public class ClassDB
    {

        public ClassDB(string s)
        {
            if (s == null) this.cs = ConfigurationManager.ConnectionStrings["Devis.Properties.Settings.BDCAtestConnectionString"].ConnectionString;
            else this.cs = s;


        }

        private string cs { get; set; }


        public bool connect()
        {
            SqlConnection myConn = new SqlConnection(cs);
            try
            {
                SqlCommand myCmd = new SqlCommand("", myConn);
                if (myConn.State != System.Data.ConnectionState.Open)
                    myConn.Open();
                // myCmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException odbcEx)
            {
                if (myConn.State != System.Data.ConnectionState.Closed)
                    myConn.Close();

                new ClassLog(odbcEx.Message);
                
                return false;
            }
        }

        public List<object[]> queryResonse(string s_cmd)
        {
            List<object[]> res = new List<object[]>();

            if (ClassSync.connect)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(cs))
                    {
                        SqlCommand cm = new SqlCommand(s_cmd, cn);

                        cn.Open();

                        SqlDataReader dr = cm.ExecuteReader();

                        while (dr.Read())
                        {
                            object[] o = new object[dr.FieldCount];

                            dr.GetValues(o);

                            res.Add(o);
                        }
                        cn.Close();
                    }

                }
                catch
                {
                    new ClassLog(cs + " error ").ClassLogSQL("\"" + s_cmd + "\"");
                }
                string r = "";
                if (res.Count < 100)
                {
                    foreach (var e in res)
                    {
                        foreach (var n in e)
                        {
                            r += "<" + n.ToString() + ">";
                        }
                    }

                }
                else
                {
                    r += "Record > 100";
                }
                ClassGlobalVar.mess.Add(s_cmd + r);
            }
            else
            {
                new ClassLog(cs + " not connect ").ClassLogSQL("\"" + s_cmd + "\"");
            }
            return res;
        }

        public int queryNonResonse(string s_cmd)
        {
            int ec = -1;
            if (ClassSync.connect)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(cs))
                    {
                        SqlCommand cm = new SqlCommand(s_cmd, cn);

                        cn.Open();

                        ec = cm.ExecuteNonQuery();

                        cn.Close();
                    }
                }
                catch
                {
                    new ClassLog(cs + " error ").ClassLogSQL("\"" + s_cmd + "\"");
                }
            }
            else
            {
                new ClassLog(cs + " not connect ").ClassLogSQL("\"" + s_cmd + "\"");
            }

            ClassGlobalVar.mess.Add(s_cmd + " result " + ec);

            return ec;
        }

    }
}
