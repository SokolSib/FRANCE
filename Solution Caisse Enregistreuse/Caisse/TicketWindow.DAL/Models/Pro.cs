using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class Pro
    {
        public Pro(Guid customerId, int sex, string name, string surname, string nameCompany, string siret, string frtva, string officeAddress,
            string officeZipCode, string officeCity, string homeAddress, string homeZipCode, string homeCity, string telephone, string mail, string password,
            Guid countrysCustomerId, Guid favoritesProductAutoCustomerId, string nclient)
        {
            //InfoClientsDiscountsTypeType infoClientDiscount = null;
            //if (idInfoClientsDiscountsType.HasValue)
            //    infoClientDiscount = RepositoryInfoClientsDiscountsType.InfoClientsDiscounts.FirstOrDefault(i => i.Id == idInfoClientsDiscountsType.Value);

            CustomerId = customerId;
            Sexint = sex;
            Sex = Sexint == 0 ? "M." : (Sexint == 1 ? "Mme." : "Mlle.");
            Name = name;
            Surname = surname;
            NameCompany = nameCompany;
            Siret = siret;
            Frtva = frtva;
            OfficeAddress = officeAddress;
            OfficeZipCode = officeZipCode;
            OfficeCity = officeCity;
            HomeAddress = homeAddress;
            HomeZipCode = homeZipCode;
            HomeCity = homeCity;
            Telephone = telephone;
            Mail = mail;
            Password = password;
            CountrysCustomerId = countrysCustomerId;
            FavoritesProductAutoCustomerId = favoritesProductAutoCustomerId;
            if (nclient != null) Nclient = nclient.Trim().ToInt();

            ProDiscounts = new List<ProDiscount>();
        }
        
        public Guid CustomerId { get; set; }
        public string DiscountName { get; set; }
        public int DiscountValue { get; set; }
        public int? DiscountType { get; set; }
        public int Sexint { get; set; }
        public string Sex { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameCompany { get; set; }
        public string Mail { get; set; }
        public string Telephone { get; set; }
        public int Nclient { get; set; }
        public string Siret { get; set; }
        public string Frtva { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeZipCode { get; set; }
        public string OfficeCity { get; set; }
        public string HomeAddress { get; set; }
        public string HomeZipCode { get; set; }
        public string HomeCity { get; set; }
        public string Password { get; set; }
        public Guid CountrysCustomerId { get; set; }
        public Guid FavoritesProductAutoCustomerId { get; set; }
        public List<ProDiscount> ProDiscounts { get; set; }

        public static int SexToInt(string sex)
        {
            switch (sex)
            {
                case "M.":
                    return 0;
                case "Mme.":
                    return 1;
                case "Mlle.":
                    return 2;
            }
            return 0;
        }

        public override string ToString()
        {
            return string.Concat(DiscountName, "", Name);
        }

        public static Pro FromXElement(XContainer element)
        {
            return new Pro(
                element.GetXElementValue("CustomerId").ToGuid(),
                SexToInt(element.GetXElementValue("Sex")),
                element.GetXElementValue("Name"),
                element.GetXElementValue("Surname"),
                element.GetXElementValue("NameCompany"),
                element.GetXElementValue("Siret"),
                element.GetXElementValue("Frtva"),
                element.GetXElementValue("OfficeAddress"),
                element.GetXElementValue("OfficeZipCode"),
                element.GetXElementValue("OfficeCity"),
                element.GetXElementValue("HomeAddress"),
                element.GetXElementValue("HomeZipCode"),
                element.GetXElementValue("HomeCity"),
                element.GetXElementValue("Telephone"),
                element.GetXElementValue("Mail"),
                element.GetXElementValue("Password"),
                element.GetXElementValue("CountrysCustomerId").ToGuid(),
                element.GetXElementValue("FavoritesProductAutoCustomerId").ToGuid(),
                element.GetXElementValue("Nclient"));
        }

        public static XElement ToXElement(Pro obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Sex", obj.Sex),
                new XElement("Name", obj.Name),
                new XElement("Surname", obj.Surname),
                new XElement("NameCompany", obj.NameCompany),
                new XElement("Siret", obj.Siret),
                new XElement("Frtva", obj.Frtva),
                new XElement("OfficeAddress", obj.OfficeAddress),
                new XElement("OfficeZipCode", obj.OfficeZipCode),
                new XElement("OfficeCity", obj.OfficeCity),
                new XElement("HomeAddress", obj.HomeAddress),
                new XElement("HomeZipCode", obj.HomeZipCode),
                new XElement("HomeCity", obj.HomeCity),
                new XElement("Telephone", obj.Telephone),
                new XElement("Mail", obj.Mail),
                new XElement("Password", obj.Password),
                new XElement("CountrysCustomerId", obj.CountrysCustomerId),
                new XElement("FavoritesProductAutoCustomerId", obj.FavoritesProductAutoCustomerId),
                new XElement("Nclient", obj.Nclient));
        }
    }
}