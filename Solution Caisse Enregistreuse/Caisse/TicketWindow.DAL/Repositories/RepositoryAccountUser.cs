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
    public class RepositoryAccountUser
    {
        public const string MainRoleName = "Administrator";
        private static readonly string Path = Config.AppPath + @"Data\AccountUsers.xml";

        public static List<AccountUser> AccountUsers = new List<AccountUser>();
        public static AccountUser LoginedUser;

        public static bool Login(string login, string password)
        {
            if (AccountUsers.Count == 0)
                Set();

            var user = AccountUsers.FirstOrDefault(u =>
                u.Login.ToLower() == login.ToLower() &&
                u.Password == password);

            LoginedUser = user;

            return user != null;
        }

        public static bool PermiseByPrivelege(Privelege privelege)
        {
            return LoginedUser.Role.Privelegies.Contains(privelege) || 
                LoginedUser.Role.Privelegies.Contains(Privelege.All);
        }

        public static void SaveFile()
        {
            var root = new XElement("AccountUsers");

            foreach (var role in AccountUsers)
                root.Add(AccountUser.ToXElement(role));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                AccountUsers.Clear();
                foreach (var element in document.GetXElementsIfExistElement("AccountUsers", "rec"))
                    AccountUsers.Add(AccountUser.FromXElement(element));
            }
        }

        public static void Set()
        {
            RepositoryAccountRole.Set();

            if (File.Exists(Path))
                LoadFile();

            var adminUser = AccountUsers.FirstOrDefault(u => u.Role.Privelegies.Contains(Privelege.All));
            if (adminUser == null)
                AccountUsers.Add(AccountUser.CreateDefoult());
            else if (string.IsNullOrEmpty(adminUser.PinCode))
                adminUser.PinCode = AccountUser.DefaultPassword;

            SaveFile();
        }
    }
}
