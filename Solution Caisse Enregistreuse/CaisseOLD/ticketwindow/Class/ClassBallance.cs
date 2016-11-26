using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    class ClassBallance
    {
        public static bool _busy_0x15 { get; set; }
        public static bool _error_0x15 { get; set; }
        public static string error { get; set; }
        public static string prix { get; set; }
        public static string poinds { get; set; }
        public static string montant { get; set; }
        private static byte[] GetBytes(String str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        static string GetString(byte[] bytes)
        {

            return System.Text.Encoding.Default.GetString(bytes);
        }

        public static SerialPort port = new SerialPort("COM1", 2400, Parity.Odd, 7, StopBits.One);

       //   public static SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);


        public static void opn()
        {
            try
            {

                port.ReadTimeout = 100;

                port.WriteTimeout = 100;

                port.Handshake = Handshake.None;

                port.Open();

            }
            catch (Exception e)
            {
                new ClassFunctuon().showMessageSB(e.Message);
            }
        }
        public static void close()
        {
            port.Close();

        }

        public static void send(decimal price, decimal tare)
        {

            _busy_0x15 = false;
            _error_0x15 = false;
            error = "price 0.0";
            prix = "0.0";
            poinds = "0.0";
            montant = "0.0";

            if (price <= 0.0m)
                price = 0.01m;
                int a = Convert.ToInt16(Math.Truncate(price));

                int b = Convert.ToInt16((price - a) * 100);

                int t = Convert.ToInt16(tare * 1000);

                byte[] ba = new byte[4];
                byte[] bb = new byte[2];

                for (int i = 0; i < ba.Length; i++)
                    ba[i] = Convert.ToByte(int.Parse(a.ToString("D4")[i].ToString()) + 48);

                for (int i = 0; i < bb.Length; i++)
                    bb[i] = Convert.ToByte(int.Parse(b.ToString("D2")[i].ToString()) + 48);


                if (port.IsOpen)
                {

                    byte[] data_1 = {   0x04, 0x02, 0x30, 0x31,
                                       0x1b, 
                                       ba[0], ba[1], ba[2], ba[3], bb[0], bb[1], 
                                       0x1b, 0x03 };

                    byte[] data_2 = { 0x04, 0x05 };

                    byte[] data_3 = { 0x04 };
                    try
                    {
                        port.Write(data_1, 0, data_1.Length);
                    }
                    catch (Exception e)
                    {
                        error += e.Message;
                    }
                    byte[] get = new byte[1];

                    try
                    {
                        port.Read(get, 0, get.Length);
                    }
                    catch (Exception e)
                    {
                        error += e.Message;
                    }
                    if (get[0] == 0x06)
                    {
                        _error_0x15 = false;



                        try
                        {
                            port.Write(data_2, 0, data_2.Length);
                        }
                        catch (Exception e)
                        {
                            error += e.Message;
                        }
                        byte[] gets = new byte[28];

                        System.Threading.Thread.Sleep(200);

                        string s = "";

                        try
                        {
                            s = port.ReadExisting();
                        }
                        catch (Exception e)
                        {
                            error += e.Message;
                            new ClassLog(error);
                        }
                        gets = GetBytes(s);

                        if (gets[0] != 0x15)
                        {
                            _busy_0x15 = false;

                            string stroka = GetString(gets);

                            error += stroka + Environment.NewLine;

                            poinds = stroka.Substring(6, 5);

                            prix = stroka.Substring(12, 6);

                            montant = stroka.Substring(20, 5);

                            error += "POINDS=" + poinds + ",PRIX=" + prix + ",MONTANT=" + montant + Environment.NewLine;

                            try
                            {
                               // System.Threading.Thread.Sleep(100);
                                port.Write(data_3, 0, data_3.Length);
                            }
                            catch (Exception e)
                            {
                                error += e.Message;
                                new ClassLog(error);
                            }
                        }
                        else
                        {
                            _busy_0x15 = true;
                        }
                    }
                    else
                    {
                        _error_0x15 = true;
                    }
                
               
            }


        }
    }
}
