using System.IO;
using System.Text;

namespace TicketWindow.DAL
{
    public static class FileService
    {
        private static readonly Encoding FileEncoding = Encoding.Unicode;

        public static T CreateFromFile<T>(string fileName)
        {
            var text = File.ReadAllText(fileName, FileEncoding);
            return Serializator.CreateFromXmlString<T>(text);
        }

        public static void WriteToFile<T>(T data, string fileName)
        {
            var text = Serializator.SerializeToXmlString(data);
            File.WriteAllText(fileName, text, FileEncoding);
        }
    }
}