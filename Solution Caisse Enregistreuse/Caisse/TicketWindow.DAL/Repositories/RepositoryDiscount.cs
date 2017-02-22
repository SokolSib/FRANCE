using System;
using TicketWindow.DAL.Models;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Memory.+
    /// </summary>
    public class RepositoryDiscount
    {
        public static Discount Client = new Discount();

        public static void RestoreDiscount()
        {
            Client.LastDateUpd = new DateTime();
            Client.Barcode = null;
            Client.NameLast = null;
            Client.NameFirst = null;
            Client.Points = 0;
            Client.Procent = 0;
            Client.AddPoints = false;
            Client.DiscountSet = false;
            Client.ShowMessaget = true;
        }

        public static DiscountCard GetDiscount(string codebar)
        {
            var discountCard = RepositoryDiscountCard.GetOneByNumber(codebar);

            if (discountCard!=null && discountCard.IsActive)
            {
                Client.Barcode = codebar;
                Client.Points = discountCard.Points;
                Client.InfoClientsCustomerId = discountCard.InfoClientsCustomerId;
                Client.Procent = 0;
                Client.LastDateUpd = discountCard.DateTimeLastAddProduct;
            }

            return discountCard;
        }

        public static void SetDiscountPoint(string codebar, int points, bool isSet)
        {
            var discountCard = RepositoryDiscountCard.GetOneByNumber(codebar);

            if (discountCard != null)
            {
                if (points >= Client.MaxPoints) points = Client.MaxPoints;
                discountCard.Points = isSet ? points : discountCard.Points;

                if (isSet) RepositoryDiscountCard.Update(discountCard);
            }
        }

        public static ClientInfo GetClientInfoById(Guid clientCustomerId)
        {
            var info = RepositoryClientInfo.GetOneByNumber(clientCustomerId);

            if (info != null)
            {
                Client.NameFirst = info.Name;
                Client.NameLast = info.Surname;
            }
            else
            {
                Client.NameFirst = " Inconnue ";
                Client.NameLast = "";
            }
            return info;
        }
    }
}