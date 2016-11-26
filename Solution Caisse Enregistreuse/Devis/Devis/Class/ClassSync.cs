using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace Devis.Class
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
                new ClassDB(null).queryNonResonse(c);

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
                            // ClassProducts.modifAddOnlyFile(ClassProducts.DbToVar(p));


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
                        Products.L.Clear();
                        ProductsBC_.list.Clear();
                        StockReal.L.Clear();
                        TVA_.L.Clear();
                        SubGrpProduct.L.Clear();
                        GrpProduct.L.Clear();
                        infoClient.list.Clear();

                        ProductsBC_.set();
                        StockReal.set();
                        TVA_.set();
                        SubGrpProduct.set();
                        GrpProduct.set();
                        ClassGroupProduct.save();
                        ClassGroupProduct.loadFromFile();
                        ProductsWeb_.set();

                        Products.set();


                        infoClient.set();


                        ClassTVA.save();
                        ClassProducts.save();
                        ClassInfoClients.save();



                    }
                }
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
                public static StockReal addIsNull(Guid customerIdProduct)
                {
                    StockReal r = new StockReal();
                    r.CustomerId = Guid.NewGuid();
                    r.IdEstablishment = ClassGlobalVar.IdEstablishment;
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
            public partial class TVA_
            {
                public static List<TVA_> L = new List<TVA_>();
                public static void set()
                {
                    List<object[]> l = new ClassDB(null).queryResonse("SELECT * FROM TVA");
                    foreach (object[] o in l)
                    {
                        TVA_ t = new TVA_();
                        t.CustumerId = (Guid)o[0];
                        t.Id = (string)o[1];
                        t.val = (string)o[2];
                        L.Add(t);
                    }
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
            public partial class Products
            {
                private static void tstDubl(List<StockReal> lsr, Products p, out Products outp, bool syncAll)
                {
                    try
                    {
                        switch (lsr.Count)
                        {
                            case 0:


                                p.StockReal_.Add(StockReal.addIsNull(p.CustumerId));

                                break;
                            case 1: p.StockReal_ = lsr; break;
                            default:
                                p.StockReal_ = lsr;

                                string log = "Double ecriture dans base de données StockReal : " + Environment.NewLine + "Produit : " + p.Name + " (" + p.CodeBare + ")" + Environment.NewLine;
                                bool first = true;
                                foreach (var e in lsr)
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
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM Products");



                    foreach (object[] s in S)
                    {
                        

                        Products p = new Products();

                        p.CustumerId = (Guid)s[0];

                        var t = TranslateNameProductsSet.get(p.CustumerId, "FR");

                        p.Name = t != null ? t.Name : (string)s[1];
                        p.CodeBare = (string)s[2];
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

                        p.SubGrpProduct = SubGrpProduct.L.Find(l => l.Id == (int)s[12]) ?? SubGrpProduct.L.First();

                        p.TVA = TVA_.L.Find(l => l.CustumerId == p.TVACustumerId);

                        List<StockReal> lsr = StockReal.L.FindAll(l => (l.ProductsCustumerId == p.CustumerId)
                            && (l.IdEstablishment == ClassGlobalVar.IdEstablishment)
                            );


                        tstDubl(lsr, p, out p, true);

                        p.ProductsWeb_ = ProductsWeb_.R.Find(l => l.CustomerId == p.ProductsWeb_CustomerId);
                        p.bc = new List<ProductsBC_>();
                        p.bc.AddRange(ProductsBC_.list.Where(l => l.CustomerIdProduct == p.CustumerId));
                        L.Add(p);


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
                    string cmd = "SELECT * FROM Products WHERE (" + "[Date] >=  CAST('{DATETIME_A}' AS DateTime) AND [Date] <= CAST('{DATETIME_B}' AS DateTime))";

                    string c = cmd.Replace("{DATETIME_A}", a.ToString()).Replace("{DATETIME_B}", b.ToString());

                    List<object[]> S = new ClassDB(null).queryResonse(c);

                    List<Products> R = new List<Products>();

                    foreach (object[] s in S)
                    {
                        Products p = new Products();
                        p.CustumerId = (Guid)s[0];
                        p.Name = (string)s[1];
                        p.CodeBare = (string)s[2];
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

                public System.Guid ProductsWeb_CustomerId { get; set; }
                public virtual SubGrpProduct SubGrpProduct { get; set; }
                public virtual TVA_ TVA { get; set; }
                public virtual ICollection<StockReal> StockReal_ { get; set; }
                public virtual ProductsWeb_ ProductsWeb_ { get; set; }

                public virtual List<ProductsBC_> bc { get; set; }
            }
            public partial class ProductsWeb_
            {
                public static List<ProductsWeb_> R = new List<ProductsWeb_>();
                public static void set()
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM ProductsWeb");


                    foreach (object[] s in S)
                    {

                        ProductsWeb_ p = new ProductsWeb_();
                        p.CustomerId = (Guid)s[0];
                        p.Visible = (bool)((s[1] is DBNull ? false : s[1]));
                        p.Images = s[2].ToString();
                        p.DescrCourt = s[3].ToString();
                        p.DescrLong = s[4].ToString();
                        p.Volume = (decimal)(s[5] is DBNull ? 0.0m : s[5]);
                        p.UniteVolume = (int)(s[6] is DBNull ? 0 : s[6]);
                        p.ContenanceBox = (decimal)(s[7] is DBNull ? 0.0m : s[7]);
                        p.ContenancePallet = (decimal)(s[8] is DBNull ? 0.0m : s[8]);
                        p.Weight = (decimal)(s[9] is DBNull ? 0.0m : s[9]);
                        p.Frozen = (bool)(s[10] is DBNull ? false : s[10]);

                        R.Add(p);

                    }

                }
                public System.Guid CustomerId { get; set; }
                public bool Visible { get; set; }
                public string Images { get; set; }
                public string DescrCourt { get; set; }
                public string DescrLong { get; set; }
                public decimal Volume { get; set; }
                public int UniteVolume { get; set; }
                public decimal ContenanceBox { get; set; }
                public decimal ContenancePallet { get; set; }
                public decimal Weight { get; set; }
                public bool Frozen { get; set; }

                public virtual ICollection<Products> Products { get; set; }
            }

            public partial class ProductsBC_
            {
                public System.Guid CustomerId { get; set; }
                public System.Guid CustomerIdProduct { get; set; }
                public string CodeBar { get; set; }
                public decimal QTY { get; set; }
                public string Description { get; set; }

                public static List<ProductsBC_> list = new List<ProductsBC_>();

                public static void set()
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM ProductsBC");


                    foreach (object[] s in S)
                    {

                        ProductsBC_ p = new ProductsBC_();
                        p.CustomerId = (Guid)s[0];
                        p.CustomerIdProduct = (Guid)((s[1] is DBNull ? Guid.Empty : s[1]));
                        p.CodeBar = s[2].ToString();
                        p.QTY = (decimal)(s[3] is DBNull ? 1.0m : s[3]);
                        p.Description = s[4].ToString();
                        list.Add(p);

                    }
                }
            }

            public partial class TranslateNameProductsSet
            {
                public System.Guid CustomerId { get; set; }
                public System.Guid ProductCustomerId { get; set; }
                public string cod_lng { get; set; }
                public string Name { get; set; }
                public string Details { get; set; }

                public static List<TranslateNameProductsSet> list = new List<TranslateNameProductsSet>();

                public static void set()
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM TranslateNameProductsSet");


                    foreach (object[] s in S)
                    {

                        TranslateNameProductsSet t = new TranslateNameProductsSet();
                        t.CustomerId = (Guid)s[0];
                        t.ProductCustomerId = (Guid)((s[1] is DBNull ? Guid.Empty : s[1]));
                        t.cod_lng = s[2].ToString();
                        t.Name = s[3].ToString();
                        t.Details = s[4].ToString();
                        list.Add(t);

                    }
                }

                public static TranslateNameProductsSet get(Guid ProductCustomerId_, string cod_lng_)
                {
                    List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM TranslateNameProductsSet WHERE ProductCustomerId='" + ProductCustomerId_.ToString() + "' AND cod_lng=' " + cod_lng_ + "'");


                    List<TranslateNameProductsSet> l = new List<TranslateNameProductsSet>();

                    foreach (object[] s in S)
                    {

                        TranslateNameProductsSet t = new TranslateNameProductsSet();
                        t.CustomerId = (Guid)s[0];
                        t.ProductCustomerId = (Guid)((s[1] is DBNull ? Guid.Empty : s[1]));
                        t.cod_lng = s[2].ToString();
                        t.Name = s[3].ToString();
                        t.Details = s[4].ToString();
                        l.Add(t);

                    }

                    return l.FirstOrDefault();
                }
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

        public class infoClient
        {
            public partial class InfoClients
            {

                public System.Guid custumerId { get; set; }
                public int TypeClient { get; set; }
                public Nullable<int> Sex { get; set; }
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
                public System.Guid Countrys_CustomerId { get; set; }
                public System.Guid FavoritesProductAutoCustomerId { get; set; }
                public string Nclient { get; set; }


            }
            public static List<InfoClients> list = new List<InfoClients>();
            public static void set()
            {
                List<object[]> S = new ClassDB(null).queryResonse("SELECT * FROM InfoClients WHERE TypeClient ='2'");

                foreach (object[] s in S)
                {

                    InfoClients ic = new InfoClients();
                    ic.custumerId = (Guid)s[0];
                    ic.TypeClient = (int)s[1];
                    ic.Sex = (int)s[2];
                    ic.Name = s[3].ToString();
                    ic.Surname = s[4].ToString();
                    ic.NameCompany = s[5].ToString();
                    ic.SIRET = s[6].ToString();
                    ic.FRTVA = s[7].ToString();
                    ic.OfficeAddress = s[8].ToString();
                    ic.OfficeZipCode = s[9].ToString();
                    ic.OfficeCity = s[10].ToString();
                    ic.HomeAddress = s[11].ToString();
                    ic.HomeZipCode = s[12].ToString();
                    ic.HomeCity = s[13].ToString();
                    ic.Telephone = s[14].ToString();
                    ic.Mail = s[15].ToString();
                    ic.Password = s[16].ToString();
                    ic.Countrys_CustomerId = (Guid)s[17];
                    ic.FavoritesProductAutoCustomerId = (Guid)s[18];
                    ic.Nclient = s[19].ToString();
                    list.Add(ic);


                }
            }
        }
        public void SyncAll()
        {


            ClassProducts.loadFronFile();

            ClassInfoClients.loadFromFile();
        }


    }
}
