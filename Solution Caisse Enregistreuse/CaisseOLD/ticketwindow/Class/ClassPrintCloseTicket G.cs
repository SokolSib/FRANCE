using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    class ClassPrintCloseTicketG
    {
        public class print
        {
            const int size = 20;
            class mTva
            {
                public ClassTVA.tva tva { get; set; }

                public decimal HT { get; set; }
                public decimal TVA { get; set; }
                public decimal TTC { get; set; }

            }
            struct mTotal
            {
                public string name { get; set; }
                public int count { get; set; }
                public decimal sr_total { get; set; }
                public decimal procent { get; set; }

                public decimal total { get; set; }
            }
            public string nameTicketWindow { get; set; }
            public class elm
            {
                public string text { get; set; }
                public Rectangle rectangle { get; set; }
                public Font font = new Font("Arial", 9);
                public Brush brush = new SolidBrush(Color.Black);
                public StringFormat stringFormat = new StringFormat();
            }

            public List<elm> E = new List<elm>();

            private List<ClassCheck.localTypesPay> getTypePay(ClassSync.ClassCloseTicket.CloseTicket e)
            {

                List<ClassCheck.localTypesPay> typePay = new List<ClassCheck.localTypesPay>();
                string st = "";

                if (e.PayCash > 0)
                {
                    st = "Cash";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayCash);
                    typePay.Add(ltp);
                }



                if (e.PayBankCards > 0)
                {
                    st = "BankCards";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayBankCards);
                    typePay.Add(ltp);
                }

                if (e.PayBankChecks > 0)
                {
                    st = "BankChecks";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayBankChecks);
                    typePay.Add(ltp);
                }

                if (e.PayResto > 0)
                {
                    st = "Resto";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayResto);
                    typePay.Add(ltp);
                }

                if (e.Pay1 > 0)
                {
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    st = "1";
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay1);
                    typePay.Add(ltp);
                }
                if (e.Pay2 > 0)
                {
                    st = "2";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay2);
                    typePay.Add(ltp);
                }
                if (e.Pay3 > 0)
                {
                    st = "3";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay3);
                    typePay.Add(ltp);
                }
                if (e.Pay4 > 0)
                {
                    st = "4";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay4);
                    typePay.Add(ltp);
                }
                if (e.Pay5 > 0)
                {
                    st = "5";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay5);
                    typePay.Add(ltp);
                }
                if (e.Pay6 > 0)
                {
                    st = "6";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay6);
                    typePay.Add(ltp);
                }
                if (e.Pay7 > 0)
                {
                    st = "7";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay7);
                    typePay.Add(ltp);
                }
                if (e.Pay8 > 0)
                {
                    st = "8";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay8);
                    typePay.Add(ltp);
                }

                if (e.Pay9 > 0)
                {
                    st = "9";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay9);
                    typePay.Add(ltp);
                }
                if (e.Pay10 > 0)
                {
                    st = "10";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay10);
                    typePay.Add(ltp);
                }
                if (e.Pay11 > 0)
                {
                    st = "11";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay11);
                    typePay.Add(ltp);
                }
                if (e.Pay12 > 0)
                {
                    st = "12";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay12);
                    typePay.Add(ltp);
                }
                if (e.Pay13 > 0)
                {
                    st = "13";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay13);
                    typePay.Add(ltp);
                }
                if (e.Pay14 > 0)
                {
                    st = "14";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay14);
                    typePay.Add(ltp);
                }
                if (e.Pay15 > 0)
                {
                    st = "15";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay15);
                    typePay.Add(ltp);
                }
                if (e.Pay16 > 0)
                {
                    st = "16";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay16);
                    typePay.Add(ltp);
                }
                if (e.Pay17 > 0)
                {
                    st = "17";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay17);
                    typePay.Add(ltp);
                }
                if (e.Pay18 > 0)
                {
                    st = "18";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay18);
                    typePay.Add(ltp);
                }
                if (e.Pay19 > 0)
                {
                    st = "19";
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay19);
                    typePay.Add(ltp);
                }
                if (e.Pay20 > 0)
                {
                    st = "20";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay20);
                    typePay.Add(ltp);
                }
                return typePay;
            }

            private List<ClassCheck.localTypesPay> getTypePay(ClassSync.ClassCloseTicket.CloseTicketG e)
            {

                List<ClassCheck.localTypesPay> typePay = new List<ClassCheck.localTypesPay>();
                string st = "";

                if (e.PayCash > 0)
                {
                    st = "Cash";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayCash ?? 0.0m);
                    typePay.Add(ltp);
                }



                if (e.PayBankCards > 0)
                {
                    st = "BankCards";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayBankCards ?? 0.0m);
                    typePay.Add(ltp);
                }

                if (e.PayBankChecks > 0)
                {
                    st = "BankChecks";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayBankChecks ?? 0.0m);
                    typePay.Add(ltp);
                }

                if (e.PayResto > 0)
                {
                    st = "Resto";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.PayResto ?? 0.0m);
                    typePay.Add(ltp);
                }

                if (e.Pay1 > 0)
                {
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    st = "1";
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay1 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay2 > 0)
                {
                    st = "2";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay2 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay3 > 0)
                {
                    st = "3";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay3 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay4 > 0)
                {
                    st = "4";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay4 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay5 > 0)
                {
                    st = "5";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay5 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay6 > 0)
                {
                    st = "6";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay6 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay7 > 0)
                {
                    st = "7";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay7 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay8 > 0)
                {
                    st = "8";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay8 ?? 0.0m);
                    typePay.Add(ltp);
                }

                if (e.Pay9 > 0)
                {
                    st = "9";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay9 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay10 > 0)
                {
                    st = "10";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay10 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay11 > 0)
                {
                    st = "11";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay11 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay12 > 0)
                {
                    st = "12";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay12 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay13 > 0)
                {
                    st = "13";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay13 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay14 > 0)
                {
                    st = "14";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay14 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay15 > 0)
                {
                    st = "15";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay15 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay16 > 0)
                {
                    st = "16";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay16 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay17 > 0)
                {
                    st = "17";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay17 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay18 > 0)
                {
                    st = "18";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay18 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay19 > 0)
                {
                    st = "19";
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay19 ?? 0.0m);
                    typePay.Add(ltp);
                }
                if (e.Pay20 > 0)
                {
                    st = "20";
                    ClassSync.TypesPayDB tp = new ClassSync.TypesPayDB();
                    ClassSync.TypesPayDB t = ClassSync.TypesPayDB.t.Find(l => l.NameCourt == st);
                    ClassCheck.localTypesPay ltp = new ClassCheck.localTypesPay(t, e.Pay20 ?? 0.0m);
                    typePay.Add(ltp);
                }
                return typePay;
            }

            private List<mTva> getTVA(ClassSync.ClassCloseTicket.CloseTicket c)
            {
                List<mTva> tva = new List<mTva>();
                foreach (ClassTVA.tva tv in ClassTVA.listTVA)
                {
                    mTva mtva = new mTva();

                    mtva.tva = tv;

                    mtva.HT = 0;

                    mtva.TVA = 0;

                    mtva.TTC = 0;                    

                    foreach (ClassSync.ClassCloseTicket.ChecksTicket check in c.ChecksTicket)
                    {
                        foreach (ClassSync.ClassCloseTicket.PayProducts product in check.PayProducts)
                        {
                            if (product.TVA == tv.val)
                            {
                                //[13:05:34 | Изменены 13:06:00] Rena: HT = TTC - (TTC/(100+TVA)*TVA)
                                //                                      HT = TTC / (1 + TVA/100)


                               // coef = decimal.Round(100 / (product.TVA + 100), 3) * product.Total;

                                //decimal ht = decimal.Round((product.Total - (product.Total / (product.TVA + 100) * product.TVA)), 2);

                              //Rinat  decimal koef = (decimal.Truncate(100 / (product.TVA + 100) * 1000))/1000;

                              //Rinat  decimal ht = decimal.Round( koef * product.Total, 2);

                                decimal ht =(product.Total / (100 + product.TVA)) * 100;

                                mtva.HT += ht;
                                mtva.TTC += product.Total;
                                mtva.TVA += (product.Total / (100 + product.TVA) * product.TVA);

                            }
                        }
                    }

                    tva.Add(mtva);
                }

                return tva;
            }



            public print(ClassSync.ClassCloseTicket.CloseTicketG G, List<ClassSync.ClassCloseTicket.CloseTicket> C)
            {
                DateTime dt = G.DateClose;

                string date = dt.ToShortDateString();

                string time = dt.ToShortTimeString();

                int X = 0;
                int Y = 134;
                int H = size;
                int W = 280;
                int sizeLine = 12;
                elm n = new elm();
                n.rectangle = new Rectangle(X, Y, W, H);
                n.stringFormat.Alignment = StringAlignment.Center;
                n.stringFormat.LineAlignment = StringAlignment.Near;
                n.text = "Date : " + date + "  -  Heure : " + time;
                E.Add(n);
                Y += H;
             
                List<mTotal> M = new List<mTotal>();

                List<List<mTva>> TotalCaseTVA = new List<List<mTva>>();

                foreach (var e in C.OrderBy(l => l.NameTicket))
                {

                    elm nl1 = new elm();
                    nl1.font = new Font("Arial", 12, FontStyle.Bold);
                    nl1.rectangle = new Rectangle(X, Y + 15, W, H);
                    nl1.stringFormat.Alignment = StringAlignment.Center;
                    nl1.text = e.NameTicket.ToUpper();
                    E.Add(nl1);
                    Y += H + 15;

                    elm nlzv = new elm();
                    nlzv.rectangle = new Rectangle(X, Y, 300, H);
                    nlzv.text = "**********************************************************";
                    E.Add(nlzv);
                    Y += H;

                    elm nl2 = new elm();
                    nl2.rectangle = new Rectangle(X, Y - 10, W, H);
                    nl2.stringFormat.Alignment = StringAlignment.Center;
                    nl2.text = e.DateClose.ToShortDateString();
                    E.Add(nl2);
                    Y += H;

                    List<ClassCheck.localTypesPay> LTP = getTypePay(e);

                    decimal sumMoney = LTP.Sum(l => l.value);

                    foreach (var l in LTP)
                    {
                        elm eNamePay = new elm();
                        eNamePay.rectangle = new Rectangle(X, Y, 120, H);
                        eNamePay.stringFormat.Alignment = StringAlignment.Near;
                        eNamePay.text = l.type.Name;
                        E.Add(eNamePay);

                        elm eValTTC = new elm();
                        eValTTC.rectangle = new Rectangle(X + 120, Y, 80, H);
                        eValTTC.stringFormat.Alignment = StringAlignment.Far;
                        eValTTC.text = l.value.ToString("C");
                        E.Add(eValTTC);

                        elm eProc = new elm();
                        eProc.rectangle = new Rectangle(X + 200, Y, 80, H);
                        eProc.stringFormat.Alignment = StringAlignment.Far;
                        eProc.text = (l.value / sumMoney).ToString("P");
                        E.Add(eProc);
                        Y += H;

                    }


                    elm nl4 = new elm();
                    nl4.font = new Font("Arial", 9, FontStyle.Bold);
                    nl4.rectangle = new Rectangle(X, Y, 120, H);
                    nl4.stringFormat.Alignment = StringAlignment.Near;
                    nl4.text = "TOTAL";
                    E.Add(nl4);

                    elm nsumMoney = new elm();
                    nsumMoney.font = new Font("Arial", 9, FontStyle.Bold);
                    nsumMoney.rectangle = new Rectangle(X + 120, Y, 80, H);
                    nsumMoney.stringFormat.Alignment = StringAlignment.Far;
                    nsumMoney.text = sumMoney.ToString("C");
                    E.Add(nsumMoney);

                    elm nsumProc = new elm();
                    nsumProc.font = new Font("Arial", 9, FontStyle.Bold);
                    nsumProc.rectangle = new Rectangle(X + 200, Y, 80, H);
                    nsumProc.stringFormat.Alignment = StringAlignment.Far;
                    nsumProc.text = 1.ToString("P");
                    E.Add(nsumProc);
                    Y += H;                   

                    elm TiTVA = new elm();
                    TiTVA.font = new Font("Arial", 10, FontStyle.Bold);
                    TiTVA.rectangle = new Rectangle(X, Y + 10, W, H);
                    TiTVA.stringFormat.Alignment = StringAlignment.Center;
                    TiTVA.text = "*** TVA ***";
                    E.Add(TiTVA);
                    Y += H + 10;

                    elm nl5 = new elm();
                    nl5.font = new Font("Arial", 9);
                    nl5.rectangle = new Rectangle(X, Y, W, H);
                    nl5.stringFormat.Alignment = StringAlignment.Near;
                    nl5.stringFormat.LineAlignment = StringAlignment.Near;
                    nl5.text = "  TAUX               HT              TVA                 TTC";
                    E.Add(nl5);
                    Y += H;

                    
                    decimal HT = 0.0m;
                    decimal TVA = 0.0m;
                    decimal TTC = 0.0m;

                    List<mTva> CaseTva = getTVA(e);

                    TotalCaseTVA.Add(CaseTva);

                    foreach (mTva m in CaseTva)
                    {
                        elm eNameTVA = new elm();
                        eNameTVA.rectangle = new Rectangle(X, Y, 45, sizeLine);
                        eNameTVA.stringFormat.Alignment = StringAlignment.Far;
                        eNameTVA.stringFormat.LineAlignment = StringAlignment.Near;
                        eNameTVA.text = m.tva.val + "%";
                        E.Add(eNameTVA);

                        elm eHT = new elm();
                        eHT.rectangle = new Rectangle(X + 50, Y, 80, sizeLine);
                        eHT.stringFormat.Alignment = StringAlignment.Far;
                        eHT.stringFormat.LineAlignment = StringAlignment.Near;
                        eHT.text = m.HT.ToString("C");
                        E.Add(eHT);

                        elm eTVA = new elm();
                        eTVA.rectangle = new Rectangle(X + 130, Y, 70, sizeLine);
                        eTVA.stringFormat.Alignment = StringAlignment.Far;
                        eTVA.stringFormat.LineAlignment = StringAlignment.Near;
                        eTVA.text = m.TVA.ToString("C");
                        E.Add(eTVA);

                        elm eTTC = new elm();
                        eTTC.rectangle = new Rectangle(X + 200, Y, 80, sizeLine);
                        eTTC.stringFormat.Alignment = StringAlignment.Far;
                        eTTC.stringFormat.LineAlignment = StringAlignment.Near;
                        eTTC.text = m.TTC.ToString("C");
                        E.Add(eTTC);

                        Y += H;
                        HT += m.HT;
                        TVA += m.TVA;
                        TTC += m.TTC;

                    }

                    elm eTotal = new elm();
                    eTotal.font = new Font("Arial", 9, FontStyle.Bold);
                    eTotal.rectangle = new Rectangle(X, Y, 50, sizeLine);
                    eTotal.stringFormat.Alignment = StringAlignment.Near;
                    eTotal.stringFormat.LineAlignment = StringAlignment.Near;
                    eTotal.text = "TOTAL";
                    E.Add(eTotal);

                    elm eTht = new elm();
                    eTht.font = new Font("Arial", 9, FontStyle.Bold);
                    eTht.rectangle = new Rectangle(X + 50, Y, 80, sizeLine);
                    eTht.stringFormat.Alignment = StringAlignment.Far;
                    eTht.stringFormat.LineAlignment = StringAlignment.Near;
                    eTht.text = HT.ToString("C");
                    E.Add(eTht);

                    elm eTTVA = new elm();
                    eTTVA.font = new Font("Arial", 9, FontStyle.Bold);
                    eTTVA.rectangle = new Rectangle(X + 130, Y, 70, sizeLine);
                    eTTVA.stringFormat.Alignment = StringAlignment.Far;
                    eTTVA.stringFormat.LineAlignment = StringAlignment.Near;
                    eTTVA.text = TVA.ToString("C");
                    E.Add(eTTVA);

                    elm eTTTC = new elm();
                    eTTTC.font = new Font("Arial", 9, FontStyle.Bold);
                    eTTTC.rectangle = new Rectangle(X + 200, Y, 80, sizeLine);
                    eTTTC.stringFormat.Alignment = StringAlignment.Far;
                    eTTTC.stringFormat.LineAlignment = StringAlignment.Near;
                    eTTTC.text = TTC.ToString("C");
                    E.Add(eTTTC);
                    Y += H;

                    mTotal mt = new mTotal();
                    mt.count = decimal.ToInt32( e.ChecksTicket.Sum(l => l.PayProducts.Sum(la=>la.QTY)));
                    mt.sr_total = mt.count != 0 ? TTC / mt.count : 0;
                    mt.name = e.NameTicket;
                    mt.total = sumMoney;
                    M.Add(mt);


                }

                if (C.Count > 1)
                {
                    elm TGLOB = new elm();
                    TGLOB.font = new Font("Arial", 12, FontStyle.Bold);
                    TGLOB.rectangle = new Rectangle(X, Y + 15, W, H);
                    TGLOB.stringFormat.Alignment = StringAlignment.Center;
                    TGLOB.stringFormat.LineAlignment = StringAlignment.Near;
                    TGLOB.text = "TOTAL GLOBAL";
                    E.Add(TGLOB);
                    Y += H + 15;

                    elm nlzvzz = new elm();
                    nlzvzz.rectangle = new Rectangle(X, Y, 300, H);
                    nlzvzz.text = "**********************************************************";
                    E.Add(nlzvzz);
                    Y += H;


                    List<ClassCheck.localTypesPay> LTPG = getTypePay(G);

                    decimal sumMoneyG = LTPG.Sum(l => l.value);

                    foreach (var l in LTPG)
                    {
                        elm eNamePay = new elm();
                        eNamePay.rectangle = new Rectangle(X, Y, 120, H);
                        eNamePay.stringFormat.Alignment = StringAlignment.Near;
                        eNamePay.text = l.type.Name;
                        E.Add(eNamePay);

                        elm eValTTC = new elm();
                        eValTTC.rectangle = new Rectangle(X + 120, Y, 80, H);
                        eValTTC.stringFormat.Alignment = StringAlignment.Far;
                        eValTTC.text = l.value.ToString("C");
                        E.Add(eValTTC);

                        elm eProc = new elm();
                        eProc.rectangle = new Rectangle(X + 200, Y, 80, H);
                        eProc.stringFormat.Alignment = StringAlignment.Far;
                        eProc.text = (l.value / sumMoneyG).ToString("P");
                        E.Add(eProc);
                        Y += H;

                    }

                    elm nl4 = new elm();
                    nl4.font = new Font("Arial", 9, FontStyle.Bold);
                    nl4.rectangle = new Rectangle(X, Y, 120, H);
                    nl4.stringFormat.Alignment = StringAlignment.Near;
                    nl4.text = "TOTAL";
                    E.Add(nl4);

                    elm nsumMoney = new elm();
                    nsumMoney.font = new Font("Arial", 9, FontStyle.Bold);
                    nsumMoney.rectangle = new Rectangle(X + 120, Y, 80, H);
                    nsumMoney.stringFormat.Alignment = StringAlignment.Far;
                    nsumMoney.text = sumMoneyG.ToString("C");
                    E.Add(nsumMoney);

                    elm nProc100 = new elm();
                    nProc100.font = new Font("Arial", 9, FontStyle.Bold);
                    nProc100.rectangle = new Rectangle(X + 200, Y, 80, H);
                    nProc100.stringFormat.Alignment = StringAlignment.Far;
                    nProc100.text = 1.ToString("P");
                    E.Add(nProc100);
                    Y += H;

                    decimal GHT = 0.0m;
                    decimal GTVA = 0.0m;
                    decimal GTTC = 0.0m;

                    List<mTva> tvaS = new List<mTva>();

                    foreach (List<mTva> m in TotalCaseTVA)
                    {
                        foreach (mTva ntva in m)
                        {
                            int indx = tvaS.FindIndex(l => l.tva == ntva.tva);

                            if (indx == -1)
                            {
                                mTva j = new mTva();

                                j.HT = ntva.HT;

                                j.TTC = ntva.TTC;

                                j.tva = ntva.tva;

                                j.TVA = ntva.TVA;

                                tvaS.Add(j);
                            }
                            else
                            {
                                tvaS[indx].HT += ntva.HT;
                                tvaS[indx].TTC += ntva.TTC;
                                tvaS[indx].TVA += ntva.TVA;
                            }
                        }
                    }

                    elm TiTVA = new elm();
                    TiTVA.font = new Font("Arial", 10, FontStyle.Bold);
                    TiTVA.rectangle = new Rectangle(X, Y + 10, W, H);
                    TiTVA.stringFormat.Alignment = StringAlignment.Center;
                    TiTVA.text = "*** TVA ***";
                    E.Add(TiTVA);
                    Y += H + 10;

                    elm nl5hed = new elm();
                    nl5hed.font = new Font("Arial", 9);
                    nl5hed.rectangle = new Rectangle(X, Y, W, H);
                    nl5hed.stringFormat.Alignment = StringAlignment.Near;
                    nl5hed.stringFormat.LineAlignment = StringAlignment.Near;
                    nl5hed.text = "  TAUX               HT              TVA                 TTC";
                    E.Add(nl5hed);
                    Y += H;

                    foreach (mTva m in tvaS)
                    {
                        elm eNameTVA = new elm();
                        eNameTVA.rectangle = new Rectangle(X, Y, 45, sizeLine);
                        eNameTVA.stringFormat.Alignment = StringAlignment.Far;
                        eNameTVA.stringFormat.LineAlignment = StringAlignment.Near;
                        eNameTVA.text = m.tva.val + "%";
                        E.Add(eNameTVA);

                        elm eHT = new elm();
                        eHT.rectangle = new Rectangle(X + 50, Y, 80, sizeLine);
                        eHT.stringFormat.Alignment = StringAlignment.Far;
                        eHT.stringFormat.LineAlignment = StringAlignment.Near;
                        eHT.text = m.HT.ToString("C");
                        E.Add(eHT);

                        elm eTVA = new elm();
                        eTVA.rectangle = new Rectangle(X + 130, Y, 70, sizeLine);
                        eTVA.stringFormat.Alignment = StringAlignment.Far;
                        eTVA.stringFormat.LineAlignment = StringAlignment.Near;
                        eTVA.text = m.TVA.ToString("C");
                        E.Add(eTVA);

                        elm eTTC = new elm();
                        eTTC.rectangle = new Rectangle(X + 200, Y, 80, sizeLine);
                        eTTC.stringFormat.Alignment = StringAlignment.Far;
                        eTTC.stringFormat.LineAlignment = StringAlignment.Near;
                        eTTC.text = m.TTC.ToString("C");
                        E.Add(eTTC);

                        Y += H;
                        GHT += m.HT;
                        GTVA += m.TVA;
                        GTTC += m.TTC;
                    }

                    elm eTotal = new elm();
                    eTotal.font = new Font("Arial", 9, FontStyle.Bold);
                    eTotal.rectangle = new Rectangle(X, Y, 50, sizeLine);
                    eTotal.stringFormat.Alignment = StringAlignment.Near;
                    eTotal.stringFormat.LineAlignment = StringAlignment.Near;
                    eTotal.text = "TOTAL";
                    E.Add(eTotal);

                    elm eTht = new elm();
                    eTht.font = new Font("Arial", 9, FontStyle.Bold);
                    eTht.rectangle = new Rectangle(X + 50, Y, 80, sizeLine);
                    eTht.stringFormat.Alignment = StringAlignment.Far;
                    eTht.stringFormat.LineAlignment = StringAlignment.Near;
                    eTht.text = GHT.ToString("C");
                    E.Add(eTht);

                    elm eTTVA = new elm();
                    eTTVA.font = new Font("Arial", 9, FontStyle.Bold);
                    eTTVA.rectangle = new Rectangle(X + 130, Y, 70, sizeLine);
                    eTTVA.stringFormat.Alignment = StringAlignment.Far;
                    eTTVA.stringFormat.LineAlignment = StringAlignment.Near;
                    eTTVA.text = GTVA.ToString("C");
                    E.Add(eTTVA);

                    elm eTTTC = new elm();
                    eTTTC.font = new Font("Arial", 9, FontStyle.Bold);
                    eTTTC.rectangle = new Rectangle(X + 200, Y, 80, sizeLine);
                    eTTTC.stringFormat.Alignment = StringAlignment.Far;
                    eTTTC.stringFormat.LineAlignment = StringAlignment.Near;
                    eTTTC.text = GTTC.ToString("C");
                    E.Add(eTTTC);

                }


                elm TSTAT = new elm();
                TSTAT.font = new Font("Arial", 12, FontStyle.Bold);
                TSTAT.rectangle = new Rectangle(X, Y + 15, W, H);
                TSTAT.stringFormat.Alignment = StringAlignment.Center;
                TSTAT.stringFormat.LineAlignment = StringAlignment.Near;
                TSTAT.text = "STATISTIQUES";
                E.Add(TSTAT);
                Y += H + 15;

                elm nlzvz = new elm();
                nlzvz.rectangle = new Rectangle(X, Y, 300, H);
                nlzvz.text = "**********************************************************";
                E.Add(nlzvz);
                Y += H;

                elm nl8 = new elm();
                nl8.font = new Font("Arial", 9);
                nl8.rectangle = new Rectangle(X, Y, W, H);
                nl8.stringFormat.Alignment = StringAlignment.Near;
                nl8.stringFormat.LineAlignment = StringAlignment.Near;
                nl8.text = "Description             QTY           Moyen             %";
                E.Add(nl8);
                Y += H;

                decimal sumM = M.Sum(l => l.total);

                foreach (mTotal m in M)
                {
                    elm nl9 = new elm();
                    nl9.rectangle = new Rectangle(X, Y, 60, H);
                    nl9.stringFormat.Alignment = StringAlignment.Near;
                    nl9.text = m.name;
                    E.Add(nl9);

                    elm nlcount = new elm();
                    nlcount.rectangle = new Rectangle(X + 60, Y, 80, H);
                    nlcount.stringFormat.Alignment = StringAlignment.Far;
                    nlcount.text = m.count.ToString();
                    E.Add(nlcount);

                    elm nlctt = new elm();
                    nlctt.rectangle = new Rectangle(X + 140, Y, 70, H);
                    nlctt.stringFormat.Alignment = StringAlignment.Far;
                    nlctt.text = m.sr_total.ToString("C");
                    E.Add(nlctt);

                    elm nlsrd = new elm();
                    nlsrd.rectangle = new Rectangle(X + 210, Y, 70, H);
                    nlsrd.stringFormat.Alignment = StringAlignment.Far;
                    nlsrd.text = (sumM == 0 ? 0 : (m.total / sumM)).ToString("P");
                    E.Add(nlsrd);
                                       
                    Y += H;

                }

                if (C.Count > 0)
                {
                    elm nl10 = new elm();
                    nl10.rectangle = new Rectangle(X, Y, 60, H);
                    nl10.stringFormat.Alignment = StringAlignment.Near;
                    nl10.text = "TOTAL";
                    E.Add(nl10);
                  

                    elm nl10Tot = new elm();
                    nl10Tot.rectangle = new Rectangle(X + 60, Y, 80, H);
                    nl10Tot.stringFormat.Alignment = StringAlignment.Far;
                    nl10Tot.text = M.Sum(l => l.count).ToString();
                    E.Add(nl10Tot);
                 

                    elm nl10mSum = new elm();
                    nl10mSum.rectangle = new Rectangle(X + 140, Y, 70, H);
                    nl10mSum.stringFormat.Alignment = StringAlignment.Far;
                    nl10mSum.text = Math.Round(M.Sum(l => l.sr_total), 2).ToString("C");
                    E.Add(nl10mSum);
                   

                    elm nl10_100 = new elm();
                    nl10_100.rectangle = new Rectangle(X + 210, Y, 70, H);
                    nl10_100.stringFormat.Alignment = StringAlignment.Far;
                    nl10_100.text = 1.ToString("P");
                    E.Add(nl10_100);
                    Y += H;

                }
                
                
                Y += H + 20;
            }


        }
        private void printPage(object o, PrintPageEventArgs e)
        {

            int x1 = 0;
            int y1 = 0;
            int W = 280;
            int H = 134;

            Rectangle FillRectangleImg = new Rectangle(x1, y1, W, H);


            e.Graphics.DrawImage((Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\anahit_9.jpg")), FillRectangleImg);
            foreach (var elm in pr.E)
                e.Graphics.DrawString(elm.text, elm.font, elm.brush, elm.rectangle, elm.stringFormat);
        }
        public print pr { get; set; }
        public ClassPrintCloseTicketG(ClassSync.ClassCloseTicket.CloseTicketG G, List<ClassSync.ClassCloseTicket.CloseTicket> C,
            String Head)
        {
            print p = new print(G, C);

            pr = p;

            PrintDocument pd = new PrintDocument();

            pd.PrintPage += printPage;

            pd.Print();
        }
    }
}
