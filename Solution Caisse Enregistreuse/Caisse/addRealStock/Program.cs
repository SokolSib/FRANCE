using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace addRealStock
{
    class Program
    {
        static Establishment value;

        static void Main(string[] args)
        {

            Console.WriteLine(System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString);


            List<Establishment> est = Establishment.sel();

            int c = 1;

            foreach (var e in est)
            {

                Console.WriteLine("{0}. {1} {2} {3} {4} {5}", c++, e.CustomerId, e.Name, e.Type, e.Adresse, e.CP);
            }


            Console.WriteLine("Ajouter nouvelle Establishment oui/non ?");



            string q = Console.ReadLine();


            if (q == "oui")
            {
                Console.WriteLine("Поехали...");

                value = new Establishment();

                value.Mail = "sarl.anahit@yahoo.fr";

                value.Adresse = "9 Boulevard de la Liberté";

                value.CP = "13001";

                value.CustomerId = Guid.Parse("3d1cb37c-1171-4831-b047-c46d3e7dabfe");

                value.Name = "INTERNET";

                value.Phone = "04 91 50 87 61";

                value.Type = 0;

                value.Ville = "MARSEILLE";

                int r = Establishment.ins(value);

                {
                    if (r > 0)
                        Console.WriteLine("ДОБАВЛЕН магазин {0}. {1} {2} {3} {4} {5}", c++, value.CustomerId, value.Name, value.Type, value.Adresse, value.CP);

                    List<Products.StockReal> L = Products.StockReal.sel(Guid.Parse("e27d5a4d-d6d3-4ee5-810b-f95b32e0bb93"));

                    List<Products.StockReal> B = Products.StockReal.sel(value.CustomerId);

                    for (int i = 0; i < L.Count; i++)
                    {
                        Console.WriteLine("Выполнено {0} %  {1}-{2}", (i * 100) / L.Count, i + 1, L.Count);

                        if (B.FindIndex(l => l.ProductsCustumerId == L[i].ProductsCustumerId) == -1)
                        {

                            L[i].CustomerId = Guid.NewGuid();

                            L[i].IdEstablishment = value.CustomerId;

                            L[i].MinQTY = 10;

                            L[i].QTY = 0;

                            Products.StockReal.ins(L[i]);
                        }
                        else
                        {
                            Console.WriteLine("Уже добавлен...");
                        }
                    }
                }

            }
        }
    }
}
