using System;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class AccountUser
    {
        public const string DefaultPassword = "admin";

        public AccountUser(Guid customerId, string fio, string login, string password, Guid roleId, string pinCode)
        {
            CustomerId = customerId;
            Fio = fio;
            Login = login;
            Password = password;
            RoleId = roleId;
            PinCode = pinCode;

            if (RepositoryAccountRole.AccountRoles.Count == 0)
                RepositoryAccountRole.Set();
            Role = RepositoryAccountRole.AccountRoles.FirstOrDefault(r => r.CustomerId == roleId);
        }

        public Guid CustomerId { get; set; }
        public string Fio { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PinCode { get; set; }
        public Guid RoleId { get; set; }
        public AccountRole Role { get; set; }

        public static AccountUser FromXElement(XContainer element)
        {
            return new AccountUser(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Fio"),
                element.GetXElementValue("Login"),
                element.GetXElementValue("Password"),
                element.GetXElementValue("RoleId").ToGuid(),
                element.Element("Pin") != null ? element.GetXElementValue("Pin") : null);
        }

        public static XElement ToXElement(AccountUser obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Fio", obj.Fio),
                new XElement("Login", obj.Login),
                new XElement("Password", obj.Password),
                new XElement("RoleId", obj.RoleId),
                new XElement("Pin", obj.PinCode));
        }

        public static void SetXmlValues(XContainer element, AccountUser obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("Fio").SetValue(obj.Fio);
            element.GetXElement("Login").SetValue(obj.Login);
            element.GetXElement("Password").SetValue(obj.Password);
            element.GetXElement("RoleId").SetValue(obj.RoleId);
            element.GetXElement("Pin").SetValue(obj.PinCode);
        }

        public static AccountUser CreateDefoult()
        {
            var megaRole = RepositoryAccountRole.GetRoleWithAllPrivelegies();
            return new AccountUser(Guid.NewGuid(), "Admin", DefaultPassword, DefaultPassword, megaRole.CustomerId, DefaultPassword);
        }

        public override string ToString()
        {
            return Fio;
        }
    }
}
