using System;
using System.Diagnostics;
using System.Windows;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;
using TicketWindow.PortClasses;
using TicketWindow.Services;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Winows.OtherWindows.Ballance
{
    /// <summary>
    ///     Логика взаимодействия для W_Ballance.xaml
    /// </summary>
    public partial class WBallance : Window
    {
        private readonly ProductType _product;
        private decimal _qty;

        public ProductType Product;

        public WBallance(object arg)
        {
            InitializeComponent();
            _product = arg as ProductType;
            xCodebar.Content = _product.CodeBare.Trim() == "" ? "not" : _product.CodeBare;
            xName.Content = _product.Name;
            xDescription.Content = _product.Desc;
            xPrix.Text = _product.Price.ToString();
            xTVA.Content = RepositoryTva.GetById(_product.TvaId);
            xBallance.Content = _product.Balance;
            xContenance.Content = _product.Contenance;
            xUniteContenance.Content = _product.UniteContenance;
            xTare.Content = _product.Tare;

            GetBallance();
            Product = _product;

            numPad.TextBox = xPrix;
            numPad.BEnter = bOk;
            numPad2.TextBox = xBallance_kg;
            numPad2.BEnter = bOk;
        }

        private void GetBallance()
        {
            var f = false;

            if (_product.Price == 0.0m)
            {
                _product.Price = 1;
                f = true;
            }
            ClassBallanceMAGELLAN_8400.Send(_product.Price, _product.Tare);

            if (ClassBallanceMAGELLAN_8400.Busy_0X15)
                FunctionsService.ShowMessageTime("Pour résoudre ce problème, il vous suffit de re-peser l'article");
            if (ClassBallanceMAGELLAN_8400.Error_0X15)
                FunctionsService.ShowMessageTime("Pour résoudre ce problème, il vous suffit de redémarrer la balance!");

            var prix = 0.0m;
            try
            {
                prix = decimal.Parse(ClassBallanceMAGELLAN_8400.Prix);
                _qty = decimal.Parse(ClassBallanceMAGELLAN_8400.Poinds)/1000;
                xBallance_kg.Text = _qty.ToString();
                xPrix_kg.Content = prix.ToString();
            }
            catch (System.Exception e)
            {
                xBallance_kg.Text = "0";
                xPrix_kg.Content = "0";

                LogService.Log(TraceLevel.Error, 22, "Error ballance.");
                xLog.Content = e.Message + Environment.NewLine;
            }
            xLog.Content += ClassBallanceMAGELLAN_8400.Error;
            try
            {
                if (!f)
                    xTotal_kg.Content = (Math.Round(decimal.Parse(ClassBallanceMAGELLAN_8400.Montant)/100, 2));
                else xTotal_kg.Content = "0.0";
            }
            catch
            {
                xTotal_kg.Content = "0";
            }
            if (_qty > 0)
            {
                _product.Contenance = _qty;
                _product.Price = prix;
            }
        }

        private void BGetClick(object sender, RoutedEventArgs e)
        {
            decimal newpice;
            if (decimal.TryParse(xPrix.Text.Replace(".", ","), out newpice))
            {
                _product.Price = newpice;
                GetBallance();
                Product = _product;
            }
            else FunctionsService.ShowMessageSb("Error prix");
        }

        private void BOkClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}