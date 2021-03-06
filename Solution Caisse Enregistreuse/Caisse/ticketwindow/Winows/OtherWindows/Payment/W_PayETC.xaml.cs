﻿using System.Windows;
using System.Windows.Controls.Primitives;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Payment
{
    /// <summary>
    /// Логика взаимодействия для W_PayCheck.xaml
    /// </summary>
    public partial class WPayEtc : Window
    {
        public TypePay PayType { get; set; }

        public decimal? MaxMoney { get; set; }

        public WPayEtc()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            decimal money;
            if (!string.IsNullOrEmpty(tbS.Text) && decimal.TryParse(tbS.Text.Trim(), out money) &&
                FunctionsService.PayWithValidation(sender, money, MaxMoney, PayType))
                FunctionsService.Click(sender);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _typesPay.Content = PayType.Name;
            tbS.Text = RepositoryCurrencyRelations.Residue().ToString();
            numPad.TextBox = tbS;
            numPad.BEnter = xEnter;
            var owner = Owner as MainWindow;
            if ( owner != null) 
            owner.qty_label.Text = "__";
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            tbS.Focus();
        }

        private void InvisibleClick(object sender, RoutedEventArgs e)
        {
            var b = (ToggleButton)sender;
            if (b.Name == "NumPadVisible")
                xNumPad.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible;
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
       
    }
}
