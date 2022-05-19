using MachineTimeAccounting.Models;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MachineTimeAccounting
{
    public partial class UsersWindow : Window
    {
        EFContext db = new EFContext();
        ICollectionView view;

        public UsersWindow()
        {
            InitializeComponent();

            db.Users.Load();
            view = CollectionViewSource.GetDefaultView(db.Users.Local);
            view.Filter = i =>
            {
                var x = i as User;
                return x.Name.ToLower().Contains(filterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = view;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) => db.SaveChanges();

        private void copyPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var item = itemsDataGrid.SelectedItem as User;
            if (item == null) return;
            Clipboard.SetText(item.Password);
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            var item = itemsDataGrid.SelectedItem as User;
            if (item == null) return;

            PrinterService.Print($"{item.Name}\n{item.Password}");
        }

        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e) => view.Refresh();
    }
}
