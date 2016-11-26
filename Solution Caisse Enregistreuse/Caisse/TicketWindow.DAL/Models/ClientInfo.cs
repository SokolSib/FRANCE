using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class ClientInfo
    {
        public ClientInfo(Guid customerId, int typeClient, int sex, string name, string surname, string nameCompany, string siret, string frtva, string officeAddress,
            string officeZipCode, string officeCity, string homeAddress, string homeZipCode, string homeCity, string telephone, string mail, string password,
            Guid countrysCustomerId, Guid favoritesProductAutoCustomerId, string nclient, Guid? idInfoClientsDiscount)
        {
            CustomerId = customerId;
            TypeClient = typeClient;
            Sex = sex;
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
            if (nclient != null) Nclient = nclient.Trim();
            IdInfoClientsDiscount = idInfoClientsDiscount;

            DiscountCards = new List<DiscountCard>();
        }

        public Guid CustomerId { get; set; }
        public int TypeClient { get; set; }
        public int Sex { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameCompany { get; set; }
        public string Siret { get; set; }
        public string Frtva { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeZipCode { get; set; }
        public string OfficeCity { get; set; }
        public string HomeAddress { get; set; }
        public string HomeZipCode { get; set; }
        public string HomeCity { get; set; }
        public string Telephone { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public Guid CountrysCustomerId { get; set; }
        public Guid FavoritesProductAutoCustomerId { get; set; }
        public string Nclient { get; set; }
        public Guid? IdInfoClientsDiscount { get; set; }
        public List<DiscountCard> DiscountCards { get; set; }

        public static ClientInfo FromXElement(XContainer element)
        {
            return new ClientInfo(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("TypeClient").ToInt(),
                element.GetXElementValue("Sex").ToInt(),
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
                element.GetXElementValue("Nclient"),
                element.GetXElementValue("IdInfoClientsDiscount").ToNullableGuid());
        }

        public static XElement ToXElement(ClientInfo obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("TypeClient", obj.TypeClient),
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
                new XElement("Nclient", obj.Nclient),
                new XElement("IdInfoClientsDiscount", obj.IdInfoClientsDiscount));
        }

        public override string ToString()
        {
            return string.Concat(Name, " ", Surname);
        }
    }
}