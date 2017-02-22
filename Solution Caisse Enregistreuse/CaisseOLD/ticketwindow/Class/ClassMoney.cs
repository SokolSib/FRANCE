using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    class ClassMoney
    {
        public int Euro { get; set; }
        public int Cent { get; set; }

        public ClassMoney( decimal euro)
        {
            this.Euro = Convert.ToInt32 ( Math.Floor(( Math.Abs( euro)*100)/100) ) ;
            this.Cent = Convert.ToInt32(  Math.Abs( euro)*100 % 100);
        }
    }

}
