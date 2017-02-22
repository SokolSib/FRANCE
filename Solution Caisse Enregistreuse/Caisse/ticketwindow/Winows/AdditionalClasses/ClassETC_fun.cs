using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Media;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.AdditionalClasses
{
    internal class ClassEtcFun
    {
        public static void WmSound(string path)
        {
            var mplayer = new SoundPlayer {SoundLocation = (AppDomain.CurrentDomain.BaseDirectory + path)};
            mplayer.Play();
        }

        public static decimal RenduCalc()
        {
            var rendu = 0.0m;
            var noRendu = 0.0m;
            var sum = RepositoryCurrencyRelations.GetSumMoney();

            foreach (var tm in RepositoryTypePay.TypePays)
            {
                rendu += tm.RenduAvoir ?? false ? RepositoryCurrencyRelations.GetMoneyFromType(tm) : 0.0m;
                noRendu += !tm.RenduAvoir ?? false ? RepositoryCurrencyRelations.GetMoneyFromType(tm) : 0.0m;
            }

            return sum - noRendu < 0 ? -rendu : sum - noRendu - rendu;
        }

        public static Window FindWindow(string nameWindow)
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

        public static decimal CalcTotal(ProductType p, decimal qty)
        {
            return Math.Round(p.Price*qty, 2); //+ p.price * qty * (ClassTVA.getTVA(p.tva) / 100), 2);
        }

        public static decimal TestFromNuul(object o)
        {
            return (string.IsNullOrEmpty(o.ToString()) ? 0.0m : (decimal) o);
        }

        public static object GetParents(object element, int parentLevel)
        {
            object returnValue = null;
            var frameworkElement = element as FrameworkElement;
            if (frameworkElement != null)
            {
                if (frameworkElement.Parent is Window)
                    returnValue = frameworkElement.Parent;
                else
                    returnValue = GetParents(frameworkElement.Parent, parentLevel + 1);
            }
            return returnValue;
        }

        public static Window GetParentWindow(DependencyObject child)
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            var parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            return GetParentWindow(parentObject);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    var children = child as T;
                    if (children != null)
                        yield return children;

                    foreach (var childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}