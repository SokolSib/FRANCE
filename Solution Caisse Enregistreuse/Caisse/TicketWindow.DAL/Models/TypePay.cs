using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class TypePay
    {
        public TypePay(int id, bool? etat, string name, string nameCourt, int? codeCompta, bool? renduAvoir, bool? tiroir, bool? curMod)
        {
            Id = id;
            Etat = etat;
            Name = name.Trim();
            NameCourt = nameCourt.Trim();
            CodeCompta = codeCompta;
            RenduAvoir = renduAvoir;
            Tiroir = tiroir;
            CurMod = curMod;
        }
        
        public int Id { get; set; }
        public bool? Etat { get; set; }
        public string Name { get; set; }
        public string CheckName { get; set; }
        public string NameCourt { get; set; }
        public int? CodeCompta { get; set; }
        public bool? RenduAvoir { get; set; }
        public bool? Tiroir { get; set; }
        public bool? CurMod { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static TypePay FromXElement(XContainer element)
        {
            return new TypePay(
                element.GetXElementValue("id").ToInt(),
                element.GetXElementValue("Etat").ToBool(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("NameCourt"),
                element.GetXElementValue("CodeCompta").ToInt(),
                element.GetXElementValue("Rendu_Avoir").ToBool(),
                element.GetXElementValue("Tiroir").ToBool(),
                element.GetXElementValue("CurMod").ToBool());
        }

        public static XElement ToXElement(TypePay obj)
        {
            return new XElement("rec",
                new XElement("id", obj.Id),
                new XElement("Name", obj.Name),
                new XElement("Etat", obj.Etat),
                new XElement("CodeCompta", obj.CodeCompta),
                new XElement("NameCourt", obj.NameCourt),
                new XElement("Rendu_Avoir", obj.RenduAvoir),
                new XElement("Tiroir", obj.Tiroir),
                new XElement("CurMod", obj.CurMod));
        }
    }
}