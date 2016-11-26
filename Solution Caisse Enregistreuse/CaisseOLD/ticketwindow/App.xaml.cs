using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ticketwindow.Class;

namespace ticketwindow
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR"); ;

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR"); ;



            FrameworkElement.LanguageProperty.OverrideMetadata(

              typeof(FrameworkElement),

              new FrameworkPropertyMetadata(

                    System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
         

        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ticketwindow.Properties.Settings.Default.Save();
        }

    }
}
