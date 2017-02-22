using System;
using System.IO;


namespace Devis.Class
{
    class ClassLog
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\error.log";
        static string pathSQL = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\sql.log";
        public ClassLog(string text)
        {
            string log = Environment.NewLine + DateTime.Now.ToString() + "[" + text + "]" + Environment.NewLine;

            File.AppendAllText(path, log);

            ClassGlobalVar.error.Add(log);
        }

        public void ClassLogSQL(string text)
        {
            string log = Environment.NewLine + DateTime.Now.ToString() + "[" + text + "]" + Environment.NewLine;

            File.AppendAllText(pathSQL, log);

            ClassGlobalVar.error.Add(log);
        }

    }
}
