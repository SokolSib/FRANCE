using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.UpdateDB
{
    /// <summary>
    ///     Логика взаимодействия для W_updateDB.xaml
    /// </summary>
    public partial class WUpdateDb : Window
    {
        public int SelectGroup = -1;

        public WUpdateDb()
        {
            InitializeComponent();
        }

        public void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var rec = RepositoryHistoryChangeProduct.Document.GetXElements("HistoryChangeProducts", "rec");

            var xElements = rec as XElement[] ?? rec.ToArray();
            var first = xElements.FirstOrDefault();

            if (first != null)
            {
                SelectGroup = first.GetXElementValue("group").ToInt();
                dataGrid1.DataContext = xElements.Where(l => l.GetXElementValue("group") == first.GetXElementValue("group"));

                var p = xElements.Count(l => l.GetXElementValue("group") == (SelectGroup + 1).ToString());
                xP.IsEnabled = p != 0;
                xN.IsEnabled = xElements.Count(l => l.GetXElementValue("group") == (SelectGroup - 1).ToString()) != 0;
            }

            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += (o, a) => { FunctionsService.Click(o); };
        }
    }
}