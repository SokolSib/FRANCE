using System.Collections.Generic;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class GroupProduct
    {
        public GroupProduct(int id, string name)
        {
            Id = id;
            Name = name;
            SubGroups = new List<SubGroupProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubGroupProduct> SubGroups { get; set; }

        public static GroupProduct FromXElement(XContainer element)
        {
            var group = new GroupProduct(
                element.GetXAttributeValue("Group", "ID").ToInt(),
                element.GetXAttributeValue("Group", "Name"));

            foreach (var subGroup in element.Elements("SubGroup"))
                group.SubGroups.Add(SubGroupProduct.FromXElement(subGroup, group));

            return group;
        }

        public static XElement ToXElement(GroupProduct obj)
        {
            var groupElement = new XElement("Palette",
                new XElement("Group",
                    new XAttribute("Name", obj.Name),
                    new XAttribute("ID", obj.Id)));

            foreach (var sg in obj.SubGroups)
                groupElement.Add(SubGroupProduct.ToXElement(sg));

            return groupElement;
        }

        public static XElement ToXElementSubGroups(XElement element, GroupProduct obj)
        {
            element.GetXElements("SubGroup").Remove();

            foreach (var sg in obj.SubGroups)
                element.Add(SubGroupProduct.ToXElement(sg));

            return element;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}