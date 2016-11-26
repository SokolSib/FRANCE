using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace addRealStock
{
    public partial class Establishment
    {

        public System.Guid CustomerId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string CP { get; set; }
        public string Ville { get; set; }
        public string Adresse { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }

        public static List<Establishment> sel()
        {
            List< object[]> l = new ClassDB(null).queryResonse("SELECT * FROM Establishment");

            List<Establishment> res_ = new List<Establishment>();

            foreach (var o in l)
            {
                Establishment res = new Establishment();
                res.CustomerId = (Guid)o[0];
                res.Type = (int)o[1];
                res.Name = (string)o[2];
                res.CP = (string)o[3];
                res.Ville = (string)o[4];
                res.Adresse = (string)o[5];
                res.Phone = (string)o[6];
                res.Mail = (string)o[7];
                res_.Add(res);
            }
            return res_;
        }

        public static int ins (Establishment est)
        {
            string c = "INSERT INTO Establishment (CustomerId,Type,Name,CP,Ville,Adresse,Phone,Mail) " +
                     "VALUES ('{CustomerId}','{Type}','{Name}','{CP}','{Ville}','{Adresse}','{Phone}', '{Mail}')";

            c = c.Replace("{CustomerId}", est.CustomerId.ToString())
                 .Replace("{Type}", est.Type.ToString())
                 .Replace("{Name}",est.Name)
                 .Replace("{CP}", est.CP)
                 .Replace("{Ville}", est.Ville)
                 .Replace("{Adresse}", est.Adresse)
                 .Replace("{Phone}", est.Phone)
                 .Replace("{Mail}", est.Mail);
            try
            {
                return new ClassDB(null).queryNonResonse(c);
            }

            catch
            {
                return -1;
            }
        }

    }
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

            private static List<StockReal> ListObjToStockReal(List<object[]> l)
            {
                List<StockReal> R = new List<StockReal>();
                foreach (object[] o in l)
                {
                    StockReal t = new StockReal();
                    t.CustomerId = (Guid)o[0];

                    t.QTY = (decimal)o[1];
                    t.MinQTY = (decimal)o[2];
                    t.Price = (decimal)o[3];
                    t.ProductsCustumerId = (Guid)o[4];
                    t.IdEstablishment = (Guid)o[5];
                    R.Add(t);
                }
                return R;
            }
            public static List<StockReal> sel( Guid IdEstablishment )
            {
                List<StockReal> r = new List<StockReal>();

                List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM StockReal WHERE Establishment_CustomerId='" + IdEstablishment + "'");
            
                r = ListObjToStockReal(l);

                return r;
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
