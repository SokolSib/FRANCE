using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Devis.Class
{
    class ClassF
    {
        string primarypath = "Data\\Anahit\\img_products"; //System.Web.Hosting.HostingEnvironment.MapPath("~") + "img_products";// "D:\\Donnees\\Anahit\\img_products";
        const string findFiles = "img_";

        public string createDirFromCustomerId(Guid customerIdProduct)
        {
            return primarypath + "\\" + customerIdProduct.ToString().Replace("-", "").Substring(0, 3) + "\\" + customerIdProduct.ToString().Replace("-", "").Substring(3, 3) + "\\" + customerIdProduct.ToString().Replace("-", "").Substring(6, 3) + "\\" + customerIdProduct.ToString().Replace("-", "").Substring(9);
        }
        public string[] getPathImg(Guid customerIdProduct)
        {
            string[] res;
            string path = createDirFromCustomerId(customerIdProduct);
            if (Directory.Exists(Path.GetFullPath(path)))
            {
                FileInfo[] files = new DirectoryInfo(path).GetFiles(findFiles + "*");


                if (files.Length > 0)
                {
                    res = new string[files.Length];

                    int i = 0;

                    while (i < res.Length)
                    {
                        res[i] = files[i].FullName;

                        i++;
                    }
                }
                else
                {
                    res = new string[1];

                    res[0] = "/img/no_image.jpg";
                }
            }
            else
            {
                res = new string[1];

                res[0] = "/img/no_image.jpg";
            }
            return res;
        }


        public static string path = @"\\SRV-ANAHIT\ANAHIT";

        public static string pathTo = @"Data\Anahit\";

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
        /*   AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsIdentity identity = new WindowsIdentity("administrateur", "@zerty!23");
            WindowsImpersonationContext context = identity.Impersonate();
            */
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

           

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {

                string temppath = Path.Combine(destDirName, file.Name);
                if (!file.Exists)
                    file.CopyTo(temppath, true);
            }
        }

        public static Window findWindow(string nameWindow)
        {
            try
            {
                return Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.Name == nameWindow);
            }
            catch
            {
                return null;
            }
        }

        public static void wm_sound(string path)
        {
            System.Media.SoundPlayer mplayer = new System.Media.SoundPlayer();

            mplayer.SoundLocation = (System.AppDomain.CurrentDomain.BaseDirectory + path);

            mplayer.Play();
        }

    }
}
