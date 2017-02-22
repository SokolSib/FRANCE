using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleGetProductFromClyo
{
    class Program
    {
        static ClassMySqlDb db = new ClassMySqlDb(null);
        static string path = @"D:\Product.xml";
        static string pat = @"D:\GroupProduct.xml";
        class product
        {
            public int id { get; set; }
            public string name { get; set; }
            public int tva { get; set; }
            public decimal price { get; set; }
            public string code_bare { get; set; }
            public int chp_cat { get; set; }
            public string chp_cat_s { get; set; }
            public int chp_fam { get; set; }
            public string chp_fam_s { get; set; }
            public int chp_ss_fam { get; set; }
            public string chp_ss_fam_s { get; set; }
            public int qty { get; set; }
            public bool balance {get;set;}
            public decimal contenance { get; set; }
            public int uniteContenance { get; set; }
            public decimal tare { get; set; }
            public string desc { get; set; }
        }
        class group
        {
            public class subGroup 
            {
                public int id { get; set; }
                public int id_group { get; set; }
                public string name { get; set; }

            }
            public int id{get;set;}
            public string name { get; set; }
            public subGroup[] sg { get; set; }
            public group (object[] o)
            {
                this.id = (int)o[0];
                this.name = (string)o[1];
                List<object[]> list = db.QueryResponse("SELECT num_ss_fam ,des, num_fam from tbl_ss_famille where num_fam ='" + this.id +"'");

                if (list != null)
                {
                    sg = new subGroup[list.Count];

                    for (int i = 0; i < sg.Length; i++)
                    {
                        subGroup d = new subGroup();

                        d.id = (int)list[i][0];
                        d.id_group = (int)list[i][2];
                        d.name = (string)list[i][1];

                        sg[i] = d;
                    }
                }
            }
        }
        static List<product> prod;
        static List<group> groups;

        static string _chp_cat_s(int chp_cat)
        {
         
            return "";
        }
        static string _chp_fam_s(int chp_fam)
        {
            return "";
        }
        static string _chp_ss_fam_s(int chp_fam)
        {
            return "";
        }
        static void getData ()
        {
            List<object[]> list = db.QueryResponse("SELECT chp_des,tva1, prix_carte, code_ean, chp_cat,chp_fam,chp_ss_fam,chp_tare,chp_balance,contenance,ut_contenance,chp_des_longue FROM corres_des");

                prod = new List<product>();

                foreach (object[] o in list)
                {
                    product p = new product();

                    p.id = prod.Count;

                    p.name = (string)o[0];

                    p.tva = (int)o[1];

                    p.price = Convert.ToDecimal( o[2]);

                    p.code_bare = (string)o[3];

                    p.chp_cat = Convert.ToInt16( o[4]);

                    p.chp_cat_s = _chp_cat_s(p.chp_cat);

                    try
                    {
                        p.chp_fam = Convert.ToInt16(o[5]);
                    }

                    catch
                    {
                        p.chp_fam = -1;
                    }
                    p.chp_fam_s = _chp_fam_s(p.chp_fam);

                    try
                    {
                        p.chp_ss_fam = Convert.ToInt16(o[6]);
                    }
                    catch
                    {
                        p.chp_ss_fam = -1;
                    }

                    p.chp_ss_fam_s = _chp_ss_fam_s(p.chp_ss_fam);


                    p.tare = Convert.ToDecimal( o[7]);

                    p.balance = ((int)o[8] == 1) ? true : false;

                    p.contenance =  Convert.ToDecimal( o[9]);

                    p.uniteContenance = Convert.ToInt16( o[10]);

                    p.desc = o[11].ToString();

                    if (p.price > 0) prod.Add(p);

                    Console.WriteLine("{0}//{1}//{2}//{3}", p.id, p.name, p.price, p.code_bare);
                }

        }
        static void saveXML()
        {
            XDocument x = new XDocument();

            x.Add(new XElement("Product"));

            foreach (product p in prod)

            x.Element("Product").Add(
                new XElement("rec",
                    new XElement("id", p.id),
                    new XElement("Name", p.name),
                    new XElement("price", p.price),
                    new XElement("tva", p.tva),
                    new XElement("qty", p.qty),
                    new XElement("CodeBare", p.code_bare),
                    new XElement("chp_cat", p.chp_cat),
                    new XElement("chp_cat_s", p.chp_cat_s),
                    new XElement("chp_fam", p.chp_fam),
                    new XElement("chp_fam_s", p.chp_fam_s),
                    new XElement("chp_ss_fam", p.chp_ss_fam),
                    new XElement("chp_ss_fam_s", p.chp_ss_fam_s),
                    new XElement("balance", p.balance),
                    new XElement("contenance", p.contenance),
                    new XElement("uniteContenance", p.uniteContenance),
                    new XElement("tare", p.tare),
                    new XElement("Desc", p.desc),
                    new XElement("date", DateTime.Now)
                    )
                );

            Console.WriteLine("Save data to {0}",path);

            x.Save(path);
        }

        static void getGroup ()
        {
            List<object[]> list = db.QueryResponse("SELECT num_fam, des FROM tbl_famille");

            groups = new List<group>();

            if (list != null) 

            foreach (object[] o in list)
            {
                groups.Add(new group(o));
            }
        }

        static void saveGroup()
        {
             XDocument x = new XDocument();

            x.Add(new XElement("Palettes"));

            foreach (group g in groups)
            {
                x.Element("Palettes").Add(new XElement("Palette", 
                    new XElement ("Group", new XAttribute("Name",g.name) , new XAttribute("ID",g.id) )
                    )
                    );
                for (int i = 0; i < g.sg.Length; i++)
                {
                    x.Element("Palettes").Elements("Palette").Where(l => l.Element("Group").Attribute("Name").Value == g.name).SingleOrDefault()
                        .Add(new XElement("SubGroup", new XAttribute("Name", g.sg[i].name), new XAttribute("ID", g.sg[i].id)));
                }
            }

            x.Save(pat);
        }

        static void Main(string[] args)
        {

            getGroup();

            saveGroup();

            getData();

            saveXML();

            Console.ReadLine();
        }
    }
}
