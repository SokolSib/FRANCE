using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using TicketWindow.Global;
using TicketWindow.Services;

namespace TicketWindow.PortClasses
{
    internal class ClassBallance
    {
        public static SerialPort Port = new SerialPort("COM1", 2400, Parity.Odd, 7, StopBits.One);
        public static bool Busy_0X15 { get; set; }
        public static bool Error_0X15 { get; set; }
        public static string Error { get; set; }
        public static string Prix { get; set; }
        public static string Poinds { get; set; }
        public static string Montant { get; set; }

        private static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private static string GetString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        //   public static SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);


        public static void Opn()
        {
            try
            {
                Port.ReadTimeout = 100;
                Port.WriteTimeout = 100;
                Port.Handshake = Handshake.None;
                Port.Open();
            }
            catch (System.Exception e)
            {
                FunctionsService.ShowMessageSb(e.Message);
            }
        }

        public static void Close()
        {
            Port.Close();
        }

        public static void Send(decimal price, decimal tare)
        {
            Busy_0X15 = false;
            Error_0X15 = false;
            Error = "price 0.0";
            Prix = "0.0";
            Poinds = "0.0";
            Montant = "0.0";

            if (price <= 0.0m)
                price = 0.01m;
            int a = Convert.ToInt16(Math.Truncate(price));

            int b = Convert.ToInt16((price - a)*100);

            var ba = new byte[4];
            var bb = new byte[2];

            for (var i = 0; i < ba.Length; i++)
                ba[i] = Convert.ToByte(int.Parse(a.ToString("D4")[i].ToString()) + 48);

            for (var i = 0; i < bb.Length; i++)
                bb[i] = Convert.ToByte(int.Parse(b.ToString("D2")[i].ToString()) + 48);
            
            if (Port.IsOpen)
            {
                byte[] data1 =
                {
                    0x04, 0x02, 0x30, 0x31,
                    0x1b,
                    ba[0], ba[1], ba[2], ba[3], bb[0], bb[1],
                    0x1b, 0x03
                };

                byte[] data2 = {0x04, 0x05};

                byte[] data3 = {0x04};
                try
                {
                    Port.Write(data1, 0, data1.Length);
                }
                catch (System.Exception e)
                {
                    Error += e.Message;
                }
                var get = new byte[1];

                try
                {
                    Port.Read(get, 0, get.Length);
                }
                catch (System.Exception e)
                {
                    Error += e.Message;
                }
                if (get[0] == 0x06)
                {
                    Error_0X15 = false;

                    try
                    {
                        Port.Write(data2, 0, data2.Length);
                    }
                    catch (System.Exception e)
                    {
                        Error += e.Message;
                    }

                    Thread.Sleep(200);

                    var s = "";

                    try
                    {
                        s = Port.ReadExisting();
                    }
                    catch (System.Exception e)
                    {
                        Error += e.Message;
                        LogService.LogText(TraceLevel.Error, Error);
                    }

                    var gets = GetBytes(s);

                    if (gets[0] != 0x15)
                    {
                        Busy_0X15 = false;
                        var stroka = GetString(gets);
                        Error += stroka + Environment.NewLine;
                        Poinds = stroka.Substring(6, 5);
                        Prix = stroka.Substring(12, 6);
                        Montant = stroka.Substring(20, 5);
                        Error += "POINDS=" + Poinds + ",PRIX=" + Prix + ",MONTANT=" + Montant + Environment.NewLine;

                        try
                        {
                            // System.Threading.Thread.Sleep(100);
                            Port.Write(data3, 0, data3.Length);
                        }
                        catch (System.Exception e)
                        {
                            Error += e.Message;
                            LogService.LogText(TraceLevel.Error, Error);
                        }
                    }
                    else
                    {
                        Busy_0X15 = true;
                    }
                }
                else
                {
                    Error_0X15 = true;
                }
            }
        }
    }
}