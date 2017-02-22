using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Models.Base;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class CheckTicket : CheckTicketBase
    {
        public CheckTicket(Guid customerId, string barCode, DateTime date, decimal payBankChecks, decimal payBankCards, decimal payCash, decimal payResto, decimal pay1,
            decimal pay2, decimal pay3, decimal pay4, decimal pay5, decimal pay6, decimal pay7, decimal pay8, decimal pay9, decimal pay10, decimal pay11, decimal pay12,
            decimal pay13, decimal pay14, decimal pay15, decimal pay16, decimal pay17, decimal pay18, decimal pay19, decimal pay20, Guid closeTicketCustomerId, decimal totalTtc,
            decimal rendu)
            : base(
                customerId, barCode, date, payBankChecks, payBankCards, payCash, payResto, pay1, pay2, pay3, pay4, pay5, pay6, pay7, pay8, pay9, pay10, pay11, pay12, pay13, pay14,
                pay15, pay16, pay17, pay18, pay19, pay20, closeTicketCustomerId, totalTtc, rendu)
        {
            PayProducts = new List<PayProduct>();
        }

        public CheckTicket(PayTicketBase payTicket)
            : base(
                payTicket.CustomerId, null, DateTime.Now, payTicket.PayBankChecks, payTicket.PayBankCards, payTicket.PayCash, payTicket.PayResto, payTicket.Pay1, payTicket.Pay2,
                payTicket.Pay3, payTicket.Pay4, payTicket.Pay5, payTicket.Pay6, payTicket.Pay7, payTicket.Pay8, payTicket.Pay9, payTicket.Pay10, payTicket.Pay11, payTicket.Pay12,
                payTicket.Pay13, payTicket.Pay14, payTicket.Pay15, payTicket.Pay16, payTicket.Pay17, payTicket.Pay18, payTicket.Pay19, payTicket.Pay20, Guid.Empty, 0, 0)
        {
            PayProducts = new List<PayProduct>();
        }

        public string ReturnBarCode { get; set; }
        public CloseTicketCheckDiscount CheckDiscount { get; set; }
        public List<PayProduct> PayProducts { get; set; }

        public static CheckTicket FromCheckXElement(XElement element, Guid customerId, Guid closeTicketCustomerId)
        {
            var checkTicket = new CheckTicket(FromXElement(element, customerId))
                              {
                                  BarCode = element.GetXAttributeValue("barcodeCheck"),
                                  Date = element.GetXAttributeValue("date").ToDateTime(),
                                  CloseTicketCustomerId = closeTicketCustomerId,
                                  TotalTtc = element.GetXAttributeValue("sum").ToDecimal(),
                                  Rendu = element.GetXAttributeValue("Rendu").ToDecimal()
                              };
            var returnBarCodeAttribute = element.GetXAttributeOrNull("returnBarcodeCheck");
            if (returnBarCodeAttribute != null)
                checkTicket.ReturnBarCode = returnBarCodeAttribute.Value;

            if (element.Attribute("DCBC") != null)
                checkTicket.CheckDiscount = CloseTicketCheckDiscount.FromXElement(element, Guid.NewGuid(), closeTicketCustomerId);

            foreach (var el in element.Elements("product"))
                checkTicket.PayProducts.Add(PayProduct.FromCheckXElement(el, Guid.NewGuid(), checkTicket.CustomerId, closeTicketCustomerId));

            return checkTicket;
        }

        public static XElement ToCheckXElement(CheckTicket obj)
        {
            var checkElement = new XElement("check");

            SetPaysAttributes(checkElement, obj);

            if (!string.IsNullOrEmpty(obj.ReturnBarCode))
                checkElement.Add(new XAttribute("returnBarcodeCheck", obj.ReturnBarCode));
            checkElement.Add(new XAttribute("barcodeCheck", obj.BarCode));
            checkElement.Add(new XAttribute("Rendu", obj.Rendu));
            checkElement.Add(new XAttribute("sum", obj.TotalTtc));
            checkElement.Add(new XAttribute("date", obj.Date));
            if (obj.CheckDiscount != null)
            {
                checkElement.Add(new XAttribute("DCBC", obj.CheckDiscount.Dcbc ?? string.Empty));
                checkElement.Add(new XAttribute("DCBC_BiloPoints", obj.CheckDiscount.DcbcBiloPoints ?? 0));
                checkElement.Add(new XAttribute("DCBC_DobavilePoints", obj.CheckDiscount.DcbcDobavilePoints ?? 0));
                checkElement.Add(new XAttribute("DCBC_OtnayliPoints", obj.CheckDiscount.DcbcOtnayliPoints ?? 0));
                checkElement.Add(new XAttribute("DCBC_OstalosPoints", obj.CheckDiscount.DcbcOstalosPoints ?? 0));
                checkElement.Add(new XAttribute("DCBC_name", obj.CheckDiscount.DcbcName ?? string.Empty));
            }
            
            foreach (var p in obj.PayProducts)
            {
                var ptemp = RepositoryProduct.Products.Find(la => la.CustomerId == p.ProductId);

                var productElement = new XElement("product",
                        new XElement("CustomerId", p.ProductId),
                        new XElement("grp", RepositorySubGroupProduct.SubGroupProducts.First(sg => sg.Id == (ptemp == null ? 3 : ptemp.CusumerIdSubGroup)).GroupId),
                        new XElement("qty", p.Qty),
                        new XElement("Name", p.Name),
                        new XElement("CodeBare", p.Barcode),
                        new XElement("price", p.PriceHt),
                        new XElement("tva", RepositoryTva.Tvases.Find(l => l.Value == p.Tva).Id),
                        new XElement("total", p.Total),
                        new XElement("Discount", p.Discount),
                        new XElement("sumDiscount", p.SumDiscount));

                checkElement.Add(productElement);
            }

            return checkElement;
        }

        public static CheckTicket FromXElement(XContainer element)
        {
            var checkTicket = new CheckTicket(FromXElementBase(element))
            {
                BarCode = element.GetXElementValue("BarCode"),
                Date = element.GetXElementValue("Date").ToDateTime(),
                CloseTicketCustomerId = element.GetXElementValue("CloseTicketCustomerId").ToGuid(),
                TotalTtc = element.GetXElementValue("TotalTtc").ToDecimal(),
                Rendu = element.GetXElementValue("Rendu").ToDecimal()
            };

            return checkTicket;
        }

        public static XElement ToXElement(CheckTicket obj)
        {
            var element = ToXElementBase(obj);
            element.Add(new XElement("BarCode", obj.BarCode));
            element.Add(new XElement("Date", obj.Date));
            element.Add(new XElement("CloseTicketCustomerId", obj.CloseTicketCustomerId));
            element.Add(new XElement("TotalTtc", obj.TotalTtc));
            element.Add(new XElement("Rendu", obj.Rendu));
            return element;
        }

        public static XElement SetPaysAttributes(XElement checkElement, CheckTicket check)
        {
            return SetPaysAttributesBase(checkElement, check);
        }
    }
}