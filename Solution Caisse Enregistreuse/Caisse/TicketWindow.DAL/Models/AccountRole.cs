using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class AccountRole
    {
        public AccountRole(Guid customerId, string roleName, string privelegiesText)
        {
            CustomerId = customerId;
            RoleName = roleName;
            PrivelegiesText = privelegiesText;
        }

        public AccountRole(Guid customerId, string roleName, List<Privelege> privelegies)
        {
            CustomerId = customerId;
            RoleName = roleName;
            Privelegies = privelegies;
        }

        public Guid CustomerId { get; set; }
        public string RoleName { get; set; }
        public List<Privelege> Privelegies { get; set; }

        public string PrivelegiesText
        {
            get { return string.Join(",", Privelegies); }
            set
            {
                Privelegies = new List<Privelege>();

                if (!string.IsNullOrEmpty(value))
                    foreach (var t in value.Split(','))
                        Privelegies.Add((Privelege)Enum.Parse(typeof(Privelege), t));
            }
        }

        public static AccountRole FromXElement(XContainer element)
        {
            return new AccountRole(element.GetXElementValue("CustomerId").ToGuid(), element.GetXElementValue("RoleName"), element.GetXElementValue("Privelegies"));
        }

        public static XElement ToXElement(AccountRole obj)
        {
            return new XElement("rec", new XElement("CustomerId", obj.CustomerId), new XElement("RoleName", obj.RoleName), new XElement("Privelegies", obj.PrivelegiesText));
        }

        public static void SetXmlValues(XContainer element, AccountRole obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("RoleName").SetValue(obj.RoleName);
            element.GetXElement("Privelegies").SetValue(obj.PrivelegiesText);
        }

        public static AccountRole CreateDefoult()
        {
            return new AccountRole(Guid.NewGuid(), RepositoryAccountUser.MainRoleName, Privelege.All.ToString());
        }

        public override string ToString()
        {
            return RoleName;
        }

        public bool IsPermiss(Privelege privelege)
        {
            return Privelegies.Contains(Privelege.All) || Privelegies.Contains(privelege);
        }
    }
}
