using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.Class
{
    internal class ClassGridProduct
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory + @"Data\GridProduct.xml";
        public static Elm[,,,] Grid = new Elm[12, 12, 12, 12];

        public static void Initialize()
        {
            Load(Path);
        }

        private static void Load(string path)
        {
            var xmlGrid = XDocument.Load(path);

            for (var I = 0; I < 12; I++)
            {
                for (var j = 0; j < 12; j++)
                {
                    var xElms = (from el in xmlGrid.Elements("GridProduct").Elements("_" + I + "x" + j).Elements("rec")
                        select el).ToList();

                    //var e = new Elm[xElms.Count];

                    foreach (var x in xElms)
                    {
                        var f = new Elm
                                {
                                    X = byte.Parse(x.GetXElementValue("X")),
                                    Y = byte.Parse(x.GetXElementValue("Y")),
                                    Description = x.GetXElementValue("Description")
                                };

                        try
                        {
                            var rgbtF = x.GetXElementValue("font").Split(',');

                            f.Font = rgbtF.Length == 3
                                ? new SolidColorBrush(Color.FromRgb(byte.Parse(rgbtF[0]), byte.Parse(rgbtF[1]), byte.Parse(rgbtF[2])))
                                : new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                        catch
                        {
                            f.Font = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                        try
                        {
                            var rgbt = x.GetXElementValue("background").Split(',');

                            f.Background = rgbt.Length == 3
                                ? new SolidColorBrush(Color.FromRgb(byte.Parse(rgbt[0]), byte.Parse(rgbt[1]), byte.Parse(rgbt[2])))
                                : new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        }
                        catch
                        {
                            f.Background = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        }

                        f.CustomerId = x.Element("customerId") != null
                            ? Guid.Parse(x.GetXElementValue("customerId"))
                            : Guid.Empty;
                        if (f.CustomerId == Guid.Empty)
                        {
                            LogService.Log(TraceLevel.Warning, 3, "Ошибка сетки проверьте продукт = " + f.Description + ".");
                        }
                        Grid[I, j, f.X, f.Y] = f;
                    }
                }
            }
        }

        public void Save(Elm el, int x, int y)
        {
            Grid[x, y, Convert.ToInt16(el.X), Convert.ToInt16(el.Y)] = el;

            var solid = (el.Background as SolidColorBrush);
            var solid2 = (el.Font as SolidColorBrush);

            var colorText = solid != null ? solid.Color.R + "," + solid.Color.G + "," + solid.Color.B : "255,0,0";
            var colorFontText = solid2 != null ? solid2.Color.R + "," + solid2.Color.G + "," + solid2.Color.B : "255,0,0";

            var doc = XDocument.Load(Path);
            XElement target;

            try
            {
                target =
                    doc.Elements("GridProduct")
                        .Elements("_" + x + "x" + y)
                        .Elements("rec")
                        .SingleOrDefault(e => (e.GetXElementValue("X") == el.X.ToString()) & (e.GetXElementValue("Y") == el.Y.ToString()));
            }
            catch
            {
                target = null;
            }

            if (target != null)
            {
                var d = target.Nodes().ToArray();
                var dateUpd = (d[1] as XElement);
                var description = (d[4] as XElement);
                var background = (d[5] as XElement);
                var font = (d[7] as XElement);

                if (d.Length > 8)
                {
                    var customerId = d[8] as XElement;
                    customerId.Value = el.CustomerId.ToString();
                }
                else
                {
                    target.Add(new XElement("customerId", el.CustomerId));
                }
                dateUpd.Value = DateTime.Now.ToString(Config.DateFormat);

                description.Value = el.Description;
                background.Value = colorText;
                font.Value = colorFontText;
            }
            else
            {
                var e = doc.Element("GridProduct").Element("_" + x + "x" + y);

                if (e != null)
                {
                    e.Add(new XElement("rec",
                            new XElement("id", (el.X*el.Y).ToString()),
                            new XElement("Date_upd", DateTime.Now.ToString(Config.DateFormat)),
                            new XElement("X", el.X),
                            new XElement("Y", el.Y),
                            new XElement("Description", el.Description),
                            new XElement("background", colorText),
                            new XElement("img", el.Img),
                            new XElement("font", el.Font),
                            new XElement("customerId", el.CustomerId)));
                }
                else
                {
                    doc.GetXElement("GridProduct").Add(new XElement("_" + x + "x" + y,
                            new XElement("rec",
                                new XElement("id", (el.X*el.Y).ToString()),
                                new XElement("Date_upd", DateTime.Now.ToString(Config.DateFormat)),
                                new XElement("X", el.X),
                                new XElement("Y", el.Y),
                                new XElement("Description", el.Description),
                                new XElement("background", colorText),
                                new XElement("img", el.Img),
                                new XElement("font", el.Font),
                                new XElement("customerId", el.CustomerId)
                                )
                            )
                        );
                }
            }
            doc.Save(Path);

            RepositoryXmlFile.SaveToDb(Path, doc);
        }

        #region Nested type: Elm

        public class Elm
        {
            public byte X { get; set; }
            public byte Y { get; set; }
            public Guid CustomerId { get; set; }
            public string Description { get; set; }
            public string DescriptionLong { get; set; }
            public string Prix { get; set; }
            public string Group { get; set; }
            public string Subgroup { get; set; }
            public string CodeBare { get; set; }
            public string Contenance { get; set; }
            public string UniteContenance { get; set; }
            public string Tare { get; set; }
            public bool Ballance { get; set; }
            public Brush Background { get; set; }
            public Image Img { get; set; }
            public Brush Font { get; set; }
        }

        #endregion
    }
}