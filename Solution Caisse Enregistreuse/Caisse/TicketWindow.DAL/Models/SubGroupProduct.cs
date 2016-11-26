using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class SubGroupProduct
    {
        public SubGroupProduct(int id, string name, int groupId)
        {
            Id = id;
            Name = name;
            GroupId = groupId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public GroupProduct Group { get; set; }

        public static SubGroupProduct FromXElement(XContainer element, GroupProduct group)
        {
            return new SubGroupProduct(
                element.GetXAttributeValue("ID").ToInt(),
                element.GetXAttributeValue("Name"),
                group.Id) {Group = group};
        }

        public static XElement ToXElement(SubGroupProduct obj)
        {
            return new XElement("SubGroup",
                new XAttribute("Name", obj.Name),
                new XAttribute("ID", obj.Id));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}