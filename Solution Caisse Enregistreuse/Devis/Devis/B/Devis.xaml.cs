using Devis.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Devis.B
{
    class ClassDevis
    {

        public void send(Guid infoClientsCustomerId, List<XElement> prod)
        {
            int maxId = ((int)new ClassDB(null).queryResonse("SELECT MAX(Id) FROM DevisId")[0][0]) + 1;

            if (maxId > 0)
            {
                ClassDB db = new ClassDB(null);

                string cmd1 = "INSERT INTO DevisId VALUES ( CAST('{Date}' AS DATETIME), {Close},'{infoClientsCustomerId}', {Total}) ";

                decimal Total = prod.Sum(l => decimal.Parse(l.Element("TOTAL").Value.Replace('.', ',')));



                cmd1 = cmd1
                    .Replace("{Id}", maxId.ToString())
                    .Replace("{Date}", DateTime.Now.ToString())
                    .Replace("{Close}", "0")
                    .Replace("{infoClientsCustomerId}", infoClientsCustomerId.ToString())
                    .Replace("{Total}", Total.ToString().Replace(",", "."));

                db.queryNonResonse(cmd1);

                maxId = ((int)new ClassDB(null).queryResonse("SELECT MAX(Id) FROM DevisId")[0][0]);

                string cmd2 = "INSERT INTO DevisWeb VALUES ( '{CustomerId}', {IdDevis}, {PrixHT}, {MonPrixHT}, {QTY}, {TotalHT}, {PayementType},{Operator},'{ProductsCustumerId}', '{InfoClients_custumerId}') ";

                foreach (XElement x in prod)
                {
                    string cmd_ = cmd2
                        .Replace("{CustomerId}", Guid.NewGuid().ToString())
                        .Replace("{IdDevis}", maxId.ToString())
                        .Replace("{PrixHT}", x.Element("price").Value.Replace(',', '.'))
                        .Replace("{MonPrixHT}", (decimal.Parse(x.Element("ContenanceBox").Value.Replace('.', ',')) == 0.0m ? 1 : decimal.Parse(x.Element("ContenanceBox").Value.Replace('.', ','))).ToString().Replace(",", "."))
                        .Replace("{QTY}", x.Element("QTY").Value.Replace(',', '.'))
                        .Replace("{TotalHT}", x.Element("TOTAL").Value.Replace(",", "."))
                        .Replace("{PayementType}", "-2")
                        .Replace("{Operator}", "1")

                        .Replace("{ProductsCustumerId}", x.Element("CustumerId").Value)
                        .Replace("{InfoClients_custumerId}", infoClientsCustomerId.ToString());
                    db.queryNonResonse(cmd_);

                    Class.ClassProducts.clrElm(new XElement[] { x });
                }

            }
        }

    }
    /// <summary>
    /// Interaction logic for Devis.xaml
    /// </summary>
    public partial class Devis : Window
    {
        public Devis()
        {
            InitializeComponent();

            Class.ClassInfoClients.loadFromFile();

            _ProGrid.DataContext = Class.ClassInfoClients.x.Element("tva").Elements("rec").OrderBy(l => l.Element("NameCompany").Value);

            pb.Visibility = Visibility.Hidden;
        }

        private System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();

        private void worker_DoWork_devis(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            XElement ic = (XElement)e.Argument;

            if (ic != null)
            {
                Guid customerId = Guid.Parse(ic.Element("custumerId").Value);

                string NameCo = ic.Element("NameCompany").Value;



                sendDevis(customerId, NameCo, Class.ClassProducts.x.Element("Product").Elements("rec").Where(l => decimal.Parse(l.Element("QTY").Value.ToString().Trim() == "" ? "0" : l.Element("QTY").Value.ToString().Trim().Replace(".", ",")) > 0).ToList());


            }

        }

        private void worker_RunWorkerCompleted_devis(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            MainWindow mw = Class.ClassF.findWindow("MainWindow_") as MainWindow;

            if (mw != null)
            {
                mw._ProductsGrid.Items.Refresh();
            }
            Close();
        }

     

        private void sendDevis(Guid infoClientsCustomerId, string NameCo, List<XElement> prod)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Data\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;

            System.IO.Directory.CreateDirectory(path);

            Class.ClassProducts.x.Save(path + "\\" + NameCo + ".xml");

            System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Product.xml", path + "\\" + NameCo +"_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + ".xml");

            new ClassDevis().send(infoClientsCustomerId, prod);

     /*       BDCAtestEntities bd = new BDCAtestEntities();

            DevisId devisId = new DevisId();

            devisId.Close = false;

            devisId.Date = DateTime.Now;

            int id = bd.DevisId.Max(l => l.Id) + 1;

            
            devisId.infoClientsCustomerId = infoClientsCustomerId;

            decimal totalHTSum = 0.0m;

            bd.DevisId.Add(devisId);

            foreach (XElement elm in prod)
            {

                DevisWeb dw = new DevisWeb();

                dw.CustomerId = Guid.NewGuid();

                dw.IdDevis = id;

                dw.InfoClients_custumerId = infoClientsCustomerId;

                dw.Operator = false;

                dw.ProductsCustumerId = Guid.Parse(elm.Element("CustumerId").Value);

                dw.PrixHT = decimal.Parse(elm.Element("price").Value.Replace('.', ','));

                dw.QTY = decimal.Parse(elm.Element("QTY").Value.Replace('.', ','));

                dw.TotalHT = decimal.Parse(elm.Element("TOTAL").Value.Replace('.', ','));

                dw.MonPrixHT = decimal.Parse(elm.Element("ContenanceBox").Value.Replace('.', ',')) == 0.0m ? 1 : decimal.Parse(elm.Element("ContenanceBox").Value.Replace('.', ','));

                dw.PayementType = -2;

                bd.DevisWeb.Add(dw);

                totalHTSum += dw.TotalHT;

                Class.ClassProducts.clrElm(new XElement[] { elm });

               
            }

            devisId.Total = totalHTSum;

            devisId.Id = id;


         

            bd.SaveChanges();


            int id_test =  bd.DevisId.Max(l => l.Id);

            if (id != id_test)
            {
            
                DevisId devis_id = bd.DevisId.FirstOrDefault(l => l.Id == id_test);

                if (devis_id != null)
                {
                    devis_id.Id = id;

                    bd.SaveChanges();

                    new Class.ClassLog("change devisId id_bd=" + id + " " + "id=" + id);
                }
            }
           */
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pb.Visibility = Visibility.Visible;

            bok.IsEnabled = false;

            bcancel.IsCancel = false;

            _ProGrid.IsEnabled = false;

            if (_ProGrid.SelectedItem != null)
            {
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork_devis);

                worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted_devis);

                worker.RunWorkerAsync(_ProGrid.SelectedItem);

            }
            else
            {
                var mes = new messageDlgTime();

                mes.message.Text = "select pro";

                mes.Show();
            }
        }
    }
}
