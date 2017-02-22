using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace TicketWindow.Winows.AdditionalClasses
{
    public static class IsRun
    {
        private const int SwShowmaximized = 3;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(int hWnd, int nCmdShow);

        [DllImport("User32.dll")]
        private static extern int FindWindow(string className, string windowName);

        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(int hWnd);

        public static bool GetIsRun()
        {
            var hWnd = FindWindow(null, "Caisse");
            if (hWnd > 0)
            {
                ShowWindowAsync(hWnd, SwShowmaximized);
                SetForegroundWindow(hWnd);
                Application.Current.Shutdown();
                return true;
            }

            return false;
        }
    }
}