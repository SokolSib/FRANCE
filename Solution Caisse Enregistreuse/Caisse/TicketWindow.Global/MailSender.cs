using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace TicketWindow.Global
{
    public class MailSender
    {
        private const string Mail = "support@ren-it.com";
        private const string SmtpHost = "smtp.1und1.de";
        private const int SmtpPort = 25;
        private const string SmtUser = "support@ren-it.com";
        private const string SmtpPassword = "---@@zerty123@";
        private const bool SmtpIsSsl = true;

        public static void Send(string titleText, string messageText)
        {
            Task<bool>.Factory
                .StartNew(
                    () =>
                    {
                    //    SendToMail(Mail, titleText, messageText);
                    //    SendToMail("denisalyona@rambler.ru", titleText, messageText);
                        return true;
                    });
        }

        private static void SendToMail(string mail, string titleText, string messageText)
        {
            var message = new MailMessage(Mail, mail)
                                  {
                                      Subject = titleText,
                                      Body = messageText
                                  };
            var client = new SmtpClient(SmtpHost, SmtpPort)
                         {
                             EnableSsl = SmtpIsSsl,
                             Credentials = new NetworkCredential(SmtUser, SmtpPassword),
                             DeliveryMethod = SmtpDeliveryMethod.Network
                         };
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}
