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
    /// <summary>
    ///     Xml.
    /// </summary>
    public class RepositoryAccountRole
    {
        private static readonly string Path = Config.AppPath + @"Data\AccountRoles.xml";

        public static List<AccountRole> AccountRoles = new List<AccountRole>();

        public static void SaveFile()
        {
            var root = new XElement("AccountRoles");

            foreach (var role in AccountRoles)
                root.Add(AccountRole.ToXElement(role));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                AccountRoles.Clear();
                foreach (var element in document.GetXElementsIfExistElement("AccountRoles", "rec"))
                    AccountRoles.Add(AccountRole.FromXElement(element));
            }
        }

        public static void Set()
        {
            if (File.Exists(Path))
                LoadFile();

            if (GetRoleWithAllPrivelegies() == null)
            {
                AccountRoles.Add(AccountRole.CreateDefoult());
                SaveFile();
            }
        }

        public static AccountRole GetRoleWithAllPrivelegies()
        {
            return AccountRoles.FirstOrDefault(r => r.Privelegies.Contains(Privelege.All));
        }
    }
}