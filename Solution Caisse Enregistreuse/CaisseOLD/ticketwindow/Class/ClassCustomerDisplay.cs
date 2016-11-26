using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    public class ClassCustomerDisplay
    {
        private static SerialPort port = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);
        public static string message { get; set; }
        private static bool open()
        {

          

            try
            {

                port.Open();

                if (!port.IsOpen)
                {
                    message += "port is closed" + Environment.NewLine;
                }


            }
            catch (Exception e)
            {
                message += e.Message + " " + Environment.NewLine;
                new ClassLog(message);
                message = " ";
            }
         
            return port.IsOpen;
        }
        private static void close()
        {
            port.Close();
        }
        public static void writePrice(string product, decimal qty, decimal prix)
        {
            string st1 = (Math.Round(qty, 3)) + "*" + Math.Round(prix, 2);

            string st2 = Math.Round((qty * prix), 2).ToString();

            string _2st = st1 + "....................".Remove(0, (st1.Length + st2.Length) > 20 ? 20 : st1.Length + st2.Length) + st2;

            string _1st = product + "                    ";
            if (open())
            {
                port.Write(new byte[] { 0x1b, 0x40 }, 0, 2);
                port.Write(_1st.Substring(0, 20).ToUpper());
                port.Write(_2st.Substring(0, 20).ToUpper());
                close();
            }
        }
        public static void hi()
        {
            if (open())
            {
                port.Write(new byte[] { 0x1b, 0x40 }, 0, 2);
                port.Write(new byte[] { 0x1f, 0x3a }, 0, 2);
                port.Write(new byte[] { 0x0c }, 0, 1);
                port.Write(new byte[] { 0x1f, 0x45, 0x00 }, 0, 3);
                port.Write("Bienvenue, la caisse est ouvert !!!");
                port.Write(new byte[] { 0x1f, 0x45, 0x0a }, 0, 3);
                port.Write(new byte[] { 0x1f, 0x3a }, 0, 2);
                port.Write(new byte[] { 0x1f, 0x5e, 0x0a, 0x64 }, 0, 4);
                close();
            }
        }

        public static void bye()
        {
            if (open())
            {
                port.Write(new byte[] { 0x1f, 0x3a }, 0, 2);
                port.Write(new byte[] { 0x0c }, 0, 1);
                port.Write(new byte[] { 0x1f, 0x45, 0x00 }, 0, 3);
                port.Write("Caisse est fermer !!!");
                port.Write(new byte[] { 0x1f, 0x45, 0x0a }, 0, 3);
                port.Write(new byte[] { 0x1f, 0x3a }, 0, 2);
                port.Write(new byte[] { 0x1f, 0x5e, 0x0a, 0x64 }, 0, 4);
                close();
            }
        }


        public static void total_sum(decimal total)
        {
            string st2_sum = Math.Round(total, 2).ToString("0.00");

            string ttl_sum = "Total:" + "....................".Remove(0, st2_sum.Length > 20 ? 20 : st2_sum.Length + 6) + st2_sum;

            if (open())
            {
                port.Write(new byte[] { 0x1b, 0x40 }, 0, 2);
                port.Write(ttl_sum.Substring(0, 20).ToUpper());
                close();
            }
        }

        public static void odd_money(decimal odd_m, decimal total)
        {
            string st2_sum = Math.Round(odd_m, 2).ToString("0.00");

            string st1_sum = Math.Round(total, 2).ToString("0.00");

            string ttl_sum = "Recu:" + "....................".Remove(0, st1_sum.Length > 20 ? 20 : st1_sum.Length + 5) + st1_sum;
            
            string rec_sum = "Rendu:" + "....................".Remove(0, st2_sum.Length > 20 ? 20 : st2_sum.Length + 6) + st2_sum;            

            if (open())
            {
                port.Write(new byte[] { 0x1b, 0x40 }, 0, 2);
                port.Write(ttl_sum.Substring(0, 20).ToUpper());
                port.Write(rec_sum.Substring(0, 20).ToUpper());
                close();
            }
        }

        public static void Reste (decimal reste, decimal total)
        {
            string st2_sum = Math.Round(reste, 2).ToString("0.00");

            string st1_sum = Math.Round(total, 2).ToString("0.00");

            string ttl_sum = "Total:" + "....................".Remove(0, st1_sum.Length > 20 ? 20 : st1_sum.Length + 6) + st1_sum;

            string rec_sum = "Reste:" + "....................".Remove(0, st2_sum.Length > 20 ? 20 : st2_sum.Length + 6) + st2_sum;

            if (open())
            {
                port.Write(new byte[] { 0x1b, 0x40 }, 0, 2);
                port.Write(ttl_sum.Substring(0, 20).ToUpper());
                port.Write(rec_sum.Substring(0, 20).ToUpper());
                close();
            }
        }
    }
}
