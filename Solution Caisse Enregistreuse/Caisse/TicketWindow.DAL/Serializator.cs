using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TicketWindow.DAL
{
    public static class Serializator
    {
        public static T CreateFromXmlString<T>(string s)
        {
            T el;
            var xs = new XmlSerializer(typeof(T));

            TextReader reader = null;
            try
            {
                reader = new StringReader(s);

                el = (T)xs.Deserialize(reader);
                reader.Close();
                reader = null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }

            return el;
        }

        public static string SerializeToXmlString<T>(T obj)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sr = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();

            StringWriter writer = null;
            try
            {
                writer = new StringWriter(sb);
                sr.Serialize(writer, obj, ns);

                writer.Close();
                writer = null;
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
            }

            return sb.ToString();
        }
    }
}
