using MachineTimeAccounting.Models;
using System;
using System.Linq;
using System.Windows;

namespace MachineTimeAccounting
{
    public partial class AuthorizationWindow : Window
    {
        EFContext db = new EFContext();

        public AuthorizationWindow()
        {
            InitializeComponent();
            Title += " v" + UpdateService.GetVersion();

            Refresh();

            // если пользователь в прошлый раз нажал "запомнить", то заходим без пароля
            var rememberedUser = db.RememberedUsers.FirstOrDefault(x => x.WindowsUserName == Environment.UserName);
            if (rememberedUser != null)
            {
                AuthorizationService.User = rememberedUser.User;

                new MainWindow().Show();
                this.Close();
            }
        }

        void Refresh()
        {
            var items = db.Users.ToList();
            itemsComboBox.ItemsSource = items;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var item = itemsComboBox.SelectedItem as User;
            if (item == null) return;

            var password = passwordPasswordBox.Password;

            if (!db.CheckPassword(item.Id, password))
            {
                MessageBox.Show("Неправильный пароль");
                return;
            }

            AuthorizationService.User = item;

            db.RememberedUsers.Add(new RememberedUser { UserId = item.Id, WindowsUserName = Environment.UserName, LoginDate = DateTime.Now });
            db.SaveChanges();

            new MainWindow().Show();
            this.Close();
        }
    }
}
