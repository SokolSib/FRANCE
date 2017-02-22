using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class InfoClientsDiscountsTypeType
    {
        public InfoClientsDiscountsTypeType(int id, string name, int type, int value, string description)
        {
            Id = id;
            Name = name;
            Type = type;
            Value = value;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        public static InfoClientsDiscountsTypeType FromXElement(XContainer element)
        {
            return new InfoClientsDiscountsTypeType(
                element.GetXElementValue("Id").ToInt(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("Type").ToInt(),
                element.GetXElementValue("Value").ToInt(),
                element.GetXElementValue("Description"));
        }

        public static XElement ToXElement(InfoClientsDiscountsTypeType obj)
        {
            return new XElement("rec",
                new XElement("Id", obj.Id),
                new XElement("Name", obj.Name),
                new XElement("Type", obj.Type),
                new XElement("Value", obj.Value),
                new XElement("Description", obj.Description));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}