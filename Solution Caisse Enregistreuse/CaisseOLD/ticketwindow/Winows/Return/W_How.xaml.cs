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

namespace ticketwindow.Winows.Return
{
    /// <summary>
    /// Логика взаимодействия для W_How.xaml
    /// </summary>
    public partial class W_How : Window
    {
        public decimal qty { get; set; }
        public bool kg { get; set; }
        public W_How(decimal qty, bool kg)
        {
            InitializeComponent();
            this.qty = qty;
            this.kg = kg;
            lQTY.Content = kg ? "kg.": "piece(s)";
            tQTY.Text = !kg ?  ((int)Math.Abs(  qty )).ToString(): Math.Abs( qty).ToString();
            lError.Visibility = Visibility.Hidden;
            tError.Visibility = Visibility.Hidden;
        }

        private bool raznica()
        {
            if (decimal.Parse(tQTY.Text) > Math.Abs(qty))
            {
                lError.Visibility = Visibility.Visible;

                tError.Visibility = Visibility.Visible;

                tError.Text = "Le type de données est trop grand";

                return false;
            }
            else
            {
                return true;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
                if (!kg)
                {
                    
                    decimal qty_ = decimal.Parse(tQTY.Text.Replace(".",",")) ;
                    if ( decimal.Truncate(qty_) - qty_ == 0)
                    {
                        if (raznica())
                        {
                            lError.Visibility = Visibility.Hidden;

                            tError.Visibility = Visibility.Hidden;

                            W_ReturnProduct w = (W_ReturnProduct)this.Owner;

                            w.recalc(qty_ * (qty > 0 ? 1 : -1), w.listDetails.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.PayProducts);

                            this.Close();
                        }
                    }
                    else
                    {
                        
                        lError.Visibility = Visibility.Visible;

                        tError.Visibility = Visibility.Visible;

                        tError.Text = " La Pièce(s) ne peuvent pas être un nombre fractionnaire";
                    }
                }
                else
                {
                    decimal qty_;

                    if (decimal.TryParse(tQTY.Text, out qty_))
                    {
                        if (raznica())
                        {
                            lError.Visibility = Visibility.Hidden;

                            tError.Visibility = Visibility.Hidden;

                            W_ReturnProduct w = (W_ReturnProduct)this.Owner;

                            w.recalc(qty_ * (qty > 0 ? 1 : -1), 
                                null);

                            this.Close();
                        }
                    }
                    else
                    {
                        lError.Visibility = Visibility.Visible;

                        tError.Visibility = Visibility.Visible;

                        tError.Text = "Le format du numéro est invalide.";
                    }
                }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
