using System;

namespace TicketWindow.Classes
{
    internal class EuroConverter
    {
        public EuroConverter(decimal euro)
        {
            Euro = Convert.ToInt32(Math.Floor((Math.Abs(euro)*100)/100));
            Cent = Convert.ToInt32(Math.Abs(euro)*100%100);
        }

        public int Euro { get; set; }
        public int Cent { get; set; }
    }
}