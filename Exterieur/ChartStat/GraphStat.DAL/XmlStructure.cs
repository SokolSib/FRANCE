using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ChartStat.Model.Models;

namespace ChartStat.Model
{
    [XmlRoot("checks")]
    public class XmlStructure
    {
        [XmlElement("check")] public Check[] Checks;

        private static ICollection<StatSalesType> GetDatas(string rootDir, DateTime? startDate, DateTime? endDate, bool isGroupNumber, ICollection<GroupType> groups)
        {
            var result = new List<StatSalesType>();
            foreach (var yearDir in new DirectoryInfo(rootDir).GetDirectories())
            {
                var year = int.Parse(yearDir.Name);
                if ((startDate.HasValue && startDate.Value.Year > year) ||
                    (endDate.HasValue && endDate.Value.Year < year))
                    continue;

                foreach (var monthDir in new DirectoryInfo(yearDir.FullName).GetDirectories())
                {
                    var month = int.Parse(monthDir.Name);
                    if ((startDate.HasValue && startDate.Value.Year == year && startDate.Value.Month > month) ||
                        (endDate.HasValue && endDate.Value.Year == year && endDate.Value.Month > month))
                        continue;

                    foreach (var dateFile in new DirectoryInfo(monthDir.FullName).GetFiles())
                    {
                        var arr = dateFile.Name.Substring(0, dateFile.Name.Length - 4).Split('_');

                        var day = int.Parse(arr[0]);
                        var hour = int.Parse(arr[1]);
                        var minute = int.Parse(arr[2]);
                        var seconds = int.Parse(arr[3]);
                        var date = new DateTime(year, month, day, hour, minute, seconds);

                        if ((startDate.HasValue && startDate.Value >= date) ||
                            (endDate.HasValue && endDate.Value <= date))
                            continue;

                        var xml = File.ReadAllText(dateFile.FullName);
                        result.AddRange(GetStatSalesDataFromString(xml, date, isGroupNumber, groups));
                    }
                }
            }

            return result;
        }

        public static ICollection<StatSalesType> GetStatSalesTypesByProductType(string rootDir, DateTime? startDate, DateTime? endDate, IEnumerable<Guid> productId)
        {
            var datas = GetDatas(rootDir, startDate, endDate, true, null);
            return datas.Where(d => productId.Contains(d.CustumerId)).ToArray();
        }

        public static ICollection<StatSalesType> GetStatSalesTypesByGroup(string rootDir, DateTime? startDate, DateTime? endDate, int groupId, ICollection<GroupType> groups)
        {
            var datas = GetDatas(rootDir, startDate, endDate, true, groups);
            return datas.Where(d => d.Number == groupId).ToArray();
        }

        public static ICollection<StatSalesType> GetStatSalesTypesBySubgroup(string rootDir, DateTime? startDate, DateTime? endDate, IEnumerable<int> subgroupIds, ICollection<GroupType> groups)
        {
            var datas = GetDatas(rootDir, startDate, endDate, false, groups);
            return datas.Where(d => subgroupIds.Contains(d.Number)).ToArray();
        }

        private static ICollection<StatSalesType> GetStatSalesDataFromString(string xml, DateTime date, bool isGroupNumber, ICollection<GroupType> groups)
        {
            var xmlData = Serializator.CreateFromXMLString<XmlStructure>(xml);

            var result = new List<StatSalesType>();
            if (xmlData != null && xmlData.Checks != null)
            {
                foreach (var check in xmlData.Checks)
                {
                    result.AddRange(
                        check.Products.Select(product => new StatSalesType(
                            product.CustumerId,
                            date,
                            groups == null ? product.Name : groups.First(g => g.Id == product.GroupId).SubGroups.First(s => s.Id == product.SubgroupId).Name,
                            check.BarCode,
                            product.Price,
                            product.Qty,
                            0,
                            product.Tva,
                            product.SumDiscount,
                            product.Total,
                            isGroupNumber ? product.GroupId : product.SubgroupId)));
                }
            }

            return result.ToArray();
        }

        public static ICollection<GroupType> GetGroups(string xml)
        {
            var palettes = Serializator.CreateFromXMLString<PaletteDatas>(xml);

            var result = new List<GroupType>();
            foreach (var palette in palettes.Palettes)
            {
                var group = new GroupType(palette.Group.Id, palette.Group.Name, palette.SubGroups[0].Id, palette.SubGroups[0].Name);

                for (var i = 1; i < palette.SubGroups.Length; i++)
                {
                    var subGroup = new SubGroupType(palette.SubGroups[i].Id, palette.SubGroups[i].Name, palette.Group.Id);
                    group.SubGroups.Add(subGroup);
                }

                result.Add(group);
            }

            return result.OrderBy(g => g.Name).ToArray();
        }

        public static ICollection<ProductType> GetProducts(string xml)
        {
            var products = Serializator.CreateFromXMLString<ProductDatas>(xml);

            var result = products.Products.Select(product => new ProductType(product.Id, product.Name, product.BarCode)).ToList();

            return result.OrderBy(p => p.Name).ToArray();
        }
    }

    [XmlRoot("Product")]
    public class ProductDatas
    {
        [XmlElement("rec")] public ProductData[] Products;
    }

    public class ProductData
    {
        [XmlElement("CustumerId")] public Guid Id;

        [XmlElement("Name")] public string Name;

        [XmlElement("CodeBare")] public string BarCode;
    }

    [XmlRoot("Palettes")]
    public class PaletteDatas
    {
        [XmlElement("Palette")] public Palette[] Palettes;
    }

    public class Palette
    {
        [XmlElement("Group")] public Group Group;

        [XmlElement("SubGroup")] public Group[] SubGroups;
    }

    [XmlInclude(typeof (SubGroup))]
    public class Group
    {
        [XmlAttribute("ID")] public int Id;

        [XmlAttribute("Name")] public string Name;
    }

    public class SubGroup : Group
    {
    }

    public class Check
    {
        [XmlAttribute("barcodeCheck")] public string BarCode;

        [XmlAttribute("date")] public DateTime Date;

        [XmlElement("product")] public Product[] Products;
    }

    public class Product
    {
        [XmlElement("CustumerId")] public Guid CustumerId;

        [XmlElement("Name")] public string Name;

        [XmlElement("date")] public DateTime Date;

        [XmlElement("sumDiscount")] public decimal SumDiscount;

        [XmlElement("total")] public decimal Total;

        [XmlElement("grp")] public int GroupId;


        [XmlElement("price")] public decimal Price;

        [XmlElement("tva")] public decimal Tva;

        [XmlElement("qty")] public decimal Qty;

        [XmlElement("sgrp")] public int SubgroupId;
    }
}