using MachineTimeAccounting.Models;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MachineTimeAccounting
{
    public partial class MainWindow : Window
    {
        EFContext context;
        EFContext context2;

        ICollectionView productView;
        ICollectionView machineView;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Title +=  $" v {UpdateService.GetVersion()} ({AuthorizationService.User.Name})";

            // Загрузка позиции и размеров окна
            var settingsService = new SettingsService();
            var settings = settingsService.Load();
            if (!(settings.Width < 100 || settings.Height < 100 || settings.Left > SystemParameters.VirtualScreenWidth - 100 || settings.Scale <= 0))
            {
                Width = settings.Width;
                Height = settings.Height;
                Top = settings.Top;
                Left = settings.Left;
                WindowStartupLocation = WindowStartupLocation.Manual;
                WindowState = settings.IsMaximized ? WindowState.Maximized : WindowState.Normal;
            }

            if (new[] { 0 }.Contains(AuthorizationService.User.RoleId))
            {
                saveButton.IsEnabled = false;
                productMachinesDataGrid.IsReadOnly = true;
                removeProductMachineProgramsButton.IsEnabled = false;
                removeProductMachinesButton.IsEnabled = false;
            }

            if (!new[] { 2 }.Contains(AuthorizationService.User.RoleId))
            {
                usersMenuItem.IsEnabled = false;
            }

            Refresh();
        }

        void Refresh()
        {
            context = new EFContext();

            var products = context.Products.ToList();
            productView = CollectionViewSource.GetDefaultView(products);
            productView.Filter = i =>
            {
                var x = i as Product;
                return x.Code.ToLower().Contains(productFilterTextBox.Text.ToLower()) || x.Name.ToLower().Contains(productFilterTextBox.Text.ToLower());
            };
            productDataGrid.ItemsSource = productView;

            context2 = new EFContext();
            
            machinesDataGrid.ItemsSource = null;
            productMachinesDataGrid.ItemsSource = null;
            productMachineProgramsDataGrid.ItemsSource = null;
        }


        private void productDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = productDataGrid.SelectedItem as Product;
            if (item == null) return;

            context2 = new EFContext();

            machinesDataGrid.ItemsSource = null;
            productMachinesDataGrid.ItemsSource = null;
            productMachineProgramsDataGrid.ItemsSource = null;

            context2.Machines.Load();
            var machines = context2.Machines.Local;
            machineView = CollectionViewSource.GetDefaultView(machines);
            machineView.Filter = i =>
            {
                var x = i as Machine;
                return x.Name.ToLower().Contains(machineFilterTextBox.Text.ToLower());
            };
            machinesDataGrid.ItemsSource = machineView;

            context2.ProductMachines.Where(x => x.ProductCode == item.Code).Load();
            var productMachines = context2.ProductMachines.Local;
            productMachinesDataGrid.ItemsSource = productMachines;
        }

        private void productMachinesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = productMachinesDataGrid.SelectedItem as ProductMachine;
            if (item == null) return;

            productMachineProgramsDataGrid.ItemsSource = item.Programs;
        }

        void Add()
        {
            var product = productDataGrid.SelectedItem as Product;
            if (product == null) return;
            var machine = machinesDataGrid.SelectedItem as Machine;
            if (machine == null) return;

            var productMachine = new ProductMachine
            {
                ProductCode = product.Code,
                Machine = machine,
                Department = AuthorizationService.User.Department
            };

            if (context2.ProductMachines.Local.Any(x => x.ProductCode == product.Code && x.Machine.Id == machine.Id))
            {
                MessageBox.Show("Такой станок уже указан в этом продукте");
                return;
            }

            context2.ProductMachines.Add(productMachine);
        }

        private void removeProductMachinesButton_Click(object sender, RoutedEventArgs e)
        {
            var items = productMachinesDataGrid.SelectedItems.OfType<ProductMachine>();
            if (items.Count() == 0) return;

            context2.ProductMachines.RemoveRange(items);
        }

        private void removeProductMachineProgramsButton_Click(object sender, RoutedEventArgs e)
        {
            var items = productMachineProgramsDataGrid.SelectedItems.OfType<ProductMachineProgram>();
            if (items.Count() == 0) return;

            context2.ProductMachinePrograms.RemoveRange(items);
        }

        public ICommand CopyCommand => new RelayCommand(o => 
        {
            var items = productMachineProgramsDataGrid.SelectedItems.OfType<ProductMachineProgram>().ToList();
            if (items.Count == 0) return;

            var newItems = items.Select(x => new ProductMachineProgram { Number = x.Number, Time = x.Time, Description = x.Description }).ToList();
            LocalClipboardService.Set(newItems);
        });

        public ICommand PasteCommand => new RelayCommand(o => 
        {
            var items = LocalClipboardService.Get<ProductMachineProgram>();
            if (items.Count == 0) return;

            var machine = productMachinesDataGrid.SelectedItem as ProductMachine;
            if (machine == null) return;

            items.ForEach(x => machine.Programs.Add(x));
        });

        private void logoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var user = AuthorizationService.User;
            var items = context.RememberedUsers.Where(x => x.UserId == user.Id);
            context.RememberedUsers.RemoveRange(items);
            context.SaveChanges();
            new AuthorizationWindow().Show();
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var settingsService = new SettingsService();
            var settings = settingsService.Load();
            settings.Width = Width;
            settings.Height = Height;
            settings.Top = Top;
            settings.Left = Left;
            settings.IsMaximized = WindowState == WindowState.Maximized;
            settingsService.Save(settings);
        }

        private void addButton_Click(object sender, RoutedEventArgs e) => Add();

        private void machinesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Add();

        private void saveButton_Click(object sender, RoutedEventArgs e) => context2.SaveChanges();

        private void productFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => productView.Refresh();

        private void machineFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => machineView.Refresh();

        private void usersMenuItem_Click(object sender, RoutedEventArgs e) => new UsersWindow().Show();

        private void machinesMenuItem_Click(object sender, RoutedEventArgs e) => new MachinesWindow().Show();

        private void refreshButton_Click(object sender, RoutedEventArgs e) => Refresh();

        
    }
}
