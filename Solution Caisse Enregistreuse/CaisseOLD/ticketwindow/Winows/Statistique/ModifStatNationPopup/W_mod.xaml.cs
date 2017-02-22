
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ticketwindow.Winows.Statistique.ModifStatNationPopup
{
    /// <summary>
    /// Логика взаимодействия для W_ins.xaml
    /// </summary>
    public partial class W_mod : Window
    {
        public List<Class.ClassSync.Stat.StatNationPopup> SNP;

        public Guid CustomerId { get; set; }

        public W_mod()
        {
            InitializeComponent();
          //  xNameNation.AutoCompleteManager.DataProvider = new SimpleStaticDataProvider(Class.ClassCountrys.listStringCountrys);
          //  xNameNation.AutoCompleteManager.AutoAppend = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Name = xNameNation.Text;
            string QTY = xQTY.Text;

            if (Name.Length == 0)
            {
                new Class.ClassFunctuon().showMessageTime("Пустое знаение ");
            }

            else
            {
                if (SNP.FindAll(l => l.NameNation == Name).Count > 0 )
                {
                    new Class.ClassFunctuon().showMessageTime("Такое имя сущ-ет ");
                }
                else
                {
                    int qty = 0;

                    if (int.TryParse(QTY, out qty))
                    {




                        int indx = SNP.FindIndex(l => l.IdCustomer == this.CustomerId);

                        if (indx != -1)
                        {
                            SNP[indx].NameNation = Name;
                            SNP[indx].QTY = qty;
                            Class.ClassSync.Stat.StatNationPopup.mod(SNP[indx]);
                        }
                        else
                        {
                            new Class.ClassFunctuon().showMessageTime("Erroer ");
                        }

                        CollectionViewSource.GetDefaultView((this.Owner as W_Grid).dataGrid.ItemsSource).Refresh();

                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(W_Stat))
                            {
                                W_Stat w = (window as W_Stat);

                                w.reload();
                            }
                        }

                        this.Close();
                    }
                    else
                    {
                        new Class.ClassFunctuon().showMessageTime("Неверое значние поля QTY ");
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
