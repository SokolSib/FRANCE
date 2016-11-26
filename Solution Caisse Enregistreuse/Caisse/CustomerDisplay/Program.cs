using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDisplay
{
    class Program
    {
        public class ClassCustomerDisplay
        {
            public static byte[] GetBytes(String str)
            {
                return Encoding.ASCII.GetBytes(str);
            }
            public static string GetString(byte[] bytes)
            {

                return System.Text.Encoding.Default.GetString(bytes);
            }
            public static string backSpace = "<BS>";
            public static string clear = "<CLR>";
            public static string error { get; set; }
            public ClassCustomerDisplay(string _1st, string _2st)
            {
                SerialPort port = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);

                try
                {

                    port.Open();
                    if (port.IsOpen)
                    {

                      // port.Write(new byte[] { 0x1f, 0x3a }, 0, 2);
                   //     port.Write(new byte[] { 0x0c }, 0, 1);
                    //   port.Write(new byte[] { 0x1f, 0x45, 0x00 }, 0, 3);
                    //    port.Write("Bonjjour !!!");
                    //    port.Write(new byte[] { 0x1f, 0x45, 0x0a }, 0, 3);
                    //   port.Write(new byte[] { 0x1f, 0x3a }, 0, 2);
                    //    port.Write(new byte[] { 0x1f, 0x5e, 0x0a, 0x64 }, 0, 4);




                       // port.Write("Bonjjour !!!");
                      // port.Write(new byte[] { 0x0c }, 0, 1);

                      // port.Write(new byte[] { 0x1b, 0x26, 0x01 }, 0, 3);
                      // port.Write(new byte[] { 0x20, 0x20, 0x05 }, 0, 3);
                      // port.Write(new byte[] { 0x92, 0xaa, 0xff, 0xaa, 0xa4 }, 0, 5);

                      // port.Write(new byte[] { 0x1b, 0x26, 0x01 }, 0, 3);
                      // port.Write(new byte[] { 0x20, 0x20, 0x05 }, 0, 3);
                      //  port.Write(new byte[] { 0x92, 0xaa, 0xff, 0xaa, 0xa4 }, 0, 5);

                       // port.Write("Bonjjour !!!");
                        //   port.Write(new byte[] { 0x0b }, 0, 1);
                        //   port.WriteLine("                                            ");
                         // port.Write(new byte[] { 0x0c }, 0, 1);
                        //  port.Write(new byte[] { 0x1b, 0x13 }, 0, 2);
                         // port.WriteLine("PRODUCTPROPR PRODUCTPROPR PRODUCTPROPR PRODUCTPROPR PRODUCTPROPR");


                        port.Write(new byte[] { 0x0c }, 0, 1);
                        port.Write(_1st.Substring(0,20).ToUpper());
                        port.Write(_2st.Substring(0,20).ToUpper());

                      //  port.Write(new byte[] { 0x80 }, 0, 1);
             


                  //      port.Write(new byte[] {0x0b}, 0, 1);
//port.WriteLine("$");
                

                        // port.Write(new byte[] { 0x1f, 0x0d }, 0, 2);
                        //  port.WriteLine("100,00");
                        //  port.Write(GetBytes("Hello"), 0, GetBytes("Hello").Length);

               
                        port.Close();
                    }
                    else
                    {
                        error += "port is closed" + Environment.NewLine;
                    }

                }
                catch (Exception e)
                {
                    error += e.Message + " " + Environment.NewLine;
                }

            }
        }
        static void Main(string[] args)
        {

            decimal qty = 10.300m;
            decimal prix = 15000.50m;

            string st1 = (   Math.Round( qty,3)   ) + "*" + Math.Round( prix,2);




            string st2 = Math.Round((qty * prix),2).ToString();

            string r = st1 + "....................".Remove(0, (st1.Length + st2.Length) > 20 ? 20 : st1.Length + st2.Length) + st2;
            


            new ClassCustomerDisplay("0123456789012345678900000000000", r);

            Console.WriteLine(ClassCustomerDisplay.error);

        }
    }
}
