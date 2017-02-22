using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ticketwindow.Class
{
    class ClassLog
    {
        static string  path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\error.log";
         static string  pathSQL = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\sql.log";
        public ClassLog (string text)
        {
            string log = Environment.NewLine + DateTime.Now.ToString() + "[" + text +"]" + Environment.NewLine;

            try
            {

                File.AppendAllText(path, log);

                ClassGlobalVar.error.Add(log);
            }
            catch
            {

            }
        }

        public void ClassLogSQL(string text)
        {
            string log = Environment.NewLine + DateTime.Now.ToString() + "[" + text + "]" + Environment.NewLine;

            File.AppendAllText(pathSQL, log);

            ClassGlobalVar.error.Add(log);
        }

    }


}
