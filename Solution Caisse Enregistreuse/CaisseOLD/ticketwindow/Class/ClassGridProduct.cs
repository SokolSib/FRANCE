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
    class ClassGridProduct
    {
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\GridProduct.xml";
        public ClassGridProduct()
        {
            load(path);
        }
        public class elm
        {
            public byte x { get; set; }
            public byte y { get; set; }
            public Guid customerId { get; set; }
            public string Description { get; set; }
            public string DescriptionLong { get; set; }
            public string prix { get; set; }
            public string group { get; set; }
            public string subgroup { get; set; }
            public string codeBare { get; set; }
            public string Contenance { get; set; }
            public string UniteContenance { get; set; }
            public string Tare { get; set; }
            public bool Ballance { get; set; }
            public System.Windows.Media.Brush background { get; set; }
            public Image img { get; set; }
            public System.Windows.Media.Brush font { get; set; }
        }
        public static elm[,,,] grid = new elm[12, 12, 12, 12];
        private void load(string path)
        {
            XDocument xmlGrid = XDocument.Load(path);

            for (int I = 0; I < 12; I++)
            {
                for (int J = 0; J < 12; J++)
                {
                    List<XElement> x_elms = (from el in xmlGrid.Elements("GridProduct").Elements("_" + I + "x" + J).Elements("rec")
                                             select el).ToList();

                    elm[] e = new elm[x_elms.Count];

                    foreach (XElement x in x_elms)
                    {
                        elm f = new elm();
                        f.x = byte.Parse(x.Element("X").Value);
                        f.y = byte.Parse(x.Element("Y").Value);
                        f.Description = x.Element("Description").Value;
                        string[] rgbt;
                        string[] rgbt_f;

                        try
                        {
                            rgbt_f = x.Element("font").Value.Split(',');
                            if (rgbt_f.Length == 3)
                                f.font = new SolidColorBrush(Color.FromRgb(byte.Parse(rgbt_f[0]), byte.Parse(rgbt_f[1]), byte.Parse(rgbt_f[2])));
                            else
                                f.font = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                        catch
                        {
                            f.font = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                        try
                        {
                            rgbt = x.Element("background").Value.Split(',');
                            if (rgbt.Length == 3)
                                f.background = new SolidColorBrush(Color.FromRgb(byte.Parse(rgbt[0]), byte.Parse(rgbt[1]), byte.Parse(rgbt[2])));
                            else
                                f.background = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        }
                        catch
                        {
                            f.background = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        }

                        f.customerId = x.Element("customerId") != null ?
                            Guid.Parse(x.Element("customerId").Value)
                            : Guid.Empty;
                        if (f.customerId == Guid.Empty)
                        {
                            new ClassLog(f.Description);
                        }
                        grid[I, J, f.x, f.y] = f;
                    }
                }
            }
        }


        public void save(elm el, int X, int Y)
        {



            grid[X, Y, Convert.ToInt16(el.x), Convert.ToInt16(el.y)] = el;

            SolidColorBrush solid = (el.background as SolidColorBrush);
            SolidColorBrush solid2 = (el.font as SolidColorBrush);

            string colorText = solid != null ? solid.Color.R + "," + solid.Color.G + "," + solid.Color.B : "255,0,0";

            string colorFontText = solid2 != null ? solid2.Color.R + "," + solid2.Color.G + "," + solid2.Color.B : "255,0,0";

            var doc = XDocument.Load(path);

            XElement target;

            try
            {
                target = doc.Elements("GridProduct").Elements("_" + X + "x" + Y).Elements("rec")
                     .Where(e => (e.Element("X").Value == el.x.ToString()) & (e.Element("Y").Value == el.y.ToString()))
                    .SingleOrDefault();
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
                XElement Description_ = (d[4] as XElement);
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

                Description_.Value = el.Description;
                background_.Value = colorText;
                // img_.Value = el.img;
                font_.Value = colorFontText;

            }

            else
            {
                XElement e = doc.Element("GridProduct").Element("_" + X + "x" + Y);

                if (e != null)
                {
                    e.Add(
                  new XElement("rec",
                  new XElement("id", (el.x * el.y).ToString()),
                  new XElement("Date_upd", DateTime.Now.ToString()),
                  new XElement("X", el.x),
                  new XElement("Y", el.y),
                  new XElement("Description", el.Description),

                  new XElement("background", colorText),
                  new XElement("img", el.img),
                  new XElement("font", el.font),
                   new XElement("customerId", el.customerId)
                  ))
                  ;
                }
                else
                {

                    doc.Element("GridProduct").Add(
                      new XElement("_" + X + "x" + Y,
                      new XElement("rec",
                      new XElement("id", (el.x * el.y).ToString()),
                      new XElement("Date_upd", DateTime.Now.ToString()),
                      new XElement("X", el.x),
                      new XElement("Y", el.y),
                      new XElement("Description", el.Description),
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

            ClassXMLDB.saveDB(path, doc);
        }


    }
}
