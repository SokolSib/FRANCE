using System.Windows;
using TicketWindow.Exception;
using TicketWindow.Properties;

namespace TicketWindow
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += (s, a) =>
                                                    {
                                                        ExceptionService.Show(a.Exception);
                                                        a.Handled = false;
                                                    };

            base.OnStartup(e);
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}