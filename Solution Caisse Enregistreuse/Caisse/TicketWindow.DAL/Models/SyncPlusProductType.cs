using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class SyncPlusProductType
    {
        public SyncPlusProductType(Guid customerId, string check, DateTime date, Guid customerIdSyncPlus)
        {
            CustomerId = customerId;
            CheckText = check;
            Date = date;
            CustomerIdSyncPlus = customerIdSyncPlus;
        }

        public Guid CustomerId { get; set; }
        public XDocument Check { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerIdSyncPlus { get; set; }

        public string CheckText
        {
            get { return Check.ToString(); }
            set { Check = XDocument.Parse(value); }
        }

        public static SyncPlusProductType FromXElement(XContainer element)
        {
            return new SyncPlusProductType(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("CheckText"),
                element.GetXElementValue("Date").ToDateTime(),
                element.GetXElementValue("CustomerIdSyncPlus").ToGuid());
        }

        public static XElement ToXElement(SyncPlusProductType obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("CheckText", obj.CheckText),
                new XElement("Date", obj.Date),
                new XElement("CustomerIdSyncPlus", obj.CustomerIdSyncPlus));
        }

        public override string ToString()
        {
            return CustomerId.ToString();
        }
    }
}