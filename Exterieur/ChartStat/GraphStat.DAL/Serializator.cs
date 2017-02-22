using System.IO;
using System.Xml.Serialization;

namespace ChartStat.Model
{
    public class Serializator
    {
        public static T CreateFromXMLString<T>(string s)
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
    }
}
