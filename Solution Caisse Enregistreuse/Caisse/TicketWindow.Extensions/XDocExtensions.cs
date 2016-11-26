using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

// ReSharper disable PossibleNullReferenceException

namespace TicketWindow.Extensions
{
    public static class XDocExtensions
    {
        public static XElement GetXElement(this XContainer element, params string[] names)
        {
            return (XElement) names.Aggregate(element, (current, name) => current.Element(name));
        }

        public static string GetXElementValue(this XContainer element, params string[] names)
        {
            return element.GetXElement(names).Value;
        }

        private static XContainer GetRootXElement(this XContainer element, string[] names, out string name)
        {
            name = names[names.Length - 1];

            for (var i = 0; i <= names.Length - 2; i++)
                element = element.Element(names[i]);

            return element;
        }

        public static IEnumerable<XElement> GetXElements(this XContainer element, params string[] names)
        {
            string name;
            element = element.GetRootXElement(names, out name);

            return element.Elements(name);
        }

        public static IEnumerable<XElement> GetXElementsIfExistElement(this XContainer element, params string[] names)
        {
            string name;
            element = element.GetRootXElement(names, out name);

            return element != null ? element.Elements(name) : new List<XElement>();
        }

        public static XAttribute GetXAttribute(this XContainer element, params string[] names)
        {
            string name;
            element = element.GetRootXElement(names, out name);

            return ((XElement) element).Attribute(name);
        }

        public static IEnumerable<XAttribute> GetXAttributes(this XContainer element, params string[] names)
        {
            string name;
            element = element.GetRootXElement(names, out name);

            return ((XElement) element).Attributes(name);
        }

        public static string GetXAttributeValue(this XContainer element, params string[] names)
        {
            return element.GetXAttribute(names).Value;
        }

        public static XAttribute GetXAttributeOrNull(this XContainer element, params string[] names)
        {
            try
            {
                return GetXAttribute(element, names);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static XElement GetXElementOrNull(this XContainer element, params string[] names)
        {
            try
            {
                return GetXElement(element, names);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}