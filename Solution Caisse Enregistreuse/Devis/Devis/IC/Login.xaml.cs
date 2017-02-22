using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Devis.IC
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public class WebClientEx : WebClient
        {
            public CookieContainer CookieContainer { get; private set; }

            public WebClientEx()
            {
                CookieContainer = new CookieContainer();
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                if (request is HttpWebRequest)
                {
                    (request as HttpWebRequest).CookieContainer = CookieContainer;
                }
                return request;
            }
        }
        private static string GetSourceForMyShowsPage(string usr, string psw)
        {
            using (var client = new WebClientEx())
            {
                var values = new NameValueCollection
            {
                { "login_name", usr},
                { "login_pass",psw },
            };
                // Authenticate
                client.UploadValues("http://admin.anahit.fr/Account/Login", values);
                // Download desired page
                return client.DownloadString("http://admin.anahit.fr/");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string usr = this.usr.Text;
            string psw = this.pswd.Password;

           // var shows = GetSourceForMyShowsPage(usr, psw);

            MessageBox.Show("ok");
        }
    }
}
