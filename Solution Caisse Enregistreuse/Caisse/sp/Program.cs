using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp
{
    class ClassBallance
    {
        public bool _busy_0x15 { get; set; }
        public bool _error_0x15 { get; set; }
        public string error { get; set; }
        public string prix { get; set; }
        public string poinds { get; set; }
        public string montant { get; set; }
        private byte[] GetBytes(String str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        static string GetString(byte[] bytes)
        {

            return System.Text.Encoding.Default.GetString(bytes);
        }

        
        public ClassBallance(decimal price, decimal tare)
        {

            SerialPort port = new SerialPort("COM1", 2400, Parity.Odd, 7, StopBits.One);

            //                port.ReadTimeout= 2500;

            //               port.WriteTimeout = 2050;

            port.Handshake = Handshake.None;

            port.Open();


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

                port.Write(data_1, 0, data_1.Length);
                byte[] get = new byte[1];
                port.Read(get, 0, get.Length);

                if (get[0] == 0x06)
                {
                    _error_0x15 = false;

                    port.Write(data_2, 0, data_2.Length);

                    byte[] gets = new byte[28];

                    //     int count =  28;

                    //      byte[] getw = new byte[28];

                    System.Threading.Thread.Sleep(2000);

                    string s = port.ReadExisting();

                    //  port.Read(gets, 0, gets.Length);

                    gets = GetBytes(s);


                    if (gets[0] != 0x15)
                    {

                        _busy_0x15 = false;

                        string stroka = GetString(gets);

                        error += stroka + Environment.NewLine;

                        poinds = stroka.Substring(6, 5);

                        prix = stroka.Substring(12, 6);

                        montant = stroka.Substring(20, 6);

                        error += "POINDS=" + poinds + ",PRIX=" + prix + ",MONTANT=" + montant + Environment.NewLine;

                        port.Write(data_3, 0, data_3.Length);
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
            port.Close();



         

        }


        static void Main(string[] args)
        {
            new ClassBallance(12.12m, 1);
        }
    }
}
