using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    public class RepositorySyncIsLoading
    {
        public static List<SyncIsLoading> SyncIsLoadings = new List<SyncIsLoading>();
        private static readonly string Path = Config.AppPath + @"Data\SyncIsLoadings.xml";

        public static void SaveFile()
        {
            var root = new XElement("SyncIsLoadings");

            foreach (var syncIsLoading in SyncIsLoadings)
                root.Add(SyncIsLoading.ToXElement(syncIsLoading));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        public static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                SyncIsLoadings.Clear();
                foreach (var element in document.GetXElements("SyncIsLoadings", "rec"))
                    SyncIsLoadings.Add(SyncIsLoading.FromXElement(element));
            }

            SetDefaultFalseIfNotExist(SyncEnum.PayProduct);
            SetDefaultFalseIfNotExist(SyncEnum.PayProductTmp);
            SetDefaultFalseIfNotExist(SyncEnum.CheckTicketTmp);
        }

        private static void SetDefaultFalseIfNotExist(SyncEnum sync)
        {
            if (SyncIsLoadings.FirstOrDefault(s => s.Name == sync) == null)
                SyncIsLoadings.Add(new SyncIsLoading(sync, false));
        }

        public static bool IsLoading(SyncEnum sync)
        {
            var syncIsLoading = SyncIsLoadings.FirstOrDefault(sl => sl.Name == sync);
            return syncIsLoading == null || syncIsLoading.IsLoading;
        }

        public static void SetSyncIsLoading(SyncEnum sync, bool isSync)
        {
            var syncIsLoading = SyncIsLoadings.FirstOrDefault(sl => sl.Name == sync);
            if (syncIsLoading == null)
            {
                syncIsLoading = new SyncIsLoading(sync, isSync);
                SyncIsLoadings.Add(syncIsLoading);
            }
            else syncIsLoading.IsLoading = isSync;
        }

        public static int CheckedCount()
        {
            return Enum.GetValues(typeof (SyncEnum)).Cast<SyncEnum>().Count(IsLoading);
        }
    }
}
