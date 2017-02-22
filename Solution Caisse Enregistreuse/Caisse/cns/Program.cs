using cns.code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cns
{
    class Program
    {

       /* static void r_xml_file()
        {
           string path = @"D:\Product.xml";
         //  string pat = @"D:\GroupProduct.xml";
           XDocument x = XDocument.Load(path);
           IEnumerable<XElement> elms = x.Element("Product").Elements("rec");
           int i = 0;
            foreach(XElement e in elms)
            {
                data.Products p = new data.Products();
                p.CustumerId = Guid.NewGuid();
                e.Element("id").Value = p.CustumerId.ToString();
                p.Name = e.Element("name").Value;
                p.Price = Convert.ToDecimal( e.Element("price").Value.Replace(".",","));
                p.Tva = Convert.ToByte(e.Element("tva").Value);
                p.QTY = Convert.ToInt32(e.Element("qty").Value);
                p.CodeBare = e.Element("code_bare").Value;
                p.Group = Convert.ToByte(e.Element("chp_fam").Value == "-1" ? "0" : e.Element("chp_fam").Value);
                p.SubGruop = Convert.ToByte(e.Element("chp_ss_fam").Value == "-1" ? "0" : e.Element("chp_ss_fam").Value);
                p.Balance = Convert.ToBoolean(e.Element("balance").Value);
                p.Contenance = Convert.ToDecimal(e.Element("contenance").Value.Replace(".", ","));
                p.UniteContenance = Convert.ToDecimal(e.Element("uniteContenance").Value.Replace(".", ","));
                p.Tare = Convert.ToDecimal(e.Element("tare").Value.Replace(".", ","));
                p.Desc = e.Element("desc").Value;
                p.Date = DateTime.Now;
                e.Element("date").Value = p.Date.ToString();
                new ClassBD.ClassProduct().add(p);
                Console.WriteLine(i++);
            }

            x.Save(path);
             
          
        }
        */
        public static int GetLuhnSecureDigital(string Num, out int sum1)
        {
            int p = 0;
            int sum = 0;
            int N = Num.Length;

            for (int i = 0; i < N; i++)
            {
                p = Num[(N - 1) - i] - '0';
                if (i % 2 != 0)
                {
                    p = 2 * p;
                    if (p > 9) 
                        
                        p = p - 9;

                }
                sum = sum + p;
            }

            sum1 =  int.Parse( Math.Truncate( (double) (sum / 10)).ToString());

            sum = (10 - (sum % 10));

            

            return sum;
        }

        private static void A ()
        {
            decimal d = 111224130612;
            

            int i = 0;

            while (d < 999999999999)
            {
                d += 33;



                string s = d.ToString();

                int lastNum = int.Parse(s.Substring(s.Length - 1, 1)) + int.Parse(s.Substring(0, 1));

                int sum1 = 0;

                int sum = GetLuhnSecureDigital(s, out sum1);

                if ((lastNum == sum1) && (sum == 10))
                {
                    string nc = d.ToString();
                    string o = "";
                    for (int j = 0; j < nc.Length; j++)
                    {
                        o += nc[j];
                        if (((j + 1) % 3) == 0)

                            o += "-";

                    }
                    new ClassBD.ClassProduct().addDisc(o.Remove(o.Length - 1, 1));
                    Console.WriteLine(o.Remove(o.Length - 1, 1));
                    i++;
                }
                if (i == 3500) break;

            }
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            A();
        }
    }
}
