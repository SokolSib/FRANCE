using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
using TicketWindow.Services;
using TicketWindow.Winows.OtherWindows.Payment;
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace TicketWindow.Class
{
    public class ClassGridGroup
    {
        public static Elm[,] Grid = new Elm[12, 12];
        public static Elm[,] GridLeft = new Elm[12, 12];
        public static Elm[,] GridTypePay = new Elm[12, 12];
        public static List<Elm[,]> GridCurrencyPath = new List<Elm[,]>();
        public static Elm[,] GridRigthBottom = new Elm[3, 3];

        public static void Initialize()
        {
            GridLeft = Load(XmlDocEnum.A);
            Grid = Load(XmlDocEnum.B);
            GridTypePay = Load(XmlDocEnum.C);
            GridRigthBottom = Load(XmlDocEnum.E);

            foreach (var p in RepositoryTypePay.TypePays)
            {
                if (p.CurMod ?? false)
                    GridCurrencyPath.Add(Load(XmlDocEnum.M, p.Id));
            }
        }

        public static Elm[,] GridCurrencyPathGetPath(TypePay tp)
        {
            Initialize();

            var r = new Elm[12, 12];

            foreach (var elm in GridCurrencyPath)
            {
                if ((elm[0, 0] != null) && (elm[0, 0].Path == AppDomain.CurrentDomain.BaseDirectory + @"\Data\" + tp.Id + ".xml"))
                    r = elm;
            }
            return r;
        }

        public static Elm[,] Load(XmlDocEnum type, int payId = -1)
        {
            var g = new Elm[12, 12];
            var xmlGrid = RepositoryXmlFile.Load(type, payId);

            var xId = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("id") select el).ToList();
            var xX = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("X") select el).ToList();
            var xY = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Y") select el).ToList();
            var xCaption = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Caption") select el).ToList();
            var xColor = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Color") select el).ToList();
            var xImg = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Img") select el).ToList();
            var xFunc = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Fun") select el).ToList();
            var xForeground = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("foreground") select el).ToList();
            
            for (var i = 0; i < xId.Count; i++)
            {
                var f = new Elm(RepositoryXmlFile.GetPathByType(type, payId), xX[i].Value.ToByte(), xY[i].Value.ToByte(), xCaption[i].Value, xFunc[i].Value);

                var argbtColor = xColor[i].Value.Split(',');
                var argbtForeground = xForeground.Count != xId.Count ? new[] { "255", "255", "255", "255" } : xForeground[i].Value.Split(',');

                if (argbtColor.Length == 0)
                    f.Background = null;

                if (argbtColor.Length == 4)
                    f.Background =
                        new SolidColorBrush(Color.FromArgb(byte.Parse(argbtColor[0]), byte.Parse(argbtColor[1]), byte.Parse(argbtColor[2]), byte.Parse(argbtColor[3])));

                if (argbtColor.Length == 3)
                    f.Background = new SolidColorBrush(Color.FromRgb(byte.Parse(argbtColor[0]), byte.Parse(argbtColor[1]), byte.Parse(argbtColor[2])));

                if (argbtForeground.Length == 0)
                    f.Foreground = null;

                if (argbtForeground.Length == 4)
                    f.Foreground =
                        new SolidColorBrush(Color.FromArgb(byte.Parse(argbtForeground[0]), byte.Parse(argbtForeground[1]), byte.Parse(argbtForeground[2]),
                            byte.Parse(argbtForeground[3])));

                if (argbtForeground.Length == 3)
                    f.Foreground = new SolidColorBrush(Color.FromRgb(byte.Parse(argbtForeground[0]), byte.Parse(argbtForeground[1]), byte.Parse(argbtForeground[2])));

                var myImage3 = new Image();
                var bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(xImg[i].Value, UriKind.Relative);
                bi3.EndInit();
                myImage3.Stretch = Stretch.Fill;
                myImage3.Source = bi3;

                f.Img = myImage3;

                g[f.X, f.Y] = f;
            }
            return g;
        }

        public static void Save(Button b)
        {
            var windowGridPay = Window.GetWindow(b) as WGridPay;
            var cr = "Default";
            var cr1 = "Default";
            var dd = b.Name.Split('x');
            var x = dd[0].Substring(2, dd[0].Length - 2);
            var y = dd[1];
            var caption = b.Content.ToString();
            var sBackground = b.Background as SolidColorBrush;
            var sForeground = b.Foreground as SolidColorBrush;

            if (sBackground != null)
                cr = sBackground.Color.A + "," + sBackground.Color.R + "," + sBackground.Color.G + "," +
                     sBackground.Color.B;
            if (sForeground != null)
                cr1 = sForeground.Color.A + "," + sForeground.Color.R + "," + sForeground.Color.G + "," +
                      sForeground.Color.B;

            string funcType = null;
            var elmTag = b.Tag as Elm;
            funcType = elmTag != null ? elmTag.Func : b.Tag.ToString();

            var typePayId = -1;
            if (windowGridPay != null)
                typePayId = windowGridPay.TypesPay.Id;

            var type = RepositoryXmlFile.ToXmlDocEnum(b.Name);
            if (type == XmlDocEnum.M)
                caption = ((Label) ((StackPanel) b.Content).FindName("mlb_" + x + "x" + y)).Content.ToString();

            var doc = RepositoryXmlFile.Load(type, typePayId);
            XElement target;

            try
            {
                target =
                    doc.GetXElements("Grid", "rec")
                        .Single(e => (e.GetXElementValue("X") == x) & (e.GetXElementValue("Y") == y));
            }
            catch
            {
                target = null;
            }

            if (target != null)
            {
                var d = target.Nodes().ToArray();
                var dateUpd = d[1] as XElement;
                var caption1 = d[4] as XElement;
                var color = d[5] as XElement;
                var color1 = d[6] as XElement;
                var fun = d[8] as XElement;

                dateUpd.Value = DateTime.Now.ToString(Config.DateFormat);
                caption1.Value = caption;
                color.Value = cr;
                color1.Value = cr1;
                fun.Value = funcType;

                if (cr == "Default")
                    target.Remove();
                if (cr1 == "Default")
                    target.Remove();
            }
            else
            {
                if (cr != "Default")
                    doc.GetXElement("Grid").Add(
                        new XElement("rec",
                            new XElement("id", (int.Parse(x)*int.Parse(y)).ToString()),
                            new XElement("Date_upd", DateTime.Now.ToString(Config.DateFormat)),
                            new XElement("X", x),
                            new XElement("Y", y),
                            new XElement("Caption", caption),
                            new XElement("Color", cr),
                            new XElement("foreground", cr1),
                            new XElement("Img", ""),
                            new XElement("Fun", funcType)
                        )
                    );
            }
            ProductType product;
            b.ToolTip = FunctionsTranslateService.GetTranslatedFunctionWithProd(funcType, out product);

            if (string.IsNullOrEmpty(b.Content?.ToString()))
                b.Content = product.Name;

            RepositoryXmlFile.Save(doc, type, typePayId);
        }

        #region Nested type: elm

        public class Elm
        {
            public const string FuncNameForProduct = "Products id=[";
            public Elm(string path, byte x, byte y, string caption, string func)
            {
                Path = path;
                X = x;
                Y = y;
                Caption = caption;
                Func = func;
                var idx = func.IndexOf(FuncNameForProduct);
                if (idx == 0)
                {
                    var idText = func.Substring(13, func.Length - 14);
                    ProductId = idText;
                }
            }

            public string Path;
            public byte X { get; set; }
            public byte Y { get; set; }
            public string Caption { get; set; }
            public Brush Background { get; set; }
            public Brush Foreground { get; set; }
            public Image Img { get; set; }
            public string Func { get; set; }
            public string ProductId { get; set; }
        }

        #endregion
    }
}