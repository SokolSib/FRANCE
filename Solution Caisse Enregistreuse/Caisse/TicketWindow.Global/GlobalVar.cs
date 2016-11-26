using System;
using System.Collections.Generic;

namespace TicketWindow.Global
{
    public class GlobalVar
    {
        public static Guid TicketWindowG;
        public static Guid TicketWindow;
        public static bool IsBreak = false;
        public static List<string> Messages = new List<string>();
        public static List<string> Errors = new List<string>();
        public static bool IsOpen { get; set; }
    }
}