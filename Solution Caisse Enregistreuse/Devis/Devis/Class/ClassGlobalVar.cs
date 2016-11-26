using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devis.Class
{
    class ClassGlobalVar
    {

        public static Guid CustumerId =  new Guid("cf1a1661-e2a7-45f3-b3d2-f055b51e29d2");

        public static string nameTicket = "plnsh";

        public static string user = "test";

        public static int numberTicket = 12;

        public static Guid TicketWindowG { get; set; }

        public static Guid TicketWindow { get; set; }

        public static string Name = "Name ";

        public static bool open { get; set; }

        public static bool break_ = false;

        public static short utc = -1;

        
        public static Guid IdEstablishment = new Guid("33a6114d-cb17-4bdc-a51f-984fc849093f");

        public static List<string> mess = new List<string>();

        public static List<string> error = new List<string>();

        public static List<Guid> listErrorRealTableIsNuul = new List<Guid>();

       
        public static ClassSync.Establishment Establishment { get; set; }

        public static void loadFromDB()
        {
            if (ClassSync.connect)
            {
                Establishment = new ClassSync.Establishment().sel(IdEstablishment);

                Name = Establishment.Name;
            }
        }
        public static bool Bureau = true;
    }
}
