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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvioRepair.Pages.Osnova
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void VhodBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Password;

            // Проверка логина и пароля
            if ((login == "admin" && password == "123") ||
                (login == "chernov" && password == "208"))
            {
                // Успешная авторизация
                ErrorText.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Добро пожаловать, {login}!", "Успешная авторизация",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                // Передаем информацию о пользователе на главную страницу
                NavigationService.Navigate(new MainPage(login));
            }
            else
            {
                // Неверные данные
                ErrorText.Visibility = Visibility.Visible;
                PasswordBox.Password = "";
                LoginBox.Focus();
            }
        }
    }
}
