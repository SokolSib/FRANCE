using System.Diagnostics;
using TicketWindow.Global;

namespace TicketWindow.Exception
{
    public class ExceptionService
    {
        public static void Show(System.Exception ex)
        {
            LogService.LogTest(TraceLevel.Error, ex.ToString());

            var window = new ExceptionWindow(ex);
            window.ShowDialog();
        }
    }
}