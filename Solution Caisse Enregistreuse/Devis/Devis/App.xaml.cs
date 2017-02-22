using System.Globalization;
using System.Threading;
using System.Windows;

namespace Devis
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR"); ;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR"); ;

           

        
            FrameworkElement.LanguageProperty.OverrideMetadata(

              typeof(FrameworkElement),

              new FrameworkPropertyMetadata(

                    System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            new Class.ClassSync().SyncAll();


            


            base.OnStartup(e);


        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Class.ClassProducts.saveLocal();
        }
    }
}
