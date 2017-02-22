using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.AdminWindows.GroupProductWindow
{
    /// <summary>
    ///     Interaction logic for GroupsProductWindow.xaml
    /// </summary>
    public partial class GroupsProductWindow : Window
    {
        public GroupsProductWindow()
        {
            InitializeComponent();
            DataGridGroups.DataContext = RepositoryGroupProduct.GroupProducts;
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataGridGroups_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedGroup = (GroupProduct) e.AddedItems[0];
                DataGridSubgroups.ItemsSource = null;
                DataGridSubgroups.ItemsSource = selectedGroup.SubGroups;
            }
        }

        private void BtnAdd1Click(object sender, RoutedEventArgs e)
        {
            var window = new TextWindow();
            if (window.ShowDialog() == true)
            {
                var group = new GroupProduct(RepositoryGroupProduct.GroupProducts.Count + 1, window.NameText);
                RepositoryGroupProduct.Add(group);

                DataGridGroups.ItemsSource = null;
                DataGridGroups.ItemsSource = RepositoryGroupProduct.GroupProducts;
            }
        }
        
        private void BtnDel1Click(object sender, RoutedEventArgs e)
        {
            var selected = DataGridGroups.SelectedItem as GroupProduct;

            if (selected != null)
            {
                var result = MessageBox.Show(Properties.Resources.MessageDeleteQuestion, Properties.Resources.LabelQuestion, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    RepositoryGroupProduct.Delete(selected);

                    DataGridGroups.ItemsSource = null;
                    DataGridGroups.ItemsSource = RepositoryGroupProduct.GroupProducts;
                }
            }
        }

        private void BtnAdd2Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = DataGridGroups.SelectedItem as GroupProduct;

            if (selectedGroup != null)
            {
                var window = new TextWindow();
                if (window.ShowDialog() == true)
                {
                    var subgroup = new SubGroupProduct(RepositorySubGroupProduct.SubGroupProducts.Count + 1, window.NameText, selectedGroup.Id)
                                   {
                                       Group = selectedGroup
                                   };
                    RepositoryGroupProduct.AddSubgroup(subgroup);

                    DataGridSubgroups.ItemsSource = null;
                    DataGridSubgroups.ItemsSource = selectedGroup.SubGroups;
                }
            }
        }
        
        private void BtnDel2Click(object sender, RoutedEventArgs e)
        {
            var selected = DataGridSubgroups.SelectedItem as SubGroupProduct;
            var selectedGroup = DataGridGroups.SelectedItem as GroupProduct;

            if (selected != null)
            {
                var result = MessageBox.Show(Properties.Resources.MessageDeleteQuestion, Properties.Resources.LabelQuestion, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    RepositoryGroupProduct.DeleteSubgroup(selected);

                    DataGridSubgroups.ItemsSource = null;
                    DataGridSubgroups.ItemsSource = selectedGroup.SubGroups;
                }
            }
        }
    }
}