using System;
using System.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Properties;

namespace TicketWindow.Services
{
    public class FunctionsTranslateService
    {
        public static string GetTranslatedFunction(string name)
        {
            switch (name)
            {
                case "None - Vide":
                case "None":
                    return Resources.LabelNone;
                case "Section des Articles":
                    return Resources.FuncProductMenu;
                case "CaseProducts":
                    return Resources.FuncCaseProducts;
                case "Fermer":
                    return Resources.BtnClose;
                case "svernut":
                    return Resources.FunTurn;
                case "Reset main Grid":
                    return Resources.FuncResetBtn;
                case "SetBarCode":
                    return Resources.FuncSetBarCode;
                case "Open CashBox":
                    return Resources.FuncOpenCashBox;
                case "Calculatrice":
                    return Resources.FuncCalculator;
                case "Carte de Fidélité":
                    return Resources.FuncDiscountCard;
                case "DiscountMini":
                    return Resources.FuncDiscountReset;
                case "Show Pro":
                    return Resources.FuncShowPro;
                case "toDevis":
                    return Resources.FuncToPro;
                case "Modification des Articles":
                    return Resources.FuncEditProduct;
                case "Modifier le prix":
                    return Resources.FuncEditPrice;
                case "Afficher un Total":
                    return Resources.FuncTotal;
                case "Supprimer une Ligne":
                    return Resources.FuncDelProduct;
                case "Supprimer le Ticket":
                    return Resources.FuncClearProducts;
                case "Annulation de ticket":
                    return Resources.FuncCancelBay;
                case "Attente +1":
                    return Resources.FuncToWait;
                case "Bande de Contrôle":
                    return Resources.FuncHistory;
                case "Le mode de Paiement":
                    return Resources.FuncPaymentMethod;
                case "Retours et Remboursements":
                    return Resources.FuncReturnProduct;
                case "General History of closing":
                    return Resources.FuncHistoryOfAllCards;
                case "Histoire de ticket":
                    return Resources.FuncHistoryChecks;
                case "Clôture":
                    return Resources.FuncCloseCashbox;
                case "UpdateDb":
                    return Resources.FuncUpdateDB;
            }
            var context = "Products id=[";
            var idx = name.IndexOf(context, StringComparison.Ordinal);
            if (idx >= 0)
            {
                name = name.Substring(context.Length, name.Length - context.Length - 1);
                var guid = new Guid(name);
                name = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == guid)?.Name;
            }
            return name;
        }
    }
}
