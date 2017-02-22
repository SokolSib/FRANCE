using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cnsActiveQTY
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassDB db = new ClassDB(null);



            Console.WriteLine("обработано {0}", db.queryNonResonse("UPDATE ProductsWeb SET Visible='false'"));

            List<object[]> obj = db.queryResonse("SELECT ProductsCustumerId FROM StockReal WHERE (QTY <> 0)");

            Console.WriteLine(" Всего найдено записей {0}", obj.Count);

            int i = 0;

            foreach (object[] o in obj)
            {
                
                Guid ProductsCustumerId =(Guid) o[0];

                List<object[]> list = db.queryResonse("SELECT ProductsWeb_CustomerId FROM Products WHERE CustumerId ='" + ProductsCustumerId  +"'");

                Guid ProductsWeb_CustomerId = (Guid)list[0][0];

                db.queryNonResonse("UPDATE ProductsWeb SET Visible='true' WHERE CustomerId='" + ProductsWeb_CustomerId + "'");

                Console.WriteLine(" Обработано записей {0} из {1}", i++, obj.Count);
            }
        }
    }
}
