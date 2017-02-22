using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cns_dst.code
{
    public partial class Products
    {
  
        public System.Guid CustumerId { get; set; }
        public string Name { get; set; }
        public string CodeBare { get; set; }
        public string Desc { get; set; }
        public short Chp_cat { get; set; }
        public bool Balance { get; set; }
        public decimal Contenance { get; set; }
        public int UniteContenance { get; set; }
        public int Tare { get; set; }
        public System.DateTime Date { get; set; }
        public System.Guid TVACustumerId { get; set; }
        public System.Guid ProductsAwaitingDeliveryCustomerId { get; set; }
        public System.Guid ProductsWeb_CustomerId { get; set; }
        public int SubGrpProduct_Id { get; set; }
        public List<StockReal> SR = new List<StockReal>(); 

        public partial class StockReal
        {
            public System.Guid CustomerId { get; set; }
            public System.Guid IdProduct { get; set; }
            public System.Guid IdEstablishment { get; set; }
            public decimal QTY { get; set; }
            public decimal MinQTY { get; set; }
            public decimal Price { get; set; }
            public System.Guid ProductsCustumerId { get; set; }

            public static int ins(StockReal r)
            {
                string c = "INSERT INTO StockReal (CustomerId,Establishment_CustomerId,QTY,MinQTY,Price,ProductsCustumerId) " +
                      "VALUES ('{CustomerId}','{Establishment_CustomerId}','{QTY}','{MinQTY}','{Price}','{ProductsCustumerId}')";
                c = c.Replace("{CustomerId}", r.CustomerId.ToString());
              
                c = c.Replace("{Establishment_CustomerId}", r.IdEstablishment.ToString());
                c = c.Replace("{QTY}", r.QTY.ToString());
                c = c.Replace("{MinQTY}", r.MinQTY.ToString());
                c = c.Replace("{Price}", r.Price.ToString().Replace(",", "."));
                c = c.Replace("{ProductsCustumerId}", r.ProductsCustumerId.ToString());

                return new ClassDB(null).queryNonResonse(c);
            }
          
        }

        public static int ins (Products p)
        {
            string cmdProductWeb = "INSERT INTO ProductsWeb (CustomerId,[Visible],[Images],[ContenancePallet],[Weight],[Frozen]) VALUES ('"
                + p.ProductsWeb_CustomerId + "', 0, '', 0,0,0 )";

            new ClassDB(null).queryNonResonse(cmdProductWeb);
            string c = "INSERT INTO Products (CustumerId,Name,CodeBare,[Desc],Chp_cat,Balance,Contenance,UniteContenance,Tare,[Date],TVACustumerId,SubGrpProduct_Id,ProductsWeb_CustomerId) " +
                       "VALUES ('{CustumerId}','{Name}','{CodeBare}','{Desc}','{Chp_cat}','{Balance}','{Contenance}','{UniteContenance}','{Tare}','{Date}','{TVACustumerId}',{SubGrpProduct_Id},'{ProductsWeb_CustomerId}')";
            c = c.Replace("{CustumerId}", p.CustumerId.ToString());
            c = c.Replace("{Name}", p.Name.ToString().Replace("'","''"));
            c = c.Replace("{CodeBare}", p.CodeBare.ToString());
            c = c.Replace("{Desc}", p.Desc.ToString().Replace("'", "''"));
            c = c.Replace("{Chp_cat}", p.Chp_cat.ToString());
            c = c.Replace("{Balance}", p.Balance.ToString());
            c = c.Replace("{Contenance}", p.Contenance.ToString().Replace(",", "."));
            c = c.Replace("{UniteContenance}", p.UniteContenance.ToString());
            c = c.Replace("{Tare}", p.Tare.ToString());
            c = c.Replace("{Date}", p.Date.ToString());
            c = c.Replace("{TVACustumerId}", p.TVACustumerId.ToString());
          //  c = c.Replace("{ProductsAwaitingDeliveryCustomerId}", p.ProductsAwaitingDeliveryCustomerId.ToString());
            c = c.Replace("{SubGrpProduct_Id}", p.SubGrpProduct_Id.ToString());
            c = c.Replace("{ProductsWeb_CustomerId}", p.ProductsWeb_CustomerId.ToString());

            int r =+ new ClassDB(null).queryNonResonse(c);
        
            r =+ StockReal.ins(p.SR.First());

         

            return r;
        }

        public static Guid getProdCustomerId (string CodeBar)
        {
            string cmd = "SELECT CustumerId FROM Products WHERE CodeBare ='" + CodeBar + "'";

            List<object[]> o = new ClassDB(null).queryResonse(cmd);

            if (o.Count > 0)
                return (Guid)o[0][0];
            else
                return Guid.Empty;
        }
       
    }
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
