using System;
using System.Runtime.InteropServices;
using TicketWindow.Services;

namespace TicketWindow.Classes
{
    public class ClassDataTimeSrv
    {
        public static DateTime DateTimeFromSrv { get; set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref Systemtime time);

        public static bool GetDateTimeFromSrv()
        {
            try
            {
                DateTimeFromSrv = (DateTime) DbService.QueryResonse(" SELECT SYSDATETIME()")[0][0];
                var ti = Math.Abs(DateTimeFromSrv.Ticks - DateTime.Now.Ticks);
                return ti > 10000*60000;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetDateTimeFromSrv()
        {
            var utc = DateTimeFromSrv.AddHours(-Global.Config.Utc);
            var time = new Systemtime
                       {
                           wYear = (ushort) utc.Year,
                           wMonth = (ushort) utc.Month,
                           wDay = (ushort) utc.Day,
                           wHour = (ushort) (utc.Hour),
                           wMinute = (ushort) utc.Minute,
                           wSecond = (ushort) utc.Second
                       };
            var r = SetSystemTime(ref time);
            return r;
        }

        #region Nested type: SYSTEMTIME

        [StructLayout(LayoutKind.Sequential)]
        public struct Systemtime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        #endregion
    }
}