using System;
using System.Windows;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Winows.OtherWindows.Return
{
    /// <summary>
    ///     Логика взаимодействия для W_How.xaml
    /// </summary>
    public partial class WHow : Window
    {
        public WHow(decimal qty, bool kg)
        {
            InitializeComponent();
            Qty = qty;
            Kg = kg;
            LQty.Content = kg ? "kg." : Properties.Resources.LabelQty;
            TQty.Text = !kg ? ((int) Math.Abs(qty)).ToString() : Math.Abs(qty).ToString();
            LError.Visibility = Visibility.Hidden;
            TError.Visibility = Visibility.Hidden;
        }

        public decimal Qty { get; set; }
        public bool Kg { get; set; }

        private bool Raznica()
        {
            if (TQty.Text.ToDecimal() > Math.Abs(Qty))
            {
                TError.Text = Properties.Resources.LabelExcess;
                LError.Visibility = Visibility.Visible;
                TError.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Kg)
            {
                var qty = TQty.Text.ToDecimal();
                if (decimal.Truncate(qty) - qty == 0)
                {
                    if (Raznica())
                    {
                        LError.Visibility = Visibility.Hidden;
                        TError.Visibility = Visibility.Hidden;
                        var w = (WReturnProduct) Owner;
                        w.Recalc(qty*(Qty > 0 ? 1 : -1), w.GridProducts.SelectedItem as PayProduct);
                        Close();
                    }
                }
                else
                {
                    LError.Visibility = Visibility.Visible;
                    TError.Visibility = Visibility.Visible;
                    TError.Text = Properties.Resources.LabelPieceNotFractional;
                }
            }
            else
            {
                decimal qty;

                if (decimal.TryParse(TQty.Text, out qty))
                {
                    if (Raznica())
                    {
                        LError.Visibility = Visibility.Hidden;
                        TError.Visibility = Visibility.Hidden;
                        var w = (WReturnProduct) Owner;
                        w.Recalc(qty*(Qty > 0 ? 1 : -1), null);
                        Close();
                    }
                }
                else
                {
                    TError.Text = Properties.Resources.LabelInvalidNumberFormat;
                    LError.Visibility = Visibility.Visible;
                    TError.Visibility = Visibility.Visible;
                }
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}