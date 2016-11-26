using System;
using System.Xml.Serialization;

namespace TicketWindow.DAL.XmlStruct
{
    [XmlRoot]
    public class ActionsCaisse
    {
        [XmlElement("item")]
        public Item[] Items;
    }

    public class Item
    {
        public DeAction DeActions;
    }

    public class DeAction
    {
        [XmlElement("type")]
        public int Type;

        [XmlElement("enabled")]
        public bool Enabled;

        [XmlElement("A")]
        public DateTime A;

        [XmlElement("B")]
        public DateTime B;

        [XmlElement("establishmentCustomerId")]
        public Guid EstablishmentCustomerId;

        [XmlElement("productCustomerId")]
        public Guid ProductCustomerId;

        [XmlElement("qty")]
        public decimal Qty;

        [XmlElement("prix")]
        public decimal Prix;

        [XmlElement("discount")]
        public decimal Discount;
    }
}
