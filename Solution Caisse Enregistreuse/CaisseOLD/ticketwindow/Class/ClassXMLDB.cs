using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    class ClassXMLDB
    {
        private static string getFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1); 
        }
        private static string fileName { get; set; }
        private static XDocument doc { get; set; }

        private static void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ClassSync.sync = true;

            ClassSync.XML_File x = ClassSync.XML_File.sel(fileName, ClassGlobalVar.user);

            if (x == null)
            {
                x = new ClassSync.XML_File();

                x.CustomerId = Guid.NewGuid();

                x.Data = doc.ToString();

                x.Date = DateTime.Now;

                x.Establishment_CustomerId = ClassGlobalVar.IdEstablishment;

                x.FileName = fileName;

                x.Upd = true;

                x.UserName = ClassGlobalVar.user;

                ClassSync.XML_File.ins(x);
            }
            else
            {
                x.Upd = true;

                x.FileName = fileName;

                x.Date = DateTime.Now;

                x.Data = doc.ToString();

                ClassSync.XML_File.mod(x);
            }
        }
        private static void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ClassSync.sync = false;
        }
    
        public static void saveDB(string path, XDocument d)
        {
            fileName = getFileNameFromPath(path);
            doc = d;
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();

        }

        public static void loadFromDB (string path)
        {

            ClassSync.XML_File x = ClassSync.XML_File.sel( getFileNameFromPath( path), ClassGlobalVar.user);

            if (x != null)
                System.IO.File.WriteAllText(path, x.Data);
        }

        public static void loadFromDBAll ()
        {
            loadFromDB(ClassGridGroup.path);
            loadFromDB(ClassGridGroup.path2);
            loadFromDB(ClassGridGroup.path3);
            loadFromDB(ClassGridGroup.path4);
            loadFromDB(ClassGridProduct.path);

        }
    }
}
