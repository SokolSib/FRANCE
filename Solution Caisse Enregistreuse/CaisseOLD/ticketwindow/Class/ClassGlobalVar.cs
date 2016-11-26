using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    class ClassGlobalVar
    {

        /*          
           public static Guid CustumerId = new Guid("cf1a1661-e2a7-45f3-b3d2-f055b51e2910");
          
           public static string user = "KARO";

           public static string nameTicket = "Caisse 1";

           public static int numberTicket = 1;
         
         public static Guid CustumerId = new Guid("cf1a1661-e2a7-45f3-b3d2-f055b51e29d2");

        public static string nameTicket = "Caisse 2";

        public static string user = "Vazgen";

        public static int numberTicket = 2;
          
           public static Guid CustumerId = new Guid("ec49ecca-7b8b-4b55-83e8-c9de77557812");
          
           public static string user = "Archak";
         
           public static string nameTicket = "Caisse 3";

           public static int numberTicket = 3;
      


        public static Guid CustumerId = new Guid("ec49ecca-7b8b-4b55-83e8-c9de77557812");

        public static string user = "Archak";

        public static string nameTicket = "Caisse 3";

        public static int numberTicket = 3;


          */
        public static Guid CustumerId = ticketwindow.Properties.Settings.Default.CustumerId;// new Guid("cf1a1661-e2a7-45f3-b3d2-f055b51e29d2");

        public static string nameTicket = ticketwindow.Properties.Settings.Default.nameTicket;

        public static string user = ticketwindow.Properties.Settings.Default.user;

        public static int numberTicket = ticketwindow.Properties.Settings.Default.numberTicket;

        public static Guid TicketWindowG;

        public static Guid TicketWindow;

        public static string Name = ticketwindow.Properties.Settings.Default.Name;

        public static bool open { get; set; }

        public static bool break_ = false;

        public static short utc = ticketwindow.Properties.Settings.Default.utc;

     //   public static Guid IdEstablishment = new Guid("42e89ba9-1012-4327-95f9-40712e232849");

        public static Guid IdEstablishment = ticketwindow.Properties.Settings.Default.IdEstablishment;

        public static Guid IdEstablishment_GROS = ticketwindow.Properties.Settings.Default.IdEstablishment_GROS;

        public static List<string> mess = new List<string>();

        public static List<string> error = new List<string>();

        public static List<Guid> listErrorRealTableIsNuul = new List<Guid>();

        public static bool gridModif = ticketwindow.Properties.Settings.Default.gridModif;

        public static bool fromLoadSyncAll = ticketwindow.Properties.Settings.Default.fromLoadSyncAll;

        public static ClassSync.Establishment Establishment { get; set; } 

        public static void loadFromDB ( )
        {
            if (ClassSync.connect)
            {
                Establishment = new ClassSync.Establishment().sel(IdEstablishment);

                Name = Establishment.Name;
            }
        }
        public static bool Bureau = ticketwindow.Properties.Settings.Default.Bureau;
    }
}
