using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketWindow.Global;
using TicketWindow.Properties;

namespace TicketWindow.Services
{
    public static class TextService
    {
        public static string GetCloseWindowText()
        {
            var tickedWindowId = GlobalVar.TicketWindow != Guid.Empty ? GlobalVar.TicketWindow.ToString() : string.Empty;

            return Environment.NewLine +
                   Resources.LabelCashBox + " : " + Config.NameTicket + Environment.NewLine +
                   Resources.LabelNumberCheck + " : " + Config.NumberTicket + Environment.NewLine +
                   Resources.LabelUserName + " : " + Config.User + Environment.NewLine + Environment.NewLine +
                   "--------------------------------" + Environment.NewLine +
                   Resources.LabelOpenTotalTW + " : " + GlobalVar.TicketWindowG + Environment.NewLine +
                   Resources.LabelOpenLocal + " : " + tickedWindowId + Environment.NewLine;
        }
    }
}
