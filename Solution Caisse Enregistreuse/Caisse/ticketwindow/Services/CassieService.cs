using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
using TicketWindow.Print;
using TicketWindow.Winows.OtherWindows.Product.FindProduct;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Services
{
    public class CassieService
    {
        public static string Mess { get; set; }

        public static void PrintHistoryChangeProducts(int group)
        {
            var elements = RepositoryHistoryChangeProduct.Document.GetXElements("HistoryChangeProducts", "rec").Where(r => r.GetXElementValue("group") == group.ToString());

            var xElements = elements as XElement[] ?? elements.ToArray();
            if (xElements.Any())
            {
                var dataForPrint = xElements.Select(element =>
                    new object[]
                    {
                        DotLiquidService.SpecTransform(element.GetXElementValue("name")),
                        element.GetXElementValue("barcode"),
                        element.GetXElementValue("price_")
                    }).ToList();

                var dt = xElements.First().GetXElementValue("date").ToDateTime();

                DotLiquidService.Print(dataForPrint, dt, group);
            }
        }

        public static void PrintCloseTicket(Guid ticketWindowG)
        {
            var closeTicketGs = RepositoryCloseTicketG.Get(ticketWindowG);

            if (closeTicketGs.Count == 1)
            {
                var closeTicket = RepositoryCloseTicket.GetByCloseTicketGId(closeTicketGs[0].CustomerId);
                PrintService.PrintCloseTicketG(closeTicketGs[0], closeTicket);
            }
            else RepositoryCloseTicketG.Mess += "Ошибка печати, " + closeTicketGs.Count + " записей в БД";
        }

        public static void RemoveProductCountFromStockReal(CloseTicketTmp closeTicket)
        {
            foreach (var checkTicket in closeTicket.ChecksTicket)
            {
                foreach (var product in checkTicket.PayProducts)
                {
                    var productFromStock = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == product.ProductId);

                    if (productFromStock != null)
                        RepositoryStockReal.UpdateProductCount(-product.Qty, productFromStock.CusumerIdRealStock);
                    else
                    {
                        var message = " нет записи об данном продукте в таблице СТОК, Детали: IDcustomer " + product.ProductId + " -  Название продукта " + product.Name +
                                      " - Количество " + product.Qty + " Штрих код : " + product.Barcode;
                        LogService.LogText(TraceLevel.Error, message);
                    }
                }
            }
        }
        
        public static void Close()
        {
            Mess = string.Empty;

            if (GlobalVar.IsOpen)
            {
                Mess += "Clôture du" + Config.NameTicket + Environment.NewLine + Environment.NewLine + " Veuillez patienter! Exportation vers base de données..." +
                        Environment.NewLine;

                var otw = RepositoryOpenTicketWindow.GetCurrent();

                RepositoryCloseTicket.Close();
                RepositoryCheck.CloseTicket();
                //PrintService.PrintCloseTicket(closeTicket, Config.NameTicket);

                Mess += "Exportation vers base de données est terminé" + Environment.NewLine;

                if (otw != null)
                {
                    otw.DateOpen = DateTime.Now;
                    otw.IsOpen = false;
                    otw.IdTicketWindow = Guid.Empty;
                    otw.IdTicketWindowG = Guid.Empty;
                    RepositoryOpenTicketWindow.Update(otw);
                    Mess += Config.NameTicket + " clôture terminée..." + Environment.NewLine;

                    GlobalVar.IsOpen = false;
                }
                else Mess += Config.NameTicket + " erreur de fermeture..." + Environment.NewLine;
            }
            else Mess += Config.NameTicket + " déjà clôturé..." + Environment.NewLine;
        }

        public static void StartCheck()
        {
            if (File.Exists(RepositoryCheck.Path))
            {
                try
                {
                    RepositoryCheck.Document = XDocument.Load(RepositoryCheck.Path);
                }
                catch (System.Exception ex)
                {
                    FunctionsService.ShowMessageSb("ошибка файла check.xml " + ex.Message);
                }
            }
            else RepositoryCheck.OpenTicket();

            if (File.Exists(RepositoryCheck.PathEnAttenete))
                RepositoryCheck.DocumentEnAttenete = XDocument.Load(RepositoryCheck.PathEnAttenete);
            else
            {
                RepositoryCheck.DocumentEnAttenete = new XDocument();

                RepositoryCheck.DocumentEnAttenete.Add(new XElement("checks",
                    new XAttribute("ticket", Config.NumberTicket),
                    new XAttribute("openDate", DateTime.Now.ToString())
                    ));

                RepositoryCheck.DocumentEnAttenete.Save(RepositoryCheck.PathEnAttenete);
            }
        }
        
        public static void OpenProductsCheck()
        {
            if ((RepositoryCheck.DocumentProductCheck == null) || (RepositoryCheck.DocumentProductCheck.Element("check") == null))
            {
                RepositoryCheck.DocumentProductCheck = new XDocument(new XElement("check", new XAttribute("barcodeCheck", RepositoryCheck.GetBarCodeCheck())));
                FunctionsService.WriteTotal();
                RepositoryCheck.DocumentProductCheck.Save(RepositoryCheck.PathProductCheck);
            }
            RepositoryCheck.C = null;
        }

        public static void ClearProductsCheck()
        {
            RepositoryCheck.DocumentProductCheck.GetXElement("check").Remove();
            OpenProductsCheck();
        }

        public static void LoadProductCheckFromFile()
        {
            if (!File.Exists(RepositoryCheck.PathProductCheck))
                OpenProductsCheck();

            RepositoryCheck.DocumentProductCheck = XDocument.Load(RepositoryCheck.PathProductCheck);
            FunctionsService.WriteTotal();
        }

        public static IEnumerable<XElement> ProductsFilter(WFindProduct wind)
        {
            var products = RepositoryProduct.Document.GetXElements("Product", "rec");

            if (wind.xName.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByName(products, wind.xName.Text);

            if (wind.xCodeBar.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByBarCode(products, wind.xCodeBar.Text);

            if (wind.spPrice.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "price", wind.xPricea.Text.ToDecimal(), wind.xPriceb.Text.ToDecimal());

            if (wind.xTVA.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementName(products, "tva", wind.xTVA.Text);

            if (wind.spQTY.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "qty", wind.xQTYa.Text.ToDecimal(), wind.xQTYb.Text.ToDecimal());

            if (wind.xGroup.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementName(products, "grp", wind.xGroup.SelectedValue.ToString());

            if (wind.xSub_group.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementName(products, "sgrp", wind.xSub_group.SelectedValue.ToString());

            if (!wind.xBalance.IsChecked ?? false)
                return RepositoryProduct.FiltrXElementsByElementName(products, "balance", true.ToString());

            if (wind.spContenance.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "contenance", wind.xContenancea.Text.ToDecimal(), wind.xContenanceb.Text.ToDecimal());

            if (wind.spUnit_contenance.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "uniteContenance", wind.xUnit_contenancea.Text.ToDecimal(),
                    wind.xUnit_contenanceb.Text.ToDecimal());

            if (wind.spTare.Visibility == Visibility.Visible)
                return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "tare", wind.xTarea.Text.ToDecimal(), wind.xTareb.Text.ToDecimal());

            return products;
        }
        
        public static bool Open()
        {
            RepositoryGeneral.Set();

            if (!RepositoryGeneral.IsOpen)
            {
                var ticketWindowGeneral = RepositoryCloseTicketG.Create();

                var general = RepositoryGeneral.Generals.FirstOrDefault(g => g.EstablishmentCustomerId == Config.IdEstablishment);

                if (general != null)
                {
                    general.Name = Config.NameTicket;
                    general.IsOpen = true;
                    general.TicketWindowGeneral = ticketWindowGeneral;
                    general.User = Config.User;
                    general.Date = DateTime.Now;
                    general.EstablishmentCustomerId = Config.IdEstablishment;
                    RepositoryGeneral.Update(general);
                }
                else
                {
                    general = new GeneralType(Guid.NewGuid(), ticketWindowGeneral, true, Config.NameTicket, Config.User, DateTime.Now, Config.IdEstablishment);
                    RepositoryGeneral.Add(general);
                }

                GlobalVar.TicketWindowG = ticketWindowGeneral;

                foreach (var rec in RepositoryOpenTicketWindow.OpenTicketWindows.FindAll(l => l.EstablishmentCustomerId == Config.IdEstablishment))
                {
                    rec.IdTicketWindowG = GlobalVar.TicketWindowG;
                    RepositoryOpenTicketWindow.Update(rec);
                }
                return true;
            }
            FunctionsService.ShowMessageTime("Уже открыта просто продолжите работать");
            return false;
        }

        public static bool Cls()
        {
            RepositoryGeneral.Set();
            var general = RepositoryGeneral.Generals.Find(l => l.EstablishmentCustomerId == Config.IdEstablishment);
            RepositoryGeneral.Mess = string.Empty;

            if (general.IsOpen ?? true)
            {
                if (RepositoryCloseTicketG.Cls())
                {
                    PrintCloseTicket(GlobalVar.TicketWindowG);
                    general.IsOpen = false;
                    general.User = Config.User;
                    general.Name = Config.NameTicket;
                    general.TicketWindowGeneral = Guid.Empty;
                    general.Date = DateTime.Now;
                    RepositoryGeneral.Update(general);
                    RepositoryGeneral.Mess = "Clôture générale est terminé";
                    return true;
                }
                RepositoryGeneral.Mess += "Vous ne pouvez pas effectuer la clôture, car :" + Environment.NewLine + RepositoryCloseTicketG.Mess;
                return false;
            }
            RepositoryGeneral.Mess += "Clôture générale a été faite";
            return false;
        }
    }
}