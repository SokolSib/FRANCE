using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using ticketwindow.Winows.Loading;
using ticketwindow.Winows.Setting;

namespace ticketwindow.Class
{
    public class ClassSync
    {

        public static bool connect = true;
        public static bool sync = false;
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

            public Establishment sel(Guid CustomerId)
            {
                object[] o = new ClassDB(null).queryResonse("SELECT * FROM Establishment WHERE CustomerId = '" + CustomerId + "'")[0];

                Establishment res = new Establishment();

                res.CustomerId = (Guid)o[0];
                res.Type = (int)o[1];
                res.Name = (string)o[2];
                res.CP = (string)o[3];
                res.Ville = (string)o[4];
                res.Adresse = (string)o[5];
                res.Phone = (string)o[6];
                res.Mail = (string)o[7];

                return res;
            }

        }
        public class ClassDataTimeSrv
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct SYSTEMTIME
            {
                public ushort wYear;
                public ushort wMonth;
                public ushort wDayOfWeek;
                public ushort wDay;
                public ushort wHour;
                public ushort wMinute;
                public ushort wSecond;
                public ushort wMilliseconds;
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetSystemTime(ref SYSTEMTIME time);

            public static DateTime dateTimeFromSrv { get; set; }
            public static bool getDateTimeFromSrv()
            {
                try
                {
                    dateTimeFromSrv = (DateTime)new ClassDB(null).queryResonse(" SELECT SYSDATETIME()")[0][0];

                    long TI = Math.Abs(dateTimeFromSrv.Ticks - DateTime.Now.Ticks);

                    return TI > 10000 * 60000;
                }
                catch
                {
                    return false;
                }
            }
            public static bool setDateTimeFromSrv()
            {
                DateTime utc = dateTimeFromSrv.AddHours(-ClassGlobalVar.utc);
                SYSTEMTIME time = new SYSTEMTIME();
                time.wYear = (ushort)utc.Year;
                time.wMonth = (ushort)utc.Month;
                time.wDay = (ushort)utc.Day;
                time.wHour = (ushort)(utc.Hour);
                time.wMinute = (ushort)utc.Minute;
                time.wSecond = (ushort)utc.Second;
                bool r = SetSystemTime(ref time);
                return r;
            }
        }
        public class ProductDB
        {
            public Guid ins(object o)
            {
                ClassProducts.product p = (ClassProducts.product)o;



                string cmdProductWeb = "INSERT INTO ProductsWeb (CustomerId,[Visible],[Images],[ContenancePallet],[Weight],[Frozen]) VALUES ('"
                    + p.ProductsWeb_CustomerId + "', 0, '', 0,0,0 )";

                new ClassDB(null).queryNonResonse(cmdProductWeb);


                string cmdInsProduct = "INSERT INTO Products (CustumerId,Name,CodeBare,[Desc],Chp_cat,Balance,Contenance,UniteContenance,Tare,[Date],TVACustumerId,"
                    //  +"ProductsAwaitingDeliveryCustomerId,"
                    + "ProductsWeb_CustomerId,SubGrpProduct_Id)"
                + "VALUES ('{CustumerId}','{Name}','{CodeBare}','{Desc}','{Chp_cat}','{Balance}','{Contenance}','{UniteContenance}','{Tare}', CAST('{Date}' AS DateTime),'{TVACustumerId}',"
                // +"'{ProductsAwaitingDeliveryCustomerId}',"
                + "'{ProductsWeb_CustomerId}','{SubGrpProduct_Id}')";



                string c = cmdInsProduct
                           .Replace("{CustumerId}", p.CustumerId.ToString())
                           .Replace("{Name}", p.Name.Replace("'", "''"))
                           .Replace("{CodeBare}", p.CodeBare)
                           .Replace("{Desc}", p.Desc.Replace("'", "''"))
                           .Replace("{Chp_cat}", "0")
                           .Replace("{Balance}", p.balance.ToString())
                           .Replace("{Contenance}", p.contenance.ToString().Replace(",", "."))
                           .Replace("{UniteContenance}", p.uniteContenance.ToString())
                           .Replace("{Tare}", p.tare.ToString())
                           .Replace("{Date}", DateTime.Now.ToString())
                           .Replace("{TVACustumerId}", ClassTVA.listTVA.Find(l => l.id == p.tva).CustumerId.ToString())
                           // .Replace("{ProductsAwaitingDeliveryCustomerId}", Guid.Empty.ToString())
                           .Replace("{ProductsWeb_CustomerId}", p.ProductsWeb_CustomerId.ToString())
                           .Replace("{SubGrpProduct_Id}", p.cusumerIdSubGroup.ToString()
                         );
                new ClassDB(null).queryNonResonse(c);

                string cmdInsStockReal = "INSERT INTO StockReal (CustomerId,QTY,MinQTY,Price,ProductsCustumerId,Establishment_CustomerId)" +
               "VALUES ('{CustomerId}','{QTY}','{MinQTY}','{Price}','{ProductsCustumerId}','{Establishment_CustomerId}')";

                c = cmdInsStockReal
                    .Replace("{CustomerId}", p.cusumerIdRealStock.ToString())

                    .Replace("{QTY}", p.qty.ToString().Replace("'", "''"))
                    .Replace("{MinQTY}", "10")
                    .Replace("{Price}", p.price.ToString().Replace(",", "."))
                    .Replace("{ProductsCustumerId}", p.CustumerId.ToString())
                    .Replace("{Establishment_CustomerId}", ClassGlobalVar.IdEstablishment.ToString()
            );
                new ClassDB(null).queryNonResonse(c);

                new LastUpdDB().mod(ClassGlobalVar.nameTicket, DateTime.Now, ClassGlobalVar.user, ClassGlobalVar.CustumerId, true);

                return p.CustumerId;
            }
            public void mod(object o)
            {
                ClassProducts.product p = (ClassProducts.product)o;
                string cmdUpd = "UPDATE Products SET [Name]='{Name}', [CodeBare]='{CodeBare}',[Desc]='{Desc}',[TVACustumerId]='{TVACustumerId}'," +
                                "[SubGrpProduct_Id]='{SubGrpProduct_Id}',[Balance]='{Balance}',[Contenance]={Contenance},[UniteContenance]={UniteContenance}," +
                                "[Tare]={Tare},[Date]=CAST('{Date}' AS DateTime) WHERE CustumerId= '{CustumerId}'";

                string c = cmdUpd
                           .Replace("{CustumerId}", p.CustumerId.ToString())
                           .Replace("{Name}", p.Name.Replace("'", "''"))
                           .Replace("{CodeBare}", p.CodeBare)
                           .Replace("{Desc}", p.Desc.Replace("'", "''"))
                           .Replace("{TVACustumerId}", ClassTVA.listTVA.Find(l => l.id == p.tva).CustumerId.ToString())

                           .Replace("{SubGrpProduct_Id}", p.sgrp.ToString())
                           .Replace("{Balance}", p.balance.ToString())
                           .Replace("{Contenance}", p.contenance.ToString().Replace(",", "."))
                           .Replace("{UniteContenance}", p.uniteContenance.ToString().Replace(",", "."))
                           .Replace("{Tare}", p.tare.ToString().Replace(",", "."))
                           .Replace("{Date}", DateTime.Now.ToString());
                //  new ClassDB(null).queryNonResonse(c);

                cmdUpd = "UPDATE StockReal SET QTY='{QTY}',MinQTY='{MinQTY}',Price='{Price}'," +
                                "ProductsCustumerId='{ProductsCustumerId}',Establishment_CustomerId='{Establishment_CustomerId}'" +
                                " WHERE CustomerId= '{CustomerId}'";
                c = cmdUpd


                           .Replace("{QTY}", p.qty.ToString().Replace(",", "."))
                           .Replace("{MinQTY}", "0")
                           .Replace("{Price}", p.price.ToString().Replace(",", "."))
                           .Replace("{ProductsCustumerId}", p.CustumerId.ToString())

                           .Replace("{Establishment_CustomerId}", ClassGlobalVar.IdEstablishment.ToString())
                           .Replace("{CustomerId}", p.cusumerIdRealStock.ToString()
                       );
                new ClassDB(null).queryNonResonse(c);

                new LastUpdDB().mod(ClassGlobalVar.nameTicket, DateTime.Now, ClassGlobalVar.user, ClassGlobalVar.CustumerId, true);
            }
            public void del(object o)
            {
                string cmd = "DELETE FROM StockReal WHERE ProductsCustumerId = '{CustumerId}'";
                ClassProducts.product p = (ClassProducts.product)o;
                string c = cmd.Replace("{CustumerId}", p.CustumerId.ToString());
                int i = new ClassDB(null).queryNonResonse(c);

                cmd = "DELETE FROM Products  WHERE CustumerId= '{CustumerId}'";
                c = cmd.Replace("{CustumerId}", p.CustumerId.ToString());
                i = new ClassDB(null).queryNonResonse(c);

                cmd = "DELETE FROM ProductsWeb  WHERE CustomerId= '{CustomerId}'";
                c = cmd.Replace("{CustomerId}", p.ProductsWeb_CustomerId.ToString());
                i = new ClassDB(null).queryNonResonse(c);

                new LastUpdDB().mod(ClassGlobalVar.nameTicket, DateTime.Now, ClassGlobalVar.user, ClassGlobalVar.CustumerId, true);

            }

            public static void syncSingleProduct()
            {
                object date = new LastUpdDB().getUpdAB();

                if (date != null)
                {
                    ClassGroupProduct.loadFromFile();
                    ClassTVA.loadFromFile();
                    ClassProducts.loadFronFile();

                    List<ProductDB.Products> productsAB = ProductDB.Products.selAB(DateTime.Parse(date.ToString()).AddMinutes(-1), DateTime.Now.AddMinutes(1));


                    if (productsAB.Count > 0)
                    {
                        string s = "Les produits suivante a été modifié : " + Environment.NewLine;

                        foreach (var p in productsAB)
                        {
                            ClassProducts.modifAddOnlyFile(ClassProducts.DbToVar(p));


                            try
                            {

                                s += p.Name + " (" + p.CodeBare + ") - " + "QTY : " + p.StockReal_.Count() + " - Prix : € " + Environment.NewLine;

                            }


                            catch (Exception ex)
                            {
                                string text = "KOD 013214" + ex.Message;

                                //   new ClassFunctuon().showMessageSB(text);

                                new ClassLog(text);
                            }


                        }

                        //   new ClassFunctuon().showMessageTimeList(s);


                    }
                }
            }

            public static void syncAll()
            {
                //   if (!new ClassSync.LastUpdDB().getUpd())
                {
                    if (connect)
                    {
                        ActionsCaisseDB.set();

                        new ClassActionsCaisse().saveFile();

                        new ClassActionsCaisse().loadFile();

                        StockReal.set();
                        TVA.set();
                        ClassTVA.save();
                        SubGrpProduct.set();

                        GrpProduct.set();
                        ClassGroupProduct.save();
                        ClassGroupProduct.loadFromFile();
                        Products.set();
                        ClassProducts.save();
                    }
                }
            }

            public static void setAsynch()
            {
                W_Loading w = new W_Loading(null);
                w.Show();
                w.BusyIndicator.IsBusy = true;
                try
                {
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();

                    worker.DoWork += (o, a) =>
                        {

                            ClassSync.sync = true;



                            set();
                        };
                    worker.RunWorkerCompleted += (o, a) =>
                    {
                        w.BusyIndicator.IsBusy = false;
                        ClassSync.sync = false;
                        w.close();
                    };

                    worker.RunWorkerAsync();
                }

                catch (Exception ex)
                {

                    string text = "KOD 007" + ex.Message;

                    new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text);
                }
            }

            public static void set()
            {

                ProductsBC.set();

                if ((Products.selABCount() > 20) || ClassGlobalVar.fromLoadSyncAll)
                {
                    syncAll();
                    ClassGlobalVar.fromLoadSyncAll = false;
                }
                else
                {
                    syncSingleProduct();
                }
                ClassGroupProduct.loadFromFile();
                ClassTVA.loadFromFile();
                //  ClassProducts.loadFronFile();
                ClassCountrys.loadFromFile();
                new LastUpdDB().mod(ClassGlobalVar.nameTicket, DateTime.Now, ClassGlobalVar.user, ClassGlobalVar.CustumerId, false);
            }
            public partial class StockReal
            {
                public static decimal add_qty(decimal qty, Guid CustomerId)
                {
                    string cmd = "UPDATE StockReal SET QTY = (SELECT QTY FROM StockReal WHERE CustomerId = '" + CustomerId.ToString() + "') + " + qty.ToString().Replace(",", ".") + " WHERE CustomerId = '" + CustomerId.ToString() + "'";

                    new ClassDB(null).queryResonse(cmd);

                    cmd = "SELECT QTY FROM StockReal WHERE CustomerId = '" + CustomerId.ToString() + "'";

                    return (decimal)new ClassDB(null).queryResonse(cmd)[0][0];
                }

                public static string query_add_qty(decimal qty, Guid CustomerId)
                {
                    string cmd = "UPDATE StockReal SET QTY = (SELECT QTY FROM StockReal WHERE CustomerId = '" + CustomerId.ToString() + "') + " + qty.ToString().Replace(",", ".") + " WHERE CustomerId = '" + CustomerId.ToString() + "'";

                    //                    new ClassDB(null).queryResonse(cmd);

                    //                  cmd = "SELECT QTY FROM StockReal WHERE CustomerId = '" + CustomerId.ToString() + "'";

                    return cmd;
                }

                public static string query_add_qty_from_est(decimal qty, Guid Establishment_CustomerId, Guid ProductsCustumerId)
                {
                    string cmd = "UPDATE StockReal SET QTY = (SELECT QTY FROM StockReal WHERE CustomerId = "
                        +
                        "(SELECT CustomerId FROM StockReal WHERE (ProductsCustumerId = '" + ProductsCustumerId + "' AND  Establishment_CustomerId ='" + Establishment_CustomerId + "') )"
                        + ") + "
                        + qty.ToString().Replace(",", ".")
                        + " WHERE CustomerId = " +
                        "(SELECT CustomerId FROM StockReal WHERE (ProductsCustumerId = '" + ProductsCustumerId + "' AND  Establishment_CustomerId ='" + Establishment_CustomerId + "') )"
                        + "";

                    return cmd;
                }

                public static int response(string cmd)
                {
                    return new ClassDB(null).queryNonResonse(cmd);
                }

                public static List<StockReal> L = new List<StockReal>();
                public static void set()
                {
                    List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM StockReal ");
                    L = ListObjToStockReal(l);
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

                public static List<StockReal> sel(Guid ProductsCustumerId, Guid Establishment_CustomerId)
                {
                    List<object[]> l = new ClassDB(null).queryResonse(
                        "SELECT * FROM StockReal WHERE (ProductsCustumerId = '" + ProductsCustumerId + "' AND  Establishment_CustomerId ='" + Establishment_CustomerId + "') ");
                    return ListObjToStockReal(l);
                }
                private static void ins(StockReal sr)
                {
                    try
                    {
                        if (sel(sr.ProductsCustumerId, sr.IdEstablishment).Count == 0)
                        {
                            string cmdInsStockReal = "INSERT INTO StockReal (CustomerId,QTY,MinQTY,Price,ProductsCustumerId,Establishment_CustomerId)" +
                       "VALUES ('{CustomerId}','{QTY}','{MinQTY}','{Price}','{ProductsCustumerId}','{Establishment_CustomerId}')";

                            string c = cmdInsStockReal
                                .Replace("{CustomerId}", sr.CustomerId.ToString())

                                .Replace("{QTY}", sr.QTY.ToString())
                                .Replace("{MinQTY}", "10")
                                .Replace("{Price}", sr.Price.ToString().Replace(",", "."))
                                .Replace("{ProductsCustumerId}", sr.ProductsCustumerId.ToString())
                                .Replace("{Establishment_CustomerId}", ClassGlobalVar.IdEstablishment.ToString()
                        );





                            new ClassDB(null).queryNonResonse(c);
                        }
                        else
                        {
                            new ClassFunctuon().showMessageTime("no add record from table stockReal " + sr.CustomerId);

                        }
                    }
                    catch (Exception ex)
                    {
                        new ClassLog("code 0400 " + ex.Message).ClassLogSQL(sr.CustomerId.ToString());
                    }
                }
                public static void del(Guid customerId)
                {
                    string c = "DELETE FROM StockReal WHERE CustomerId='" + customerId + "'";
                    new ClassDB(null).queryNonResonse(c);
                }
                public static StockReal addIsNull(Guid customerIdProduct, Guid est)
                {
                    StockReal r = new StockReal();
                    r.CustomerId = Guid.NewGuid();
                    r.IdEstablishment = est;
                    r.MinQTY = 0;
                    r.Price = 0.0m;
                    r.ProductsCustumerId = customerIdProduct;
                    r.QTY = 0;
                    ins(r);

                    return r;
                }
                public System.Guid CustomerId { get; set; }
                public decimal QTY { get; set; }
                public decimal MinQTY { get; set; }
                public decimal Price { get; set; }
                public System.Guid ProductsCustumerId { get; set; }
                public System.Guid IdEstablishment { get; set; }
                public virtual Products Products { get; set; }
            }
            public partial class TVA
            {
                public static List<TVA> L = new List<TVA>();
                public static void set()
                {
                    List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM TVA");
                    foreach (object[] o in l)
                    {
                        TVA t = new TVA();
                        t.CustumerId = (Guid)o[0];
                        t.Id = (string)o[1];
                        t.val = (string)o[2];
                        L.Add(t);
                    }
                }
                public TVA()
                {
                    this.Products = new HashSet<Products>();
                }

                public System.Guid CustumerId { get; set; }
                public string Id { get; set; }
                public string val { get; set; }

                public virtual ICollection<Products> Products { get; set; }
            }
            public partial class SubGrpProduct
            {
                public static List<SubGrpProduct> L = new List<SubGrpProduct>();
                public static void set()
                {
                    List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM SubGrpNameSet");
                    foreach (object[] o in l)
                    {
                        SubGrpProduct sg = new SubGrpProduct();
                        sg.Id = (int)o[0];
                        sg.SubGroupName = (string)o[1];
                        sg.GrpProductId = (int)o[2];
                        sg.GrpProductSet = null;
                        L.Add(sg);
                    }
                }
                public SubGrpProduct()
                {
                    this.Products = new HashSet<Products>();
                }

                public int Id { get; set; }
                public string SubGroupName { get; set; }
                public int GrpProductId { get; set; }

                public virtual GrpProduct GrpProductSet { get; set; }
                public virtual ICollection<Products> Products { get; set; }
            }
            public partial class GrpProduct
            {
                public static List<GrpProduct> L = new List<GrpProduct>();
                public static void set()
                {
                    List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM GrpProductSet");
                    foreach (object[] o in l)
                    {
                        GrpProduct sg = new GrpProduct();
                        sg.Id = (int)o[0];
                        sg.GroupName = (string)o[1];
                        foreach (var s in SubGrpProduct.L.FindAll(k => k.GrpProductId == sg.Id))
                        {
                            sg.SubGrpNameSet.Add(s);
                        }
                        L.Add(sg);
                    }
                }
                public GrpProduct()
                {
                    this.SubGrpNameSet = new HashSet<SubGrpProduct>();
                }

                public int Id { get; set; }
                public string GroupName { get; set; }

                public virtual ICollection<SubGrpProduct> SubGrpNameSet { get; set; }
            }

            private static string getallcodebar (string cb, Guid custumerIdProduct)
            {

                string res = '[' + cb.Replace(" ", "") + ']';

                var pbc = ProductsBC.L.FindAll(l => l.CustomerIdProduct == custumerIdProduct);

                if (pbc.Count > 0)
                {

                    foreach (var bc in pbc)
                    {
                        res += '[' + bc.CodeBar.Replace(" ","")  + "]" + '^' + bc.QTY;
                    }
                }

                return res ;
            }

            public partial class Products
            {
                private static void tstDubl(List<StockReal> lsr, Products p, out Products outp, bool syncAll)
                {
                    try
                    {
                        switch (lsr.Count)
                        {
                            case 0:


                                p.StockReal_.Add(StockReal.addIsNull(p.CustumerId, ClassGlobalVar.IdEstablishment));
                                p.StockReal_.Add(StockReal.addIsNull(p.CustumerId, ClassGlobalVar.IdEstablishment_GROS));

                                break;
                            case 1:
                                p.StockReal_ = lsr;

                                var k = lsr.FirstOrDefault(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment_GROS);

                                p.StockReal_.Add(k);

                                p.StockReal_.Add(StockReal.addIsNull(
                                    p.CustumerId
                                    ,
                                    (k == null) ?
                                    ClassGlobalVar.IdEstablishment_GROS
                                    :
                                    ClassGlobalVar.IdEstablishment
                                    ));
                                break;


                            case 2: p.StockReal_ = lsr; break;

                            default:
                                p.StockReal_ = lsr;

                                string log = "Double ecriture dans base de données StockReal : " + Environment.NewLine + "Produit : " + p.Name + " (" + p.CodeBare + ")" + Environment.NewLine;
                                bool first = true;
                                foreach (var e in lsr.FindAll(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment))
                                {
                                    log += (first ? "Record " : "Supprimer ") + " Id : " + e.CustomerId + " QTY : " + e.QTY + " Prix : " + e.Price + Environment.NewLine + " IdEST : " + e.IdEstablishment + " IdProduit : " + e.ProductsCustumerId + Environment.NewLine;
                                    if (!first)
                                    {
                                        StockReal.del(e.CustomerId);

                                    }
                                    first = false;
                                }
                                first = true;
                                foreach (var e in lsr.FindAll(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment_GROS))
                                {
                                    log += (first ? "Record " : "Supprimer ") + " Id : " + e.CustomerId + " QTY : " + e.QTY + " Prix : " + e.Price + Environment.NewLine + " IdEST : " + e.IdEstablishment + " IdProduit : " + e.ProductsCustumerId + Environment.NewLine;
                                    if (!first)
                                    {
                                        StockReal.del(e.CustomerId);

                                    }
                                    first = false;
                                }
                                new ClassLog(log);
                                //   new ClassFunctuon().showMessageTimeList(log);
                                break;
                        }
                        var sr_ = p.StockReal_.First(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment_GROS);

                        var sr = p.StockReal_.First(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment);

                        /*    decimal tva = decimal.Parse(p.TVA.val.Replace('.',','));

                            sr_.Price =(sr_.Price * (1.0m + tva/100));
                            */
                        if (sr_.Price > sr.Price)
                        {
                            string sd = "code 077 Плохая цена для про, продукт " + p.Name + " в "
                                + ClassGlobalVar.Establishment.Name + ":" + sr.Price.ToString() + " цена про без  НДС :" + sr_.Price;

                            //  new ClassFunctuon().showMessageTimeList(sd);

                            new ClassLog(sd);
                        }
                    }
                    catch (Exception ex)
                    {
                        new ClassLog("code 0356 product : " + p.Name + " - " + p.CodeBare + " - " + p.CustumerId + " - " + ex.Message);
                    }
                    outp = p;
                }
                public static List<Products> L = new List<Products>();
                public static void set()
                {
                    List<object[]> S = new ClassDB(null).queryResonse(@"SELECT   
       [CustumerId]
      ,[Name]
      ,[CodeBare]
      ,[Desc]
      ,[Chp_cat]
      ,[Balance]
      , (SELECT[ContenanceBox] FROM[ProductsWeb] WHERE[CustomerId] = [Products].ProductsWeb_CustomerId)
      ,[UniteContenance]
      ,[Tare]
      ,[Date]
      ,[TVACustumerId]
      ,[ProductsWeb_CustomerId]
      ,[SubGrpProduct_Id]
      ,[FavoritesProductCustomerId] FROM Products");



                    foreach (object[] s in S)
                    {
                        try
                        {
                            Products p = new Products();
                            p.CustumerId = (Guid)s[0];
                            p.Name = (string)s[1];
                            p.CodeBare = getallcodebar((string)s[2], p.CustumerId); 
                            p.Desc = (string)s[3];
                            p.Chp_cat = (short)s[4];
                            p.Balance = (bool)s[5];
                            p.Contenance = (decimal)((s[6] is DBNull) ? (decimal)0.0 : s[6]);
                            p.UniteContenance = (int)s[7];
                            p.Tare = (int)s[8];
                            p.Date = (DateTime)s[9];
                            p.TVACustumerId = (Guid)s[10];
                            p.ProductsWeb_CustomerId = (Guid)s[11];
                            //   p.ProductsAwaitingDeliveryCustomerId = (Guid)s[11];

                            p.SubGrpProduct = SubGrpProduct.L.Find(l => l.Id == (int)s[12]) ?? SubGrpProduct.L.First();
                            p.TVA = TVA.L.Find(l => l.CustumerId == p.TVACustumerId);

                            List<StockReal> lsr = StockReal.L.FindAll(l => (l.ProductsCustumerId == p.CustumerId)
                                && ((l.IdEstablishment == ClassGlobalVar.IdEstablishment) || l.IdEstablishment == ClassGlobalVar.IdEstablishment_GROS)
                                );


                            tstDubl(lsr, p, out p, true);


                            L.Add(p);
                        }
                        catch (Exception ex)
                        {
                            string sd = "code 0111 " + s[1].ToString() + " " + ex.Message;

                            //    new ClassFunctuon().showMessageTimeList(sd);
                            new ClassLog(sd);
                        }


                    }
                }
                public static int selABCount()
                {
                    object date = new LastUpdDB().getUpdAB();

                    if (date != null)
                    {
                        DateTime a = DateTime.Parse(date.ToString()).AddMinutes(-1);
                        DateTime b = DateTime.Now.AddMinutes(1);

                        string cmd = "SELECT * FROM Products WHERE (" +
                            "[Date] >=  CAST('{DATETIME_A}' AS DateTime) AND [Date] <= CAST('{DATETIME_B}' AS DateTime))";

                        string c = cmd.Replace("{DATETIME_A}", a.ToString())
                            .Replace("{DATETIME_B}", b.ToString());

                        return new ClassDB(null).queryResonse(c).Count();
                    }

                    return 0;
                }

                public static List<Products> selAB(DateTime a, DateTime b)
                {
                    string cmd = @"SELECT 
       [CustumerId]
      ,[Name]
      ,[CodeBare]
      ,[Desc]
      ,[Chp_cat]
      ,[Balance]
      ,(SELECT [ContenanceBox] FROM [ProductsWeb] WHERE [CustomerId] = ProductsWeb_CustomerId)
      ,[UniteContenance]
      ,[Tare]
      ,[Date]
      ,[TVACustumerId]
      ,[ProductsWeb_CustomerId]
      ,[SubGrpProduct_Id]
      ,[FavoritesProductCustomerId] 
        FROM Products WHERE (" + "[Date] >=  CAST('{DATETIME_A}' AS DateTime) AND [Date] <= CAST('{DATETIME_B}' AS DateTime))";

                    string c = cmd.Replace("{DATETIME_A}", a.ToString()).Replace("{DATETIME_B}", b.ToString());

                    List<object[]> S = new ClassDB(null).queryResonse(c);

                    List<Products> R = new List<Products>();

                    foreach (object[] s in S)
                    {
                        Products p = new Products();
                        p.CustumerId = (Guid)s[0];
                        p.Name = (string)s[1];
                        p.CodeBare = getallcodebar( (string)s[2], p.CustumerId );
                        p.Desc = (string)s[3];
                        p.Chp_cat = (short)s[4];
                        p.Balance = (bool)s[5];
                        p.Contenance = (decimal)s[6];
                        p.UniteContenance = (int)s[7];
                        p.Tare = (int)s[8];
                        p.Date = (DateTime)s[9];
                        p.TVACustumerId = (Guid)s[10];
                        p.ProductsWeb_CustomerId = (Guid)s[11];
                        //   p.ProductsAwaitingDeliveryCustomerId = (Guid)s[11];

                        List<ClassSync.ProductDB.GrpProduct> L = ClassGroupProduct.groupXMLToGrpProductTabl();

                        List<SubGrpProduct> lgp = L.Find(l => l.SubGrpNameSet.SingleOrDefault(lz => lz.Id == (int)s[12]) != null).SubGrpNameSet.ToList();

                        p.SubGrpProduct = lgp.Find(l => l.Id == (int)s[12]);


                        p.TVA = ClassTVA.xmlToDb().Find(l => l.CustumerId == p.TVACustumerId);

                        List<StockReal> lsr = StockReal.sel(p.CustumerId, ClassGlobalVar.IdEstablishment);

                        lsr.AddRange(StockReal.sel(p.CustumerId, ClassGlobalVar.IdEstablishment_GROS));

                        tstDubl(lsr, p, out p, false);


                        R.Add(p);
                    }

                    return R;
                }
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
                //  public System.Guid ProductsAwaitingDeliveryCustomerId { get; set; }
                public System.Guid ProductsWeb_CustomerId { get; set; }
                public virtual SubGrpProduct SubGrpProduct { get; set; }
                public virtual TVA TVA { get; set; }
                public virtual ICollection<StockReal> StockReal_ { get; set; }
            }
            public partial class ProductsBC
            {

                public System.Guid CustomerId { get; set; }

                public System.Guid? CustomerIdProduct { get; set; }

                public string CodeBar { get; set; }

                public decimal QTY { get; set; }

                public string Description { get; set; }

                public static void set()
                {

                    string cmd = @"SELECT [CustomerId],[CustomerIdProduct],[CodeBar],[QTY] FROM [ProductsBC]";

                    List<object[]> l = new ClassDB(null).queryResonse(cmd);

                    foreach(object[] o in l)
                    {
                        ProductsBC bc = new ProductsBC();

                        bc.CustomerId = (Guid)o[0];
                        bc.CustomerIdProduct = (Guid)o[1];
                        bc.CodeBar = (string)o[2];
                        bc.QTY = (decimal)o[3];
                        // bc.Description = (string)o[4];

                        L.Add(bc);
                    }

                }

                public static List<ProductsBC> L = new List<ProductsBC>();
            }

        }
        public class LastUpdDB
        {
            public static Guid CustumerId { get; set; }
            public void ins()
            {
                string cmdIns = "INSERT INTO LastUpd (CustumerId,NameTicket,DateLastUpd,[User],Upd,IdEstablishment) VALUES ('{CustumerId}','{NameTicket}','{DateLastUpd}','{User}', 'false', '{IdEstablishment}')";


                string c = cmdIns
                           .Replace("{CustumerId}", ClassGlobalVar.CustumerId.ToString())
                           .Replace("{NameTicket}", ClassGlobalVar.nameTicket.Replace("'", "''"))
                           .Replace("{DateLastUpd}", DateTime.Now.ToString())
                           .Replace("{User}", ClassGlobalVar.user.Replace("'", "''"))
                           .Replace("{IdEstablishment}", ClassGlobalVar.IdEstablishment.ToString())
                           ;
                new ClassDB(null).queryNonResonse(c);
            }
            private void recall(Guid CustumerId)
            {
                string cmdUpd = "UPDATE LastUpd SET  Upd='false' WHERE NOT CustumerId = '{CustumerId}'";
                string c = cmdUpd
                          .Replace("{CustumerId}", CustumerId.ToString())
                          .Replace("{IdEstablishment}", ClassGlobalVar.IdEstablishment.ToString())
                          ;
                new ClassDB(null).queryNonResonse(c);
            }
            public void mod(string nameTicket, DateTime dateLastUpd, string user, Guid CustumerId, bool Recall)
            {
                string cmdUpd = "UPDATE LastUpd SET NameTicket='{NameTicket}',DateLastUpd='{DateLastUpd}',[User]='{User}', Upd='true',IdEstablishment='{IdEstablishment}' WHERE CustumerId= '{CustumerId}'";

                string c = cmdUpd
                           .Replace("{CustumerId}", CustumerId.ToString())
                           .Replace("{NameTicket}", nameTicket.Replace("'", "''"))
                           .Replace("{DateLastUpd}", dateLastUpd.ToString())
                           .Replace("{User}", user.Replace("'", "''"))
                           .Replace("{IdEstablishment}", ClassGlobalVar.IdEstablishment.ToString())
                           ;
                new ClassDB(null).queryNonResonse(c);
                if (Recall) recall(CustumerId);
            }
            public bool getUpd()
            {
                string cmd = "SELECT Upd FROM LastUpd WHERE CustumerId='{CustumerId}'";

                string c = cmd.Replace("{CustumerId}", ClassGlobalVar.CustumerId.ToString());

                List<object[]> l = (new ClassDB(null).queryResonse(c));



                if (l.Count == 0)
                {
                    ins();
                    return false;
                }
                else return (bool)l[0][0];
            }
            public void del(string nameTicket, DateTime dateLastUpd, string user, Guid CustumerId)
            {
                string cmd = "DELETE FROM LastUpd  WHERE CustumerId= '{CustumerId}'";

                string c = cmd.Replace("{CustumerId}", CustumerId.ToString());
                new ClassDB(null).queryNonResonse(c);
            }
            public object getUpdAB()
            {
                string cmd = "SELECT Upd, DateLastUpd FROM LastUpd WHERE CustumerId='{CustumerId}'";

                string c = cmd.Replace("{CustumerId}", ClassGlobalVar.CustumerId.ToString());

                List<object[]> l = (new ClassDB(null).queryResonse(c));

                object res = null;

                if (l.Count == 0)
                {
                    ins();

                }
                else
                {

                    if (!(bool)l[0][0])

                        res = l[0][1];
                }

                return res;
            }
            public class PayProductsDB
            {
                public partial class PayProducts
                {
                    public Guid IdCheckTicket { get; set; }
                    public System.Guid ProductId { get; set; }
                    public decimal QTY { get; set; }
                    public string Name { get; set; }
                    public string Barcode { get; set; }
                    public decimal Price { get; set; }
                    public System.Guid ChecksTicketCustumerId { get; set; }
                    public System.Guid ChecksTicketCloseTicketCustumerId { get; set; }
                }
                public void ins(PayProducts p)
                {
                    string cmdIns = "INSERT INTO PayProducts (IdCheckTicket,ProductId,QTY,Name,Barcode,Price)" +
                        " VALUES ('{IdCheckTicket}','{ProductId}','{QTY}','{Name}','{Barcode}','{Price}')";

                    Guid g = Guid.NewGuid();

                    string c = cmdIns
                               .Replace("{IdCheckTicket}", p.IdCheckTicket.ToString())
                               .Replace("{ProductId}", p.ProductId.ToString())
                               .Replace("{QTY}", p.QTY.ToString())
                               .Replace("{Name}", p.Name.Replace("'", "''"))
                               .Replace("{Barcode}", p.Name.Replace("'", "''"))
                               .Replace("{Price}", p.Name.Replace("'", "''"));
                }
                public void mod()
                {

                }
                public void rem()
                {

                }
            }
        }
        public class ClassCloseTicket
        {
            private void setPayType(CloseTicketG t, XDocument x)
            {


                foreach (var type in Class.ClassSync.TypesPayDB.t)
                {


                    switch (type.NameCourt)
                    {
                        case "BankChecks": t.PayBankChecks = decimal.Parse(x.Element("checks").Attribute("BankChecks").Value.Replace(".", ",")); break;
                        case "BankCards": t.PayBankCards = decimal.Parse(x.Element("checks").Attribute("BankCards").Value.Replace(".", ",")); break;
                        case "Cash": t.PayCash = decimal.Parse(x.Element("checks").Attribute("Cash").Value.Replace(".", ",")); break;
                        case "Resto": t.PayResto = decimal.Parse(x.Element("checks").Attribute("Resto").Value.Replace(".", ",")); break;
                        case "1": t.Pay1 = decimal.Parse(x.Element("checks").Attribute("1").Value.Replace(".", ",")); break;
                        case "2": t.Pay2 = decimal.Parse(x.Element("checks").Attribute("2").Value.Replace(".", ",")); break;
                        case "3": t.Pay3 = decimal.Parse(x.Element("checks").Attribute("3").Value.Replace(".", ",")); break;
                        case "4": t.Pay4 = decimal.Parse(x.Element("checks").Attribute("4").Value.Replace(".", ",")); break;
                        case "5": t.Pay5 = decimal.Parse(x.Element("checks").Attribute("5").Value.Replace(".", ",")); break;
                        case "6": t.Pay6 = decimal.Parse(x.Element("checks").Attribute("6").Value.Replace(".", ",")); break;
                        case "7": t.Pay7 = decimal.Parse(x.Element("checks").Attribute("7").Value.Replace(".", ",")); break;
                        case "8": t.Pay8 = decimal.Parse(x.Element("checks").Attribute("8").Value.Replace(".", ",")); break;
                        case "9": t.Pay9 = decimal.Parse(x.Element("checks").Attribute("9").Value.Replace(".", ",")); break;
                        case "10": t.Pay10 = decimal.Parse(x.Element("checks").Attribute("10").Value.Replace(".", ",")); break;
                        case "11": t.Pay11 = decimal.Parse(x.Element("checks").Attribute("11").Value.Replace(".", ",")); break;
                        case "12": t.Pay12 = decimal.Parse(x.Element("checks").Attribute("12").Value.Replace(".", ",")); break;
                        case "13": t.Pay13 = decimal.Parse(x.Element("checks").Attribute("13").Value.Replace(".", ",")); break;
                        case "14": t.Pay14 = decimal.Parse(x.Element("checks").Attribute("14").Value.Replace(".", ",")); break;
                        case "15": t.Pay15 = decimal.Parse(x.Element("checks").Attribute("15").Value.Replace(".", ",")); break;
                        case "16": t.Pay16 = decimal.Parse(x.Element("checks").Attribute("16").Value.Replace(".", ",")); break;
                        case "17": t.Pay17 = decimal.Parse(x.Element("checks").Attribute("17").Value.Replace(".", ",")); break;
                        case "18": t.Pay18 = decimal.Parse(x.Element("checks").Attribute("18").Value.Replace(".", ",")); break;
                        case "19": t.Pay19 = decimal.Parse(x.Element("checks").Attribute("19").Value.Replace(".", ",")); break;
                        default: break;
                    }
                }

            }
            private void setPayType(CloseTicket t, XDocument x)
            {


                foreach (var type in Class.ClassSync.TypesPayDB.t)
                {


                    switch (type.NameCourt)
                    {

                        case "BankChecks": t.PayBankChecks = decimal.Parse(x.Element("checks").Attribute("BankChecks").Value.Replace(".", ",")); break;
                        case "BankCards": t.PayBankCards = decimal.Parse(x.Element("checks").Attribute("BankCards").Value.Replace(".", ",")); break;
                        case "Cash": t.PayCash = decimal.Parse(x.Element("checks").Attribute("Cash").Value.Replace(".", ",")); break;
                        case "Resto": t.PayResto = decimal.Parse(x.Element("checks").Attribute("Resto").Value.Replace(".", ",")); break;
                        case "1": t.Pay1 = decimal.Parse(x.Element("checks").Attribute("1").Value.Replace(".", ",")); break;
                        case "2": t.Pay2 = decimal.Parse(x.Element("checks").Attribute("2").Value.Replace(".", ",")); break;
                        case "3": t.Pay3 = decimal.Parse(x.Element("checks").Attribute("3").Value.Replace(".", ",")); break;
                        case "4": t.Pay4 = decimal.Parse(x.Element("checks").Attribute("4").Value.Replace(".", ",")); break;
                        case "5": t.Pay5 = decimal.Parse(x.Element("checks").Attribute("5").Value.Replace(".", ",")); break;
                        case "6": t.Pay6 = decimal.Parse(x.Element("checks").Attribute("6").Value.Replace(".", ",")); break;
                        case "7": t.Pay7 = decimal.Parse(x.Element("checks").Attribute("7").Value.Replace(".", ",")); break;
                        case "8": t.Pay8 = decimal.Parse(x.Element("checks").Attribute("8").Value.Replace(".", ",")); break;
                        case "9": t.Pay9 = decimal.Parse(x.Element("checks").Attribute("9").Value.Replace(".", ",")); break;
                        case "10": t.Pay10 = decimal.Parse(x.Element("checks").Attribute("10").Value.Replace(".", ",")); break;
                        case "11": t.Pay11 = decimal.Parse(x.Element("checks").Attribute("11").Value.Replace(".", ",")); break;
                        case "12": t.Pay12 = decimal.Parse(x.Element("checks").Attribute("12").Value.Replace(".", ",")); break;
                        case "13": t.Pay13 = decimal.Parse(x.Element("checks").Attribute("13").Value.Replace(".", ",")); break;
                        case "14": t.Pay14 = decimal.Parse(x.Element("checks").Attribute("14").Value.Replace(".", ",")); break;
                        case "15": t.Pay15 = decimal.Parse(x.Element("checks").Attribute("15").Value.Replace(".", ",")); break;
                        case "16": t.Pay16 = decimal.Parse(x.Element("checks").Attribute("16").Value.Replace(".", ",")); break;
                        case "17": t.Pay17 = decimal.Parse(x.Element("checks").Attribute("17").Value.Replace(".", ",")); break;
                        case "18": t.Pay18 = decimal.Parse(x.Element("checks").Attribute("18").Value.Replace(".", ",")); break;
                        case "19": t.Pay19 = decimal.Parse(x.Element("checks").Attribute("19").Value.Replace(".", ",")); break;
                        case "20": t.Pay20 = decimal.Parse(x.Element("checks").Attribute("20").Value.Replace(".", ",")); break;

                    }
                }


            }
            private void setPayType(ChecksTicket t, XElement x)
            {


                foreach (var type in Class.ClassSync.TypesPayDB.t)
                {


                    switch (type.NameCourt.TrimEnd())
                    {
                        case "BankChecks": t.PayBankChecks = decimal.Parse(x.Attribute("BankChecks").Value.Replace(".", ",")); break;
                        case "BankCards": t.PayBankCards = decimal.Parse(x.Attribute("BankCards").Value.Replace(".", ",")); break;
                        case "Cash": t.PayCash = decimal.Parse(x.Attribute("Cash").Value.Replace(".", ",")); break;
                        case "Resto": t.PayResto = decimal.Parse(x.Attribute("Resto").Value.Replace(".", ",")); break;
                        case "1": t.Pay1 = decimal.Parse(x.Attribute("1").Value.Replace(".", ",")); break;
                        case "2": t.Pay2 = decimal.Parse(x.Attribute("2").Value.Replace(".", ",")); break;
                        case "3": t.Pay3 = decimal.Parse(x.Attribute("3").Value.Replace(".", ",")); break;
                        case "4": t.Pay4 = decimal.Parse(x.Attribute("4").Value.Replace(".", ",")); break;
                        case "5": t.Pay5 = decimal.Parse(x.Attribute("5").Value.Replace(".", ",")); break;
                        case "6": t.Pay6 = decimal.Parse(x.Attribute("6").Value.Replace(".", ",")); break;
                        case "7": t.Pay7 = decimal.Parse(x.Attribute("7").Value.Replace(".", ",")); break;
                        case "8": t.Pay8 = decimal.Parse(x.Attribute("8").Value.Replace(".", ",")); break;
                        case "9": t.Pay9 = decimal.Parse(x.Attribute("9").Value.Replace(".", ",")); break;
                        case "10": t.Pay10 = decimal.Parse(x.Attribute("10").Value.Replace(".", ",")); break;
                        case "11": t.Pay11 = decimal.Parse(x.Attribute("11").Value.Replace(".", ",")); break;
                        case "12": t.Pay12 = decimal.Parse(x.Attribute("12").Value.Replace(".", ",")); break;
                        case "13": t.Pay13 = decimal.Parse(x.Attribute("13").Value.Replace(".", ",")); break;
                        case "14": t.Pay14 = decimal.Parse(x.Attribute("14").Value.Replace(".", ",")); break;
                        case "15": t.Pay15 = decimal.Parse(x.Attribute("15").Value.Replace(".", ",")); break;
                        case "16": t.Pay16 = decimal.Parse(x.Attribute("16").Value.Replace(".", ",")); break;
                        case "17": t.Pay17 = decimal.Parse(x.Attribute("17").Value.Replace(".", ",")); break;
                        case "18": t.Pay18 = decimal.Parse(x.Attribute("18").Value.Replace(".", ",")); break;
                        case "19": t.Pay19 = decimal.Parse(x.Attribute("19").Value.Replace(".", ",")); break;
                        default: break;

                    }
                }

            }
            public ClassCloseTicket(string path, string file)
            {

                XDocument x = XDocument.Load(file);

                CloseTicket closeTicket = new CloseTicket();
                setPayType(closeTicket, x);
                closeTicket.DateOpen = DateTime.Parse(x.Element("checks").Attribute("openDate").Value);
                closeTicket.DateClose = DateTime.Parse(x.Element("checks").Attribute("closeDate").Value);
                closeTicket.NameTicket = x.Element("checks").Attribute("ticket").Value;
                closeTicket.CloseTicketGCustumerId = ClassGlobalVar.TicketWindowG;
                closeTicket.CustumerId = ClassGlobalVar.TicketWindow;


                IEnumerable<XElement> elm = x.Element("checks").Elements("check");

                foreach (XElement e in elm)
                {
                    ChecksTicket t = new ChecksTicket();
                    t.BarCode = e.Attribute("barcodeCheck").Value;
                    t.Date = DateTime.Parse(e.Attribute("date").Value);
                    setPayType(t, e);
                    t.CloseTicketCustumerId = closeTicket.CustumerId;
                    t.CustumerId = Guid.NewGuid();
                    t.Rendu = decimal.Parse(e.Attribute("Rendu").Value.Replace(".", ","));
                    t.TotalTTC = decimal.Parse(e.Attribute("sum").Value.Replace(".", ","));
                    IEnumerable<XElement> pr = e.Elements("product");
                    closeTicket.PayCash = closeTicket.PayCash + t.Rendu;

                    if (e.Attribute("DCBC") != null)
                    {
                        t.CheckDiscount = new CloseTicketCheckDiscount();
                        t.CheckDiscount.DCBC = e.Attribute("DCBC").Value;
                        t.CheckDiscount.DCBC_BiloPoints = int.Parse(e.Attribute("DCBC_BiloPoints").Value);
                        t.CheckDiscount.DCBC_DobavilePoints = int.Parse(e.Attribute("DCBC_DobavilePoints").Value);
                        t.CheckDiscount.DCBC_OtnayliPoints = int.Parse(e.Attribute("DCBC_OtnayliPoints").Value);
                        t.CheckDiscount.DCBC_OstalosPoints = int.Parse(e.Attribute("DCBC_OstalosPoints").Value);
                        t.CheckDiscount.DCBC_name = e.Attribute("DCBC_name").Value;
                        t.CheckDiscount.CloseTicketCheckcCustomer = t.CustumerId;
                        t.CheckDiscount.DiscountCardsCustumerId = Guid.Empty;
                        t.CheckDiscount.CustumerId = Guid.NewGuid();
                    }
                    foreach (XElement xe in pr)
                    {
                        PayProducts p = new PayProducts();
                        p.ProductId = Guid.Parse(xe.Element("CustumerId").Value);
                        p.Name = xe.Element("Name").Value;
                        p.Barcode = xe.Element("CodeBare").Value;
                        p.PriceHT = decimal.Parse(xe.Element("price").Value.Replace('.', ','));
                        p.QTY = decimal.Parse(xe.Element("qty").Value.Replace('.', ','));
                        p.TVA = ClassTVA.getTVA(int.Parse(xe.Element("tva").Value));
                        p.Total = decimal.Parse(xe.Element("total").Value.Replace('.', ','));
                        p.ChecksTicketCloseTicketCustumerId = t.CloseTicketCustumerId;
                        p.ChecksTicketCustumerId = t.CustumerId;
                        p.IdCheckTicket = Guid.NewGuid();
                        p.Discount = decimal.Parse(xe.Element("Discount").Value.Replace('.', ','));
                        p.sumDiscount = decimal.Parse(xe.Element("sumDiscount").Value.Replace('.', ','));
                        t.PayProducts.Add(p);

                    }
                    closeTicket.ChecksTicket.Add(t);
                }

                closeTicket.ins(closeTicket);



                //    OpenTicketWindow.close();

            }
            public static ICollection<CloseTicket> selectCloseTicket(Guid CloseTicketGCustumerId)
            {
                string cmd = "SELECT * FROM CloseTicket WHERE CloseTicketGCustumerId = '" + CloseTicketGCustumerId.ToString() + "'";

                List<object[]> table = new ClassDB(null).queryResonse(cmd);

                List<CloseTicket> R = new List<CloseTicket>();

                foreach (var e in table)
                {
                    CloseTicket ct = new CloseTicket();

                    ct.CustumerId = (Guid)e[0];
                    ct.NameTicket = (string)e[1];
                    ct.DateOpen = (DateTime)e[2];
                    ct.DateClose = (DateTime)e[3];
                    ct.PayBankChecks = ClassETC_fun.testFromNuul(e[4]);
                    ct.PayBankCards = ClassETC_fun.testFromNuul(e[5]);
                    ct.PayCash = ClassETC_fun.testFromNuul(e[6]);
                    ct.PayResto = ClassETC_fun.testFromNuul(e[7]);
                    ct.Pay1 = ClassETC_fun.testFromNuul(e[8]);
                    ct.Pay2 = ClassETC_fun.testFromNuul(e[9]);
                    ct.Pay3 = ClassETC_fun.testFromNuul(e[10]);
                    ct.Pay4 = ClassETC_fun.testFromNuul(e[11]);
                    ct.Pay5 = ClassETC_fun.testFromNuul(e[12]);
                    ct.Pay6 = ClassETC_fun.testFromNuul(e[13]);
                    ct.Pay7 = ClassETC_fun.testFromNuul(e[14]);
                    ct.Pay8 = ClassETC_fun.testFromNuul(e[15]);
                    ct.Pay9 = ClassETC_fun.testFromNuul(e[16]);
                    ct.Pay10 = ClassETC_fun.testFromNuul(e[17]);
                    ct.Pay11 = ClassETC_fun.testFromNuul(e[18]);
                    ct.Pay12 = ClassETC_fun.testFromNuul(e[19]);
                    ct.Pay13 = ClassETC_fun.testFromNuul(e[20]);
                    ct.Pay14 = ClassETC_fun.testFromNuul(e[21]);
                    ct.Pay15 = ClassETC_fun.testFromNuul(e[22]);
                    ct.Pay16 = ClassETC_fun.testFromNuul(e[23]);
                    ct.Pay17 = ClassETC_fun.testFromNuul(e[24]);
                    ct.Pay18 = ClassETC_fun.testFromNuul(e[25]);
                    ct.Pay19 = ClassETC_fun.testFromNuul(e[26]);
                    ct.Pay20 = ClassETC_fun.testFromNuul(e[27]);
                    ct.CloseTicketGCustumerId = (Guid)e[28];



                    ct.ChecksTicket = new ChecksTicket().get(ct.CustumerId);

                    foreach (ChecksTicket ch in ct.ChecksTicket)
                    {
                        ch.PayProducts = new PayProducts().get(ch.CustumerId);
                    }
                    R.Add(ct);
                }

                return R;
            }
            public partial class PayProducts
            {
                public List<PayProducts> get(Guid g)
                {
                    string cmd = "SELECT * FROM PayProducts WHERE ChecksTicketCustumerId = '" + g.ToString() + "'";

                    List<object[]> L = new ClassDB(null).queryResonse(cmd);

                    List<PayProducts> PP = new List<PayProducts>();

                    foreach (var o in L)
                    {
                        PayProducts p = new PayProducts();

                        p.IdCheckTicket = (Guid)o[0];

                        p.ProductId = (Guid)o[1];

                        p.Name = (string)o[2];

                        p.Barcode = (string)o[3];

                        p.QTY = (decimal)o[4];

                        p.TVA = (decimal)o[5];

                        p.PriceHT = (decimal)o[6];

                        p.Total = (decimal)o[7];

                        p.ChecksTicketCustumerId = (Guid)o[8];

                        p.ChecksTicketCloseTicketCustumerId = (Guid)o[9];

                        p.Discount = o[10] is DBNull ? 0.0m : (decimal)o[10];

                        p.sumDiscount = o[11] is DBNull ? 0.0m : (decimal)o[11];



                        PP.Add(p);
                    }
                    return PP;
                }
                public System.Guid IdCheckTicket { get; set; }
                public System.Guid ProductId { get; set; }
                public string Name { get; set; }
                public string Barcode { get; set; }
                public decimal QTY { get; set; }
                public decimal TVA { get; set; }
                public decimal PriceHT { get; set; }
                public decimal Total { get; set; }
                public System.Guid ChecksTicketCustumerId { get; set; }
                public System.Guid ChecksTicketCloseTicketCustumerId { get; set; }
                public decimal Discount { get; set; }
                public decimal sumDiscount { get; set; }

            }

            public partial class CloseTicketCheckDiscount
            {
                public System.Guid CustumerId { get; set; }
                public System.Guid CloseTicketCheckcCustomer { get; set; }
                public System.Guid DiscountCardsCustumerId { get; set; }
                public string DCBC { get; set; }
                public Nullable<int> DCBC_BiloPoints { get; set; }
                public Nullable<int> DCBC_DobavilePoints { get; set; }
                public Nullable<int> DCBC_OtnayliPoints { get; set; }
                public Nullable<int> DCBC_OstalosPoints { get; set; }
                public string DCBC_name { get; set; }

                public CloseTicketCheckDiscount get(Guid customerId)
                {
                    CloseTicketCheckDiscount discount = new CloseTicketCheckDiscount();



                    string cmd = "SELECT * FROM CloseTicketCheckDiscount WHERE CloseTicketCheckcCustomer ='" + customerId.ToString() + "'";

                    List<object[]> o = new ClassDB(null).queryResonse(cmd);

                    if (o.Count == 1)
                    {
                        discount.CustumerId = (Guid)o[0][0];
                        discount.CloseTicketCheckcCustomer = (Guid)o[0][1];
                        discount.DiscountCardsCustumerId = (Guid)o[0][2];
                        discount.DCBC = (string)o[0][3];
                        discount.DCBC_BiloPoints = (int)o[0][4];
                        discount.DCBC_DobavilePoints = (int)o[0][5];
                        discount.DCBC_OtnayliPoints = (int)o[0][6];
                        discount.DCBC_OstalosPoints = (int)o[0][7];
                        discount.DCBC_name = (string)o[0][8];

                    }


                    return discount;
                }
            }

            public partial class ChecksTicket
            {

                public ChecksTicket()
                {
                    this.PayProducts = new HashSet<PayProducts>();
                }



                public List<ChecksTicket> get(Guid g)
                {
                    string cmd = "SELECT * FROM ChecksTicket WHERE CloseTicketCustumerId ='" + g.ToString() + "'";

                    List<ChecksTicket> R = new List<ChecksTicket>();

                    List<object[]> l = new ClassDB(null).queryResonse(cmd);

                    foreach (var o in l)
                    {
                        ChecksTicket check = new ChecksTicket();

                        check.CustumerId = (Guid)o[0];
                        check.BarCode = (string)o[1];
                        check.Date = (DateTime)o[2];
                        check.PayBankChecks = ClassETC_fun.testFromNuul(o[3]);
                        check.PayBankCards = ClassETC_fun.testFromNuul(o[4]);
                        check.PayCash = ClassETC_fun.testFromNuul(o[5]);
                        check.PayResto = ClassETC_fun.testFromNuul(o[6]);
                        check.Pay1 = ClassETC_fun.testFromNuul(o[7]);
                        check.Pay2 = ClassETC_fun.testFromNuul(o[8]);
                        check.Pay3 = ClassETC_fun.testFromNuul(o[9]);
                        check.Pay4 = ClassETC_fun.testFromNuul(o[10]);
                        check.Pay5 = ClassETC_fun.testFromNuul(o[11]);
                        check.Pay6 = ClassETC_fun.testFromNuul(o[12]);
                        check.Pay7 = ClassETC_fun.testFromNuul(o[13]);
                        check.Pay8 = ClassETC_fun.testFromNuul(o[14]);
                        check.Pay9 = ClassETC_fun.testFromNuul(o[15]);
                        check.Pay10 = ClassETC_fun.testFromNuul(o[16]);
                        check.Pay11 = ClassETC_fun.testFromNuul(o[17]);
                        check.Pay12 = ClassETC_fun.testFromNuul(o[18]);
                        check.Pay13 = ClassETC_fun.testFromNuul(o[19]);
                        check.Pay14 = ClassETC_fun.testFromNuul(o[20]);
                        check.Pay15 = ClassETC_fun.testFromNuul(o[21]);
                        check.Pay16 = ClassETC_fun.testFromNuul(o[22]);
                        check.Pay17 = ClassETC_fun.testFromNuul(o[23]);
                        check.Pay18 = ClassETC_fun.testFromNuul(o[24]);
                        check.Pay19 = ClassETC_fun.testFromNuul(o[25]);
                        check.Pay20 = ClassETC_fun.testFromNuul(o[26]);
                        check.CloseTicketCustumerId = (Guid)o[27];
                        check.Rendu = ClassETC_fun.testFromNuul(o[28]);
                        check.CheckDiscount = new CloseTicketCheckDiscount().get(check.CustumerId);
                        R.Add(check);
                    }
                    return R;
                }
                public CloseTicketCheckDiscount CheckDiscount { get; set; }
                public System.Guid CustumerId { get; set; }
                public string BarCode { get; set; }
                public System.DateTime Date { get; set; }
                public decimal PayBankChecks { get; set; }
                public decimal PayBankCards { get; set; }
                public decimal PayCash { get; set; }
                public decimal PayResto { get; set; }
                public decimal Pay1 { get; set; }
                public decimal Pay2 { get; set; }
                public decimal Pay3 { get; set; }
                public decimal Pay4 { get; set; }
                public decimal Pay5 { get; set; }
                public decimal Pay6 { get; set; }
                public decimal Pay7 { get; set; }
                public decimal Pay8 { get; set; }
                public decimal Pay9 { get; set; }
                public decimal Pay10 { get; set; }
                public decimal Pay11 { get; set; }
                public decimal Pay12 { get; set; }
                public decimal Pay13 { get; set; }
                public decimal Pay14 { get; set; }
                public decimal Pay15 { get; set; }
                public decimal Pay16 { get; set; }
                public decimal Pay17 { get; set; }
                public decimal Pay18 { get; set; }
                public decimal Pay19 { get; set; }
                public decimal Pay20 { get; set; }
                public System.Guid CloseTicketCustumerId { get; set; }
                public decimal TotalTTC { get; set; }
                public decimal Rendu { get; set; }
                public virtual ICollection<PayProducts> PayProducts { get; set; }
                public virtual CloseTicket CloseTicket { get; set; }
            }
            public partial class CloseTicket
            {
                public List<CloseTicket> get(Guid TicketWindowG)
                {
                    List<object[]> L = new ClassDB(null).queryResonse("SELECT * FROM CloseTicket WHERE CloseTicketGCustumerId ='" + TicketWindowG + "'");

                    List<CloseTicket> R = new List<CloseTicket>();

                    foreach (var e in L)
                    {
                        CloseTicket c = new CloseTicket();
                        c.CustumerId = (Guid)e[0];
                        c.NameTicket = (string)e[1];
                        c.DateOpen = (DateTime)e[2];
                        c.DateClose = (DateTime)e[3];
                        c.PayBankChecks = (decimal)e[4];
                        c.PayBankCards = (decimal)e[5];
                        c.PayCash = (decimal)e[6];
                        c.PayResto = (decimal)e[7];
                        c.Pay1 = ClassETC_fun.testFromNuul(e[8]);
                        c.Pay2 = ClassETC_fun.testFromNuul(e[9]);
                        c.Pay3 = ClassETC_fun.testFromNuul(e[10]);
                        c.Pay4 = ClassETC_fun.testFromNuul(e[11]);
                        c.Pay5 = ClassETC_fun.testFromNuul(e[12]);
                        c.Pay6 = ClassETC_fun.testFromNuul(e[13]);
                        c.Pay7 = ClassETC_fun.testFromNuul(e[14]);
                        c.Pay8 = ClassETC_fun.testFromNuul(e[15]);
                        c.Pay9 = ClassETC_fun.testFromNuul(e[16]);
                        c.Pay10 = ClassETC_fun.testFromNuul(e[17]);
                        c.Pay11 = ClassETC_fun.testFromNuul(e[18]);
                        c.Pay12 = ClassETC_fun.testFromNuul(e[19]);
                        c.Pay13 = ClassETC_fun.testFromNuul(e[20]);
                        c.Pay14 = ClassETC_fun.testFromNuul(e[21]);
                        c.Pay15 = ClassETC_fun.testFromNuul(e[22]);
                        c.Pay16 = ClassETC_fun.testFromNuul(e[23]);
                        c.Pay17 = ClassETC_fun.testFromNuul(e[24]);
                        c.Pay18 = ClassETC_fun.testFromNuul(e[25]);
                        c.Pay19 = ClassETC_fun.testFromNuul(e[26]);
                        c.Pay20 = ClassETC_fun.testFromNuul(e[27]);
                        c.CloseTicketGCustumerId = (Guid)e[28];
                        R.Add(c);
                    }

                    return R;
                }
                public void ins(CloseTicket CloseTicket)
                {
                    W_Close_TicketWindow w = ClassETC_fun.findWindow("W_Close_Ticket") as W_Close_TicketWindow;

                    w.BusyIndicator.IsBusy = true;
                    w.BusyIndicator.BusyContent = "Initializing...";
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                    worker.DoWork += (o, a) =>
                    {

                        string cmdInsCloseTicket =
               "INSERT INTO CloseTicket (CustumerId,NameTicket,DateOpen,DateClose,PayBankChecks,PayBankCards,PayCash,PayResto,CloseTicketGCustumerId)"
                + "VALUES ('{CustumerId}','{NameTicket}','{DateOpen}','{DateClose}',{PayBankChecks},{PayBankCards},{PayCash},{PayResto},'{CloseTicketGCustumerId}')";
                        string c = cmdInsCloseTicket
                                    .Replace("{CustumerId}", CloseTicket.CustumerId.ToString())
                                    .Replace("{NameTicket}", CloseTicket.NameTicket.Replace("'", "''"))
                                    .Replace("{DateOpen}", CloseTicket.DateOpen.ToString())
                                    .Replace("{DateClose}", CloseTicket.DateClose.ToString())
                                    .Replace("{PayBankChecks}", CloseTicket.PayBankChecks.ToString().Replace(",", "."))
                                    .Replace("{PayBankCards}", CloseTicket.PayBankCards.ToString().Replace(",", "."))
                                    .Replace("{PayCash}", CloseTicket.PayCash.ToString().Replace(",", "."))
                                    .Replace("{PayResto}", CloseTicket.PayResto.ToString().Replace(",", "."))
                                    .Replace("{CloseTicketGCustumerId}", CloseTicket.CloseTicketGCustumerId.ToString())

                                    ;
                        new ClassDB(null).queryNonResonse(c);


                        foreach (var e in CloseTicket.ChecksTicket)
                        {
                            string cmdInsChecksTicket =
                        "INSERT INTO ChecksTicket (CustumerId,BarCode,[Date],PayBankChecks,PayBankCards,PayCash,PayResto,CloseTicketCustumerId,Rendu,TotalTTC)"
                         + "VALUES ('{CustumerId}','{BarCode}',CAST('{Date}' AS DateTime), {PayBankChecks},{PayBankCards},{PayCash},{PayResto},'{CloseTicketCustumerId}','{Rendu}', '{TotalTTC}')";

                            c = cmdInsChecksTicket
                                       .Replace("{CustumerId}", e.CustumerId.ToString())
                                       .Replace("{BarCode}", e.BarCode)
                                       .Replace("{Date}", e.Date.ToString())
                                       .Replace("{PayBankChecks}", e.PayBankChecks.ToString().Replace(",", "."))
                                       .Replace("{PayBankCards}", e.PayBankCards.ToString().Replace(",", "."))
                                       .Replace("{PayCash}", e.PayCash.ToString().Replace(",", "."))
                                       .Replace("{PayResto}", e.PayResto.ToString().Replace(",", "."))
                                       .Replace("{CloseTicketCustumerId}", e.CloseTicketCustumerId.ToString())
                                       .Replace("{Rendu}", e.Rendu.ToString().Replace(",", "."))
                                       .Replace("{TotalTTC}", e.TotalTTC.ToString().Replace(",", "."));
                            new ClassDB(null).queryNonResonse(c);

                            foreach (var m in e.PayProducts)
                            {

                                string cmdInsPayProducts =
                            "INSERT INTO PayProducts (ProductId,Name,Barcode,QTY,TVA,PriceHT,Total,ChecksTicketCustumerId,ChecksTicketCloseTicketCustumerId,IdCheckTicket,Discount,sumDiscount)"
                             + "VALUES ('{ProductId}','{Name}','{Barcode}',{QTY},{TVA},{PriceHT},{Total},'{ChecksTicketCustumerId}','{ChecksTicketCloseTicketCustumerId}','{IdCheckTicket}',{Discount},{sumDiscount})";

                                c = cmdInsPayProducts
                                     .Replace("{ProductId}", m.ProductId.ToString())
                                     .Replace("{Name}", m.Name.Replace("'", "''"))
                                     .Replace("{Barcode}", m.Barcode.ToString())
                                     .Replace("{QTY}", m.QTY.ToString().Replace(",", "."))
                                     .Replace("{TVA}", m.TVA.ToString().Replace(",", "."))
                                     .Replace("{PriceHT}", m.PriceHT.ToString().Replace(",", "."))
                                     .Replace("{Total}", m.Total.ToString().Replace(",", "."))
                                     .Replace("{ChecksTicketCustumerId}", m.ChecksTicketCustumerId.ToString())
                                     .Replace("{ChecksTicketCloseTicketCustumerId}", m.ChecksTicketCloseTicketCustumerId.ToString())
                                     .Replace("{IdCheckTicket}", m.IdCheckTicket.ToString())
                                     .Replace("{Discount}", (m.Discount).ToString().Replace(",", "."))
                                     .Replace("{sumDiscount}", (m.sumDiscount).ToString().Replace(",", "."));
                                new ClassDB(null).queryNonResonse(c);


                            }
                            if (e.CheckDiscount != null)
                            {
                                string cmdCloseTicketCheckDiscount =
                                   "INSERT INTO CloseTicketCheckDiscount (CustumerId,CloseTicketCheckcCustomer,DiscountCardsCustumerId,DCBC,DCBC_BiloPoints,DCBC_DobavilePoints,DCBC_OtnayliPoints,DCBC_OstalosPoints,DCBC_name)"
                             + "VALUES ('{CustumerId}','{CloseTicketCheckcCustomer}','{DiscountCardsCustumerId}','{DCBC}',{DCBC_BiloPoints},{DCBC_DobavilePoints},{DCBC_OtnayliPoints},{DCBC_OstalosPoints},'{DCBC_name}')";
                                ;

                                c = cmdCloseTicketCheckDiscount
                                     .Replace("{CustumerId}", e.CheckDiscount.CustumerId.ToString())
                                    .Replace("{CloseTicketCheckcCustomer}", e.CheckDiscount.CloseTicketCheckcCustomer.ToString())
                                    .Replace("{DiscountCardsCustumerId}", e.CheckDiscount.DiscountCardsCustumerId.ToString())
                                    .Replace("{DCBC}", e.CheckDiscount.DCBC.ToString())
                                    .Replace("{DCBC_BiloPoints}", e.CheckDiscount.DCBC_BiloPoints.ToString())
                                    .Replace("{DCBC_DobavilePoints}", e.CheckDiscount.DCBC_DobavilePoints.ToString())
                                    .Replace("{DCBC_OtnayliPoints}", e.CheckDiscount.DCBC_OtnayliPoints.ToString())
                                    .Replace("{DCBC_OstalosPoints}", e.CheckDiscount.DCBC_OstalosPoints.ToString())
                                    .Replace("{DCBC_name}", e.CheckDiscount.DCBC_name.ToString());
                                new ClassDB(null).queryNonResonse(c);
                            }
                        }
                    };
                    worker.RunWorkerCompleted += (o, a) =>
                    {
                        w.BusyIndicator.IsBusy = false;
                    };
                    worker.RunWorkerAsync();


                }
                public CloseTicket()
                {
                    this.ChecksTicket = new HashSet<ChecksTicket>();
                }

                public System.Guid CustumerId { get; set; }
                public string NameTicket { get; set; }
                public System.DateTime DateOpen { get; set; }
                public System.DateTime DateClose { get; set; }
                public decimal PayBankChecks { get; set; }
                public decimal PayBankCards { get; set; }
                public decimal PayCash { get; set; }
                public decimal PayResto { get; set; }
                public decimal Pay1 { get; set; }
                public decimal Pay2 { get; set; }
                public decimal Pay3 { get; set; }
                public decimal Pay4 { get; set; }
                public decimal Pay5 { get; set; }
                public decimal Pay6 { get; set; }
                public decimal Pay7 { get; set; }
                public decimal Pay8 { get; set; }
                public decimal Pay9 { get; set; }
                public decimal Pay10 { get; set; }
                public decimal Pay11 { get; set; }
                public decimal Pay12 { get; set; }
                public decimal Pay13 { get; set; }
                public decimal Pay14 { get; set; }
                public decimal Pay15 { get; set; }
                public decimal Pay16 { get; set; }
                public decimal Pay17 { get; set; }
                public decimal Pay18 { get; set; }
                public decimal Pay19 { get; set; }
                public decimal Pay20 { get; set; }
                public System.Guid CloseTicketGCustumerId { get; set; }
                public virtual ICollection<ChecksTicket> ChecksTicket { get; set; }
            }
            public partial class CloseTicketG
            {
                public static string mess { get; set; }
                private static CloseTicketG setPayTypeNULL(CloseTicketG t)
                {
                    t.DateClose = new DateTime(2014, 1, 1);

                    foreach (var type in Class.ClassSync.TypesPayDB.t)
                    {


                        switch (type.NameCourt)
                        {
                            case "BankChecks": t.PayBankChecks = 0.0m; break;
                            case "BankCards": t.PayBankCards = 0.0m; break;
                            case "Cash": t.PayCash = 0.0m; break;
                            case "Resto": t.PayResto = 0.0m; break;
                            case "1": t.Pay1 = 0.0m; break;
                            case "2": t.Pay2 = 0.0m; break;
                            case "3": t.Pay3 = 0.0m; break;
                            case "4": t.Pay4 = 0.0m; break;
                            case "5": t.Pay5 = 0.0m; break;
                            case "6": t.Pay6 = 0.0m; break;
                            case "7": t.Pay7 = 0.0m; break;
                            case "8": t.Pay8 = 0.0m; break;
                            case "9": t.Pay9 = 0.0m; break;
                            case "10": t.Pay10 = 0.0m; break;
                            case "11": t.Pay11 = 0.0m; break;
                            case "12": t.Pay12 = 0.0m; break;
                            case "13": t.Pay13 = 0.0m; break;
                            case "14": t.Pay14 = 0.0m; break;
                            case "15": t.Pay15 = 0.0m; break;
                            case "16": t.Pay16 = 0.0m; break;
                            case "17": t.Pay17 = 0.0m; break;
                            case "18": t.Pay18 = 0.0m; break;
                            case "19": t.Pay19 = 0.0m; break;
                            default: break;
                        }
                    }
                    return t;
                }
                private static void ins(CloseTicketG ct)
                {
                    ct = setPayTypeNULL(ct);
                    string cmdInsCloseTicketG =
              "INSERT INTO CloseTicketG (CustumerId,EstablishmentCustomerId,DateOpen,DateClose,PayBankChecks,PayBankCards,PayCash,PayResto,Pay1)"
               + "VALUES ('{CustumerId}','{EstablishmentCustomerId}','{DateOpen}','{DateClose}',{PayBankChecks},{PayBankCards},{PayCash},{PayResto},{Pay1})";
                    string c = cmdInsCloseTicketG
                               .Replace("{CustumerId}", ct.CustumerId.ToString())
                               .Replace("{EstablishmentCustomerId}", ClassGlobalVar.IdEstablishment.ToString())
                               .Replace("{DateOpen}", ct.DateOpen.ToString())
                               .Replace("{DateClose}", ct.DateClose.ToString())
                               .Replace("{PayBankChecks}", ct.PayBankChecks != null ? ct.PayBankChecks.ToString().Replace(",", ".") : "0")
                               .Replace("{PayBankCards}", ct.PayBankCards != null ? ct.PayBankCards.ToString().Replace(",", ".") : "0")
                               .Replace("{PayCash}", ct.PayCash != null ? ct.PayCash.ToString().Replace(",", ".") : "0")
                               .Replace("{PayResto}", ct.PayResto != null ? ct.PayResto.ToString().Replace(",", ".") : "0")
                                .Replace("{Pay1}", ct.PayResto != null ? ct.PayResto.ToString().Replace(",", ".") : "0")
                               ;
                    new ClassDB(null).queryNonResonse(c);



                }
                public static List<CloseTicketG> sel(Guid g)
                {
                    List<CloseTicketG> R = new List<CloseTicketG>();
                    R.Clear();

                    string cmdSel = g == Guid.Empty ? "SELECT * FROM CloseTicketG" : "SELECT * FROM CloseTicketG WHERE CustumerId='" + g + "'";
                    List<object[]> o = new ClassDB(null).queryResonse(cmdSel);
                    foreach (var e in o)
                    {
                        CloseTicketG ct = new CloseTicketG();

                        ct.CustumerId = (Guid)e[0];
                        ct.DateOpen = (DateTime)e[1];
                        ct.DateClose = (DateTime)e[2];
                        ct.PayBankChecks = ClassETC_fun.testFromNuul(e[3]);
                        ct.PayBankCards = ClassETC_fun.testFromNuul(e[4]);
                        ct.PayCash = ClassETC_fun.testFromNuul(e[5]);
                        ct.PayResto = ClassETC_fun.testFromNuul(e[6]);
                        ct.Pay1 = ClassETC_fun.testFromNuul(e[7]);
                        ct.Pay2 = ClassETC_fun.testFromNuul(e[8]);
                        ct.Pay3 = ClassETC_fun.testFromNuul(e[9]);
                        ct.Pay4 = ClassETC_fun.testFromNuul(e[10]);
                        ct.Pay5 = ClassETC_fun.testFromNuul(e[11]);
                        ct.Pay6 = ClassETC_fun.testFromNuul(e[12]);
                        ct.Pay7 = ClassETC_fun.testFromNuul(e[13]);
                        ct.Pay8 = ClassETC_fun.testFromNuul(e[14]);
                        ct.Pay9 = ClassETC_fun.testFromNuul(e[15]);
                        ct.Pay10 = ClassETC_fun.testFromNuul(e[16]);
                        ct.Pay11 = ClassETC_fun.testFromNuul(e[17]);
                        ct.Pay12 = ClassETC_fun.testFromNuul(e[18]);
                        ct.Pay13 = ClassETC_fun.testFromNuul(e[19]);
                        ct.Pay14 = ClassETC_fun.testFromNuul(e[20]);
                        ct.Pay15 = ClassETC_fun.testFromNuul(e[21]);
                        ct.Pay16 = ClassETC_fun.testFromNuul(e[22]);
                        ct.Pay17 = ClassETC_fun.testFromNuul(e[23]);
                        ct.Pay18 = ClassETC_fun.testFromNuul(e[24]);
                        ct.Pay19 = ClassETC_fun.testFromNuul(e[25]);
                        ct.Pay20 = ClassETC_fun.testFromNuul(e[26]);
                        ct.EstablishmentCustomerId = (Guid)e[27];
                        R.Add(ct);
                    }

                    return R;
                }

                private static void upd(CloseTicketG m)
                {
                    string cmdUpd = "UPDATE CloseTicketG SET DateOpen='{DateOpen}',DateClose='{DateClose}',PayBankChecks='{PayBankChecks}',PayBankCards={PayBankCards},PayCash='{PayCash}'," +
                                    "PayResto={PayResto},[Pay1]={Pay1} WHERE CustumerId= '{CustumerId}'";
                    string c = cmdUpd
                                .Replace("{DateOpen}", m.DateOpen.ToString())
                                .Replace("{DateClose}", m.DateClose.ToString())
                                .Replace("{PayBankChecks}", m.PayBankChecks.ToString().Replace(",", "."))
                                .Replace("{PayBankCards}", m.PayBankCards.ToString().Replace(",", "."))
                                .Replace("{PayCash}", m.PayCash.ToString().Replace(",", "."))
                                .Replace("{PayResto}", m.PayResto.ToString().Replace(",", "."))
                                .Replace("{Pay1}", m.Pay1.ToString().Replace(",", "."))
                                .Replace("{CustumerId}", m.CustumerId.ToString())
                               ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static Guid create()
                {
                    CloseTicketG g = new CloseTicketG();
                    g.CustumerId = Guid.NewGuid();
                    g.DateOpen = DateTime.Now;
                    g = setPayTypeNULL(g);
                    ins(g);
                    return g.CustumerId;
                }
                public static bool cls()
                {
                    mess = "";



                    OpenTicketWindow.set();

                    List<OpenTicketWindow> O = OpenTicketWindow.table.FindAll(l => ((l.isOpen) && (l.IdTicketWindowG == ClassGlobalVar.TicketWindowG)));

                    bool flag = false;

                    if (O.Count == 0)
                    {
                        List<CloseTicket> T = new CloseTicket().get(ClassGlobalVar.TicketWindowG);

                        List<CloseTicketG> G = sel(ClassGlobalVar.TicketWindowG);

                        mess += "Veuillez patienter! La clôture du " + ClassGlobalVar.nameTicket + ". Exportation vers base de données..." + Environment.NewLine;

                        if (G.Count == 1)
                        {
                            foreach (var el in T)
                            {
                                G[0].Pay1 += el.Pay1;
                                G[0].Pay2 += el.Pay2;
                                G[0].Pay3 += el.Pay3;
                                G[0].Pay4 += el.Pay4;
                                G[0].Pay5 += el.Pay5;
                                G[0].Pay6 += el.Pay6;
                                G[0].Pay7 += el.Pay7;
                                G[0].Pay8 += el.Pay8;
                                G[0].Pay9 += el.Pay9;
                                G[0].Pay10 += el.Pay10;
                                G[0].Pay11 += el.Pay11;
                                G[0].Pay12 += el.Pay12;
                                G[0].Pay13 += el.Pay13;
                                G[0].Pay14 += el.Pay14;
                                G[0].Pay15 += el.Pay15;
                                G[0].Pay16 += el.Pay16;
                                G[0].Pay17 += el.Pay17;
                                G[0].Pay18 += el.Pay18;
                                G[0].Pay19 += el.Pay19;
                                G[0].Pay20 += el.Pay20;
                                G[0].PayBankCards += el.PayBankCards;
                                G[0].PayBankChecks += el.PayBankChecks;
                                G[0].PayCash += el.PayCash;
                                G[0].PayResto += el.PayResto;
                            }

                            G[0].DateClose = DateTime.Now;

                            upd(G[0]);



                            flag = true;
                        }
                        else
                        {
                            mess += "Erreur fatale N° 001" + Environment.NewLine;
                        }
                    }

                    else
                    {
                        foreach (var e in O)
                            mess += e.nameTicket + " est ouverte" + Environment.NewLine;
                    }



                    return flag;
                }
                public static void prnt(Guid TicketWindowG)
                {
                    List<CloseTicketG> list = sel(TicketWindowG);

                    if (list.Count == 0)
                    {
                        mess += "Ошибка печати, нет таких записей";
                    }

                    if (list.Count == 1)
                    {
                        CloseTicketG g = list[0];

                        List<CloseTicket> s = ClassCloseTicket.selectCloseTicket(g.CustumerId).ToList();

                        new ClassPrintCloseTicketG(g, s, "");
                    }

                    if (list.Count > 1)
                    {
                        mess += "Ошибка печати, " + list.Count + " записей в БД";
                    }
                }
                public CloseTicketG()
                {
                    this.CloseTicket = new HashSet<CloseTicket>();
                }
                public System.Guid CustumerId { get; set; }
                public System.DateTime DateOpen { get; set; }
                public System.DateTime DateClose { get; set; }
                public Nullable<decimal> PayBankChecks { get; set; }
                public Nullable<decimal> PayBankCards { get; set; }
                public Nullable<decimal> PayCash { get; set; }
                public Nullable<decimal> PayResto { get; set; }
                public Nullable<decimal> Pay1 { get; set; }
                public Nullable<decimal> Pay2 { get; set; }
                public Nullable<decimal> Pay3 { get; set; }
                public Nullable<decimal> Pay4 { get; set; }
                public Nullable<decimal> Pay5 { get; set; }
                public Nullable<decimal> Pay6 { get; set; }
                public Nullable<decimal> Pay7 { get; set; }
                public Nullable<decimal> Pay8 { get; set; }
                public Nullable<decimal> Pay9 { get; set; }
                public Nullable<decimal> Pay10 { get; set; }
                public Nullable<decimal> Pay11 { get; set; }
                public Nullable<decimal> Pay12 { get; set; }
                public Nullable<decimal> Pay13 { get; set; }
                public Nullable<decimal> Pay14 { get; set; }
                public Nullable<decimal> Pay15 { get; set; }
                public Nullable<decimal> Pay16 { get; set; }
                public Nullable<decimal> Pay17 { get; set; }
                public Nullable<decimal> Pay18 { get; set; }
                public Nullable<decimal> Pay19 { get; set; }
                public Nullable<decimal> Pay20 { get; set; }
                public Guid EstablishmentCustomerId { get; set; }
                public virtual ICollection<CloseTicket> CloseTicket { get; set; }
            }
        }
        public class ClassCloseTicketTmp
        {
            /*           private decimal getQTYProduct(Guid CustumerId)
                       {
                           string c = "SELECT QTY FROM Products" +
                                         " WHERE CustumerId= '{CustumerId}'".Replace("{CustumerId}", CustumerId.ToString());
                           return (decimal)new ClassDB(null).queryResonse(c)[0][0];
                       }
                       private void tackeQTYProduct(decimal qty, Guid CustumerId)
                       {
                           decimal qty_old = getQTYProduct(CustumerId);
                           qty = qty_old - qty;
                           string c = "UPDATE Products SET QTY='{QTY}'".Replace("{QTY}", qty.ToString("0.000").Replace(",", ".")) +
                                 " WHERE CustumerId= '{CustumerId}'".Replace("{CustumerId}", CustumerId.ToString());
                           new ClassDB(null).queryNonResonse(c);
                           ClassProducts.updQTYProduct(qty, CustumerId);
                       }
                       private void appendQTY(decimal qty, Guid CustumerId)
                       {
                           decimal qty_old = getQTYProduct(CustumerId);
                           qty = qty_old + qty;
                           string c = "UPDATE Products SET QTY={QTY}".Replace("{QTY}", qty.ToString()) +
                                 " WHERE CustumerId= '{CustumerId}'".Replace("{CustumerId}", CustumerId.ToString());
                           new ClassDB(null).queryNonResonse(c);
                           ClassProducts.updQTYProduct(qty, CustumerId);
                       }*/
            private void ins_(CloseTicket ct)
            {

                string c = "SELECT CustumerId FROM CloseTicketTmp WHERE CustumerId='" + ct.CustumerId + "'";
                List<object[]> o = new ClassDB(null).queryResonse(c);
                if (o.Count == 0)
                {
                    string cmdInsCloseTicket =
                "INSERT INTO CloseTicketTmp (CustumerId,NameTicket,DateOpen,DateClose,PayBankChecks,PayBankCards,PayCash,PayResto,"
                + "Pay1,Pay2,Pay3,Pay4,Pay5,Pay6,Pay7,Pay8,Pay9,Pay10,Pay11,Pay12,Pay13,Pay14,Pay15,Pay16,Pay17,Pay18,Pay19,Pay20,EstablishmentCustomerId)"
                 + "VALUES ('{CustumerId}','{NameTicket}','{DateOpen}','{DateClose}',{PayBankChecks},{PayBankCards},{PayCash},{PayResto},"
                 + "{Pay1},{Pay2},{Pay3},{Pay4},{Pay5},{Pay6},{Pay7},{Pay8},{Pay9},{Pay10},{Pay11},{Pay12},{Pay13},{Pay14},{Pay15},{Pay16},{Pay17},{Pay18},{Pay19},{Pay20},'{EstablishmentCustomerId}')";
                    c = cmdInsCloseTicket
                              .Replace("{CustumerId}", ct.CustumerId.ToString())
                              .Replace("{NameTicket}", ct.NameTicket.Replace("'", "''"))
                              .Replace("{DateOpen}", ct.DateOpen.ToString())
                              .Replace("{DateClose}", ct.DateClose.ToString())
                              .Replace("{PayBankChecks}", ct.PayBankChecks.ToString().Replace(",", "."))
                              .Replace("{PayBankCards}", ct.PayBankCards.ToString().Replace(",", "."))
                              .Replace("{PayCash}", ct.PayCash.ToString().Replace(",", "."))
                              .Replace("{PayResto}", ct.PayResto.ToString().Replace(",", "."))
                              .Replace("{Pay1}", ct.Pay1.ToString().Replace(",", "."))
                              .Replace("{Pay2}", ct.Pay2.ToString().Replace(",", "."))
                              .Replace("{Pay3}", ct.Pay3.ToString().Replace(",", "."))
                              .Replace("{Pay4}", ct.Pay4.ToString().Replace(",", "."))
                              .Replace("{Pay5}", ct.Pay5.ToString().Replace(",", "."))
                              .Replace("{Pay6}", ct.Pay6.ToString().Replace(",", "."))
                              .Replace("{Pay7}", ct.Pay7.ToString().Replace(",", "."))
                              .Replace("{Pay8}", ct.Pay8.ToString().Replace(",", "."))
                              .Replace("{Pay9}", ct.Pay9.ToString().Replace(",", "."))
                              .Replace("{Pay10}", ct.Pay10.ToString().Replace(",", "."))
                              .Replace("{Pay11}", ct.Pay11.ToString().Replace(",", "."))
                              .Replace("{Pay12}", ct.Pay12.ToString().Replace(",", "."))
                              .Replace("{Pay13}", ct.Pay13.ToString().Replace(",", "."))
                              .Replace("{Pay14}", ct.Pay14.ToString().Replace(",", "."))
                              .Replace("{Pay15}", ct.Pay15.ToString().Replace(",", "."))
                              .Replace("{Pay16}", ct.Pay16.ToString().Replace(",", "."))
                              .Replace("{Pay17}", ct.Pay17.ToString().Replace(",", "."))
                              .Replace("{Pay18}", ct.Pay18.ToString().Replace(",", "."))
                              .Replace("{Pay19}", ct.Pay19.ToString().Replace(",", "."))
                              .Replace("{Pay20}", ct.Pay20.ToString().Replace(",", "."))
                              .Replace("{EstablishmentCustomerId}", ct.EstablishmentCustomerId.ToString())
                              ;
                    //   new ClassDB(null).queryNonResonse(c);
                }
                else
                {
                    ct.CustumerId = (Guid)o[0][0];
                }
                foreach (var e in ct.ChecksTicket)
                {
                    e.CloseTicketCustumerId = ct.CustumerId;
                    string cmdInsChecksTicket =
                "INSERT INTO ChecksTicketTmp (CustumerId,BarCode,[Date],PayBankChecks,PayBankCards,PayCash,PayResto,CloseTicketTmpCustumerId,"
                 + "Pay1,Pay2,Pay3,Pay4,Pay5,Pay6,Pay7,Pay8,Pay9,Pay10,Pay11,Pay12,Pay13,Pay14,Pay15,Pay16,Pay17,Pay18,Pay19,Pay20, TotalTTC,Rendu"
                + ",DCBC,DCBC_BiloPoints,DCBC_DobavilePoints,DCBC_OtnayliPoints,DCBC_OstalosPoints,DCBC_name)"

                 + "VALUES ('{CustumerId}','{BarCode}', CAST('{Date}' AS DateTime),{PayBankChecks},{PayBankCards},{PayCash},{PayResto},'{CloseTicketTmpCustumerId}',"
                    + "{Pay1},{Pay2},{Pay3},{Pay4},{Pay5},{Pay6},{Pay7},{Pay8},{Pay9},{Pay10},{Pay11},{Pay12},{Pay13},{Pay14},{Pay15},{Pay16},{Pay17},{Pay18},{Pay19},{Pay20},{TotalTTC},{Rendu},"
                    + "'{DCBC}',{DCBC_BiloPoints},{DCBC_DobavilePoints},{DCBC_OtnayliPoints},{DCBC_OstalosPoints},'{DCBC_name}')";

                    c = cmdInsChecksTicket
                               .Replace("{CustumerId}", e.CustumerId.ToString())
                               .Replace("{BarCode}", e.BarCode)
                               .Replace("{Date}", e.Date.ToString())
                               .Replace("{PayBankChecks}", e.PayBankChecks.ToString().Replace(",", "."))
                               .Replace("{PayBankCards}", e.PayBankCards.ToString().Replace(",", "."))
                               .Replace("{PayCash}", e.PayCash.ToString().Replace(",", "."))
                               .Replace("{PayResto}", e.PayResto.ToString().Replace(",", "."))
                               .Replace("{CloseTicketTmpCustumerId}", e.CloseTicketCustumerId.ToString())
                                .Replace("{Pay1}", e.Pay1.ToString().Replace(",", "."))
                              .Replace("{Pay2}", e.Pay2.ToString().Replace(",", "."))
                              .Replace("{Pay3}", e.Pay3.ToString().Replace(",", "."))
                              .Replace("{Pay4}", e.Pay4.ToString().Replace(",", "."))
                              .Replace("{Pay5}", e.Pay5.ToString().Replace(",", "."))
                              .Replace("{Pay6}", e.Pay6.ToString().Replace(",", "."))
                              .Replace("{Pay7}", e.Pay7.ToString().Replace(",", "."))
                              .Replace("{Pay8}", e.Pay8.ToString().Replace(",", "."))
                              .Replace("{Pay9}", e.Pay9.ToString().Replace(",", "."))
                              .Replace("{Pay10}", e.Pay10.ToString().Replace(",", "."))
                              .Replace("{Pay11}", e.Pay11.ToString().Replace(",", "."))
                              .Replace("{Pay12}", e.Pay12.ToString().Replace(",", "."))
                              .Replace("{Pay13}", e.Pay13.ToString().Replace(",", "."))
                              .Replace("{Pay14}", e.Pay14.ToString().Replace(",", "."))
                              .Replace("{Pay15}", e.Pay15.ToString().Replace(",", "."))
                              .Replace("{Pay16}", e.Pay16.ToString().Replace(",", "."))
                              .Replace("{Pay17}", e.Pay17.ToString().Replace(",", "."))
                              .Replace("{Pay18}", e.Pay18.ToString().Replace(",", "."))
                              .Replace("{Pay19}", e.Pay19.ToString().Replace(",", "."))
                              .Replace("{Pay20}", e.Pay20.ToString().Replace(",", "."))
                              .Replace("{TotalTTC}", e.TotalTTC.ToString().Replace(",", "."))
                              .Replace("{Rendu}", e.Rendu.ToString().Replace(",", "."))
                              .Replace("{DCBC}", e.DCBC != null ? e.DCBC.ToString() : "")
                              .Replace("{DCBC_BiloPoints}", e.DCBC_BiloPoints.ToString())
                              .Replace("{DCBC_DobavilePoints}", e.DCBC_DobavilePoints.ToString())
                              .Replace("{DCBC_OtnayliPoints}", e.DCBC_OtnayliPoints.ToString())
                              .Replace("{DCBC_OstalosPoints}", e.DCBC_OstalosPoints.ToString())
                             .Replace("{DCBC_name}", e.DCBC_name)

                               ;
                    //      new ClassDB(null).queryNonResonse(c);

                    string cmd = "";

                    List<PayProducts> tmp = new List<PayProducts>();

                    tmp.AddRange(e.PayProducts);

                    foreach (var m in tmp)
                    {
                        var r = e.PayProducts.Where(l => l.ProductId == m.ProductId).ToList();

                        if (r.Count() > 1)
                        {
                            decimal qty = r.Sum(l => l.QTY);

                            decimal total = r.Sum(l => l.Total);

                            for (int i = 1; i < r.Count(); i++)
                            {

                                e.PayProducts.Remove(r[i]);
                            }

                            var elm = e.PayProducts.First(l => l.ProductId == m.ProductId);

                            elm.QTY = qty;

                            elm.Total = total;

                        }
                    }


                    foreach (var m in e.PayProducts)
                    {
                        try
                        {

                            Guid g = ClassProducts.listProducts.Find(l => l.CustumerId == m.ProductId).cusumerIdRealStock;


                            cmd += " " + ClassSync.ProductDB.StockReal.query_add_qty(-m.QTY, g);
                            // decimal qty = ClassSync.ProductDB.StockReal.add_qty(-m.QTY, g);

                            //  ClassProducts.updQTYProduct(qty, m.ProductId);
                        }

                        catch
                        {
                            string s = " нет записи об данном продукте в таблице СТОК, Детали: IDcustomer " + m.ProductId + " -  Название продукта " + m.Name + " - Количество " + m.QTY + " Штрих код : " + m.Barcode;
                            new ClassLog(s).ClassLogSQL(s);
                        }

                        string cmdInsPayProducts =
                    "INSERT INTO PayProductsTmp (ProductId,Name,Barcode,QTY,TVA,PriceHT,Total,ChecksTicketTmpCustumerId,IdCheckTicket,Discount,sumDiscount )"
                     + "VALUES ('{ProductId}','{Name}','{Barcode}',{QTY},{TVA},{PriceHT},{Total},'{ChecksTicketTmpCustumerId}','{IdCheckTicket}',{Discount},{sumDiscount})";

                        c = cmdInsPayProducts
                             .Replace("{ProductId}", m.ProductId.ToString())
                             .Replace("{Name}", m.Name.Replace("'", "''"))
                             .Replace("{Barcode}", m.Barcode)
                             .Replace("{QTY}", m.QTY.ToString().Replace(",", "."))
                             .Replace("{TVA}", m.TVA.ToString().Replace(",", "."))
                             .Replace("{PriceHT}", m.PriceHT.ToString().Replace(",", "."))
                             .Replace("{Total}", m.Total.ToString().Replace(",", "."))
                             .Replace("{ChecksTicketTmpCustumerId}", m.ChecksTicketCustumerId.ToString())
                             .Replace("{IdCheckTicket}", m.IdCheckTicket.ToString())
                             .Replace("{Discount}", m.Discount.ToString())
                             .Replace("{sumDiscount}", m.sumDiscount.ToString())
                             ;
                        //     new ClassDB(null).queryNonResonse(c);

                        StockLogsDB.StockLogs stock = new StockLogsDB.StockLogs(true, m.Name, m.Barcode, m.QTY, ClassGlobalVar.user, ClassGlobalVar.nameTicket + " " + ClassGlobalVar.Name + " " + ClassGlobalVar.Establishment);

                        //   new StockLogsDB().ins(stock);

                        cmd += " " + StockLogsDB.ins_(stock);
                    }

                    int iss = ClassSync.ProductDB.StockReal.response(cmd);
                }

            }


            private void ins(CloseTicket ct)
            {
                foreach (var e in ct.ChecksTicket)
                {
                    e.CloseTicketCustumerId = ct.CustumerId;
                    string cmd = "";

                    List<PayProducts> tmp = new List<PayProducts>();

                    tmp.AddRange(e.PayProducts);

                    foreach (var m in tmp)
                    {
                        var r = e.PayProducts.Where(l => l.ProductId == m.ProductId).ToList();

                        if (r.Count() > 1)
                        {
                            decimal qty = r.Sum(l => l.QTY);

                            decimal total = r.Sum(l => l.Total);

                            for (int i = 1; i < r.Count(); i++)
                            {

                                e.PayProducts.Remove(r[i]);
                            }

                            var elm = e.PayProducts.First(l => l.ProductId == m.ProductId);

                            elm.QTY = qty;

                            elm.Total = total;

                        }
                    }



                    foreach (var m in e.PayProducts)
                    {
                        try
                        {

                            Guid g = ClassProducts.listProducts.Find(l => l.CustumerId == m.ProductId).cusumerIdRealStock;


                            cmd += " " + ClassSync.ProductDB.StockReal.query_add_qty(-m.QTY, g);

                            //      if (ClassProMode.devis)
                            {
                                //        cmd += " " + ClassSync.ProductDB.StockReal.query_add_qty_from_est(m.QTY, ClassGlobalVar.IdEstablishment_GROS, m.ProductId);
                            }
                        }

                        catch
                        {
                            string s = " нет записи об данном продукте в таблице СТОК, Детали: IDcustomer " + m.ProductId + " -  Название продукта " + m.Name + " - Количество " + m.QTY + " Штрих код : " + m.Barcode;
                            new ClassLog(s).ClassLogSQL(s);
                        }



                        //  StockLogsDB.StockLogs stock = new StockLogsDB.StockLogs(true, m.Name, m.Barcode, m.QTY, ClassGlobalVar.user, ClassGlobalVar.nameTicket + " " + ClassGlobalVar.Name );



                        //   cmd += " " + StockLogsDB.ins_(stock);
                    }

                    int iss = ClassSync.ProductDB.StockReal.response(cmd);
                }
            }


            public static ChecksTicket findCheck(string codebar)
            {
                ChecksTicket t = new ChecksTicket();
                List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM ChecksTicketTmp WHERE BarCode='" + codebar + "'");
                if (l.Count > 0)
                {


                    t.CustumerId = (Guid)l[0][0];
                    t.BarCode = (string)l[0][1];
                    t.Date = (DateTime)l[0][2];
                    if (l[0][3].ToString() != "")
                        t.PayBankChecks = (decimal)l[0][3];
                    if (l[0][4].ToString() != "")
                        t.PayBankCards = (decimal)l[0][4];
                    if (l[0][5].ToString() != "")
                        t.PayCash = (decimal)l[0][5];
                    if (l[0][6].ToString() != "")
                        t.PayResto = (decimal)l[0][6];
                    if (l[0][7].ToString() != "")
                        t.Pay1 = (decimal)l[0][7];
                    if (l[0][8].ToString() != "")
                        t.Pay2 = (decimal)l[0][8];
                    if (l[0][9].ToString() != "")
                        t.Pay3 = (decimal)l[0][9];
                    if (l[0][10].ToString() != "")
                        t.Pay4 = (decimal)l[0][10];
                    if (l[0][11].ToString() != "")
                        t.Pay5 = (decimal)l[0][11];
                    if (l[0][12].ToString() != "")
                        t.Pay6 = (decimal)l[0][12];
                    if (l[0][13].ToString() != "")
                        t.Pay7 = (decimal)l[0][13];
                    if (l[0][14].ToString() != "")
                        t.Pay8 = (decimal)l[0][14];
                    if (l[0][15].ToString() != "")
                        t.Pay9 = (decimal)l[0][15];
                    if (l[0][16].ToString() != "")
                        t.Pay10 = (decimal)l[0][16];
                    if (l[0][17].ToString() != "")
                        t.Pay11 = (decimal)l[0][17];
                    if (l[0][18].ToString() != "")
                        t.Pay12 = (decimal)l[0][18];
                    if (l[0][19].ToString() != "")
                        t.Pay13 = (decimal)l[0][19];
                    if (l[0][20].ToString() != "")
                        t.Pay14 = (decimal)l[0][20];
                    if (l[0][21].ToString() != "")
                        t.Pay15 = (decimal)l[0][21];
                    if (l[0][22].ToString() != "")
                        t.Pay16 = (decimal)l[0][22];
                    if (l[0][23].ToString() != "")
                        t.Pay17 = (decimal)l[0][23];
                    if (l[0][24].ToString() != "")
                        t.Pay18 = (decimal)l[0][24];
                    if (l[0][25].ToString() != "")
                        t.Pay19 = (decimal)l[0][25];
                    if (l[0][26].ToString() != "")
                        t.Pay20 = (decimal)l[0][26];
                    List<object[]> o = new ClassDB(null).queryResonse("SELECT * FROM PayProductsTmp WHERE ChecksTicketTmpCustumerId='" + t.CustumerId + "'");
                    foreach (var e in o)
                    {
                        PayProducts p = new PayProducts();
                        p.IdCheckTicket = (Guid)e[0];
                        p.ProductId = (Guid)e[1];
                        p.Name = (string)e[2];
                        p.Barcode = (string)e[3];
                        p.QTY = (decimal)e[4];
                        p.TVA = ClassTVA.listTVA.Find(la => la.val == decimal.Parse(e[5].ToString())).id;
                        p.PriceHT = (decimal)e[6];
                        p.Total = (decimal)e[7];
                        p.ChecksTicketCustumerId = (Guid)e[8];
                        t.PayProducts.Add(p);
                    }
                }
                else t = null;
                return t;
            }
            private void setPayType(ChecksTicket t, XElement x)
            {


                foreach (var type in Class.ClassSync.TypesPayDB.t)
                {


                    switch (type.NameCourt.TrimEnd())
                    {
                        case "BankChecks": t.PayBankChecks = decimal.Parse(x.Attribute("BankChecks").Value.Replace(".", ",")); break;
                        case "BankCards": t.PayBankCards = decimal.Parse(x.Attribute("BankCards").Value.Replace(".", ",")); break;
                        case "Cash": t.PayCash = decimal.Parse(x.Attribute("Cash").Value.Replace(".", ",")); break;
                        case "Resto": t.PayResto = decimal.Parse(x.Attribute("Resto").Value.Replace(".", ",")); break;
                        case "1": t.Pay1 = decimal.Parse(x.Attribute("1").Value.Replace(".", ",")); break;
                        case "2": t.Pay2 = decimal.Parse(x.Attribute("2").Value.Replace(".", ",")); break;
                        case "3": t.Pay3 = decimal.Parse(x.Attribute("3").Value.Replace(".", ",")); break;
                        case "4": t.Pay4 = decimal.Parse(x.Attribute("4").Value.Replace(".", ",")); break;
                        case "5": t.Pay5 = decimal.Parse(x.Attribute("5").Value.Replace(".", ",")); break;
                        case "6": t.Pay6 = decimal.Parse(x.Attribute("6").Value.Replace(".", ",")); break;
                        case "7": t.Pay7 = decimal.Parse(x.Attribute("7").Value.Replace(".", ",")); break;
                        case "8": t.Pay8 = decimal.Parse(x.Attribute("8").Value.Replace(".", ",")); break;
                        case "9": t.Pay9 = decimal.Parse(x.Attribute("9").Value.Replace(".", ",")); break;
                        case "10": t.Pay10 = decimal.Parse(x.Attribute("10").Value.Replace(".", ",")); break;
                        case "11": t.Pay11 = decimal.Parse(x.Attribute("11").Value.Replace(".", ",")); break;
                        case "12": t.Pay12 = decimal.Parse(x.Attribute("12").Value.Replace(".", ",")); break;
                        case "13": t.Pay13 = decimal.Parse(x.Attribute("13").Value.Replace(".", ",")); break;
                        case "14": t.Pay14 = decimal.Parse(x.Attribute("14").Value.Replace(".", ",")); break;
                        case "15": t.Pay15 = decimal.Parse(x.Attribute("15").Value.Replace(".", ",")); break;
                        case "16": t.Pay16 = decimal.Parse(x.Attribute("16").Value.Replace(".", ",")); break;
                        case "17": t.Pay17 = decimal.Parse(x.Attribute("17").Value.Replace(".", ",")); break;
                        case "18": t.Pay18 = decimal.Parse(x.Attribute("18").Value.Replace(".", ",")); break;
                        case "19": t.Pay19 = decimal.Parse(x.Attribute("19").Value.Replace(".", ",")); break;
                        default: break;

                    }
                }

            }
            private static void setPayType_s(ChecksTicket t, XElement x)
            {


                foreach (var type in Class.ClassSync.TypesPayDB.t)
                {


                    switch (type.NameCourt.TrimEnd())
                    {
                        case "BankChecks": t.PayBankChecks = decimal.Parse(x.Attribute("BankChecks").Value.Replace(".", ",")); break;
                        case "BankCards": t.PayBankCards = decimal.Parse(x.Attribute("BankCards").Value.Replace(".", ",")); break;
                        case "Cash": t.PayCash = decimal.Parse(x.Attribute("Cash").Value.Replace(".", ",")); break;
                        case "Resto": t.PayResto = decimal.Parse(x.Attribute("Resto").Value.Replace(".", ",")); break;
                        case "1": t.Pay1 = decimal.Parse(x.Attribute("1").Value.Replace(".", ",")); break;
                        case "2": t.Pay2 = decimal.Parse(x.Attribute("2").Value.Replace(".", ",")); break;
                        case "3": t.Pay3 = decimal.Parse(x.Attribute("3").Value.Replace(".", ",")); break;
                        case "4": t.Pay4 = decimal.Parse(x.Attribute("4").Value.Replace(".", ",")); break;
                        case "5": t.Pay5 = decimal.Parse(x.Attribute("5").Value.Replace(".", ",")); break;
                        case "6": t.Pay6 = decimal.Parse(x.Attribute("6").Value.Replace(".", ",")); break;
                        case "7": t.Pay7 = decimal.Parse(x.Attribute("7").Value.Replace(".", ",")); break;
                        case "8": t.Pay8 = decimal.Parse(x.Attribute("8").Value.Replace(".", ",")); break;
                        case "9": t.Pay9 = decimal.Parse(x.Attribute("9").Value.Replace(".", ",")); break;
                        case "10": t.Pay10 = decimal.Parse(x.Attribute("10").Value.Replace(".", ",")); break;
                        case "11": t.Pay11 = decimal.Parse(x.Attribute("11").Value.Replace(".", ",")); break;
                        case "12": t.Pay12 = decimal.Parse(x.Attribute("12").Value.Replace(".", ",")); break;
                        case "13": t.Pay13 = decimal.Parse(x.Attribute("13").Value.Replace(".", ",")); break;
                        case "14": t.Pay14 = decimal.Parse(x.Attribute("14").Value.Replace(".", ",")); break;
                        case "15": t.Pay15 = decimal.Parse(x.Attribute("15").Value.Replace(".", ",")); break;
                        case "16": t.Pay16 = decimal.Parse(x.Attribute("16").Value.Replace(".", ",")); break;
                        case "17": t.Pay17 = decimal.Parse(x.Attribute("17").Value.Replace(".", ",")); break;
                        case "18": t.Pay18 = decimal.Parse(x.Attribute("18").Value.Replace(".", ",")); break;
                        case "19": t.Pay19 = decimal.Parse(x.Attribute("19").Value.Replace(".", ",")); break;
                        default: break;

                    }
                }

            }
            public ClassCloseTicketTmp(XDocument x)
            {

                CloseTicket closeTicket = new CloseTicket();

                closeTicket.DateOpen = DateTime.Parse(x.Element("checks").Attribute("openDate").Value);
                closeTicket.DateClose = DateTime.Now;
                closeTicket.NameTicket = x.Element("checks").Attribute("ticket").Value;
                closeTicket.CustumerId = Guid.Parse(x.Element("checks").Attribute("idTicketWindow").Value);   //Guid.NewGuid();


                closeTicket.EstablishmentCustomerId = ClassGlobalVar.IdEstablishment;
                XElement e = x.Element("checks").Elements("check").Last();
                ChecksTicket t = new ChecksTicket();
                t.BarCode = e.Attribute("barcodeCheck").Value;
                t.Date = DateTime.Parse(e.Attribute("date").Value);
                setPayType(t, e);
                t.TotalTTC = decimal.Parse(e.Attribute("sum").Value.Replace('.', ','));
                t.Rendu = decimal.Parse(e.Attribute("Rendu").Value.Replace('.', ','));
                t.CloseTicketCustumerId = closeTicket.CustumerId;
                t.CustumerId = Guid.NewGuid();

                if (e.Attribute("DCBC") != null)
                {
                    t.DCBC = e.Attribute("DCBC").Value;
                    t.DCBC_BiloPoints = int.Parse(e.Attribute("DCBC_BiloPoints").Value);
                    t.DCBC_DobavilePoints = int.Parse(e.Attribute("DCBC_DobavilePoints").Value);
                    t.DCBC_OtnayliPoints = int.Parse(e.Attribute("DCBC_OtnayliPoints").Value);
                    t.DCBC_OstalosPoints = int.Parse(e.Attribute("DCBC_OstalosPoints").Value);
                    t.DCBC_name = e.Attribute("DCBC_name").Value;
                }
                IEnumerable<XElement> pr = e.Elements("product");

                foreach (XElement xe in pr)
                {
                    PayProducts p = new PayProducts();
                    p.ProductId = Guid.Parse(xe.Element("CustumerId").Value);
                    p.Name = xe.Element("Name").Value;
                    p.Barcode = xe.Element("CodeBare").Value;
                    p.PriceHT = decimal.Parse(xe.Element("price").Value.Replace('.', ','));
                    p.QTY = decimal.Parse(xe.Element("qty").Value.Replace('.', ','));
                    p.TVA = ClassTVA.getTVA(int.Parse(xe.Element("tva").Value));
                    p.Total = decimal.Parse(xe.Element("total").Value.Replace('.', ','));
                    p.ChecksTicketCloseTicketCustumerId = t.CloseTicketCustumerId;
                    p.ChecksTicketCustumerId = t.CustumerId;
                    p.IdCheckTicket = Guid.NewGuid();
                    t.PayProducts.Add(p);
                }
                closeTicket.ChecksTicket.Add(t);


                ins(closeTicket);

            }

            public partial class PayProducts
            {
                public static List<PayProducts> sel(Guid checkCustomerId)
                {
                    List<PayProducts> R = new List<PayProducts>();

                    string cmd = "SELECT * FROM PayProductsTmp WHERE ChecksTicketTmpCustumerId ='" + checkCustomerId + "'";

                    foreach (var e in new ClassDB(null).queryResonse(cmd))
                    {
                        PayProducts p = new PayProducts();

                        p.IdCheckTicket = (Guid)e[0];
                        p.ProductId = (Guid)e[1];
                        p.Name = (string)e[2];
                        p.Barcode = (string)e[3];
                        p.QTY = (decimal)e[4];
                        p.TVA = (decimal)e[5];
                        p.PriceHT = (decimal)e[6];
                        p.Total = (decimal)e[7];
                        p.ChecksTicketCustumerId = (Guid)e[8];
                        R.Add(p);
                    }

                    return R;
                }
                public System.Guid IdCheckTicket { get; set; }
                public System.Guid ProductId { get; set; }
                public string Name { get; set; }
                public string Barcode { get; set; }
                public decimal QTY { get; set; }
                public decimal TVA { get; set; }
                public decimal PriceHT { get; set; }
                public decimal Total { get; set; }
                public System.Guid ChecksTicketCustumerId { get; set; }
                public System.Guid ChecksTicketCloseTicketCustumerId { get; set; }
                public decimal Discount { get; set; }
                public decimal sumDiscount { get; set; }

            }
            public partial class ChecksTicket
            {
                public ChecksTicket()
                {
                    this.PayProducts = new HashSet<PayProducts>();
                }
                public static List<ChecksTicket> sel(Guid customerId)
                {
                    List<ChecksTicket> R = new List<ChecksTicket>();

                    string cmd = "SELECT * FROM ChecksTicketTmp WHERE  CloseTicketTmpCustumerId ='" + customerId + "'  ORDER BY Date DESC";

                    List<object[]> L = new ClassDB(null).queryResonse(cmd);


                    foreach (var e in L)
                    {
                        ChecksTicket ct = new ChecksTicket();
                        ct.CustumerId = (Guid)e[0];
                        ct.BarCode = (string)e[1];
                        ct.Date = (DateTime)e[2];
                        ct.PayBankChecks = ClassETC_fun.testFromNuul(e[3]);
                        ct.PayBankCards = ClassETC_fun.testFromNuul(e[4]);
                        ct.PayCash = ClassETC_fun.testFromNuul(e[5]);
                        ct.PayResto = ClassETC_fun.testFromNuul(e[6]);
                        ct.Pay1 = ClassETC_fun.testFromNuul(e[7]);
                        ct.Pay2 = ClassETC_fun.testFromNuul(e[8]);
                        ct.Pay3 = ClassETC_fun.testFromNuul(e[9]);
                        ct.Pay4 = ClassETC_fun.testFromNuul(e[10]);
                        ct.Pay5 = ClassETC_fun.testFromNuul(e[11]);
                        ct.Pay6 = ClassETC_fun.testFromNuul(e[12]);
                        ct.Pay7 = ClassETC_fun.testFromNuul(e[13]);
                        ct.Pay8 = ClassETC_fun.testFromNuul(e[14]);
                        ct.Pay9 = ClassETC_fun.testFromNuul(e[15]);
                        ct.Pay10 = ClassETC_fun.testFromNuul(e[16]);
                        ct.Pay11 = ClassETC_fun.testFromNuul(e[17]);
                        ct.Pay12 = ClassETC_fun.testFromNuul(e[18]);
                        ct.Pay13 = ClassETC_fun.testFromNuul(e[19]);
                        ct.Pay14 = ClassETC_fun.testFromNuul(e[20]);
                        ct.Pay15 = ClassETC_fun.testFromNuul(e[21]);
                        ct.Pay16 = ClassETC_fun.testFromNuul(e[22]);
                        ct.Pay17 = ClassETC_fun.testFromNuul(e[23]);
                        ct.Pay18 = ClassETC_fun.testFromNuul(e[24]);
                        ct.Pay19 = ClassETC_fun.testFromNuul(e[25]);
                        ct.Pay20 = ClassETC_fun.testFromNuul(e[26]);
                        ct.TotalTTC = ClassETC_fun.testFromNuul(e[27]);
                        ct.Rendu = ClassETC_fun.testFromNuul(e[28]);
                        ct.CloseTicketCustumerId = (Guid)e[29];
                        ct.DCBC = (string)e[30];
                        ct.DCBC_BiloPoints = (int)e[31];
                        ct.DCBC_DobavilePoints = (int)e[32];
                        ct.DCBC_OtnayliPoints = (int)e[33];
                        ct.DCBC_OstalosPoints = (int)e[34];
                        ct.DCBC_name = (string)e[35];

                        R.Add(ct);
                    }


                    return R;
                }

                public static void del(string bc)
                {
                    string cmd = "DELETE FROM ChecksTicketTmp WHERE BarCode = '{BarCode}'";

                    new ClassDB(null).queryNonResonse(cmd.Replace("{BarCode}", bc));
                }
                public System.Guid CustumerId { get; set; }
                public string BarCode { get; set; }
                public System.DateTime Date { get; set; }
                public decimal PayBankChecks { get; set; }
                public decimal PayBankCards { get; set; }
                public decimal PayCash { get; set; }
                public decimal PayResto { get; set; }
                public decimal Pay1 { get; set; }
                public decimal Pay2 { get; set; }
                public decimal Pay3 { get; set; }
                public decimal Pay4 { get; set; }
                public decimal Pay5 { get; set; }
                public decimal Pay6 { get; set; }
                public decimal Pay7 { get; set; }
                public decimal Pay8 { get; set; }
                public decimal Pay9 { get; set; }
                public decimal Pay10 { get; set; }
                public decimal Pay11 { get; set; }
                public decimal Pay12 { get; set; }
                public decimal Pay13 { get; set; }
                public decimal Pay14 { get; set; }
                public decimal Pay15 { get; set; }
                public decimal Pay16 { get; set; }
                public decimal Pay17 { get; set; }
                public decimal Pay18 { get; set; }
                public decimal Pay19 { get; set; }
                public decimal Pay20 { get; set; }
                public System.Guid CloseTicketCustumerId { get; set; }
                public decimal TotalTTC { get; set; }
                public decimal Rendu { get; set; }
                public virtual ICollection<PayProducts> PayProducts { get; set; }
                public virtual CloseTicket CloseTicket { get; set; }
                public string DCBC { get; set; }
                public int DCBC_BiloPoints { get; set; }
                public int DCBC_DobavilePoints { get; set; }
                public int DCBC_OtnayliPoints { get; set; }
                public int DCBC_OstalosPoints { get; set; }
                public string DCBC_name { get; set; }

            }
            public partial class CloseTicket
            {
                public CloseTicket()
                {
                    this.ChecksTicket = new HashSet<ChecksTicket>();
                }
                public static List<CloseTicket> sel()
                {
                    string cmd = "SELECT * FROM CloseTicketTmp ORDER BY DateOpen DESC";

                    List<object[]> l = new ClassDB(null).queryResonse(cmd);

                    List<CloseTicket> LCTT = new List<CloseTicket>();

                    foreach (var e in l)
                    {
                        CloseTicket ct = new CloseTicket();
                        ct.CustumerId = (Guid)e[0];
                        ct.NameTicket = (string)e[1];
                        ct.DateClose = (DateTime)e[2];
                        ct.DateOpen = (DateTime)e[3];
                        ct.PayBankChecks = ClassETC_fun.testFromNuul(e[4]);
                        ct.PayBankCards = ClassETC_fun.testFromNuul(e[5]);
                        ct.PayCash = ClassETC_fun.testFromNuul(e[6]);
                        ct.PayResto = ClassETC_fun.testFromNuul(e[7]);
                        ct.Pay1 = ClassETC_fun.testFromNuul(e[8]);
                        ct.Pay2 = ClassETC_fun.testFromNuul(e[9]);
                        ct.Pay3 = ClassETC_fun.testFromNuul(e[10]);
                        ct.Pay4 = ClassETC_fun.testFromNuul(e[11]);
                        ct.Pay5 = ClassETC_fun.testFromNuul(e[12]);
                        ct.Pay6 = ClassETC_fun.testFromNuul(e[13]);
                        ct.Pay7 = ClassETC_fun.testFromNuul(e[14]);
                        ct.Pay8 = ClassETC_fun.testFromNuul(e[15]);
                        ct.Pay9 = ClassETC_fun.testFromNuul(e[16]);
                        ct.Pay10 = ClassETC_fun.testFromNuul(e[17]);
                        ct.Pay11 = ClassETC_fun.testFromNuul(e[18]);
                        ct.Pay12 = ClassETC_fun.testFromNuul(e[19]);
                        ct.Pay13 = ClassETC_fun.testFromNuul(e[20]);
                        ct.Pay14 = ClassETC_fun.testFromNuul(e[21]);
                        ct.Pay15 = ClassETC_fun.testFromNuul(e[22]);
                        ct.Pay16 = ClassETC_fun.testFromNuul(e[23]);
                        ct.Pay17 = ClassETC_fun.testFromNuul(e[24]);
                        ct.Pay18 = ClassETC_fun.testFromNuul(e[25]);
                        ct.Pay19 = ClassETC_fun.testFromNuul(e[26]);
                        ct.Pay20 = ClassETC_fun.testFromNuul(e[27]);
                        ct.EstablishmentCustomerId = (e[28].ToString() == "" ? Guid.Empty : (Guid)e[28]);
                        LCTT.Add(ct);
                    }

                    return LCTT;
                }
                public System.Guid CustumerId { get; set; }
                public string NameTicket { get; set; }
                public System.DateTime DateOpen { get; set; }
                public System.DateTime DateClose { get; set; }
                public decimal PayBankChecks { get; set; }
                public decimal PayBankCards { get; set; }
                public decimal PayCash { get; set; }
                public decimal PayResto { get; set; }
                public decimal Pay1 { get; set; }
                public decimal Pay2 { get; set; }
                public decimal Pay3 { get; set; }
                public decimal Pay4 { get; set; }
                public decimal Pay5 { get; set; }
                public decimal Pay6 { get; set; }
                public decimal Pay7 { get; set; }
                public decimal Pay8 { get; set; }
                public decimal Pay9 { get; set; }
                public decimal Pay10 { get; set; }
                public decimal Pay11 { get; set; }
                public decimal Pay12 { get; set; }
                public decimal Pay13 { get; set; }
                public decimal Pay14 { get; set; }
                public decimal Pay15 { get; set; }
                public decimal Pay16 { get; set; }
                public decimal Pay17 { get; set; }
                public decimal Pay18 { get; set; }
                public decimal Pay19 { get; set; }
                public decimal Pay20 { get; set; }
                public Guid EstablishmentCustomerId { get; set; }
                public ICollection<ChecksTicket> ChecksTicket { get; set; }
            }
        }
        public class StockLogsDB
        {
            public Guid ins(StockLogs o)
            {

                string cmdInsProduct = "INSERT INTO StockLogs (CustomerId,[DateTime],TypeOperation,Name,Barcode,QTY,[User],Details)"
                + "VALUES ('{CustumerId}','{DateTime}','{TypeOperation}','{Name}','{Barcode}',{QTY},'{User}','{Details}')";

                Guid r = Guid.NewGuid();

                string c = cmdInsProduct
                           .Replace("{CustumerId}", o.CustomerId.ToString())
                           .Replace("{DateTime}", o.DateTime.ToString())
                           .Replace("{TypeOperation}", o.TypeOperation.ToString())
                           .Replace("{Name}", o.Name.Replace("'", "''"))
                           .Replace("{Barcode}", o.Barcode)
                           .Replace("{QTY}", o.QTY.ToString().Replace(",", "."))
                           .Replace("{User}", o.User.Replace("'", "''"))
                           .Replace("{Details}", o.Details.Replace("'", "''"));

                new ClassDB(null).queryNonResonse(c);


                return r;
            }
            public static string ins_(StockLogs o)
            {

                string cmdInsProduct = "INSERT INTO StockLogs (CustomerId,[DateTime],TypeOperation,Name,Barcode,QTY,[User],Details)"
                + "VALUES ('{CustumerId}','{DateTime}','{TypeOperation}','{Name}','{Barcode}',{QTY},'{User}','{Details}')";

                Guid r = Guid.NewGuid();

                string c = cmdInsProduct
                           .Replace("{CustumerId}", o.CustomerId.ToString())
                           .Replace("{DateTime}", o.DateTime.ToString())
                           .Replace("{TypeOperation}", o.TypeOperation.ToString())
                           .Replace("{Name}", o.Name.Replace("'", "''"))
                           .Replace("{Barcode}", o.Barcode)
                           .Replace("{QTY}", o.QTY.ToString().Replace(",", "."))
                           .Replace("{User}", o.User.Replace("'", "''"))
                           .Replace("{Details}", o.Details.Replace("'", "''"));

                // new ClassDB(null).queryNonResonse(c);


                return c;
            }
            public class StockLogs
            {
                public System.Guid CustomerId { get; set; }
                public System.DateTime DateTime { get; set; }
                public bool TypeOperation { get; set; }
                public string Name { get; set; }
                public string Barcode { get; set; }
                public decimal QTY { get; set; }
                public string User { get; set; }
                public string Details { get; set; }
                public StockLogs(bool TypeOperation, string Name, string Barcode, decimal QTY, string User, string Details)
                {
                    this.CustomerId = Guid.NewGuid();
                    this.DateTime = DateTime.Now;
                    this.TypeOperation = TypeOperation;
                    this.Name = Name;
                    this.Barcode = Barcode;
                    this.QTY = QTY;
                    this.User = User;
                    this.Details = Details;
                }
            }
        }
        public class TypesPayDB
        {

            public static List<TypesPayDB> t = new List<TypesPayDB>();
            static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\TypePay.xml";
            public TypesPayDB(int Id, bool? Etat, string Name, string NameCourt, int? CodeCompta, bool? Rendu_Avoir, bool? Tiroir, bool? CurMod)
            {
                this.Id = Id;
                this.Etat = Etat;
                this.Name = Name;
                this.NameCourt = NameCourt;
                this.CodeCompta = CodeCompta;
                this.Rendu_Avoir = Rendu_Avoir;
                this.Tiroir = Tiroir;
                this.CurMod = CurMod;
            }
            public TypesPayDB(TypesPayDB type)
            {
                this.Id = type.Id;
                this.Etat = type.Etat;
                this.Name = type.Name;
                this.NameCourt = type.NameCourt;
                this.CodeCompta = type.CodeCompta;
                this.Rendu_Avoir = type.Rendu_Avoir;
                this.Tiroir = type.Tiroir;
                this.CurMod = CurMod;
            }
            public TypesPayDB()
            { }
            public int Id { get; set; }
            public Nullable<bool> Etat { get; set; }
            public string Name { get; set; }
            public string NameCourt { get; set; }
            public Nullable<int> CodeCompta { get; set; }
            public Nullable<bool> Rendu_Avoir { get; set; }
            public Nullable<bool> Tiroir { get; set; }
            public Nullable<bool> CurMod { get; set; }
            public static List<TypesPayDB> sel()
            {
                List<object[]> obj = new ClassDB(null).queryResonse("SELECT * FROM TypesPay");

                List<TypesPayDB> list = new List<TypesPayDB>();

                foreach (var o in obj)
                {
                    list.Add(new TypesPayDB((int)o[0], (bool)o[1],
                                             (string)o[2], ((string)o[3]).TrimEnd(), (int)o[4], (bool)o[5], (bool)o[6], (bool)o[7]));
                }

                return list;
            }
            public void ins(TypesPayDB rec)
            {

            }
            public void rem(TypesPayDB rec)
            {

            }
            public void mod(TypesPayDB rec)
            {

            }
            public static void TypePayLoadFromXML()
            {
                XDocument x = XDocument.Load(path);

                IEnumerable<XElement> e = x.Element("typesPay").Elements("rec");

                foreach (XElement el in e)
                {
                    TypesPayDB g = new TypesPayDB();
                    g.Id = int.Parse(el.Element("id").Value);
                    g.Name = el.Element("Name").Value;
                    g.Etat = bool.Parse(el.Element("Etat").Value);
                    g.CodeCompta = int.Parse(el.Element("CodeCompta").Value);
                    g.NameCourt = (el.Element("NameCourt").Value);
                    g.Rendu_Avoir = bool.Parse((el.Element("Rendu_Avoir").Value));
                    g.Tiroir = bool.Parse((el.Element("Tiroir").Value));
                    g.CurMod = bool.Parse((el.Element("CurMod").Value));
                    t.Add(g);
                }
            }
            public static void saveXML(List<TypesPayDB> TypesPays)
            {
                XDocument x;
                if (System.IO.File.Exists(path))
                {
                    x = XDocument.Load(path);
                    x.Element("typesPay").RemoveAll();
                }
                else
                {
                    x = new XDocument();
                    x.Add(new XElement("typesPay"));
                }

                foreach (TypesPayDB p in TypesPays)
                {

                    x.Element("typesPay").Add(
                        new XElement("rec",
                            new XElement("id", p.Id),
                            new XElement("Name", p.Name),
                            new XElement("Etat", p.Etat),
                            new XElement("CodeCompta", p.CodeCompta),
                            new XElement("NameCourt", p.NameCourt),
                            new XElement("Rendu_Avoir", p.Rendu_Avoir),
                            new XElement("Tiroir", p.Tiroir),
                            new XElement("CurMod", p.CurMod)
                            )
                        );
                }
                x.Save(path);

            }
            public static void sync()
            {
                if (ClassSync.connect)
                {
                    t = sel();

                    saveXML(t);
                }
                else TypePayLoadFromXML();
            }
        }
        public class Currency
        {
            public Currency()
            {

            }
            public Currency(System.Guid CustomerId, decimal Currency_money, string Desc, int TypesPayId)
            {

                this.CustomerId = CustomerId;
                this.Currency_money = Currency_money;
                this.Desc = Desc;
                this.TypesPayId = TypesPayId;
                this.TypesPay = FindTypesPayDB(TypesPayId);
            }
            public System.Guid CustomerId { get; set; }
            public decimal Currency_money { get; set; }
            public string Desc { get; set; }
            public int TypesPayId { get; set; }
            public virtual TypesPayDB TypesPay { get; set; }
            private static TypesPayDB FindTypesPayDB(int customerId)
            {
                return ClassSync.TypesPayDB.t.Find(l => l.Id == customerId);
            }
            public static List<Currency> sel()
            {
                List<object[]> obj = new ClassDB(null).queryResonse("SELECT * FROM Currency");

                List<Currency> list = new List<Currency>();

                foreach (var o in obj)
                {
                    list.Add(new Currency((Guid)o[0], (decimal)o[1],
                                             (string)o[2], (int)o[3]));
                }

                return list;
            }

            static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Currency.xml";
            public static List<Currency> List_Currency { get; set; }
            public static void TypePayLoadFromXML()
            {
                XDocument x = XDocument.Load(path);

                IEnumerable<XElement> e = x.Element("Currency").Elements("rec");

                List_Currency = new List<Currency>();

                foreach (XElement el in e)
                {
                    Currency g = new Currency();
                    g.CustomerId = Guid.Parse(el.Element("CustomerId").Value);
                    g.Currency_money = decimal.Parse(el.Element("Currency_money").Value.Replace(".", ","));
                    g.Desc = (el.Element("Desc").Value);
                    g.TypesPayId = int.Parse(el.Element("TypesPayId").Value);
                    g.TypesPay = FindTypesPayDB(g.TypesPayId);
                    List_Currency.Add(g);
                }
            }
            public static void saveXML(List<Currency> Currency_)
            {
                XDocument x;
                if (System.IO.File.Exists(path))

                    x = XDocument.Load(path);
                else
                {
                    x = new XDocument();
                    x.Add(new XElement("Currency"));
                }
                x.Element("Currency").Elements("rec").Remove();
                foreach (Currency p in Currency_)
                {

                    x.Element("Currency").Add(
                        new XElement("rec",
                            new XElement("CustomerId", p.CustomerId),
                            new XElement("Currency_money", p.Currency_money),
                            new XElement("Desc", p.Desc),
                            new XElement("TypesPayId", p.TypesPayId)
                            )
                        );
                }
                x.Save(path);

            }
            public static void sync()
            {
                if (ClassSync.connect)
                {
                    List_Currency = sel();

                    saveXML(List_Currency);
                }
                else
                {
                    TypePayLoadFromXML();
                }
            }
        }
        public class OpenTicketWindow
        {
            public System.Guid custumerId { get; set; }
            public System.Guid idTicketWindow { get; set; }
            public string nameTicket { get; set; }
            public string user { get; set; }
            public int numberTicket { get; set; }
            public bool isOpen { get; set; }
            public System.DateTime dateOpen { get; set; }
            public Nullable<System.Guid> IdTicketWindowG { get; set; }
            public Guid Establishment_CustomerId { get; set; }

            public static List<OpenTicketWindow> table = new List<OpenTicketWindow>();
            public static string mess { get; set; }
            public static bool open { get; set; }
            public static void crt()
            {
                if (!table.Exists(l => l.custumerId == ClassGlobalVar.CustumerId))
                {
                    OpenTicketWindow otw = new OpenTicketWindow();

                    otw.custumerId = ClassGlobalVar.CustumerId;

                    otw.idTicketWindow = Guid.Empty;

                    otw.nameTicket = ClassGlobalVar.nameTicket;
                    otw.user = ClassGlobalVar.user;
                    otw.numberTicket = ClassGlobalVar.numberTicket;
                    otw.isOpen = false;
                    otw.dateOpen = (DateTime.Now);

                    otw.IdTicketWindowG = Guid.Empty;
                    otw.Establishment_CustomerId = ClassGlobalVar.IdEstablishment;
                    table.Add(otw);
                    ins(otw);
                }
            }
            public static void set()
            {
                List<object[]> t = new ClassDB(null).queryResonse("SELECT * FROM OpenTicketWindow");
                table.Clear();

                foreach (object[] el in t)
                {
                    OpenTicketWindow otw = new OpenTicketWindow();
                    otw.custumerId = (Guid)el[0];
                    otw.idTicketWindow = (Guid)el[1];

                    otw.nameTicket = el[2].ToString();
                    otw.user = el[3].ToString();
                    otw.numberTicket = (int)el[4];
                    otw.isOpen = (bool)el[5];
                    otw.dateOpen = (DateTime)el[6];

                    otw.IdTicketWindowG = Guid.Parse((el[7].ToString() == "") ? Guid.Empty.ToString() : el[7].ToString());
                    otw.Establishment_CustomerId = Guid.Parse((el[8].ToString() == "") ? Guid.Empty.ToString() : el[8].ToString());
                    table.Add(otw);
                }
                open = false;
                crt();

                if (table.Count > 0)
                {
                    open = table.First(l => l.custumerId == ClassGlobalVar.CustumerId).isOpen;

                    if (open)
                    {
                        ClassGlobalVar.TicketWindow = table.First(l => l.custumerId == ClassGlobalVar.CustumerId).idTicketWindow;
                        ClassGlobalVar.TicketWindowG = table.First(l => l.custumerId == ClassGlobalVar.CustumerId).IdTicketWindowG ?? Guid.Empty;
                        ClassGlobalVar.open = table.First(l => l.custumerId == ClassGlobalVar.CustumerId).isOpen;
                    }
                }
            }
            private static void ins(OpenTicketWindow o)
            {
                string cmdOpenTicketWindow =
         "INSERT INTO OpenTicketWindow (custumerId,idTicketWindow,nameTicket,[user],numberTicket,isOpen,dateOpen,Establishment_CustomerId)"
          + "VALUES ('{custumerId}','{idTicketWindow}','{nameTicket}','{user}',{numberTicket},'{isOpen}','{dateOpen}','{Establishment_CustomerId}')";

                string c = cmdOpenTicketWindow
                             .Replace("{custumerId}", o.custumerId.ToString())
                             .Replace("{idTicketWindow}", o.idTicketWindow.ToString())
                             .Replace("{nameTicket}", o.nameTicket)
                             .Replace("{user}", o.user)
                             .Replace("{numberTicket}", o.numberTicket.ToString())
                             .Replace("{isOpen}", o.isOpen.ToString())
                             .Replace("{dateOpen}", o.dateOpen.ToString())
                             .Replace("{Establishment_CustomerId}", o.Establishment_CustomerId.ToString())
                             ;
                new ClassDB(null).queryNonResonse(c);
            }
            public static void upd(OpenTicketWindow o)
            {
                string cmdUpd = "UPDATE OpenTicketWindow SET idTicketWindow='{idTicketWindow}',nameTicket='{nameTicket}',[user]='{user}',numberTicket={numberTicket},isOpen='{isOpen}',dateOpen=CAST('{dateOpen}' AS DateTime), IdTicketWindowG = '{IdTicketWindowG}', Establishment_CustomerId ='{Establishment_CustomerId}'" +
                          " WHERE custumerId= '{custumerId}'";

                string c = cmdUpd
                           .Replace("{custumerId}", o.custumerId.ToString())
                           .Replace("{idTicketWindow}", o.idTicketWindow.ToString())
                           .Replace("{nameTicket}", o.nameTicket)
                           .Replace("{user}", o.user)
                           .Replace("{numberTicket}", o.numberTicket.ToString())
                           .Replace("{isOpen}", o.isOpen.ToString())
                           .Replace("{dateOpen}", o.dateOpen.ToString())
                           .Replace("{IdTicketWindowG}", o.IdTicketWindowG.ToString() ?? Guid.Empty.ToString())
                           .Replace("{Establishment_CustomerId}", o.Establishment_CustomerId.ToString())
                ;
                new ClassDB(null).queryNonResonse(c);
            }
            public static bool opn()
            {
                OpenTicketWindow o = new OpenTicketWindow();

                int indx = table.FindIndex(l => l.custumerId == ClassGlobalVar.CustumerId);

                ClassGlobalVar.TicketWindow = Guid.NewGuid();

                if (indx == -1)
                {

                    o.custumerId = ClassGlobalVar.CustumerId;

                    o.idTicketWindow = ClassGlobalVar.TicketWindow;
                    o.nameTicket = ClassGlobalVar.nameTicket;
                    o.user = ClassGlobalVar.user;
                    o.numberTicket = ClassGlobalVar.numberTicket;
                    o.isOpen = true;
                    o.dateOpen = DateTime.Now;
                    o.IdTicketWindowG = ClassGlobalVar.TicketWindowG;
                    ins(o);
                }
                else
                {

                    o = table[indx];
                    o.idTicketWindow = ClassGlobalVar.TicketWindow;
                    o.nameTicket = ClassGlobalVar.nameTicket;
                    o.user = ClassGlobalVar.user;
                    o.numberTicket = ClassGlobalVar.numberTicket;
                    o.dateOpen = DateTime.Now;
                    o.isOpen = true;
                    o.IdTicketWindowG = ClassGlobalVar.TicketWindowG;

                    upd(o);

                }

                ClassCheck.openTicket();

                ClassGlobalVar.open = true;

                return true;
            }
            public static void close()
            {
                mess = "";

                if (ClassGlobalVar.open)
                {
                    mess += "Clôture du" + ClassGlobalVar.nameTicket + Environment.NewLine + Environment.NewLine + " Veuillez patienter! Exportation vers base de données..." + Environment.NewLine;


                    ClassCheck.closeTicket();

                    mess += "Exportation vers base de données est terminé" + Environment.NewLine;

                    int indx = table.FindIndex(l => l.custumerId == ClassGlobalVar.CustumerId);
                    if (indx != -1)
                    {
                        OpenTicketWindow o = table[indx];
                        o.dateOpen = DateTime.Now;
                        o.isOpen = false;
                        o.idTicketWindow = Guid.Empty;
                        o.IdTicketWindowG = Guid.Empty;
                        upd(o);
                        mess += ClassGlobalVar.nameTicket + " clôture terminée..." + Environment.NewLine;

                        ClassGlobalVar.open = false;
                    }
                    else
                    {
                        mess += ClassGlobalVar.nameTicket + " erreur de fermeture..." + Environment.NewLine;
                    }
                }
                else
                {
                    mess += ClassGlobalVar.nameTicket + " déjà clôturé..." + Environment.NewLine;
                }
            }

        }
        public class General
        {
            public System.Guid Id { get; set; }
            public System.Guid TicketWindowGeneral { get; set; }
            public Nullable<bool> Open { get; set; }
            public string Name { get; set; }
            public string User { get; set; }
            public System.DateTime Date { get; set; }
            public System.Guid Establishment { get; set; }

            public static List<General> table = new List<General>();
            public static bool open { get; set; }
            public static string mess { get; set; }
            public static void set()
            {
                table.Clear();
                List<object[]> L = new ClassDB(null).queryResonse("SELECT * FROM General");

                //      if (L.Count > 1) mess += "Erreur! Plusieurs ecritures dans le tableau Générale..." + Environment.NewLine;
                //      else
                //      {
                foreach (var e in L)
                {
                    General general = new General();

                    general.Id = (Guid)e[0];
                    general.TicketWindowGeneral = (Guid)e[1];
                    general.Open = (bool)e[2];

                    general.Name = (string)e[3];
                    general.User = (string)e[4];
                    general.Date = (DateTime)e[5];
                    general.Establishment = (Guid)e[6];  // ClassGlobalVar.IdEstablishment;


                    table.Add(general);
                }
                //    }

                int indx = table.FindIndex(l => l.Establishment == ClassGlobalVar.IdEstablishment);
                if (indx == -1)
                {
                    open = false;
                    General n = new General();
                    n.Id = Guid.NewGuid();
                    n.Name = ClassGlobalVar.nameTicket;
                    n.Open = false;
                    n.TicketWindowGeneral = Guid.Empty;
                    n.User = ClassGlobalVar.user;
                    n.Date = DateTime.Now;
                    n.Establishment = ClassGlobalVar.IdEstablishment;
                    ClassGlobalVar.TicketWindowG = n.TicketWindowGeneral;
                    ins(n);
                    table.Add(n);
                }
                else
                {
                    open = table[indx].Open ?? false;
                    ClassGlobalVar.TicketWindowG = table[indx].TicketWindowGeneral;
                }
            }

            public static void upd(General general)
            {
                string cmdUpd = "UPDATE General SET TicketWindowGeneral='{TicketWindowGeneral}',[Open]='{Open}',Name='{Name}',[User]='{User}',[Date]= CAST('{Date}' AS DateTime)" +
                                " WHERE Id= '{Id}'";
                string c = cmdUpd
                            .Replace("{TicketWindowGeneral}", general.TicketWindowGeneral.ToString())
                            .Replace("{Open}", general.Open.ToString())
                            .Replace("{Name}", general.Name.ToString())
                            .Replace("{User}", general.User.ToString())
                            .Replace("{Date}", general.Date.ToString())
                            .Replace("{Id}", general.Id.ToString())
                           ;
                new ClassDB(null).queryNonResonse(c);
            }

            public static void ins(General general)
            {
                string cmdInsCloseTicketG =
          "INSERT INTO General (Id,TicketWindowGeneral,[Open],Name,[User],[Date],Establishment_CustomerId)"
           + "VALUES ('{Id}','{TicketWindowGeneral}','{Open}','{Name}','{User}', CAST('{Date}' AS DateTime),'{Establishment_CustomerId}')";
                string c = cmdInsCloseTicketG
                           .Replace("{Id}", general.Id.ToString())
                           .Replace("{TicketWindowGeneral}", general.TicketWindowGeneral.ToString())
                           .Replace("{Open}", general.Open.ToString())
                           .Replace("{Name}", general.Name.ToString())
                           .Replace("{User}", general.User.ToString())
                           .Replace("{Date}", general.Date.ToString())
                           .Replace("{Establishment_CustomerId}", general.Establishment.ToString());
                ;
                new ClassDB(null).queryNonResonse(c);
            }

            public static bool opn()
            {

                set();

                if (!open)
                {
                    Guid TicketWindowGeneral = ClassCloseTicket.CloseTicketG.create();


                    General n = new General();

                    int indx = table.FindIndex(l => l.Establishment == ClassGlobalVar.IdEstablishment);

                    if (indx != -1)
                    {
                        n.Id = table[indx].Id;
                        n.Name = ClassGlobalVar.nameTicket;
                        n.Open = true;
                        n.TicketWindowGeneral = TicketWindowGeneral;
                        n.User = ClassGlobalVar.user;
                        n.Date = DateTime.Now;
                        n.Establishment = ClassGlobalVar.IdEstablishment;
                        upd(n);
                    }

                    else
                    {
                        n.Id = Guid.NewGuid();
                        n.Name = ClassGlobalVar.nameTicket;
                        n.Open = true;
                        n.TicketWindowGeneral = TicketWindowGeneral;
                        n.User = ClassGlobalVar.user;
                        n.Date = DateTime.Now;
                        n.Establishment = ClassGlobalVar.IdEstablishment;
                        ins(n);
                    }

                    ClassGlobalVar.TicketWindowG = TicketWindowGeneral;

                    foreach (var rec in OpenTicketWindow.table.FindAll(l => l.Establishment_CustomerId == ClassGlobalVar.IdEstablishment))
                    {
                        rec.IdTicketWindowG = ClassGlobalVar.TicketWindowG;

                        OpenTicketWindow.upd(rec);
                    }


                    return true;

                }
                else
                {
                    new ClassFunctuon().showMessageTime("Уже открыта просто продолжите работать");
                    return false;
                }
            }

            public static bool cls()
            {
                set();

                General g = table.Find(l => l.Establishment == ClassGlobalVar.IdEstablishment);

                mess = "";

                if (g.Open ?? true)
                {
                    if (ClassCloseTicket.CloseTicketG.cls())
                    {

                        ClassCloseTicket.CloseTicketG.prnt(ClassGlobalVar.TicketWindowG);
                        g.Open = false;

                        g.User = ClassGlobalVar.user;

                        g.Name = ClassGlobalVar.nameTicket;

                        g.TicketWindowGeneral = Guid.Empty;

                        g.Date = DateTime.Now;

                        upd(g);



                        mess = "Clôture générale est terminé";



                        return true;
                    }
                    else
                    {

                        mess += "Vous ne pouvez pas effectuer la clôture, car :" + Environment.NewLine + ClassCloseTicket.CloseTicketG.mess;

                        return false;
                    }
                }
                else
                {
                    mess += "Clôture générale a été faite";
                    return false;
                }
            }

        }
        public class Discount
        {
            public static void set()
            {
                Discount.DiscountCards.set();

                Discount.InfoClients.set();

            }
            public partial class DiscountCards
            {
                public System.Guid custumerId { get; set; }
                public string numberCard { get; set; }
                public int points { get; set; }
                public bool active { get; set; }

                public Guid InfoClients_custumerId { get; set; }

                public DateTime DateTimeLastAddProduct { get; set; }

                public static List<DiscountCards> LDiscountCards = new List<DiscountCards>();

                public static DiscountCards OneDiscountCards(string BCDC)
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM DiscountCards WHERE numberCard = '" + BCDC + "'");

                    DiscountCards dc = new DiscountCards();

                    if (S.Count == 1)
                    {
                        foreach (var e in S)
                        {

                            dc.custumerId = (Guid)e[0];

                            dc.numberCard = (string)e[1];

                            dc.points = (int)e[2];

                            dc.active = (bool)e[3];

                            try
                            {
                                dc.InfoClients_custumerId = (Guid)e[4];
                            }

                            catch
                            {
                                dc.InfoClients_custumerId = Guid.Empty;
                            }

                            dc.DateTimeLastAddProduct = (DateTime)e[5];
                        }

                    }

                    if (S.Count == 0)
                    {
                        dc = null;
                    }

                    if (S.Count > 1)
                    {
                        new ClassLog("Опасность повтор данных в дисконтных данных ");

                        foreach (var e in S)
                        {

                            dc.custumerId = (Guid)e[0];

                            dc.numberCard = (string)e[1];

                            dc.points = (int)e[2];

                            dc.active = (bool)e[3];

                            try
                            {
                                dc.InfoClients_custumerId = (Guid)e[4];
                            }

                            catch
                            {
                                dc.InfoClients_custumerId = Guid.Empty;
                            }

                            dc.DateTimeLastAddProduct = (DateTime)e[5];
                        }
                    }
                    return dc;
                }

                public static void set()
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM DiscountCards ");

                    foreach (var e in S)
                    {
                        DiscountCards dc = new DiscountCards();

                        dc.custumerId = (Guid)e[0];

                        dc.numberCard = (string)e[1];

                        dc.points = (int)e[2];

                        dc.active = (bool)e[3];



                        try
                        {
                            dc.InfoClients_custumerId = (Guid)e[4];
                        }

                        catch
                        {
                            dc.InfoClients_custumerId = Guid.Empty;
                        }

                        dc.DateTimeLastAddProduct = (DateTime)e[5];

                        LDiscountCards.Add(dc);
                    }

                }
                public static int upd(DiscountCards dc)
                {
                    DateTime now = DateTime.Now;

                    string cmdUpd = "UPDATE DiscountCards SET points={points}, DateTimeLastAddProduct = '{DateTimeLastAddProduct}'" +
                               " WHERE custumerId= '{custumerId}'";
                    string c = cmdUpd
                                .Replace("{points}", dc.points.ToString())
                                .Replace("{custumerId}", dc.custumerId.ToString())
                                .Replace("{DateTimeLastAddProduct}", now.ToString())
                               ;
                    //   LDiscountCards[LDiscountCards.FindIndex(l => l.custumerId == dc.custumerId)].points = dc.points;
                    //   LDiscountCards[LDiscountCards.FindIndex(l => l.custumerId == dc.custumerId)].DateTimeLastAddProduct = now;
                    return new ClassDB(null).queryNonResonse(c);
                }

            }
            public partial class InfoClients
            {
                public InfoClients()
                {

                    this.DiscountCards = new HashSet<DiscountCards>();

                }

                public System.Guid custumerId { get; set; }
                public int TypeClient { get; set; }
                public int Sex { get; set; }
                public string Name { get; set; }
                public string Surname { get; set; }
                public string NameCompany { get; set; }
                public string SIRET { get; set; }
                public string FRTVA { get; set; }
                public string OfficeAddress { get; set; }
                public string OfficeZipCode { get; set; }
                public string OfficeCity { get; set; }
                public string HomeAddress { get; set; }
                public string HomeZipCode { get; set; }
                public string HomeCity { get; set; }
                public string Telephone { get; set; }
                public string Mail { get; set; }
                public string Password { get; set; }
                public Guid Countrys_CustomerId { get; set; }
                public virtual ICollection<DiscountCards> DiscountCards { get; set; }

                public static List<InfoClients> LInfoClients = new List<InfoClients>();
                public static void set()
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM InfoClients");

                    foreach (var e in S)
                    {
                        InfoClients inf = new InfoClients();

                        inf.custumerId = (Guid)e[0];

                        inf.TypeClient = (int)e[1];

                        inf.Sex = (int)e[2];

                        inf.Name = (string)e[3];

                        inf.Surname = e[4] as string;

                        inf.NameCompany = e[5] as string;

                        inf.SIRET = e[6] as string;

                        inf.FRTVA = e[7] as string;

                        inf.OfficeAddress = e[8] as string;

                        inf.OfficeZipCode = e[9] as string;

                        inf.OfficeCity = e[10] as string;

                        inf.HomeAddress = e[11] as string;

                        inf.HomeZipCode = e[12] as string;

                        inf.HomeCity = e[13] as string;

                        inf.Telephone = e[14] as string;

                        inf.Mail = e[15] as string;

                        inf.Password = e[16] as string;

                        inf.Countrys_CustomerId = (Guid)e[17];

                        List<DiscountCards> k = Discount.DiscountCards.LDiscountCards.FindAll(l => l.InfoClients_custumerId == inf.custumerId);

                        foreach (var elm in k)
                            inf.DiscountCards.Add(elm);

                        LInfoClients.Add(inf);
                    }
                }
                public static InfoClients sel(Guid custumerId)
                {


                    InfoClients inf = new InfoClients();

                    List<object[]> S = new List<object[]>();

                    if (custumerId != Guid.Empty)
                        S = new ClassDB(null).queryResonse("SELECT * FROM InfoClients WHERE custumerId='" + custumerId + "'");


                    if (S.Count == 0)
                    {
                        inf = null;
                    }

                    if (S.Count > 1)
                    {
                        new ClassFunctuon().showMessageSB("Есть повторы ");
                    }

                    if (S.Count == 1)
                    {
                        var e = S[0];

                        inf.custumerId = (Guid)e[0];

                        inf.TypeClient = (int)e[1];

                        inf.Sex = (int)e[2];

                        inf.Name = (string)e[3];

                        inf.Surname = e[4] as string;

                        inf.NameCompany = e[5] as string;

                        inf.SIRET = e[6] as string;

                        inf.FRTVA = e[7] as string;

                        inf.OfficeAddress = e[8] as string;

                        inf.OfficeZipCode = e[9] as string;

                        inf.OfficeCity = e[10] as string;

                        inf.HomeAddress = e[11] as string;

                        inf.HomeZipCode = e[12] as string;

                        inf.HomeCity = e[13] as string;

                        inf.Telephone = e[14] as string;

                        inf.Mail = e[15] as string;

                        inf.Password = e[16] as string;

                        inf.Countrys_CustomerId = (Guid)e[17];

                    }
                    return inf;
                }
                public static void mod()
                {

                }
            }

        }
        public class En_attenete
        {
            public void set(XDocument check)
            {
                SyncPlus sp = new SyncPlus();

                sp = new SyncPlus().sel(ClassGlobalVar.CustumerId);

                if (sp == null)
                {
                    sp = new SyncPlus();
                    sp.customerId = ClassGlobalVar.CustumerId;
                    sp.date = DateTime.Now;
                    sp.nameCasse = ClassGlobalVar.nameTicket;
                    new SyncPlus().ins(sp);
                }

                SyncPlusProducts spp = new SyncPlusProducts();
                spp.customerId = Guid.NewGuid();
                spp.check = check;
                spp.date = sp.date;
                spp.customerIdSyncPlus = sp.customerId;


                new SyncPlusProducts().ins(spp);
            }

            public XDocument get(Guid customerId)
            {
                SyncPlusProducts spp = new SyncPlusProducts().sel_one(customerId);

                new SyncPlusProducts().del(customerId);

                List<SyncPlusProducts> list = new SyncPlusProducts().sel(spp.customerIdSyncPlus);

                if (list.Count == 0)
                    new SyncPlus().del(spp.customerIdSyncPlus);

                return spp.check;
            }
            public partial class SyncPlus
            {
                public Guid customerId { get; set; }
                public DateTime date { get; set; }
                public string nameCasse { get; set; }


                public void ins(SyncPlus s)
                {
                    string cmd = "INSERT INTO SyncPlus VALUES ('{customerId}', CAST ('{date}' AS datetime), '{nameCasse}' )";

                    string c = cmd.Replace("{customerId}", s.customerId.ToString())
                        .Replace("{date}", s.date.ToString())
                        .Replace("{nameCasse}", s.nameCasse)
                        ;

                    new ClassDB(null).queryNonResonse(c);
                }

                public void del(Guid customerId)
                {
                    string cmd = "DELETE FROM SyncPlus WHERE customerId='" + customerId + "'";

                    new ClassDB(null).queryNonResonse(cmd);
                }

                public SyncPlus sel(Guid customerId)
                {
                    string cmd = "SELECT * FROM SyncPlus WHERE customerId='" + customerId + "'";

                    List<object[]> L = new ClassDB(null).queryResonse(cmd);

                    if (L.Count > 0)
                    {
                        object[] o = L[0];

                        SyncPlus p = new SyncPlus();

                        p.customerId = (Guid)o[0];

                        p.date = (DateTime)o[1];

                        p.nameCasse = (string)o[2];

                        return p;
                    }
                    else return null;
                }

                public List<SyncPlus> sel()
                {
                    string cmd = "SELECT * FROM SyncPlus";

                    List<object[]> l = new ClassDB(null).queryResonse(cmd);

                    List<SyncPlus> R = new List<SyncPlus>();

                    foreach (var o in l)
                    {
                        SyncPlus p = new SyncPlus();

                        p.customerId = (Guid)o[0];

                        p.date = (DateTime)o[1];

                        p.nameCasse = (string)o[2];

                        R.Add(p);
                    }
                    return R;
                }
            }
            public partial class SyncPlusProducts
            {
                public System.Guid customerId { get; set; }
                public XDocument check { get; set; }
                public DateTime date { get; set; }
                public Guid customerIdSyncPlus { get; set; }

                public void del(Guid customerId)
                {
                    string cmd = "DELETE FROM SyncPlusProducts WHERE customerId='" + customerId.ToString() + "'";

                    new ClassDB(null).queryNonResonse(cmd);
                }

                public void ins(SyncPlusProducts sp)
                {
                    string cmd = "INSERT INTO SyncPlusProducts VALUES ('{customerId}', '{check}', CAST ('{date}' AS DATE) ,'{customerIdSyncPlus}')";
                    var reader = sp.check.CreateReader();
                    reader.MoveToContent();
                    string c = cmd
                        .Replace("{customerId}", sp.customerId.ToString())
                        .Replace("{check}", sp.check.ToString().Replace("'", "''"))
                        .Replace("{date}", sp.date.ToString())
                        .Replace("{customerIdSyncPlus}", sp.customerIdSyncPlus.ToString())
                        ;



                    new ClassDB(null).queryNonResonse(c);
                }

                public List<SyncPlusProducts> sel(Guid customerIdSyncPlus)
                {
                    string cmd = "SELECT * FROM SyncPlusProducts WHERE customerIdSyncPlus ='" + customerIdSyncPlus + "'";

                    List<object[]> L = new ClassDB(null).queryResonse(cmd);

                    List<SyncPlusProducts> R = new List<SyncPlusProducts>();

                    foreach (var o in L)
                    {
                        SyncPlusProducts spp = new SyncPlusProducts();
                        spp.customerId = (Guid)o[0];
                        spp.check = XDocument.Parse((string)o[1]);
                        spp.date = (DateTime)o[2];
                        spp.customerIdSyncPlus = (Guid)o[3];
                        R.Add(spp);
                    }

                    return R;
                }
                public SyncPlusProducts sel_one(Guid customerId)
                {
                    string cmd = "SELECT * FROM SyncPlusProducts WHERE customerId ='" + customerId + "'";

                    object[] o = new ClassDB(null).queryResonse(cmd)[0];
                    SyncPlusProducts spp = new SyncPlusProducts();
                    spp.customerId = (Guid)o[0];
                    spp.check = XDocument.Parse(((string)o[1]).Replace("''", "'"));
                    spp.date = (DateTime)o[2];
                    spp.customerIdSyncPlus = (Guid)o[3];
                    return spp;
                }
            }
        }
        public class Countrys
        {

            public System.Guid CustomerId { get; set; }
            public string NameCountry { get; set; }
            public string Capital { get; set; }
            public string Continent { get; set; }

            public List<Countrys> selAll()
            {
                List<Countrys> R = new List<Countrys>();

                string cmd = "SELECT * FROM Countrys";

                List<object[]> L = new ClassDB(null).queryResonse(cmd);

                foreach (var e in L)
                {
                    Countrys c = new Countrys();
                    c.CustomerId = (Guid)e[0];
                    c.NameCountry = (string)e[1];
                    c.Capital = (string)e[2];
                    c.Continent = (string)e[3];
                    R.Add(c);
                }

                return R;
            }

        }
        public class Stat
        {
            public partial class StatNationPopup
            {
                public System.Guid IdCustomer { get; set; }
                public string NameNation { get; set; }
                public int QTY { get; set; }
                public static void ins(StatNationPopup sn)
                {
                    string cmd = "INSERT INTO StatNationPopup VALUES ('{IdCustomer}','{NameNation}',{QTY})";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())
                        .Replace("{NameNation}", sn.NameNation)
                        .Replace("{QTY}", sn.QTY.ToString())
                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static void mod(StatNationPopup sn)
                {
                    string cmd = "UPDATE StatNationPopup SET NameNation='{NameNation}',QTY={QTY} WHERE  IdCustomer='{IdCustomer}'";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())
                        .Replace("{NameNation}", sn.NameNation)
                        .Replace("{QTY}", sn.QTY.ToString())
                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static void del(StatNationPopup sn)
                {
                    string cmd = "DELETE FROM StatNationPopup WHERE  IdCustomer='{IdCustomer}'";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())

                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static StatNationPopup sel(StatNationPopup sn)
                {
                    string cmd = "SELECT * FROM StatNationPopup WHERE IdCustomer = '{IdCustomer}'";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())
                        ;
                    List<object[]> L = new ClassDB(null).queryResonse(c);

                    List<StatNationPopup> R = new List<StatNationPopup>();

                    foreach (var o in L)
                    {
                        StatNationPopup sp = new StatNationPopup();

                        sp.IdCustomer = (Guid)o[0];

                        sp.NameNation = (string)o[1];

                        sp.QTY = (int)o[2];

                        R.Add(sp);
                    }

                    return R[0];
                }
                public static List<StatNationPopup> selAll()
                {
                    string cmd = "SELECT * FROM StatNationPopup ";


                    List<object[]> L = new ClassDB(null).queryResonse(cmd);

                    List<StatNationPopup> R = new List<StatNationPopup>();

                    foreach (var o in L)
                    {
                        StatNationPopup sp = new StatNationPopup();

                        sp.IdCustomer = (Guid)o[0];

                        sp.NameNation = (string)o[1];

                        sp.QTY = (int)o[2];

                        R.Add(sp);
                    }

                    return R;
                }
            }
            public partial class StatPlaceArrond
            {
                public System.Guid IdCustomer { get; set; }
                public string NamePlaceArrond { get; set; }
                public int QTY { get; set; }
                public static void ins(StatPlaceArrond sn)
                {
                    string cmd = "INSERT INTO StatPlaceArrond VALUES ('{IdCustomer}','{NamePlaceArrond}',{QTY})";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())
                        .Replace("{NamePlaceArrond}", sn.NamePlaceArrond)
                        .Replace("{QTY}", sn.QTY.ToString())
                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static void mod(StatPlaceArrond sn)
                {
                    string cmd = "UPDATE StatPlaceArrond SET NamePlaceArrond='{NamePlaceArrond}',QTY={QTY} WHERE  IdCustomer='{IdCustomer}'";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())
                        .Replace("{NamePlaceArrond}", sn.NamePlaceArrond)
                        .Replace("{QTY}", sn.QTY.ToString())
                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static void del(StatPlaceArrond sn)
                {
                    string cmd = "DELETE FROM StatPlaceArrond WHERE  IdCustomer='{IdCustomer}'";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())

                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static StatPlaceArrond sel(StatPlaceArrond sn)
                {
                    string cmd = "SELECT * FROM StatPlaceArrond WHERE IdCustomer = '{IdCustomer}'";

                    string c = cmd
                        .Replace("{IdCustomer}", sn.IdCustomer.ToString())
                        ;
                    List<object[]> L = new ClassDB(null).queryResonse(c);

                    List<StatPlaceArrond> R = new List<StatPlaceArrond>();

                    foreach (var o in L)
                    {
                        StatPlaceArrond sp = new StatPlaceArrond();

                        sp.IdCustomer = (Guid)o[0];

                        sp.NamePlaceArrond = (string)o[1];

                        sp.QTY = (int)o[2];

                        R.Add(sp);
                    }

                    return R[0];
                }
                public static List<StatPlaceArrond> selAll()
                {
                    string cmd = "SELECT * FROM StatPlaceArrond";


                    List<object[]> L = new ClassDB(null).queryResonse(cmd);

                    List<StatPlaceArrond> R = new List<StatPlaceArrond>();

                    foreach (var o in L)
                    {
                        StatPlaceArrond sp = new StatPlaceArrond();

                        sp.IdCustomer = (Guid)o[0];

                        sp.NamePlaceArrond = (string)o[1];

                        sp.QTY = (int)o[2];

                        R.Add(sp);
                    }

                    return R;
                }
            }
            public partial class StatNation
            {
                public System.Guid CustomerId { get; set; }
                public System.DateTime Date { get; set; }
                public System.Guid IdCaisse { get; set; }
                public System.Guid IdNation { get; set; }
                public System.Guid IdArrond { get; set; }
                public static void ins(StatNation sn)
                {
                    string cmd = "INSERT INTO StatNation VALUES ('{CustomerId}', CAST('{Date}' AS DATETIME),'{IdCaisse}', '{IdNation}', '{IdArrond}')";

                    string c = cmd
                        .Replace("{CustomerId}", sn.CustomerId.ToString())
                        .Replace("{Date}", sn.Date.ToString())
                        .Replace("{IdCaisse}", sn.IdCaisse.ToString())
                        .Replace("{IdNation}", sn.IdNation.ToString())
                        .Replace("{IdArrond}", sn.IdArrond.ToString())

                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static void mod(StatNation sn)
                {
                    string cmd = "UPDATE StatNation SET Date=CAST ('{Date}' AS DATETIME),IdCaisse='{IdCaisse}',IdNation='{IdNation}',IdArrond='{IdArrond}'   WHERE  CustomerId='{CustomerId}'";

                    string c = cmd
                        .Replace("{CustomerId}", sn.CustomerId.ToString())
                        .Replace("{Date}", sn.Date.ToString())
                        .Replace("{IdCaisse}", sn.IdCaisse.ToString())
                        .Replace("{IdNation}", sn.IdNation.ToString())
                        .Replace("{IdArrond}", sn.IdArrond.ToString())
                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static void del(StatNation sn)
                {
                    string cmd = "DELETE FROM StatNation WHERE  CustomerId='{CustomerId}'";

                    string c = cmd
                        .Replace("{CustomerId}", sn.CustomerId.ToString())

                        ;
                    new ClassDB(null).queryNonResonse(c);
                }
                public static StatNation sel(StatNation sn)
                {
                    string cmd = "SELECT * FROM StatNation WHERE CustomerId = '{CustomerId}'";

                    string c = cmd
                        .Replace("{CustomerId}", sn.CustomerId.ToString())
                        ;
                    List<object[]> L = new ClassDB(null).queryResonse(c);

                    List<StatNation> R = new List<StatNation>();

                    foreach (var o in L)
                    {
                        StatNation sp = new StatNation();

                        sp.CustomerId = (Guid)o[0];

                        sp.Date = (DateTime)o[1];

                        sp.IdCaisse = (Guid)o[2];

                        sp.IdNation = (Guid)o[3];

                        sp.IdArrond = (Guid)o[4];

                        R.Add(sp);
                    }

                    return R[0];
                }
                public static List<StatNation> selAll()
                {
                    string cmd = "SELECT * FROM StatNation";


                    List<object[]> L = new ClassDB(null).queryResonse(cmd);

                    List<StatNation> R = new List<StatNation>();

                    foreach (var o in L)
                    {
                        StatNation sp = new StatNation();

                        sp.CustomerId = (Guid)o[0];

                        sp.Date = (DateTime)o[1];

                        sp.IdCaisse = (Guid)o[2];

                        sp.IdNation = (Guid)o[3];

                        sp.IdArrond = (Guid)o[4];

                        R.Add(sp);
                    }

                    return R;
                }
            }
        }
        public class XML_File
        {
            public System.Guid CustomerId { get; set; }
            public string FileName { get; set; }
            public System.DateTime Date { get; set; }
            public bool Upd { get; set; }
            public string UserName { get; set; }
            public string Data { get; set; }
            public Guid Establishment_CustomerId { get; set; }
            public static void ins(XML_File sn)
            {
                string cmd = "INSERT INTO XML_File VALUES ('{CustomerId}','{FileName}', CAST('{Date}' AS DATETIME),'{Upd}', '{UserName}', '{Data}','{Establishment_CustomerId}')";

                string c = cmd
                    .Replace("{CustomerId}", sn.CustomerId.ToString())
                    .Replace("{FileName}", sn.FileName.ToString())
                    .Replace("{Date}", sn.Date.ToString())
                    .Replace("{Upd}", sn.Upd.ToString())
                    .Replace("{UserName}", sn.UserName.ToString())
                    .Replace("{Data}", sn.Data.ToString().Replace("'", "''"))
                      .Replace("{Establishment_CustomerId}", sn.Establishment_CustomerId.ToString())
                    ;
                new ClassDB(null).queryNonResonse(c);
            }
            public static void mod(XML_File sn)
            {
                string cmd = "UPDATE XML_File SET CustomerId='{CustomerId}', FileName='{FileName}', Date=CAST ('{Date}' AS DATETIME),Upd='{Upd}',UserName='{UserName}',Data='{Data}', Establishment_CustomerId='{Establishment_CustomerId}'   WHERE  CustomerId='{CustomerId}'";

                string c = cmd
                     .Replace("{CustomerId}", sn.CustomerId.ToString())
                     .Replace("{FileName}", sn.FileName.ToString())
                     .Replace("{Date}", sn.Date.ToString())
                     .Replace("{Upd}", sn.Upd.ToString())
                     .Replace("{UserName}", sn.UserName.ToString())
                     .Replace("{Data}", sn.Data.ToString().Replace("'", "''"))
                       .Replace("{Establishment_CustomerId}", sn.Establishment_CustomerId.ToString())
                     ;
                new ClassDB(null).queryNonResonse(c);
            }
            public static void del(XML_File sn)
            {
                string cmd = "DELETE FROM XML_File WHERE  CustomerId='{CustomerId}'";

                string c = cmd
                    .Replace("{CustomerId}", sn.CustomerId.ToString())

                    ;
                new ClassDB(null).queryNonResonse(c);
            }
            public static XML_File sel(string FileName, string UserName)
            {
                string cmd = "SELECT * FROM XML_File WHERE (UserName = '{UserName}' AND FileName ='{FileName}'  )";

                string c = cmd
                    .Replace("{UserName}", UserName)
                    .Replace("{FileName}", FileName)
                    ;
                List<object[]> L = new ClassDB(null).queryResonse(c);

                List<XML_File> R = new List<XML_File>();

                foreach (var o in L)
                {
                    XML_File sp = new XML_File();

                    sp.CustomerId = (Guid)o[0];

                    sp.FileName = (string)o[1];

                    sp.Date = (DateTime)o[2];

                    sp.Upd = (bool)o[3];

                    sp.UserName = (string)o[4];

                    sp.Data = (string)o[5];

                    sp.Establishment_CustomerId = (Guid)o[6];

                    R.Add(sp);
                }

                return R.Count > 0 ? R[0] : null;
            }
            public static List<XML_File> selAll()
            {
                string cmd = "SELECT * FROM XML_File";


                List<object[]> L = new ClassDB(null).queryResonse(cmd);

                List<XML_File> R = new List<XML_File>();

                foreach (var o in L)
                {
                    XML_File sp = new XML_File();

                    sp.CustomerId = (Guid)o[0];

                    sp.FileName = (string)o[1];

                    sp.Date = (DateTime)o[2];

                    sp.Upd = (bool)o[3];

                    sp.UserName = (string)o[4];

                    sp.Data = (string)o[5];

                    sp.Establishment_CustomerId = (Guid)o[6];

                    R.Add(sp);
                }

                return R;
            }
        }
        public class isRun
        {
            private const int SW_SHOWNORMAL = 1;
            private const int SW_SHOWMINIMIZED = 2;
            private const int SW_SHOWMAXIMIZED = 3;

            [DllImport("user32.dll")]
            private static extern bool ShowWindowAsync(int hWnd, int nCmdShow);

            [DllImportAttribute("User32.dll")]
            private static extern int FindWindow(String ClassName, String WindowName);


            [DllImportAttribute("User32.dll")]
            private static extern IntPtr SetForegroundWindow(int hWnd);

            public bool isRun_()
            {
                int hWnd = FindWindow(null, "Caisse");
                if (hWnd > 0)
                {


                    ShowWindowAsync(hWnd, SW_SHOWMAXIMIZED);

                    SetForegroundWindow(hWnd);

                    Application.Current.Shutdown();

                    return true;

                }

                return false;
            }
        }


        #region ProMode
        public class classPro
        {
            public class InfoCliientsDisconts
            {
                public Guid CustomerId { get; set; }
                public Guid CustomerIdInfoClients { get; set; }
                public int IdInfoClientsDiscountsType { get; set; }
            }

            public class InfoClientsDiscountsType
            {
                public InfoClientsDiscountsType(object[] o)
                {
                    this.Id = (int)o[0];
                    this.Name = (string)o[1];
                    this.Type = (int)o[2];
                    this.Value = (int)o[3];
                    this.Description = (string)o[4];
                }
                public int Id { get; set; }
                public string Name { get; set; }
                public int Type { get; set; }
                public int Value { get; set; }
                public string Description { get; set; }

                public static List<InfoClientsDiscountsType> L = new List<InfoClientsDiscountsType>();

                public static void get()
                {
                    string cmd_ = @"SELECT [Id],[Name],[Type],[Value],[Description] FROM [InfoClientsDiscountsType]";

                    var InfoClientsDiscountsType = new ClassDB(null).queryResonse(cmd_);

                    foreach (var o in InfoClientsDiscountsType)
                    {
                        L.Add(new InfoClientsDiscountsType(o));
                    }
                }
            }

            public static int SexToInt(string s)
            {
                switch (s)
                {
                    case "M.": return 0;
                    case "Mme.": return 1;
                    case "Mlle.": return 2;
                    default: break;
                }
                return 0;
            }
            public classPro(object[] o)
            {
                this.custumerId = (Guid)o[0];

                this.sexint = (int)(o[1]);

                this.Sex = this.sexint == 0 ? "M." : (this.sexint == 1 ? "Mme." : "Mlle.");

                this.Name = o[2].ToString();

                this.SurName = o[3].ToString();

                this.NameCompany = o[4].ToString();

                this.Mail = o[5].ToString();

                this.Telephone = o[6].ToString();

                this.Nclient = int.Parse(o[7].ToString());

                this.SIRET = o[8].ToString();

                this.FRTVA = o[9].ToString();

                this.OfficeAddress = o[10].ToString();

                this.OfficeZipCode = o[11].ToString();

                this.OfficeCity = o[12].ToString();

                this.HomeAddress = o[13].ToString();

                this.HomeZipCode = o[14].ToString();

                this.HomeCity = o[15].ToString();

                this.Password = o[16].ToString();

                this.Countrys_CustomerId = (Guid)o[17];

                this.FavoritesProductAutoCustomerId = (Guid)o[18];

                this.DiscountType = (int)(o[19] is DBNull ? 0 : o[19]);

                var d = InfoClientsDiscountsType.L.Find(k => k.Id == this.DiscountType);

                this.DiscountName = d.Name;

                this.DiscountValue = d.Value;
            }
            private Guid custumerId { get; set; }
            public static Guid getcustumerId(classPro p)
            {
                return p.custumerId;
            }
            public string DiscountName { get; set; }
            private int DiscountValue { get; set; }
            public static int getDiscountValue(classPro p) { return p.DiscountValue; }
            private int DiscountType { get; set; }
            public static int getDiscountType(classPro p) { return p.DiscountType; }
            private int sexint { get; set; }
            private string Sex { get; set; }
            private string Name { get; set; }
            private string SurName { get; set; }
            public string NameCompany { get; set; }
            public string Mail { get; set; }
            public string Telephone { get; set; }
            public int Nclient { get; set; }
            private string SIRET { get; set; }
            private string FRTVA { get; set; }
            private string OfficeAddress { get; set; }
            private string OfficeZipCode { get; set; }
            private string OfficeCity { get; set; }
            private string HomeAddress { get; set; }
            private string HomeZipCode { get; set; }
            private string HomeCity { get; set; }
            private string Password { get; set; }
            private Guid Countrys_CustomerId { get; set; }
            private Guid FavoritesProductAutoCustomerId { get; set; }
            public static string getSex(classPro p) { return p.Sex; }

            public static string getName(classPro p) { return p.Name ?? ""; }
            public static string getSurName(classPro p) { return p.SurName ?? ""; }
            public static string getSIRET(classPro p)
            {
                return p.SIRET ?? "";
            }
            public static string getFRTVA(classPro p) { return p.FRTVA ?? ""; }
            public static string getOfficeAddress(classPro p) { return p.OfficeAddress ?? ""; }
            public static string getOfficeZipCode(classPro p) { return p.OfficeZipCode ?? ""; }
            public static string getOfficeCity(classPro p) { return p.OfficeCity ?? ""; }
            public static string getHomeAddress(classPro p) { return p.HomeAddress ?? ""; }
            public static string getHomeZipCode(classPro p) { return p.HomeZipCode ?? ""; }
            public static string getHomeCity(classPro p) { return p.HomeCity ?? ""; }
            public static string getPassword(classPro p) { return p.Password ?? ""; }
            public static Guid getCountrys_CustomerId(classPro p) { return p.Countrys_CustomerId; }
            public static Guid getFavoritesProductAutoCustomerId(classPro p) { return p.FavoritesProductAutoCustomerId; }

            public static int getNclient()
            {
                string cmd = @"SELECT MAX( CAST( Nclient AS int ))  FROM [InfoClients] WHERE [TypeClient]=2";

                return (int)new ClassDB(null).queryResonse(cmd)[0][0];
            }

            public static List<classPro> get()
            {
                InfoClientsDiscountsType.get();

                string cmd = @"SELECT [custumerId],[Sex], [Name],[Surname],[NameCompany],[Mail],[Telephone],[Nclient],[SIRET]
      ,[FRTVA]
      ,[OfficeAddress]
      ,[OfficeZipCode]
      ,[OfficeCity]
      ,[HomeAddress]
      ,[HomeZipCode]
      ,[HomeCity]
      ,[Password]
      ,[Countrys_CustomerId]
      ,[FavoritesProductAutoCustomerId], (SELECT
      [IdInfoClientsDiscountsType]
  FROM [InfoCliientsDisconts] WHERE [CustomerIdInfoClients] = [InfoClients].[custumerId]) FROM [InfoClients] WHERE [TypeClient]=2";

                var pro = new ClassDB(null).queryResonse(cmd);

                List<classPro> Pro = new List<classPro>();

               

                foreach (var item in pro)
                {
                    Pro.Add(new classPro(item));
                }

                return Pro;
            }

            public static int add(classPro pro)
            {
                pro.Nclient = (getNclient() + 10);

                string cmd = @"INSERT INTO[dbo].[InfoClients] 
           ([custumerId] 
           ,[TypeClient]
           ,[Sex]
           ,[Name]
           ,[Surname]
           ,[NameCompany]
           ,[SIRET]
           ,[FRTVA]
           ,[OfficeAddress]
           ,[OfficeZipCode]
           ,[OfficeCity]
           ,[HomeAddress]
           ,[HomeZipCode]
           ,[HomeCity]
           ,[Telephone]
           ,[Mail]
           ,[Password]
           ,[Countrys_CustomerId]
           ,[FavoritesProductAutoCustomerId]
           ,[Nclient])
     VALUES
           (NEWID()
           ,'<TypeClient>'
           ,'<Sex>'
           ,'<Name>'
           ,'<Surname>'
           ,'<NameCompany>'
           ,'<SIRET>'
           ,'<FRTVA>'
           ,'<OfficeAddress>'
           ,'<OfficeZipCode>'
           ,'<OfficeCity>'
           ,'<HomeAddress>'
           ,'<HomeZipCode>'
           ,'<HomeCity>'
           ,'<Telephone>'
           ,'<Mail>'
           ,'<Password>'
           ,'<Countrys_CustomerId>'
           ,'<FavoritesProductAutoCustomerId>'
           ,'<Nclient>')";

                cmd = cmd.Replace("<TypeClient>", "2").
                    Replace("<Sex>", pro.sexint.ToString()).
                    Replace("<Name>", pro.Name).
                    Replace("<Surname>", pro.SurName).
                    Replace("<NameCompany>", pro.NameCompany).
                    Replace("<SIRET>", pro.SIRET).
                    Replace("<FRTVA>", pro.FRTVA).
                    Replace("<OfficeAddress>", pro.OfficeAddress).
                     Replace("<OfficeZipCode>", pro.OfficeZipCode).
                      Replace("<OfficeCity>", pro.OfficeCity).
                    Replace("<HomeAddress>", pro.HomeAddress).
                    Replace("<HomeCity>", pro.HomeCity).
                    Replace("<Telephone>", pro.Telephone).
                    Replace("<Mail>", pro.Mail).
                    Replace("<Password>", "").
                    Replace("<Countrys_CustomerId>", "F3D5541D-9860-4D93-ADCF-18F631ADAD74").
                    Replace("<FavoritesProductAutoCustomerId>", Guid.Empty.ToString()).
                    Replace("<Nclient>", pro.Nclient.ToString());
                return new ClassDB(null).queryNonResonse(cmd);
            }
        }

        public class classPriceGros
        {
            public classPriceGros(object[] o)
            {
                this.CustomerId = (Guid)o[0];

                this.ProductsCustumerId = (Guid)o[1];

                this.Prix = (decimal)o[2];
            }

            public static string Establishment_CustomerId = ClassGlobalVar.Establishment.CustomerId.ToString();

            public Guid CustomerId { get; set; }

            public Guid ProductsCustumerId { get; set; }

            public Decimal Prix { get; set; }

            private static string cmd_product_Prix(Guid customerIdProduct, bool f)
            {


                return f ? "SELECT [CustomerId],[ProductsCustumerId],[Price] FROM [StockReal] WHERE [Establishment_CustomerId] = '"
                        + classPriceGros.Establishment_CustomerId +
                        "'  AND [ProductsCustumerId] ='" + customerIdProduct + "'" : "  OR [ProductsCustumerId] ='" + customerIdProduct + "'";
            }

            public static classPriceGros[] get(Guid[] CustomerIdProduct)
            {
                classPriceGros[] g = new classPriceGros[CustomerIdProduct.Length];

                string cmd = String.Empty;

                foreach (var item in CustomerIdProduct)
                {
                    cmd += cmd_product_Prix(item, cmd == String.Empty);
                }

                var A = new ClassDB(null).queryResonse(cmd);

                for (int i = 0; i < g.Length; i++)
                {
                    g[i] = new classPriceGros(A[i]);
                }
                return g;
            }
        }
        public partial class TES
        {
            public TES()
            {
                this.TESproducts = new HashSet<TESproducts>();
                this.TESreglement = new HashSet<TESreglement>();
            }

            public static int maxId(int type)
            {
                return (int)new ClassDB(null).queryResonse("(SELECT MAX(Id) FROM TES WHERE  Type  = " + type + " )")[0][0];
            }

            public static int ins(TES tes)
            {
                string cmd = "";

                cmd += @"INSERT INTO [dbo].[TES]
           ([CustomerId]
           ,[Id]
           ,[Type]
           ,[DateTime]
           ,[Payement]
           ,[Livraison]
           ,[a_CodeFournisseur]
           ,[a_Sex]
           ,[a_Name]
           ,[a_Surname]
           ,[a_NameCompany]
           ,[a_SIRET]
           ,[a_FRTVA]
           ,[a_OfficeAddress]
           ,[a_OfficeZipCode]
           ,[a_OfficeCity]
           ,[a_Telephone]
           ,[a_Mail]
           ,[v_NameCompany]
           ,[v_CP]
           ,[v_Ville]
           ,[v_Adresse]
           ,[v_Phone]
           ,[v_Mail]
           ,[v_SIRET]
           ,[v_FRTVA]
           ,[v_CodeNAF]
           ,[v_Fax]
           ,[Montant]
           ,[Description]
           ,[Nclient])
     VALUES
           ('<CustomerId>'
           ,<Id>
           ,'<Type>'
           ,'<DateTime>'
           ,'<Payement>'
           ,'<Livraison>'
           ,'<a_CodeFournisseur>'
           ,'<a_Sex>'
           ,'<a_Name>'
           ,'<a_Surname>'
           ,'<a_NameCompany>'
           ,'<a_SIRET>'
           ,'<a_FRTVA>'
           ,'<a_OfficeAddress>'
           ,'<a_OfficeZipCode>'
           ,'<a_OfficeCity>'
           ,'<a_Telephone>'
           ,'<a_Mail>'
           ,'<v_NameCompany>'
           ,'<v_CP>'
           ,'<v_Ville>'
           ,'<v_Adresse>'
           ,'<v_Phone>'
           ,'<v_Mail>'
           ,'<v_SIRET>'
           ,'<v_FRTVA>'
           ,'<v_CodeNAF>'
           ,'<v_Fax>'
           ,<Montant>
           ,'<Description>'
           ,'<Nclient>') ";

                cmd = cmd.Replace("<CustomerId>", tes.CustomerId.ToString())
                    .Replace("<Id>", tes.Id.ToString())
                    .Replace("<Type>", tes.Type.ToString())
                    .Replace("<DateTime>", tes.DateTime.ToString())
                    .Replace("<Payement>", tes.Payement.ToString())
                    .Replace("<Livraison>", tes.Livraison.ToString())
                    .Replace("<a_CodeFournisseur>", tes.a_CodeFournisseur.ToString())
                    .Replace("<a_Sex>", tes.a_Sex.Value.ToString())
                    .Replace("<a_Name>", tes.a_Name.Replace("'", "''"))
                    .Replace("<a_Surname>", tes.a_Surname.Replace("'", "''"))
                    .Replace("<a_NameCompany>", tes.a_NameCompany.Replace("'", "''"))
                    .Replace("<a_SIRET>", tes.a_SIRET)
                    .Replace("<a_FRTVA>", tes.a_FRTVA)
                    .Replace("<a_OfficeAddress>", tes.a_OfficeAddress.Replace("'", "''"))
                    .Replace("<a_OfficeZipCode>", tes.a_OfficeZipCode.Replace("'", "''"))
                    .Replace("<a_OfficeCity>", tes.a_OfficeCity.Replace("'", "''"))
                    .Replace("<a_Telephone>", tes.a_Telephone)
                    .Replace("<a_Mail>", tes.a_Mail)
                    .Replace("<v_NameCompany>", tes.a_NameCompany.Replace("'", "''"))
                    .Replace("<v_CP>", tes.v_CP)
                    .Replace("<v_Ville>", tes.v_Ville.Replace("'", "''"))
                    .Replace("<v_Adresse>", tes.v_Adresse.Replace("'", "''"))
                    .Replace("<v_Phone>", tes.v_Phone)
                    .Replace("<v_Mail>", tes.v_Mail)
                    .Replace("<v_SIRET>", tes.v_SIRET)
                    .Replace("<v_FRTVA>", tes.v_FRTVA)
                    .Replace("<v_CodeNAF>", tes.v_CodeNAF)
                    .Replace("<v_Fax>", tes.v_Fax)
                    .Replace("<Montant>", tes.Montant.GetValueOrDefault().ToString().Replace(',', '.'))
                    .Replace("<Description>", tes.Description.Replace("'", "''"))
                    .Replace("<Nclient>", tes.Nclient);

                foreach (var item in tes.TESreglement)
                {
                    cmd += @"INSERT INTO [dbo].[TESreglement] 
                           ([CustomerId],[DateTime],[Caisse],[TypePay],[Montant],[TESCustomerId]) 
                     VALUES('<CustomerId>',GETDATE(),'<Caisse>','<TypePay>',<Montant>,'<TESCustomerId>') ";

                    cmd = cmd
                        .Replace("<CustomerId>", item.CustomerId.ToString())
                        .Replace("<Caisse>", ClassGlobalVar.Name)
                        .Replace("<TypePay>", item.TypePay)
                        .Replace("<Montant>", item.Montant.GetValueOrDefault().ToString().Replace(',', '.'))
                        .Replace("<TESCustomerId>", item.TESCustomerId.ToString());
                }
                foreach (var item in tes.TESproducts)
                {
                    cmd += @"INSERT INTO [dbo].[TESproducts]
           ([TypeID]
           ,[Date]
           ,[CustomerIdProduct]
           ,[NameProduct]
           ,[CodeBar]
           ,[Balance]
           ,[PrixHT]
           ,[QTY]
           ,[TVA]
           ,[TotalHT]
           ,[Description]
           ,[ProductsWeb]
           ,[SubGroup]
           ,[Group]
           ,[TESCustomerId]
           ,[ConditionAchat])
     VALUES
           ('<TypeID>'
           ,'<Date>'
           ,'<CustomerIdProduct>'
           ,'<NameProduct>'
           ,'<CodeBar>'
           ,'<Balance>'
           ,<PrixHT>
           ,<QTY>
           ,<TVA>
           ,<TotalHT>
           ,'<Description>'
           ,'<ProductsWeb>'
           ,'<SubGroup>'
           ,'<Group>'
           ,'<TESCustomerId>'
           ,<ConditionAchat>) ";

                    cmd = cmd.Replace("<TypeID>", item.TypeID.ToString())
                           .Replace("<Date>", item.Date.ToString())
                           .Replace("<CustomerIdProduct>", item.CustomerIdProduct.ToString())
                           .Replace("<NameProduct>", item.NameProduct.Replace("'", "''"))
                           .Replace("<CodeBar>", item.CodeBar)
                           .Replace("<Balance>", item.Balance.ToString())
                           .Replace("<PrixHT>", item.PrixHT.ToString("0.00").Replace(',', '.'))
                           .Replace("<QTY>", item.QTY.ToString("0.000").Replace(',', '.'))
                           .Replace("<TVA>", item.TVA.ToString("0.00").Replace(',', '.'))
                           .Replace("<TotalHT>", item.TotalHT.ToString("0.00").Replace(',', '.'))
                           .Replace("<Description>", item.Description.Replace("'", "''"))
                           .Replace("<ProductsWeb>", item.ProductsWeb.ToString())
                           .Replace("<SubGroup>", item.SubGroup)
                           .Replace("<Group>", item.Group)
                           .Replace("<TESCustomerId>", item.TESCustomerId.ToString())
                           .Replace("<ConditionAchat>", item.ConditionAchat.GetValueOrDefault().ToString("0.00").Replace(',', '.'));
                }



                return new ClassDB(null).queryNonResonse(cmd);
            }

            public System.Guid CustomerId { get; set; }
            public Nullable<int> Id { get; set; }
            public int Type { get; set; }
            public System.DateTime DateTime { get; set; }
            public bool Payement { get; set; }
            public bool Livraison { get; set; }
            public Nullable<short> a_CodeFournisseur { get; set; }
            public Nullable<int> a_Sex { get; set; }
            public string a_Name { get; set; }
            public string a_Surname { get; set; }
            public string a_NameCompany { get; set; }
            public string a_SIRET { get; set; }
            public string a_FRTVA { get; set; }
            public string a_OfficeAddress { get; set; }
            public string a_OfficeZipCode { get; set; }
            public string a_OfficeCity { get; set; }
            public string a_Telephone { get; set; }
            public string a_Mail { get; set; }
            public string v_NameCompany { get; set; }
            public string v_CP { get; set; }
            public string v_Ville { get; set; }
            public string v_Adresse { get; set; }
            public string v_Phone { get; set; }
            public string v_Mail { get; set; }
            public string v_SIRET { get; set; }
            public string v_FRTVA { get; set; }
            public string v_CodeNAF { get; set; }
            public string v_Fax { get; set; }
            public Nullable<decimal> Montant { get; set; }
            public string Description { get; set; }
            public string Nclient { get; set; }

            public virtual ICollection<TESproducts> TESproducts { get; set; }
            public virtual ICollection<TESreglement> TESreglement { get; set; }
        }
        public partial class TESproducts
        {
            public int CustomerId { get; set; }
            public int TypeID { get; set; }
            public System.DateTime Date { get; set; }
            public System.Guid CustomerIdProduct { get; set; }
            public string NameProduct { get; set; }
            public string CodeBar { get; set; }
            public bool Balance { get; set; }
            public decimal PrixHT { get; set; }
            public decimal QTY { get; set; }
            public decimal TVA { get; set; }
            public decimal TotalHT { get; set; }
            public string Description { get; set; }
            public System.Guid ProductsWeb { get; set; }
            public string SubGroup { get; set; }
            public string Group { get; set; }
            public System.Guid TESCustomerId { get; set; }
            public Nullable<decimal> ConditionAchat { get; set; }

            public virtual TES TES { get; set; }
        }
        public partial class TESreglement
        {
            public class SelectListItem
            {
                public SelectListItem(string Text, string Value)
                {
                    this.Text = Text;

                    this.Value = Value;
                }
                public string Text { get; set; }
                public string Value { get; set; }
            }

            public System.Guid CustomerId { get; set; }
            public System.DateTime DateTime { get; set; }
            public string Caisse { get; set; }
            public string TypePay { get; set; }
            public IEnumerable<SelectListItem> TypePayItems
            {
                get
                {
                    List<SelectListItem> I =
                        new List<SelectListItem>()
                        {
                        new SelectListItem( "BankCards", "Carte bancaire" ),
                        new SelectListItem( "BankChecks", "Cheque" ),
                        new SelectListItem( "Cash","En espèces" ),
                        new SelectListItem( "Resto", "None exist" ),
                        new SelectListItem( "BondAchat", "Avoir" )

                        };
                    return I;
                }
            }
            public Nullable<decimal> Montant { get; set; }
            public System.Guid TESCustomerId { get; set; }

            public virtual TES TES { get; set; }
        }

        public partial class DevisId
        {
            public string ins(DevisId di)
            {
                string cmd = @"INSERT INTO [DevisId]
           ([Date]
           ,[Close]
           ,[infoClientsCustomerId]
           ,[Total])
     VALUES
           ('<Date>'
           ,'<Close>'
           ,'<infoClientsCustomerId>'
           ,<Total>)";

                return cmd
                    .Replace("<Date>", di.Date.Value.ToString())
                    .Replace("<Close>", di.Close.Value.ToString())
                    .Replace("<infoClientsCustomerId>", di.infoClientsCustomerId.Value.ToString())
                    .Replace("<Total>", di.Total.Value.ToString().Replace(',', '.'));
            }
            public int getMaxId()
            {
                return (int)new ClassDB(null).queryResonse("SELECT MAX(Id) FROM [DevisId]")[0][0];
            }
            public int Id { get; set; }
            public Nullable<System.DateTime> Date { get; set; }
            public Nullable<bool> Close { get; set; }
            public Nullable<System.Guid> infoClientsCustomerId { get; set; }
            public Nullable<decimal> Total { get; set; }

            public virtual ICollection<DevisWeb> divisWeb { get; set; }
        }
        public partial class DevisWeb
        {

            public string ins(DevisWeb dw)
            {
                string cmd = @"INSERT INTO [DevisWeb]
           ([CustomerId]
           ,[IdDevis]
           ,[PrixHT]
           ,[MonPrixHT]
           ,[QTY]
           ,[TotalHT]
           ,[PayementType]
           ,[Operator]
           ,[ProductsCustumerId]
           ,[InfoClients_custumerId])
     VALUES
           ('<CustomerId>'
           ,<IdDevis>
           ,<PrixHT>
           ,<MonPrixHT>
           ,<QTY>
           ,<TotalHT>
           ,<PayementType>
           ,'<Operator>'
           ,'<ProductsCustumerId>'
           ,'<InfoClients_custumerId>')";

                return cmd
                    .Replace("<CustomerId>", dw.CustomerId.ToString())
                    .Replace("<IdDevis>", dw.IdDevis.ToString())
                    .Replace("<PrixHT>", dw.PrixHT.ToString().Replace(',', '.'))
                    .Replace("<MonPrixHT>", dw.MonPrixHT.ToString().Replace(',', '.'))
                    .Replace("<QTY>", dw.QTY.ToString().Replace(',', '.'))
                    .Replace("<TotalHT>", dw.TotalHT.ToString().Replace(',', '.'))
                    .Replace("<PayementType>", dw.PayementType.ToString())
                    .Replace("<Operator>", dw.Operator.ToString())
                    .Replace("<ProductsCustumerId>", dw.ProductsCustumerId.ToString())
                    .Replace("<InfoClients_custumerId>", dw.InfoClients_custumerId.ToString());
            }
            public System.Guid CustomerId { get; set; }
            public int IdDevis { get; set; }
            public decimal PrixHT { get; set; }
            public decimal MonPrixHT { get; set; }
            public decimal QTY { get; set; }
            public decimal TotalHT { get; set; }
            public short PayementType { get; set; }
            public bool Operator { get; set; }
            public System.Guid ProductsCustumerId { get; set; }
            public System.Guid InfoClients_custumerId { get; set; }
        }

        public static int InsDevis(DevisId di)
        {
            if (new ClassDB(null).queryNonResonse(new DevisId().ins(di)) == 1)
            {
                int id = new DevisId().getMaxId();

                ClassProMode.ndevis = id;

                string cmd = "";

                foreach (var ei in di.divisWeb)
                {
                    ei.IdDevis = id;

                    cmd += new DevisWeb().ins(ei) + Environment.NewLine;
                }

                if (new ClassDB(null).queryNonResonse(cmd) != di.divisWeb.Count)
                {
                    new ClassLog("error 903 [" + cmd + "]");
                }

                return di.divisWeb.Count + 1;
            }
            else
                return 0;
        }

        #endregion

        #region akcii
        public class ActionsCaisseDB
        {
            public partial class ActionsCaisse
            {
                public System.Guid CustomerId { get; set; }
                public string NameAction { get; set; }
                public Nullable<System.DateTime> Date { get; set; }
                public Nullable<System.DateTime> A { get; set; }
                public Nullable<System.DateTime> B { get; set; }
                public Nullable<System.Guid> Est { get; set; }
                public Nullable<bool> Enabled { get; set; }
                public string XML { get; set; }
            }

            public static List<ActionsCaisse> L = new List<ActionsCaisse>();

            public static void set()
            {
                string cmd = @"SELECT [CustomerId],[NameAction],[Date],[A],[B],[Est] ,[Enabled],[XML] FROM [ActionsCaisse] WHERE ([Enabled] = 1) AND ([Est] = '" + ClassGlobalVar.IdEstablishment +"') ";

                List<object[]> l = new ClassDB(null).queryResonse(cmd);

                foreach (var o in l)
                {
                    var a = new ActionsCaisse();

                    a.CustomerId = (Guid)o[0];

                    a.NameAction = (string)o[1];

                    a.Date = (DateTime)o[2];

                    a.A = (DateTime)o[3];

                    a.B = (DateTime)o[4];

                    a.Est = (Guid)o[5];

                    a.Enabled = (bool)o[6];

                    a.XML = (string)o[7];

                    L.Add(a);
                }
            }
        } 
        #endregion

        public void SyncAll()
        {

            connect = new ClassDB(null).connect();

            ClassGlobalVar.loadFromDB();

            ProductDB.set();

            General.set();

            OpenTicketWindow.set();

            TypesPayDB.sync();

            ClassXMLDB.loadFromDBAll();

            new ClassGridGroup();

            new ClassGridProduct();

            new ClassGridStatistique_Region_et_Pays();

            new ClassCheck();

            if (!connect)
            {
                ClassProducts.loadFronFile();
                ClassGlobalVar.open = true;
                ClassCheck.openTicket();

            }
            if (connect)
            {
                if (TypesPayDB.t.Count == 0)
                {
                    new ClassLog("Tableau des modes de paiement est vide");

                    ClassGlobalVar.open = false;
                }
                Currency.sync();


                if (Currency.List_Currency.Count == 0)
                {
                    new ClassLog("Tableau des billets de monnai est vide");

                    ClassGlobalVar.open = false;
                }
                if (ClassDataTimeSrv.getDateTimeFromSrv())
                {

                    string text = "L'heure du serveur : " + ClassDataTimeSrv.dateTimeFromSrv.ToString() + Environment.NewLine +
                                  "L'heure du caisse : " + DateTime.Now + Environment.NewLine;

                    W_dateTimeSrv wdsn = new W_dateTimeSrv(text);
                    //   wdsn.Topmost = true;
                    wdsn.ShowDialog();
                }


                if (!ClassGlobalVar.Bureau)
                {

                    if (General.table.Find(
                        l => l.Establishment == ClassGlobalVar.IdEstablishment).Date.ToShortDateString()
                        != (DateTime.Now).ToShortDateString()
                        && General.table.Find(l => l.Establishment == ClassGlobalVar.IdEstablishment).TicketWindowGeneral != Guid.Empty
                        )
                    {
                        string errorlist = "Maintenant : " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "--------------------------------" + Environment.NewLine;

                        errorlist += " Ouverture Générale : " + ticketwindow.Class.ClassSync.General.table.First().Date.ToLongDateString() + Environment.NewLine;

                        errorlist += "Ouverture Local " + ticketwindow.Class.ClassGlobalVar.nameTicket + " : " + ticketwindow.Class.ClassSync.OpenTicketWindow.table.Find(l => l.custumerId == ticketwindow.Class.ClassGlobalVar.CustumerId).dateOpen.ToLongDateString() + Environment.NewLine;

                        W_Close_TicketWindow wct = new W_Close_TicketWindow(errorlist);

                        //  wct.Topmost = true;

                        wct.ShowDialog();

                        new ClassCheck();

                        General.set();

                        OpenTicketWindow.set();

                    }


                }

                if (!ClassGlobalVar.Bureau)
                {


                    if (!ClassGlobalVar.break_)
                    {
                        if (!General.open)
                        {
                            string status = Environment.NewLine + "--------------------------------" + Environment.NewLine +
                                "Caisse : " + ClassGlobalVar.nameTicket + Environment.NewLine +
                                "Post : " + ClassGlobalVar.numberTicket + Environment.NewLine +
                                "Nom d'usager : " + ClassGlobalVar.user + Environment.NewLine + Environment.NewLine +
                                "--------------------------------" + Environment.NewLine +
                                "La clé d'ouverture générale : " + ClassGlobalVar.TicketWindowG + Environment.NewLine +
                                "La clé d'ouverture local : " + ClassGlobalVar.TicketWindow + Environment.NewLine;

                            var w = new W_open_ticlet_g(status);

                            w.ShowDialog();
                        }
                        if (ClassGlobalVar.TicketWindowG != Guid.Empty)
                        {
                            if (!OpenTicketWindow.open)
                            {
                                string status = Environment.NewLine + "--------------------------------" + Environment.NewLine +
                                    "Caisse : " + ClassGlobalVar.nameTicket + Environment.NewLine +
                                    "Post : " + ClassGlobalVar.numberTicket + Environment.NewLine +
                                    "Nom d'usager : " + ClassGlobalVar.user + Environment.NewLine + Environment.NewLine +
                                    "--------------------------------" + Environment.NewLine +
                                    "La clé d'ouverture générale : " + ClassGlobalVar.TicketWindowG + Environment.NewLine +
                                    "La clé d'ouverture local : " + ClassGlobalVar.TicketWindow + Environment.NewLine;
                                var w = new W_open_ticket(status);

                                w.ShowDialog();

                            }
                        }
                    }
                }
            }

            ClassDotLiquid.setPath(0);

            ClassDotLiquid.setPath(1);

            ClassCheck.loadFromFile();
        }


    }
}
