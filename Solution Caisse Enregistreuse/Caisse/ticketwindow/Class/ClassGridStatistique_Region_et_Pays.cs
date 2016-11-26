using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.Class
{
    internal class ClassGridStatistiqueRegionEtPays
    {
        private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + @"\Data\GridStatistique_Region_et_Pays.xml";
        public static Elm[,,,] Grid = new Elm[12, 12, 12, 12];

        public static void Initialize()
        {
            Load(Path);
        }

        private static void Load(string path)
        {
            var xmlGrid = XDocument.Load(path);

            for (var i = 0; i < 12; i++)
            {
                for (var j = 0; j < 12; j++)
                {
                    var xElms = (from el in xmlGrid.Elements("GridStatistique_Region_et_Pays").Elements("_" + i + "x" + j).Elements("rec")
                        select el).ToList();
                    
                    foreach (var x in xElms)
                    {
                        var f = new Elm
                                {
                                    X = byte.Parse(x.GetXElementValue("X")),
                                    Y = byte.Parse(x.GetXElementValue("Y")),
                                    NameCountry = x.GetXElementValue("NameCountry")
                                };
                        var rgbt = x.GetXElementValue("background").Split(',');
                        try
                        {
                            f.Background = new SolidColorBrush(Color.FromRgb(byte.Parse(rgbt[0]), byte.Parse(rgbt[1]), byte.Parse(rgbt[2])));
                        }
                        catch
                        {
                            f.Background = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        }

                        f.Font = x.GetXElementValue("font");
                        f.CustomerId = x.Element("customerId") != null ? Guid.Parse(x.GetXElementValue("customerId")) : Guid.Empty;
                        Grid[i, j, f.X, f.Y] = f;
                    }
                }
            }
        }

        public void Save(Elm el, int x, int y)
        {
            Grid[x, y, Convert.ToInt16(el.X), Convert.ToInt16(el.Y)] = el;
            var solid = (el.Background as SolidColorBrush);
            var colorText = solid != null ? solid.Color.R + "," + solid.Color.G + "," + solid.Color.B : "255,0,0";
            var doc = XDocument.Load(Path);

            XElement target;

            try
            {
                target =
                    doc.Elements("GridStatistique_Region_et_Pays").Elements("_" + x + "x" + y).Elements("rec").Single(
                        e => (e.GetXElementValue("X") == el.X.ToString()) & (e.GetXElementValue("Y") == el.Y.ToString()));
            }
            catch
            {
                target = null;
            }

            if (target != null)
            {
                var d = target.Nodes().ToArray();
                var dateUpd = (d[1] as XElement);
                var nameCountry = (d[4] as XElement);
                var background = (d[5] as XElement);
                var font = (d[7] as XElement);

                if (d.Length > 8)
                {
                    var customerId = (d[8] as XElement);
                    customerId.Value = el.CustomerId.ToString();
                }
                else
                {
                    target.Add(new XElement("customerId", el.CustomerId));
                }
                dateUpd.Value = DateTime.Now.ToString(Config.DateFormat);

                nameCountry.Value = el.NameCountry;
                background.Value = colorText;
                font.Value = el.Font;
            }
            else
            {
                var e = doc.GetXElement("GridStatistique_Region_et_Pays", "_" + x + "x" + y);

                if (e != null)
                {
                    e.Add(
                        new XElement("rec",
                            new XElement("id", (el.X*el.Y).ToString()),
                            new XElement("Date_upd", DateTime.Now.ToString(Config.DateFormat)),
                            new XElement("X", el.X),
                            new XElement("Y", el.Y),
                            new XElement("NameCountry", el.NameCountry),
                            new XElement("background", colorText),
                            new XElement("img", el.Img),
                            new XElement("font", el.Font),
                            new XElement("customerId", el.CustomerId)));
                }
                else
                {
                    doc.GetXElement("GridStatistique_Region_et_Pays").Add(
                        new XElement("_" + x + "x" + y,
                            new XElement("rec",
                                new XElement("id", (el.X*el.Y).ToString()),
                                new XElement("Date_upd", DateTime.Now.ToString(Config.DateFormat)),
                                new XElement("X", el.X),
                                new XElement("Y", el.Y),
                                new XElement("NameCountry", el.NameCountry),
                                new XElement("background", colorText),
                                new XElement("img", el.Img),
                                new XElement("font", el.Font),
                                new XElement("customerId", el.CustomerId))));
                }
            }
            doc.Save(Path);
        }

        #region Nested type: elm

        public class Elm
        {
            public byte X { get; set; }
            public byte Y { get; set; }
            public Guid CustomerId { get; set; }
            public string NameCountry { get; set; }
            public string Capital { get; set; }
            public string Continent { get; set; }
            public Brush Background { get; set; }
            public Image Img { get; set; }
            public string Font { get; set; }
        }

        #endregion
    }
}