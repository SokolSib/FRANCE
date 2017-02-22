
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ticketwindow.Class;


namespace ticketwindow
{
    /// <summary>
    /// Логика взаимодействия для W_edit.xaml
    /// </summary>
    public partial class W_edit : Window
    {
        public string sub { get; set; }
        public W_edit()
        {
            InitializeComponent();
            cb.ItemsSource = ClassSync.TypesPayDB.t;
            xDescription.ItemsSource = Class.ClassProducts.listProducts;
            xDescription.ItemFilter += (searchText, obj) =>
                    (searchText.Length > 1) && (obj as Class.ClassProducts.product).Name.IndexOf(searchText.ToUpper()) != -1;
        }

        public string getSelected(GroupBox grpBox)
        {
            string res = "None - Vide";
            //   Grid grd = (Grid)grpBox.Content;
            UIElementCollection uiec = panelA.Children;
            foreach (UIElement uelement in uiec)
            {
                if (typeof(RadioButton) == uelement.GetType())
                {
                    RadioButton rbtn = (RadioButton)uelement;
                    if ((bool)rbtn.IsChecked)
                    {
                        cb.SelectedItem = null;
                        res = rbtn.Content.ToString();
                    }
                }
            }
            uiec = panelB.Children;
            foreach (UIElement uelement in uiec)
            {
                if (typeof(RadioButton) == uelement.GetType())
                {
                    RadioButton rbtn = (RadioButton)uelement;
                    if ((bool)rbtn.IsChecked)
                    {
                        cb.SelectedItem = null;
                        res = rbtn.Content.ToString();
                    }
                }
            }

            if (cb.SelectedItem != null)
                res = "_TypesPayDynamic" + (cb.SelectedItem as ClassSync.TypesPayDB).Id;

            if (xDescription.Text.TrimEnd() != "")
            {
                 List<Class.ClassProducts.product> lp = Class.ClassProducts.getProduct(xDescription.Text);

                    switch (lp.Count)
                    {
                        case 0: statusMes.Content = ("С таким именем продукт не найден "); this.xtbc.SelectedIndex = 3; _expander.IsExpanded = true; break;

                        case 1: statusMes.Content = (""); res = "Products id=[" + lp[0].CustumerId.ToString() + "]"; break;

                        default:
                            Class.ClassProducts.product pe = list.SelectedItem as Class.ClassProducts.product;
                            if (pe == null)
                            {
                                this.xtbc.SelectedIndex = 3;
                                _expander.IsExpanded = true;
                            }

                            else

                                res = "Products id=[" + lp[0].CustumerId.ToString() + "]";

                            break;

                    }
                 
            }

            return res;
        }
        public void setSelected(GroupBox grpBox, string textSelector)
        {
            bool flgrbt = false;
            //    Grid grd = (Grid)grpBox.Content;
            UIElementCollection uiec = panelA.Children;

            foreach (UIElement uelement in uiec)
            {
                if (typeof(RadioButton) == uelement.GetType())
                {
                    RadioButton rbtn = (RadioButton)uelement;
                    rbtn.IsChecked = false;
                    if (rbtn.Content.ToString() == textSelector)
                    {
                        rbtn.IsChecked = true;
                        flgrbt = true;
                    }
                }
            }
            uiec = panelB.Children;

            foreach (UIElement uelement in uiec)
            {
                if (typeof(RadioButton) == uelement.GetType())
                {
                    RadioButton rbtn = (RadioButton)uelement;
                    rbtn.IsChecked = false;
                    if (rbtn.Content.ToString() == textSelector)
                    {
                        rbtn.IsChecked = true;
                        flgrbt = true;
                    }
                }
            }
            if ((!flgrbt) && (textSelector.Substring(0, textSelector.Length > 15 ? 15 : 0) == "_TypesPayDynamic"))
            {
                int indx = -1;
                if (int.TryParse(textSelector == null ? "-1" : textSelector.Replace("_TypesPayDynamic", ""), out indx))
                    cb.SelectedItem = ClassBond.getTypesPayDBFromId(indx);
            }

            if ((!flgrbt) && (textSelector.Substring(0, textSelector.Length > 13 ? 13 : 0) == "Products id=["))
            {

                string sd = textSelector.Substring(textSelector.IndexOf("[")+1, textSelector.IndexOf("]") - textSelector.IndexOf("[")-1);

                Guid g = Guid.Parse(sd  );

                List<Class.ClassProducts.product> lp = Class.ClassProducts.listProducts.FindAll(l=>l.CustumerId == g);

                switch (lp.Count)
                {
                    case 0: statusMes.Content = ("С таким именем продукт не найден "); this.xtbc.SelectedIndex = 3; _expander.IsExpanded = true; break;

                    case 1: statusMes.Content = (""); xDescription.Text = lp[0].Name; break;

                    default:
                        Class.ClassProducts.product pe = list.SelectedItem as Class.ClassProducts.product;
                        if (pe == null)
                        {
                            this.xtbc.SelectedIndex = 3;
                            _expander.IsExpanded = true;
                        }

                    //    else

                            xDescription.Text = lp[0].Name;

                        break;

                }
            }
        }

   
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            

            MainWindow main = this.Owner as MainWindow;

            string name = sub + "_" + this.xName.Content + "x" + this.yName.Content;

            Button b = (Button)(main.FindName(name));



            if (b != null)
            {

             

                    b.Background = new SolidColorBrush(xColor.SelectedColor);

                    b.Foreground = new SolidColorBrush(xColorFont.SelectedColor);

                    b.Content = (this.xCaption.Text);

                    b.ToolTip = getSelected(xGBFunction);

                    Button b_reset = (Button)sender;
                    if (b_reset.ToolTip != null)
                        if ((b_reset).ToolTip.ToString() == "Réinitialiser la configuration")
                            b.ToolTip = "None - Vide";
                    if (b.ToolTip.ToString() == "None - Vide")
                    {
                        b.ClearValue(Button.BackgroundProperty);// new SolidColorBrush(Color.FromArgb(0,255,0,0));
                        b.Content = "";

                    }
                    new ClassGridGroup().save(b);
                    this.Close();
             
               
            }
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setSelected(null, "");
        }


        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            foreach (RadioButton bs in ClassETC_fun.FindVisualChildren<RadioButton>(this.panelA))
            {
                bs.IsChecked = false;
            }
            foreach (RadioButton bs in ClassETC_fun.FindVisualChildren<RadioButton>(this.panelB))
            {
                bs.IsChecked = false;
            }
            rb.IsChecked = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(RadioButton bs in ClassETC_fun.FindVisualChildren<RadioButton>(this.panelA))
            {
                bs.Click += RadioButton_Click;
            }
            foreach (RadioButton bs in ClassETC_fun.FindVisualChildren<RadioButton>(this.panelB))
            {
                bs.Click += RadioButton_Click;
            }
           
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            statusMes.Content = ("Choisissez une seule produit dans la liste :");
            string val =  xDescription.Text;
            if (val != "")
            {

                List<Class.ClassProducts.product> lp = Class.ClassProducts.getProduct(val);

                list.DataContext = lp;

                CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();

            }
        }

        private void xtbc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
              //  new ClassFunctuon().showMessageSB(xtbc.SelectedIndex.ToString());
 
                if (xtbc.SelectedIndex == 0)
                {
                   
                   
                }
            }
        }

       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
