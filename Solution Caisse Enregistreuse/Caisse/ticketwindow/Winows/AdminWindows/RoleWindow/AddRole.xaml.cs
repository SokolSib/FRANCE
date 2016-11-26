using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;

namespace TicketWindow.Winows.AdminWindows.RoleWindow
{
    /// <summary>
    /// Interaction logic for AddRole.xaml
    /// </summary>
    public partial class AddRole : Window
    {
        public AddRole()
        {
            InitializeComponent();

            foreach (Privelege privelege in Enum.GetValues(typeof (Privelege)))
            {
                if (privelege != Privelege.All)
                {
                    var btn =
                        new CheckBox
                        {
                            Content = PrivelegeToText(privelege),
                            FontSize = 25,
                            Margin = new Thickness(5),
                            Padding = new Thickness(10),
                            VerticalContentAlignment = VerticalAlignment.Center,
                            Tag = privelege
                        };
                    BtnsPanel.Children.Add(btn);
                }
            }
        }

        public AddRole(AccountRole role) : this()
        {
            RoleName = role.RoleName;
            Priveleges = role.Privelegies;
            BtnAdd.Content = Properties.Resources.BtnRedact;
        }

        public string RoleName
        {
            get { return RoleNameBox.Text; }
            set { RoleNameBox.Text = value; }
        }

        public List<Privelege> Priveleges
        {
            get
            {
                return (from box in BtnsPanel.Children.Cast<CheckBox>() where box.IsChecked == true select ((Privelege) box.Tag)).ToList();
            }
            set
            {
                foreach (var box in BtnsPanel.Children.Cast<CheckBox>())
                {
                    var privelege = ((Privelege) box.Tag);
                    if (value.Contains(privelege) || value.Contains(Privelege.All)) box.IsChecked = true;
                }
            }
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(RoleName))
                DialogResult = true;
        }

        public string PrivelegiesToText(List<Privelege> privelegies)
        {
            if (privelegies.Count >= Enum.GetNames(typeof (Privelege)).Length - 1 || privelegies.Contains(Privelege.All))
                return PrivelegeToText(Privelege.All);
            return string.Join(", ", privelegies.Select(PrivelegeToText));
        }

        public static string PrivelegeToText(Privelege privelege)
        {
            switch (privelege)
            {
                case Privelege.All:
                    return Properties.Resources.PermissionAll;
                case Privelege.RedactRole:
                    return Properties.Resources.PermissionRedactRole;
                case Privelege.RedactUser:
                    return Properties.Resources.PermissionRedactUser;
                case Privelege.RedactButton:
                    return Properties.Resources.PermissionRedactButton;
                case Privelege.RedactTva:
                    return Properties.Resources.PermissionRedactVat;
                case Privelege.RedactGroupsProduct:
                    return Properties.Resources.PermissionRedactGroupsProduct;
                case Privelege.RedactSyncSettings:
                    return Properties.Resources.PermissionRedactSync;
                case Privelege.DeleteProductFromCheck:
                    return Properties.Resources.PermissionDeleteProductFromCheck;
                default:
                    throw new ArgumentOutOfRangeException("privelege", privelege, null);
            }
        }
    }
}
