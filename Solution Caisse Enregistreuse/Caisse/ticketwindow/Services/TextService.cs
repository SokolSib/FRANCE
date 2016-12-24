using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;
using TicketWindow.Properties;

namespace TicketWindow.Services
{
    public static class TextService
    {
        public static string GetCloseWindowText()
        {
            var tickedWindowId = GlobalVar.TicketWindow != Guid.Empty ? GlobalVar.TicketWindow.ToString() : string.Empty;

            return Environment.NewLine +
                   Resources.LabelCashBox + " : " + Config.NameTicket + Environment.NewLine +
                   Resources.LabelNumberCheck + " : " + Config.NumberTicket + Environment.NewLine +
                   Resources.LabelUserName + " : " + Config.User + Environment.NewLine + Environment.NewLine +
                   "--------------------------------" + Environment.NewLine +
                   Resources.LabelOpenTotalTW + " : " + GlobalVar.TicketWindowG + Environment.NewLine +
                   Resources.LabelOpenLocal + " : " + tickedWindowId + Environment.NewLine;
        }

        public static List<ProductType> FindProductsByText(string text, bool useBarcode)
        {
            var textOriginal = text.Trim();
            var textTranslated = text.Trim();
            for (var i = 0; i < Config.SymbolsForReplace.Length; i++)
            {
                textTranslated = textTranslated.Replace(Config.SymbolsForReplace[i], Config.SymbolsToReplace[i]);
            }

            var dic = new Dictionary<Guid, ProductType>();

            if (useBarcode)
                foreach (var product in RepositoryProduct.Products.Where(
                    p => p.Name.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1 ||
                         p.Name.IndexOf(textTranslated, StringComparison.OrdinalIgnoreCase) != -1 ||
                         p.CodeBare.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1).ToList())
                {
                    dic[product.CustomerId] = product;
                }
            else
                foreach (var product in RepositoryProduct.Products.Where(
                    p => p.Name.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1 ||
                         p.Name.IndexOf(textTranslated, StringComparison.OrdinalIgnoreCase) != -1).ToList())
                {
                    dic[product.CustomerId] = product;
                }

            var originalWords = textOriginal.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var translatedWords = textTranslated.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var findResult = RepositoryProduct.Products.Where(p => IsExist(p, originalWords, translatedWords));

            foreach (var product in findResult)
            {
                dic[product.CustomerId] = product;
            }
            return dic.Values.ToList();
        }

        private static bool IsExist(ProductType product, string[] originalWords, string[] translatedWords)
        {
            var result = true;

            for (var i = 0; i < originalWords.Length; i++)
            {
                if (product.Name.IndexOf(originalWords[i], StringComparison.OrdinalIgnoreCase) == -1)
                    result = false;
            }

            if (result) return true;

            for (var i = 0; i < translatedWords.Length; i++)
            {
                if (product.Name.IndexOf(originalWords[i], StringComparison.OrdinalIgnoreCase) == -1)
                    return false;
            }

            return true;
        }
    }
}
