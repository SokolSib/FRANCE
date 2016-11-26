using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    class ClassGridStatistique_Region_et_Pays
    {

        static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\GridStatistique_Region_et_Pays.xml";
            public ClassGridStatistique_Region_et_Pays()
            {
                load(path);
            }
            public class elm
            {
                public byte x { get; set; }
                public byte y { get; set; }
                public Guid customerId { get; set; }
                public string NameCountry { get; set; }
                public string Capital { get; set; }
                public string Continent { get; set; }
                public System.Windows.Media.Brush background { get; set; }
                public Image img { get; set; }
                public string font { get; set; }
            }
            public static elm[, , ,] grid = new elm[12, 12, 12, 12];
            private void load(string path)
            {
                XDocument xmlGrid = XDocument.Load(path);

                for (int I = 0; I < 12; I++)
                {
                    for (int J = 0; J < 12; J++)
                    {
                        List<XElement> x_elms = (from el in xmlGrid.Elements("GridStatistique_Region_et_Pays").Elements("_" + I + "x" + J).Elements("rec")
                                                 select el).ToList();

                        elm[] e = new elm[x_elms.Count];

                        foreach (XElement x in x_elms)
                        {
                            elm f = new elm();
                            f.x = byte.Parse(x.Element("X").Value);
                            f.y = byte.Parse(x.Element("Y").Value);
                            f.NameCountry = x.Element("NameCountry").Value;
                            string[] rgbt = x.Element("background").Value.Split(',');
                            try
                            {
                                f.background = new SolidColorBrush(Color.FromRgb(byte.Parse(rgbt[0]), byte.Parse(rgbt[1]), byte.Parse(rgbt[2])));
                            }
                            catch
                            {
                                f.background = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                            }

                            f.font = x.Element("font").Value;
                            f.customerId = x.Element("customerId") != null ? Guid.Parse(x.Element("customerId").Value) : Guid.Empty;
                            grid[I, J, f.x, f.y] = f;
                        }
                    }
                }
            }


            public void save(elm el, int X, int Y)
            {



                grid[X, Y, Convert.ToInt16(el.x), Convert.ToInt16(el.y)] = el;

                SolidColorBrush solid = (el.background as SolidColorBrush);

                string colorText = solid != null ? solid.Color.R + "," + solid.Color.G + "," + solid.Color.B : "255,0,0";

                var doc = XDocument.Load(path);

                XElement target;

                try
                {
                    target = doc.Elements("GridStatistique_Region_et_Pays").Elements("_" + X + "x" + Y).Elements("rec")
                         .Where(e => (e.Element("X").Value == el.x.ToString()) & (e.Element("Y").Value == el.y.ToString()))
                        .Single();
                }

                catch
                {
                    target = null;
                }

                if (target != null)
                {

                    XNode[] d = target.Nodes().ToArray();
                    XElement id_ = (d[0] as XElement);
                    XElement Date_upd_ = (d[1] as XElement);
                    XElement X_ = (d[2] as XElement);
                    XElement Y_ = (d[3] as XElement);
                    XElement NameCountry_ = (d[4] as XElement);
                    XElement background_ = (d[5] as XElement);
                    XElement img_ = (d[6] as XElement);
                    XElement font_ = (d[7] as XElement);

                    XElement customerId_;
                    if (d.Length > 8)
                    {
                        customerId_ = (d[8] as XElement);
                        customerId_.Value = el.customerId.ToString();
                    }
                    else
                    {
                        target.Add(new XElement("customerId", el.customerId));

                    }
                    Date_upd_.Value = DateTime.Now.ToString();

                    NameCountry_.Value = el.NameCountry;
                    background_.Value = colorText;
                    // img_.Value = el.img;
                    font_.Value = el.font;

                }

                else
                {
                    XElement e = doc.Element("GridStatistique_Region_et_Pays").Element("_" + X + "x" + Y);

                    if (e != null)
                    {
                        e.Add(
                      new XElement("rec",
                      new XElement("id", (el.x * el.y).ToString()),
                      new XElement("Date_upd", DateTime.Now.ToString()),
                      new XElement("X", el.x),
                      new XElement("Y", el.y),
                      new XElement("NameCountry", el.NameCountry),

                      new XElement("background", colorText),
                      new XElement("img", el.img),
                      new XElement("font", el.font),
                       new XElement("customerId", el.customerId)
                      ))
                      ;
                    }
                    else
                    {

                        doc.Element("GridStatistique_Region_et_Pays").Add(
                          new XElement("_" + X + "x" + Y,
                          new XElement("rec",
                          new XElement("id", (el.x * el.y).ToString()),
                          new XElement("Date_upd", DateTime.Now.ToString()),
                          new XElement("X", el.x),
                          new XElement("Y", el.y),
                          new XElement("NameCountry", el.NameCountry),
                          new XElement("background", colorText),
                          new XElement("img", el.img),
                          new XElement("font", el.font),
                           new XElement("customerId", el.customerId)
                          )
                           )
                          );
                    }
                }
                doc.Save(path);

            }
        }
    
}
