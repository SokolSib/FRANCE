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
                var closeTicket = RepositoryCloseTicket.GetByCloseTicketGId(closeTicketGs.First().CustomerId);
                PrintService.PrintCloseTicketG(closeTicketGs.First(), closeTicket);
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
                        RepositoryStockReal.AddProductCount(-product.Qty, productFromStock.CusumerIdRealStock);
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
                Mess += Properties.Resources.FuncCloseCashbox + Config.NameTicket + Environment.NewLine +
                        Environment.NewLine + Environment.NewLine;

                var otw = RepositoryOpenTicketWindow.GetCurrent();

                RepositoryCloseTicket.Close();
                RepositoryCheck.CloseTicket();
                //PrintService.PrintCloseTicket(closeTicket, Config.NameTicket);

                Mess += Properties.Resources.LabelOperationComplete + Environment.NewLine;

                if (otw != null)
                {
                    otw.DateOpen = DateTime.Now;
                    otw.IsOpen = false;
                    otw.IdTicketWindow = Guid.Empty;
                    otw.IdTicketWindowG = Guid.Empty;
                    RepositoryOpenTicketWindow.Update(otw);
                    Mess += Config.NameTicket + " " + Properties.Resources.LabelCloseEnd.ToLower() + "..." +
                            Environment.NewLine;

                    GlobalVar.IsOpen = false;
                }
                else
                    Mess += Config.NameTicket + " " + Properties.Resources.LabelErrorClosing.ToLower() + "..." +
                            Environment.NewLine;
            }
            else
                Mess += Config.NameTicket + " " + Properties.Resources.LabelAlreadyClosing.ToLower() + "..." +
                        Environment.NewLine;
        }

        public static void StartCheck()
        {
            if (File.Exists(RepositoryCheck.Path))
                try
                {
                    RepositoryCheck.Document = XDocument.Load(RepositoryCheck.Path);
                }
                catch (System.Exception ex)
                {
                    FunctionsService.ShowMessageSb("ошибка файла check.xml " + ex.Message);
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

        public static List<ProductType> ProductsFilter(WFindProduct wind)
        {
            var products = RepositoryProduct.Products;

            if (wind.xName.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByName(products, wind.xName.Text);
                products = TextService.FindProductsByText(wind.xName.Text, false);

            if (wind.xCodeBar.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByBarCode(products, wind.xCodeBar.Text);
                products = products.Where(p => p.CodeBare.Contains(wind.xCodeBar.Text.Trim())).ToList();

            if (wind.spPrice.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "price", wind.xPricea.Text.ToDecimal(), wind.xPriceb.Text.ToDecimal());
                products = products.Where(p => p.Price==wind.xPricea.Text.ToDecimal()).ToList();

            if (wind.xTVA.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementName(products, "tva", wind.xTVA.Text);
                products = products.Where(p => p.Tva.Value == wind.xTVA.Text.ToInt()).ToList();

            if (wind.spQTY.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "qty", wind.xQTYa.Text.ToDecimal(), wind.xQTYb.Text.ToDecimal());
                products = products.Where(p => p.Qty == wind.xTVA.Text.ToDecimal()).ToList();

            if (wind.xGroup.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementName(products, "grp", wind.xGroup.SelectedValue.ToString());
                products =
                    products.Where(p => p.SubGrpProduct.Group.Id == ((GroupProduct) wind.xGroup.SelectedItem).Id).ToList();

            if (wind.xSub_group.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementName(products, "sgrp", wind.xSub_group.SelectedValue.ToString());
                products =
                    products.Where(p => p.SubGrpProduct.Id == ((SubGroupProduct)wind.xSub_group.SelectedItem).Id).ToList();

            if (!wind.xBalance.IsChecked ?? false)
                //return RepositoryProduct.FiltrXElementsByElementName(products, "balance", true.ToString());
                products = products.Where(p => p.Balance).ToList();

            if (wind.spContenance.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "contenance", wind.xContenancea.Text.ToDecimal(), wind.xContenanceb.Text.ToDecimal());
                products = products.Where(p => p.Contenance == wind.xContenancea.Text.ToDecimal()).ToList();

            if (wind.spUnit_contenance.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "uniteContenance", wind.xUnit_contenancea.Text.ToDecimal(),
                //    wind.xUnit_contenanceb.Text.ToDecimal());
                products = products.Where(p => p.UniteContenance== wind.xUnit_contenancea.Text.ToDecimal()).ToList();

            if (wind.spTare.Visibility == Visibility.Visible)
                //return RepositoryProduct.FiltrXElementsByElementNameMinMax(products, "tare", wind.xTarea.Text.ToDecimal(), wind.xTareb.Text.ToDecimal());
                products = products.Where(p => p.Tare == wind.xTarea.Text.ToDecimal()).ToList();

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

        public static bool CloseWithMessage()
        {
            RepositoryGeneral.Set();
            var general = RepositoryGeneral.Generals.Find(l => l.EstablishmentCustomerId == Config.IdEstablishment);
            RepositoryGeneral.Mess = string.Empty;

            if (general.IsOpen ?? true)
            {
                if (CloseOpenTicketWindows())
                {
                    PrintCloseTicket(GlobalVar.TicketWindowG);
                    general.IsOpen = false;
                    general.User = Config.User;
                    general.Name = Config.NameTicket;
                    general.TicketWindowGeneral = Guid.Empty;
                    general.Date = DateTime.Now;
                    RepositoryGeneral.Update(general);
                    RepositoryGeneral.Mess = Properties.Resources.LabelGeneralCloseEnd;
                    return true;
                }
                RepositoryGeneral.Mess += Properties.Resources.LabelErrorGeneralClosing + " :" + Environment.NewLine +
                                          RepositoryCloseTicketG.Mess;
                return false;
            }
            RepositoryGeneral.Mess += Properties.Resources.LabelAlreadyGeneralClosing;
            return false;
        }


        private static bool CloseOpenTicketWindows()
        {
            RepositoryOpenTicketWindow.Sync(false);

            var openTicketWindows = RepositoryOpenTicketWindow.OpenTicketWindows.FindAll(otw => otw.IsOpen && (otw.IdTicketWindowG == GlobalVar.TicketWindowG));
            var flag = false;

            if (openTicketWindows.Count == 0)
            {
                var closeTicketG = RepositoryCloseTicketG.Get(GlobalVar.TicketWindowG).FirstOrDefault();
                RepositoryGeneral.Mess += Config.NameTicket + Environment.NewLine;

                if (closeTicketG != null)
                {
                    foreach (var el in RepositoryCloseTicket.GetByCloseTicketGId(GlobalVar.TicketWindowG))
                    {
                        closeTicketG.Pay1 += el.Pay1;
                        closeTicketG.Pay2 += el.Pay2;
                        closeTicketG.Pay3 += el.Pay3;
                        closeTicketG.Pay4 += el.Pay4;
                        closeTicketG.Pay5 += el.Pay5;
                        closeTicketG.Pay6 += el.Pay6;
                        closeTicketG.Pay7 += el.Pay7;
                        closeTicketG.Pay8 += el.Pay8;
                        closeTicketG.Pay9 += el.Pay9;
                        closeTicketG.Pay10 += el.Pay10;
                        closeTicketG.Pay11 += el.Pay11;
                        closeTicketG.Pay12 += el.Pay12;
                        closeTicketG.Pay13 += el.Pay13;
                        closeTicketG.Pay14 += el.Pay14;
                        closeTicketG.Pay15 += el.Pay15;
                        closeTicketG.Pay16 += el.Pay16;
                        closeTicketG.Pay17 += el.Pay17;
                        closeTicketG.Pay18 += el.Pay18;
                        closeTicketG.Pay19 += el.Pay19;
                        closeTicketG.Pay20 += el.Pay20;
                        closeTicketG.PayBankCards += el.PayBankCards;
                        closeTicketG.PayBankChecks += el.PayBankChecks;
                        closeTicketG.PayCash += el.PayCash;
                        closeTicketG.PayResto += el.PayResto;
                    }

                    closeTicketG.DateClose = DateTime.Now;
                    RepositoryCloseTicketG.Update(closeTicketG);
                    flag = true;
                }
                else RepositoryGeneral.Mess += Properties.Resources.LabelErrorClosing + Environment.NewLine;
            }
            else
                foreach (var window in openTicketWindows)
                    RepositoryGeneral.Mess += window.NameTicket + " " + Properties.Resources.LabelOpened.ToLower() + Environment.NewLine;

            return flag;
        }
    }
}