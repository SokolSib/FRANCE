using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ticketwindow.Class;
using ticketwindow.Winows.Loading;

namespace ticketwindow.Winows.Ballance
{
    /// <summary>
    /// Логика взаимодействия для W_Ballance.xaml
    /// </summary>
    public partial class W_Ballance : Window
    {
        private Class.ClassProducts.product p { get; set; }
        public object po;
        private decimal qty { get; set; }
    
        private void getBallance ( )
        {
            bool f = false;

            if (p.price == 0.0m)
            {
                p.price = 1;
                f = true;
            }
            Class.ClassBallance.send(p.price, p.tare);

            if (Class.ClassBallance._busy_0x15)
                new ClassFunctuon().showMessageTime("Pour résoudre ce problème, il vous suffit de re-peser l'article");
            if (Class.ClassBallance._error_0x15)
                new ClassFunctuon().showMessageTime("Pour résoudre ce problème, il vous suffit de redémarrer la balance!");
           
            decimal prix = 0.0m;
            try
            {
                prix = decimal.Parse(Class.ClassBallance.prix) / 100;
                qty = decimal.Parse(Class.ClassBallance.poinds) / 1000;
                xBallance_kg.Text = qty.ToString();
                xPrix_kg.Content = prix.ToString();
            }
            catch (Exception e)
            {
                xBallance_kg.Text = "0";
                xPrix_kg.Content = "0";

                new Class.ClassLog("Error ballance code 22");
                xLog.Content = e.Message + Environment.NewLine;
            }
            xLog.Content += Class.ClassBallance.error;
            try
            {
                if (!f)
                    xTotal_kg.Content = (Math.Round(decimal.Parse(Class.ClassBallance.montant) / 100, 2));
                else xTotal_kg.Content = "0.0";
            }
            catch
            {
                xTotal_kg.Content = "0";
            }
            if (qty > 0)
            {
                p.contenance = qty;
                p.price = prix;
            }
        }

        
        public W_Ballance(object arg)
        {  

            InitializeComponent();
            p = arg as Class.ClassProducts.product;
            xCodebar.Content = p.CodeBare.TrimEnd().TrimStart() == "" ? "not" : p.CodeBare;
            xName.Content = p.Name;
            xDescription.Content = p.Desc;
            xPrix.Text = p.price.ToString();
            xTVA.Content = Class.ClassTVA.getTVA(p.tva);
            xBallance.Content = p.balance;
            xContenance.Content = p.contenance;
            xUniteContenance.Content = p.uniteContenance;
            xTare.Content = p.tare;
           
            getBallance();

            po = p;
      
            this.numPad.textBox = xPrix;
            this.numPad.bEnter = bOk;

            this.numPad2.textBox = xBallance_kg;
            this.numPad2.bEnter = bOk;
        }

        private void bGet_Click(object sender, RoutedEventArgs e)
        {
            decimal newpice = 0.0m;
            if (decimal.TryParse(xPrix.Text.Replace(".", ","), out newpice))
            {
                p.price = newpice;

                getBallance(); // new Class.ClassFunctuon().Click(sender, p);

                po = p;
            }
            else
            {
                new ClassFunctuon().showMessageSB("Error prix");
            }           
        }

        private void bOk_Click(object sender, RoutedEventArgs e)
        {
           
            new ClassFunctuon().Click(sender);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

     
    }
}
