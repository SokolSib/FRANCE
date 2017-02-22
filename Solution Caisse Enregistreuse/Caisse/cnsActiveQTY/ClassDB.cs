using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;


namespace cnsActiveQTY
{
    public class ClassDB
    {

        public ClassDB(string s)
        {
            if (s == null) this.cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            else this.cs = s;


        }

        private string cs { get; set; }



        public List<object[]> queryResonse(string s_cmd)
        {
            List<object[]> res = new List<object[]>();

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
            return res;
        }

        public int queryNonResonse(string s_cmd)
        {
            int ec = -1;

            using (SqlConnection cn = new SqlConnection(cs))
            {
                SqlCommand cm = new SqlCommand(s_cmd, cn);

                cn.Open();

                ec = cm.ExecuteNonQuery();

                cn.Close();
            }

            return ec;
        }

    }
}
