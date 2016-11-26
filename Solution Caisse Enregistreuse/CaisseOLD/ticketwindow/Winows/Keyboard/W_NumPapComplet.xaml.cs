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
using ticketwindow.Class;

namespace ticketwindow.Winows.Keyboard
{
    /// <summary>
    /// Логика взаимодействия для W_NumPadComplet.xaml
    /// </summary>
    public partial class W_NumPadComplet : UserControl
    {
        public W_NumPadComplet()
        {
            InitializeComponent();
        }


        public TextBox textBox { get; set; }
        public Button bEnter { get; set; }
        public bool clr = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = sender as Button;

                if (b.ToolTip != null)
                {
                    if (b.ToolTip.ToString() == "WNumPadComplet")
                    {
                        string getValue = b.Name.Remove(0, 1);
                        if (clr)
                            textBox.Text = "";
                        switch (getValue)
                        {
                            case "Sup": this.textBox.Text = ""; break;

                            case "Entree": new ClassFunctuon().Click(bEnter); break;

                            case "Point": this.textBox.Text += ","; break;


                            default:
                                int f;
                                if (int.TryParse(getValue, out f))
                                    this.textBox.Text += getValue; break;
                        }

                        this.clr = false;
                    }
                }
            }
        }

        private void UserControl_Loaded1(object sender, RoutedEventArgs e)
        {
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
                bs.Click += Button_Click;

        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {

        }

    }
}
