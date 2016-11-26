using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ticketwindow.Class
{
    class ClassETC_fun
    {
        public static void wm_sound(string path)
        {
            System.Media.SoundPlayer  mplayer = new System.Media.SoundPlayer();

            mplayer.SoundLocation = (System.AppDomain.CurrentDomain.BaseDirectory + path);

            mplayer.Play();
        }
        public static decimal renduCalc()
        {
            decimal rendu = 0.0m;

            decimal noRendu = 0.0m;

            decimal sum = ClassBond.getSumMoney();

            foreach (var tm in ClassSync.TypesPayDB.t)
            {
                rendu += tm.Rendu_Avoir ?? false ? ClassBond.getMoneyFromType(tm) : 0.0m;

                noRendu += !tm.Rendu_Avoir ?? false ? ClassBond.getMoneyFromType(tm) : 0.0m;
            }

            return sum - noRendu < 0 ? -rendu : sum - noRendu - rendu;
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

        public static decimal calc_total(ClassProducts.product p, decimal qty)
        {
            return Math.Round(p.price * qty, 2);//+ p.price * qty * (ClassTVA.getTVA(p.tva) / 100), 2);
        }

        public static decimal testFromNuul(object o)
        {
            return (string.IsNullOrEmpty(o.ToString()) ? 0.0m : (decimal)o);
        }
        public static object GetParents(Object element, int parentLevel)
        {
            object returnValue = null;
            if (element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is Window)
                    returnValue = ((FrameworkElement)element).Parent;
                else
                    returnValue = GetParents(((FrameworkElement)element).Parent, parentLevel + 1);
            }
            return returnValue;
        }
        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}
