//using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibUsbDotNet.DeviceNotify;

namespace TicketWindow.PortClasses
{
    class ClassDrawer
    {

        //static CashDrawer m_Drawer = null;

        //public static void load ()
        //{
        //    string strLogicalName = "b";

        //    try
        //    {
               
        //        PosExplorer posExplorer = new PosExplorer();

        //        DeviceInfo deviceInfo = null;

        //        try
        //        {
        //            deviceInfo = posExplorer.GetDevice(DeviceType.CashDrawer, strLogicalName);
        //            m_Drawer = (CashDrawer)posExplorer.CreateInstance(deviceInfo);
        //        }
        //        catch 
        //        {
        //             return;
        //        }

        //        m_Drawer.Open();

        //        m_Drawer.Claim(1000);

        //        m_Drawer.DeviceEnabled = true;

        //    }
        //    catch (PosControlException)
        //    {
                
        //    }
        //}

        //public static void open()
        //{
         
        //    try
        //    {
        //        m_Drawer.OpenDrawer();

        //        while (m_Drawer.DrawerOpened == false)
        //        {
        //            System.Threading.Thread.Sleep(100);
        //        }

        //        m_Drawer.WaitForDrawerClose(10000, 2000, 100, 1000);

        //    }
        //    catch (PosControlException)
        //    {
        //    }
        
        //}
    }
}
