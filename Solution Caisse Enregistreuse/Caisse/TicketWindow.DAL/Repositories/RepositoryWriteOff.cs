using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml +.
    /// </summary>
    public class RepositoryWriteOff
    {
        private static XDocument _document;
        private static readonly string Path = Config.AppPath + @"Data\WriteOff.xml";

        public static List<WriteOffType> WriteOffs = new List<WriteOffType>();

        public static void LoadFile()
        {
            if (File.Exists(Path))
            {
                _document = XDocument.Load(Path);

                WriteOffs.Clear();
                foreach (var element in _document.GetXElements("WriteOffs", "rec"))
                    WriteOffs.Add(WriteOffType.FromXElement(element));
            }
        }

        public static void SaveFile()
        {
            _document = new XDocument(new XElement("WriteOffs"));

            foreach (var writeOff in WriteOffs)
                _document.GetXElement("tva").Add(WriteOffType.ToXElement(writeOff));

            _document.Save(Path);
        }
        
        public static void Add(WriteOffType writeOff)
        {
            if (!File.Exists(Path)) SaveFile();

            var document = XDocument.Load(Path);
            document.GetXElement("WriteOffs").Add(WriteOffType.ToXElement(writeOff));
            File.WriteAllText(Path, document.ToString());

            WriteOffs.Add(writeOff);
        }
    }
}