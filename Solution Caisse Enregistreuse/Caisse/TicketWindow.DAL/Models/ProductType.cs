using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class ProductType
    {
        public ProductType(Guid customerId, string name, string codeBare, string desc, short chpCat, bool balance,
            decimal contenance, int uniteContenance, int tare, DateTime date, Guid tvaCustomerId, Guid productsWebCustomerId,
            int subGrpProductId)
        {
            CustomerId = customerId;
            Name = name.Trim().ToUpper();
            CodeBare = RepositoryProductBc.GetAllBarCodes(codeBare, customerId);
            Desc = desc.ToUpper();
            ChpCat = chpCat;
            Balance = balance;
            Contenance = contenance;
            UniteContenance = uniteContenance;
            Tare = tare;
            Date = date;
            TvaCustomerId = tvaCustomerId;
            ProductsWebCustomerId = productsWebCustomerId;

            SubGrpProduct = RepositorySubGroupProduct.SubGroupProducts.FirstOrDefault(sg => sg.Id == subGrpProductId);
            if (SubGrpProduct != null)
            {
                CusumerIdSubGroup = SubGrpProduct.Id;
                Sgrp = SubGrpProduct.Id;
                Grp = SubGrpProduct.GroupId;
            }
            Tva = RepositoryTva.Tvases.FirstOrDefault(t => t.CustomerId == tvaCustomerId);
            if (Tva != null)
                TvaId = Tva.Id;

            StockReal = new List<StockReal>();
        }

        public int Ii { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int TvaId { get; set; }
        public decimal Price { get; set; }
        public decimal PriceGros { get; set; }
        public string CodeBare { get; set; }
        public decimal Qty { get; set; }
        public bool Balance { get; set; }
        public decimal Contenance { get; set; }
        public int UniteContenance { get; set; }
        public int Tare { get; set; }
        public int Grp { get; set; }
        public int Sgrp { get; set; }
        public Guid ProductsWebCustomerId { get; set; }
        public DateTime? Date { get; set; }
        public Guid CusumerIdRealStock { get; set; }
        public int CusumerIdSubGroup { get; set; }
        public decimal Total { get; set; }
        public short ChpCat { get; set; }
        public Guid TvaCustomerId { get; set; }
        public SubGroupProduct SubGrpProduct { get; set; }
        public Tva Tva { get; set; }
        public List<StockReal> StockReal { get; set; }
        // for check
        public decimal Discount { get; set; }
        public decimal SumDiscount { get; set; }

        public static ProductType FromXElement(XContainer element)
        {
            var tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == element.GetXElementValue("tva").ToInt());
            var subGrpProduct = RepositorySubGroupProduct.SubGroupProducts.FirstOrDefault(sg => sg.Id == element.GetXElementValue("cusumerIdSubGroup").ToInt());

            var product = new ProductType(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Name").Trim().ToUpper(),
                element.GetXElementValue("CodeBare"),
                element.GetXElementValue("Desc").ToUpper(),
                0,
                element.GetXElementValue("balance").ToBool(),
                element.GetXElementValue("contenance").ToDecimal(),
                element.GetXElementValue("uniteContenance").ToInt(),
                element.GetXElementValue("tare").ToInt(),
                element.GetXElementValue("date").ToDateTime(),
                tva != null ? tva.CustomerId : Guid.Empty,
                element.GetXElementValue("ProductsWeb_CustomerId").ToGuid(),
                subGrpProduct != null ? subGrpProduct.Id : 0)
                          {
                              Ii = element.GetXElementValue("ii").ToInt(),
                              TvaId = tva != null ? tva.Id : element.GetXElementValue("tva").ToInt(),
                              Price = element.GetXElementValue("price").ToDecimal(),
                              PriceGros = element.GetXElementValue("priceGros").ToDecimal(),
                              Qty = element.GetXElementValue("qty").ToDecimal(),
                              CusumerIdRealStock = element.GetXElementValue("cusumerIdRealStock").ToGuid(),
                              Grp = subGrpProduct != null ? subGrpProduct.GroupId : element.GetXElementValue("grp").ToInt(),
                              Sgrp = subGrpProduct != null ? subGrpProduct.Id : element.GetXElementValue("sgrp").ToInt(),
                              Total = element.Element("toatl") == null ? 0.0m : element.GetXElementValue("toatl").ToDecimal()
                          };

            if (subGrpProduct == null)
            {
                product.SubGrpProduct = RepositorySubGroupProduct.SubGroupProducts.FirstOrDefault(sg => sg.Id == product.Sgrp);
                if (product.SubGrpProduct != null)
                {
                    product.CusumerIdSubGroup = product.SubGrpProduct.Id;
                    product.Sgrp = product.SubGrpProduct.Id;
                    product.Grp = product.SubGrpProduct.GroupId;
                }
            }
            if (tva == null)
            {
                product.Tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == product.TvaId);
                if (product.Tva != null)
                    product.TvaId = product.Tva.Id;
            }

            return product;
        }

        public static XElement ToXElement(ProductType obj, XElement productsElement)
        {
            obj.Ii = productsElement.Elements("rec").Count();

            return new XElement("rec",
                new XElement("ii", obj.Ii),
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Name", obj.Name.Trim().ToUpper()),
                new XElement("Desc", obj.Desc.ToUpper()),
                new XElement("price", obj.Price),
                new XElement("priceGros", obj.PriceGros),
                new XElement("tva", obj.TvaId),
                new XElement("qty", obj.Qty),
                new XElement("CodeBare", obj.CodeBare),
                new XElement("balance", obj.Balance),
                new XElement("contenance", obj.Contenance),
                new XElement("uniteContenance", obj.UniteContenance),
                new XElement("tare", obj.Tare),
                new XElement("date", obj.Date ?? DateTime.Now),
                new XElement("cusumerIdRealStock", obj.CusumerIdRealStock),
                new XElement("cusumerIdSubGroup", obj.CusumerIdSubGroup),
                new XElement("ProductsWeb_CustomerId", obj.ProductsWebCustomerId),
                new XElement("grp", obj.Grp),
                new XElement("sgrp", obj.Sgrp),
                new XElement("toatl", obj.Total));
        }

        public static XElement ToCheckXElement(ProductType obj, List<XElement> productElements)
        {
            if (productElements != null)
                obj.Ii = productElements.Count();

            return new XElement("product",
                new XElement("ii", obj.Ii),
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Name", obj.Name),
                new XElement("Desc", obj.Desc),
                new XElement("price", obj.Price),
                new XElement("priceGros", obj.PriceGros),
                new XElement("tva", obj.TvaId),
                new XElement("qty", obj.Qty),
                new XElement("CodeBare", obj.CodeBare),
                new XElement("balance", obj.Balance),
                new XElement("contenance", obj.Contenance),
                new XElement("uniteContenance", obj.UniteContenance),
                new XElement("tare", obj.Tare),
                new XElement("date", obj.Date ?? DateTime.Now),
                new XElement("cusumerIdRealStock", obj.CusumerIdRealStock),
                new XElement("cusumerIdSubGroup", obj.CusumerIdSubGroup),
                new XElement("ProductsWeb_CustomerId", obj.ProductsWebCustomerId),
                new XElement("grp", obj.Grp),
                new XElement("sgrp", obj.Sgrp),
                new XElement("Discount", obj.Discount),
                new XElement("sumDiscount", obj.SumDiscount),
                new XElement("total", obj.Total));
        }

        public static ProductType FromCheckXElement(XContainer element)
        {
            var tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == element.GetXElementValue("tva").ToInt());
            var subGrpProduct =
                RepositorySubGroupProduct.SubGroupProducts.FirstOrDefault(
                    sg => sg.Id == element.GetXElementValue("cusumerIdSubGroup").ToInt());

            var product = new ProductType(
                              element.GetXElementValue("CustomerId").ToGuid(),
                              element.GetXElementValue("Name").Trim().ToUpper(),
                              element.GetXElementValue("CodeBare"),
                              element.GetXElementValue("Desc").ToUpper(),
                              0,
                              element.GetXElementValue("balance").ToBool(),
                              element.GetXElementValue("contenance").ToDecimal(),
                              element.GetXElementValue("uniteContenance").ToInt(),
                              element.GetXElementValue("tare").ToInt(),
                              element.GetXElementValue("date").ToDateTime(),
                              tva?.CustomerId ?? Guid.Empty,
                              element.GetXElementValue("ProductsWeb_CustomerId").ToGuid(),
                              subGrpProduct?.Id ?? 0)
                          {
                              Ii = element.GetXElementValue("ii").ToInt(),
                              Price = element.GetXElementValue("price").ToDecimal(),
                              PriceGros = element.GetXElementValue("priceGros").ToDecimal(),
                              Qty = element.GetXElementValue("qty").ToDecimal(),
                              Balance = element.GetXElementValue("balance").ToBool(),
                              CusumerIdRealStock = element.GetXElementValue("cusumerIdRealStock").ToGuid(),
                              ProductsWebCustomerId = element.GetXElementValue("ProductsWeb_CustomerId").ToGuid(),
                              Discount = element.GetXElementValue("Discount").ToDecimal(),
                              SumDiscount = element.GetXElementValue("sumDiscount").ToDecimal(),
                              Total = element.GetXElementValue("total").ToDecimal(),
                          };

            return product;
        }

        public static void SetXmlValues(XElement element, ProductType obj)
        {
            element.GetXElement("ii").SetValue(obj.Ii);
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("Name").SetValue(obj.Name.Trim().ToUpper());
            element.GetXElement("Desc").SetValue(obj.Desc.ToUpper());
            element.GetXElement("price").SetValue(obj.Price);
            element.GetXElement("priceGros").SetValue(obj.PriceGros);
            element.GetXElement("tva").SetValue(obj.TvaId);
            element.GetXElement("qty").SetValue(obj.Qty);
            element.GetXElement("CodeBare").SetValue(obj.CodeBare);
            element.GetXElement("balance").SetValue(obj.Balance);
            element.GetXElement("contenance").SetValue(obj.Contenance);
            element.GetXElement("uniteContenance").SetValue(obj.UniteContenance);
            element.GetXElement("tare").SetValue(obj.Tare);
            element.GetXElement("date").SetValue(DateTime.Now);
            element.GetXElement("cusumerIdRealStock").SetValue(obj.CusumerIdRealStock);
            element.GetXElement("cusumerIdSubGroup").SetValue(obj.CusumerIdSubGroup);
            element.GetXElement("ProductsWeb_CustomerId").SetValue(obj.ProductsWebCustomerId);
            element.GetXElement("grp").SetValue(obj.Grp);
            element.GetXElement("sgrp").SetValue(obj.Sgrp);
            element.GetXElement("toatl").SetValue(obj.Total);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}