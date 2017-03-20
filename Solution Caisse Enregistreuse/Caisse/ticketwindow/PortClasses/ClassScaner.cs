using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace TicketWindow.PortClasses
{
    class ClassScaner
    {
        public static SerialPort _serialPort;

        public static void open( )
        {
            _serialPort = new SerialPort("COM1");

            _serialPort.ReadTimeout = 500;

            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
        
        }


        public static void enabled ()
        {
            _serialPort.WriteLine("E");
        }

        public static void disabled()
        {
            _serialPort.WriteLine("D");
        }

        public static void close ()
        {
            _serialPort.Close();
        }

        public static string Read()
        {
                try
                {
                    string message =  _serialPort.ReadExisting();

                    return  message.Length > 2 ? message : null; 
                }
                catch (TimeoutException)
                {
                    return "error scan 1088";
                }
            
        }
    }
}
