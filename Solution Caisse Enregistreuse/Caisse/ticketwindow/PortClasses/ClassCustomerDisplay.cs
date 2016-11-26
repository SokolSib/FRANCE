using System;
using System.Diagnostics;
using System.IO.Ports;
using TicketWindow.Global;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.PortClasses
{
    public class ClassCustomerDisplay
    {
        private static readonly SerialPort Port = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);
        public static string Message { get; set; }

        private static bool Open()
        {
            try
            {
                Port.Open();

                if (!Port.IsOpen)
                    Message += "port is closed" + Environment.NewLine;
            }
            catch (System.Exception e)
            {
                Message += e.Message + " " + Environment.NewLine;
                LogService.LogText(TraceLevel.Error, Message);
                Message = " ";
            }

            return Port.IsOpen;
        }

        private static void Close()
        {
            Port.Close();
        }

        public static void WritePrice(string product, decimal qty, decimal prix)
        {
            var st1 = (Math.Round(qty, 3)) + "*" + Math.Round(prix, 2);
            var st2 = Math.Round((qty*prix), 2).ToString();
            var st2Format = st1 + "....................".Remove(0, (st1.Length + st2.Length) > 20 ? 20 : st1.Length + st2.Length) + st2;
            var st1Format = product + "                    ";

            if (Open())
            {
                Port.Write(new byte[] {0x1b, 0x40}, 0, 2);
                Port.Write(st1Format.Substring(0, 20).ToUpper());
                Port.Write(st2Format.Substring(0, 20).ToUpper());
                Close();
            }
        }

        public static void Hi()
        {
            if (Open())
            {
                Port.Write(new byte[] {0x1b, 0x40}, 0, 2);
                Port.Write(new byte[] {0x1f, 0x3a}, 0, 2);
                Port.Write(new byte[] {0x0c}, 0, 1);
                Port.Write(new byte[] {0x1f, 0x45, 0x00}, 0, 3);
                Port.Write("Bienvenue, la caisse est ouvert !!!");
                Port.Write(new byte[] {0x1f, 0x45, 0x0a}, 0, 3);
                Port.Write(new byte[] {0x1f, 0x3a}, 0, 2);
                Port.Write(new byte[] {0x1f, 0x5e, 0x0a, 0x64}, 0, 4);
                Close();
            }
        }

        public static void Bye()
        {
            if (Open())
            {
                Port.Write(new byte[] {0x1f, 0x3a}, 0, 2);
                Port.Write(new byte[] {0x0c}, 0, 1);
                Port.Write(new byte[] {0x1f, 0x45, 0x00}, 0, 3);
                Port.Write("Caisse est fermer !!!");
                Port.Write(new byte[] {0x1f, 0x45, 0x0a}, 0, 3);
                Port.Write(new byte[] {0x1f, 0x3a}, 0, 2);
                Port.Write(new byte[] {0x1f, 0x5e, 0x0a, 0x64}, 0, 4);
                Close();
            }
        }

        public static void TotalSum(decimal total)
        {
            var st2Sum = Math.Round(total, 2).ToString("0.00");

            var ttlSum = "Total:" + "....................".Remove(0, st2Sum.Length > 20 ? 20 : st2Sum.Length + 6) + st2Sum;

            if (Open())
            {
                Port.Write(new byte[] {0x1b, 0x40}, 0, 2);
                Port.Write(ttlSum.Substring(0, 20).ToUpper());
                Close();
            }
        }

        public static void OddMoney(decimal oddM, decimal total)
        {
            var st2Sum = Math.Round(oddM, 2).ToString("0.00");
            var st1Sum = Math.Round(total, 2).ToString("0.00");
            var ttlSum = "Recu:" + "....................".Remove(0, st1Sum.Length > 20 ? 20 : st1Sum.Length + 5) + st1Sum;
            var recSum = "Rendu:" + "....................".Remove(0, st2Sum.Length > 20 ? 20 : st2Sum.Length + 6) + st2Sum;

            if (Open())
            {
                Port.Write(new byte[] {0x1b, 0x40}, 0, 2);
                Port.Write(ttlSum.Substring(0, 20).ToUpper());
                Port.Write(recSum.Substring(0, 20).ToUpper());
                Close();
            }
        }

        public static void Reste(decimal reste, decimal total)
        {
            var st2Sum = Math.Round(reste, 2).ToString("0.00");
            var st1Sum = Math.Round(total, 2).ToString("0.00");
            var ttlSum = "Total:" + "....................".Remove(0, st1Sum.Length > 20 ? 20 : st1Sum.Length + 6) + st1Sum;
            var recSum = "Reste:" + "....................".Remove(0, st2Sum.Length > 20 ? 20 : st2Sum.Length + 6) + st2Sum;

            if (Open())
            {
                Port.Write(new byte[] {0x1b, 0x40}, 0, 2);
                Port.Write(ttlSum.Substring(0, 20).ToUpper());
                Port.Write(recSum.Substring(0, 20).ToUpper());
                Close();
            }
        }
    }
}