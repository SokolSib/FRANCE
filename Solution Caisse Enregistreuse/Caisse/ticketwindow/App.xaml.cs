using System.Threading;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using TicketWindow.Exception;
using TicketWindow.Properties;

namespace TicketWindow
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex;

        /// <summary>
        /// Проверка что прога уже запущена.
        /// </summary>
        private static bool InstanceCheck()
        {
            bool isNew;
            _mutex = new Mutex(true, "TicketWindow", out isNew);
            return isNew;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += (s, a) =>
                                                    {
                                                        ExceptionService.Show(a.Exception);
                                                        a.Handled = false;
                                                    };
            // Прога уже запущена
            if (!InstanceCheck())
                Current.Shutdown();
            else
            {
                PortClasses.ClassScaner.open();
                PortClasses.ClassScaner.enabled();
                //PortClasses.ClassDrawer.load();

                base.OnStartup(e);
            }
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            if (InstanceCheck())
            {
                PortClasses.ClassScaner.disabled();
                PortClasses.ClassScaner.close();
                Settings.Default.Save();
            }
        }
    }
}