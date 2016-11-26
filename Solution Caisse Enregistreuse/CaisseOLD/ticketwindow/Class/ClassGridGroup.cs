using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Data;
using ticketwindow.Winows.Payment;
namespace ticketwindow.Class
{
    public  class ClassGridGroup
    {

        public static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\GridGroup.xml";
        public static string path2 = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\GridLeft.xml";
        public static string path3 = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\GridTypePay.xml";
        public static string path4 = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\GridRigthBottom.xml";

       

        public ClassGridGroup ()
        {
            grid = load(path);
            gridLeft = load(path2);
            gridTypePay = load(path3);
            gridRigthBottom = load(path4);
            foreach (ClassSync.TypesPayDB p in ClassSync.TypesPayDB.t)
            {
                if (p.CurMod ?? false)
                {
                    string s= System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\"+p.Id +".xml";
                    if (System.IO.File.Exists(s)) 
                    gridCurrencyPath.Add(load(s));
                    else
                    {
                        XDocument doc = new XDocument();
                        doc.Add(new XElement("Grid"));
                        doc.Save(s);
                    }

                }
            }
        }
        public class elm
        {
            public byte x { get; set; }
            public byte y { get; set; }
            public string caption { get; set; }
            public System.Windows.Media.Brush background { get; set; }
            public System.Windows.Media.Brush foreground { get; set; }
            public Image img { get; set; }
            public string func { get; set; }

            public string path;
        }
        public static elm[,] grid = new elm[12, 12];
        public static elm[,] gridLeft = new elm[12, 12];
        public static elm[,] gridTypePay = new elm[12, 12];
        public static List<elm[,]> gridCurrencyPath = new List<elm[,]>();
        public static elm[,] gridRigthBottom = new elm[3, 3];
        public static elm[,] gridCurrencyPath_get_path (ClassSync.TypesPayDB tp)
        {
            elm[,] r = new elm[12,12] ;

            foreach (elm[,] elm in gridCurrencyPath)
            {
                if ((elm[0, 0] != null)&&(elm[0, 0].path == System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\" + tp.Id + ".xml"))
                    r = elm;
            }
            return r;
        }
     //   public static elm[,] gridEvro = new elm[12, 12];
        public elm[,] load(string path)
        {
            elm[,] g = new elm[12, 12];

            XDocument xmlGrid = XDocument.Load(path);

            List<XElement> x_id = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("id")
                                   select el).ToList();
            List<XElement> x_Date_upd = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Date_upd")
                                         select el).ToList();
            List<XElement> x_X = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("X")
                                  select el).ToList();
            List<XElement> x_Y = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Y")
                                  select el).ToList();
            List<XElement> x_Caption = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Caption")
                                        select el).ToList();
            List<XElement> x_Color = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Color")
                                      select el).ToList();
            List<XElement> x_Img = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Img")
                                    select el).ToList();
            List<XElement> x_Func = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("Fun")
                                    select el).ToList();
            List<XElement> x_foreground = (from el in xmlGrid.Elements("Grid").Elements("rec").Elements("foreground")
                                     select el).ToList();

            elm[] e = new elm[x_id.Count];

            for (int i = 0; i < e.Length; i++)
            {
                elm f = new elm();

                f.x = byte.Parse(x_X[i].Value);
                f.y = byte.Parse(x_Y[i].Value);

                f.caption = x_Caption[i].Value;
                string[] argbt = x_Color[i].Value.Split(',');
                string[] argbt_ = x_foreground.Count != e.Length ? new string[4] {"255", "255", "255", "255"} : x_foreground[i].Value.Split(',');

              //  try
                {
                    if (argbt.Length == 0)
                        f.background = null;

                    if (argbt.Length == 4)

                    f.background = new SolidColorBrush(Color.FromArgb(byte.Parse(argbt[0]), byte.Parse(argbt[1]), byte.Parse(argbt[2]), byte.Parse(argbt[3])));

                    if (argbt.Length == 3)
                        f.background = new SolidColorBrush(Color.FromRgb(byte.Parse(argbt[0]), byte.Parse(argbt[1]), byte.Parse(argbt[2])));


                    if (argbt_.Length == 0)
                        f.foreground = null;

                    if (argbt_.Length == 4)

                        f.foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(argbt_[0]), byte.Parse(argbt_[1]), byte.Parse(argbt_[2]), byte.Parse(argbt_[3])));

                    if (argbt_.Length == 3)
                        f.foreground = new SolidColorBrush(Color.FromRgb(byte.Parse(argbt_[0]), byte.Parse(argbt_[1]), byte.Parse(argbt_[2])));
                }

           //    catch
                {
             //       f.background = new SolidColorBrush(Color.FromArgb(255,0, 255, 255));
                }
                Image myImage3 = new Image();
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(x_Img[i].Value, UriKind.Relative);
                bi3.EndInit();
                myImage3.Stretch = Stretch.Fill;
                myImage3.Source = bi3;

                f.path = path;

                f.func = x_Func[i].Value;

                f.img = myImage3;

                g[f.x, f.y] = f;

            }
            return g;
        }
    /*    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }*/
        public void save(Button b)
        {
            W_GridPay ws = Window.GetWindow((Button)b) as W_GridPay;

            string x, y, Caption;//, Img;

            string Cr = "Default";

            string Cr_ = "Default";

            string[] dd =b.Name.ToString().Split('x');

            x = dd[0].Substring(2, dd[0].Length - 2);

            y = dd[1];

            Caption = b.Content.ToString();

            SolidColorBrush s = b.Background as SolidColorBrush;
            SolidColorBrush s_ = b.Foreground as SolidColorBrush;

            if (s != null)

                Cr = s.Color.A + ","+ s.Color.R + "," + s.Color.G + "," + s.Color.B;
            if (s_ != null)

                Cr_ = s_.Color.A + "," + s_.Color.R + "," + s_.Color.G + "," + s_.Color.B;

            string funcType = b.ToolTip == null ? "" : b.ToolTip.ToString();

            XDocument doc = null;
            if (b.Name.Substring(0, 1) == "e") doc = XDocument.Load(path4);
            if (b.Name.Substring(0, 1) == "c") doc = XDocument.Load(path3);
            if (b.Name.Substring(0, 1) == "a") doc = XDocument.Load(path2);
            if (b.Name.Substring(0, 1) == "b") doc = XDocument.Load(path);
            if (b.Name.Substring(0, 1) == "m")
            {
              
                
                doc = XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\" + ws.typesPay.Id + ".xml");
                Caption = ((Label)((StackPanel)b.Content).FindName("mlb_" + x + "x" + y)).Content.ToString();
            }
            XElement target;

            try
            {
                target = doc.Elements("Grid").Elements("rec")
                     .Where(e => (e.Element("X").Value == x) & (e.Element("Y").Value == y))
                    .Single();
            }

            catch
            {
                target = null;
            }

            if (target != null)
            {

                XNode[] d =target.Nodes().ToArray();

                XElement id_ = (d[0] as XElement);

                XElement Date_upd_ = (d[1] as XElement);

                XElement X_ = (d[2] as XElement);

                XElement Y_ = (d[3] as XElement);

                XElement Caption_ = (d[4] as XElement);

                XElement Color_ = (d[5] as XElement);

                XElement Color__ = (d[6] as XElement);

                XElement Img_ = (d[7] as XElement);

                XElement Fun_ = (d[8] as XElement);

                Date_upd_.Value = DateTime.Now.ToString();

                Caption_.Value = Caption;

                Color_.Value = Cr;

                Color__.Value = Cr_;

                Fun_.Value = funcType;
                if (Cr == "Default")
                    target.Remove();
                if (Cr_ == "Default")
                    target.Remove();
            }

            else 
            {
                if (Cr != "Default")
                {
                    doc.Element("Grid").Add(
                      new XElement("rec",
                      new XElement("id", (int.Parse(x) * int.Parse(y)).ToString()),
                      new XElement("Date_upd", DateTime.Now.ToString()),
                      new XElement("X", x),
                      new XElement("Y", y),
                      new XElement("Caption", Caption),
                      new XElement("Color", Cr),
                      new XElement("foreground", Cr_),
                      new XElement("Img", ""),
                      new XElement("Fun", funcType)
                      )
                      );
                }
            }

          

         

            switch (b.Name.Substring(0, 1))
            {
                case "m":
                    doc.Save(System.AppDomain.CurrentDomain.BaseDirectory + @"Data\" + ws.typesPay.Id + ".xml");
                    ClassXMLDB.saveDB(System.AppDomain.CurrentDomain.BaseDirectory + @"Data\" + ws.typesPay.Id + ".xml", doc);
                    break;
                case "c":
                    doc.Save(path3);
                    ClassXMLDB.saveDB(path3, doc);
                    break;
                case "a": 
                    doc.Save(path2);
                    ClassXMLDB.saveDB(path2, doc);
                    break;
                case "b":
                    doc.Save(path);
                    ClassXMLDB.saveDB(path, doc);
                    break;
                case "e":
                    doc.Save(path4);
                    ClassXMLDB.saveDB(path4, doc);
                    break;
            }
        }

     
      
    }
}
