using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibUsbDotNet.Main;
using LibUsbDotNet;
using System.Threading;
namespace ticketwindow.Class
{
    class ClassUsbTicket
    {
     
        internal class ReadWrite
        {
            public static UsbDevice MyUsbDevice;

            #region SET YOUR USB Vendor and Product ID!

            public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(0x1504, 0x001f);

            #endregion

            public static void open()
            {
                ErrorCode ec = ErrorCode.None;

                try
                {
                    MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
                    if (MyUsbDevice == null) throw new Exception("Device Not Found.");
                    IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        wholeUsbDevice.SetConfiguration(1);
                        wholeUsbDevice.ClaimInterface(0);
                    }
                
                    UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
                    byte[] bytesToSend = { 0x1b, 0x70, 0x00, 0x19, 0xff };

                    int bytesWritten;
                    ec = writer.Write(bytesToSend, 2000, out bytesWritten);
                    if (ec != ErrorCode.None) throw new Exception(UsbDevice.LastErrorString);

             

                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine((ec != ErrorCode.None ? ec + ":" : String.Empty) + ex.Message);
                }
                finally
                {
                    if (MyUsbDevice != null)
                    {
                        if (MyUsbDevice.IsOpen)
                        {
                            IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                            if (!ReferenceEquals(wholeUsbDevice, null))
                            {
                                // Release interface #0.
                                wholeUsbDevice.ReleaseInterface(0);
                            }

                            MyUsbDevice.Close();
                        }
                        MyUsbDevice = null;
                        UsbDevice.Exit();

                    }
                  
                }
            }
        }
    }
}
