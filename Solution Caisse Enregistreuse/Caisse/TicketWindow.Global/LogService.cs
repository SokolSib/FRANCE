using System;
using System.Diagnostics;
using System.IO;

namespace TicketWindow.Global
{
    public static class LogService
    {
        private static readonly string PathTest = AppDomain.CurrentDomain.BaseDirectory + @"\Data\TestError.log";
        private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + @"\Data\error.log";
        private static readonly string PathSql = AppDomain.CurrentDomain.BaseDirectory + @"\Data\sql.log";

        public static string EstablishmentInfo { get; set; }

        public static void LogTest(TraceLevel level, string text)
        {
            WriteInFileFormatText(level, PathTest, text);
        }

        public static void LogText(TraceLevel level, string text)
        {
            WriteInFileFormatText(level, Path, text);
        }

        public static void Log(TraceLevel level, int code, string text = null)
        {
            var textMessage = string.Format("Error code {0:0000}.", code);

            if (!string.IsNullOrEmpty(text))
                textMessage = string.Format("{0} {1}", textMessage, text);

            WriteInFileFormatText(level, Path, textMessage);
        }

        public static void SqlLog(TraceLevel level, string text)
        {
            WriteInFileFormatText(level, PathSql, text);
        }

        private static void WriteInFileFormatText(TraceLevel level, string fileName, string text)
        {
            var log = string.Format("{0} - {1} [{2}]{3}", DateTime.Now, level, text, Environment.NewLine);

            try
            {
                File.AppendAllText(fileName, log);
                GlobalVar.Errors.Add(log);


                var info = "CONFIG DATA" + "\r\n" +
                           "User = " + Config.User + "\r\n" +
                           "Name = " + Config.Name + "\r\n" +
                           "ConnectionString = " + Config.ConnectionString + "\r\n" +
                           "IsUseServer = " + Config.IsUseServer + "\r\n" +
                           "IdEstablishment = " + Config.IdEstablishment + "\r\n" +
                           "CustomerId = " + Config.CustomerId + "\r\n" +
                           "IdEstablishmentGros = " + Config.IdEstablishmentGros + "\r\n" +
                           "NameTicket = " + Config.NameTicket + "\r\n" +
                           "Language = " + Config.Language + "\r\n" +
                           "Utc = " + Config.Utc + "\r\n" +
                           "NumberTicket = " + Config.NumberTicket + "\r\n" +
                           "AppPath = " + Config.AppPath + "\r\n" +
                           "GridModif = " + Config.GridModif + "\r\n" +
                           "FromLoadSyncAll = " + Config.FromLoadSyncAll + "\r\n" +
                           "Bureau = " + Config.Bureau + "\r\n" +
                           "-----------------------------------------\r\n\r\n";

                if (!string.IsNullOrEmpty(EstablishmentInfo))
                    info += "ESTABLISHMENT INFO\r\n" + EstablishmentInfo + "\r\n-----------------------------------------\r\n\r\n";

                MailSender.Send(level + " - Caisse error " + Config.Name + " " + Config.User, info + log);
            }
            catch
            {
                // ignored
            }
        }
    }
}