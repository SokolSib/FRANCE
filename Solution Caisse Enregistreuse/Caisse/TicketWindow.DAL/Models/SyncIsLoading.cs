using System;
using System.Xml.Linq;
using TicketWindow.DAL.Additional;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class SyncIsLoading
    {
        public SyncIsLoading(SyncEnum name, bool isLoading)
        {
            Name = name;
            IsLoading = isLoading;
        }

        public SyncEnum Name { get; set; }
        public bool IsLoading { get; set; }


        public static SyncIsLoading FromXElement(XContainer element)
        {
            return new SyncIsLoading(
                (SyncEnum) Enum.Parse(typeof (SyncEnum), element.GetXElementValue("Name")),
                element.GetXElementValue("IsLoading").ToBool());
        }

        public static XElement ToXElement(SyncIsLoading obj)
        {
            return new XElement("rec",
                new XElement("Name", obj.Name),
                new XElement("IsLoading", obj.IsLoading));
        }
    }
}
