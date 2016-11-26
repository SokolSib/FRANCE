using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Winows.OtherWindows.addProduct
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private bool _isModif;

        public AddProduct()
        {
            InitializeComponent();
            xContenance.Visibility = Visibility.Hidden;
            xUnit_contenance.Visibility = Visibility.Hidden;
            xTare.Visibility = Visibility.Hidden;
        }

        private bool IsValidTextBox()
        {
            string message = null;

            message += ValidTextBox(xCodeBar);
            message += ValidTextBox(xName);
            message += ValidTextBox(xPrice);
            message += ValidTextBox(xQTY);

            if (!xBalance.IsChecked ?? true)
            {
                message += ValidTextBox(xUnit_contenance);
                message += ValidTextBox(xContenance);
                message += ValidTextBox(xTare);
            }

            return message == string.Empty;
        }

        private ProductType FormToVar()
        {
            int uniteContenance;
            int.TryParse(xUnit_contenance.Text, out uniteContenance);

            int tare;
            int.TryParse(xTare.Text, out tare);

            string tvaId = null;
            if (xTVA.SelectedValue != null)
                tvaId = xTVA.SelectedValue.ToString();

            var tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == tvaId.ToInt());

            return new ProductType(Guid.NewGuid(), xName.Text, xCodeBar.Text, null, 0, !xBalance.IsChecked ?? false, 0,
                uniteContenance, tare, DateTime.Now,tva!=null? tva.CustomerId: Guid.Empty, Guid.NewGuid(), 0)
                   {
                       Price = decimal.Parse(xPrice.Text),
                       Qty = int.Parse(xQTY.Text)
                   };
        }

        private void ClearForm()
        {
            xCodeBar.Text = "";
            xName.Text = "";
            xPrice.Text = "";
            xTVA.SelectedValue = 0;
            xQTY.Text = "";
            xDetails.Text = "";
            xGroup.SelectedValue = 1;
            xSub_group.SelectedValue = 1;
            xBalance.IsChecked = true;
            xContenance.Text = "";
            xUnit_contenance.Text = "";
            xTare.Text = "";
            _isModif = false;
            Cancel.Content = "Anuller";
            Save.Content = "Ajouter";
        }

        private void AddElm()
        {
            RepositoryProduct.Add(FormToVar());
            dataGrid1.DataContext = null;
            dataGrid1.DataContext = RepositoryProduct.Document.GetXElements("Product", "rec");
            ClearForm();
        }

        private void Modif()
        {
            dataGrid1.DataContext = null;
            dataGrid1.DataContext = RepositoryProduct.Document.GetXElements("Product", "rec");
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
            {
                if (_isModif) Modif();
                else AddElm();
            }
        }

        private string ValidTextBox(object sender)
        {
            string listError = null;
            var box = (TextBox) sender;

            var name = box.Name.Substring(1);
            switch (box.Name)
            {
                case "xCodeBar":
                    try
                    {
                        if ((RepositoryProduct.GetXElementByBarcode(long.Parse(box.Text).ToString()) != null) || (box.Text.Length < 5))
                            listError = ("the CodeBare is not correct");
                    }
                    catch
                    {
                        listError = ("the CodeBare is not correct");
                    }
                    break;
                case "xName":
                    if ((RepositoryProduct.GetXElementByElementName("Name", box.Text.Trim().ToUpper()) != null) || (box.Text.Length < 2))
                        listError = ("the Name is not correct");
                    break;
                case "xTare":
                case "xPrice":
                case "xQTY":
                case "xContenance":
                case "xUnit_contenance":
                    decimal decimalValue;
                    if (!decimal.TryParse(box.Text, out decimalValue))
                        listError = (string.Format("the {0} is not correct", name));
                    break;
            }
            box.Foreground = (listError != null)
                                ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0))
                                : new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

            ((Label)FindName("l" + box.Name)).Foreground = box.Foreground;

            return listError;
        }

        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            ValidTextBox(sender);
        }

        private void BSaveClick(object sender, RoutedEventArgs e)
        {
            if (RepositoryProduct.Products.Count > 0)
            {
                RepositoryProduct.AddRange(RepositoryProduct.Products);
                Close();
            }
        }

        private void XBalanceClick(object sender, RoutedEventArgs e)
        {
            var visibility = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
            xContenance.Visibility = visibility;
            xUnit_contenance.Visibility = visibility;
            xTare.Visibility = visibility;
        }

        private void LoadFromWindow(ProductType p)
        {
            xCodeBar.Text = p.CodeBare;
            xName.Text = p.Name;
            xPrice.Text = p.Price.ToString();
            xTVA.SelectedValue = p.Tva;
            xQTY.Text = p.Qty.ToString();
            xBalance.IsChecked = !p.Balance;
            XBalanceClick(xBalance, null);
            xContenance.Text = p.Contenance.ToString();
            xUnit_contenance.Text = p.UniteContenance.ToString();
            xTare.Text = p.Tare.ToString();
            _isModif = true;
            Cancel.Content = "Ajouter Product";
            Save.Content = "Modifie Product";
        }

        private void DataGrid1Selected(object sender, RoutedEventArgs e)
        {
            var selected = (XElement)dataGrid1.SelectedItem;

            if (selected != null) 
                LoadFromWindow(ProductType.FromXElement(selected));
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ClearForm();
            Close();
        }
    }
}