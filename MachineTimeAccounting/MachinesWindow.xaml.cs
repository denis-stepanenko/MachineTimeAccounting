using MachineTimeAccounting.Models;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MachineTimeAccounting
{
    public partial class MachinesWindow : Window
    {
        EFContext db = new EFContext();
        ICollectionView view;

        public MachinesWindow()
        {
            InitializeComponent();

            if (new[] { 0 }.Contains(AuthorizationService.User.RoleId))
            {
                saveButton.IsEnabled = false;
            }

            db.Machines.Load();
            view = CollectionViewSource.GetDefaultView(db.Machines.Local);
            view.Filter = i =>
            {
                var x = i as Machine;
                return x.Name.ToLower().Contains(filterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = view;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            var items = itemsDataGrid.SelectedItems.OfType<Machine>().ToList();
            if (items.Count == 0) return;

            foreach (var item in items)
            {
                var codes = db.ProductMachines.Where(x => x.MachineId == item.Id).Select(x => x.ProductCode).ToList();
                if (codes.Count > 0)
                {
                    var str = string.Join("\n", codes);
                    Clipboard.SetText(str);
                    MessageBox.Show($"Станок {item.Name} используется. Список продуктов, в которых он используется скопирован в буфер обмена");
                    return;
                }
            }

            db.Machines.RemoveRange(items);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) => db.SaveChanges();

        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e) => view.Refresh();
    }
}
