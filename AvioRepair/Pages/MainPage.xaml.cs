using AvioRepair.AppData;
using AvioRepair.Pages.Osnova;
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

namespace AvioRepair.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string _currentUser;
        public MainPage(string currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            // Скрываем кнопки для пользователя chernov
            if (_currentUser == "chernov")
            {
                UsersBtn.Visibility = Visibility.Collapsed;
                StoreRegistorBtn.Visibility = Visibility.Collapsed;
            }
        }  

        private void ZehBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new ZehPage());
        }

        private void SotrudnikiBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new SotrudnikiPage());
        }

        private void EquipmentBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new EquipmentPage());
        }

        private void WorkBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new WorkPage());
        }

        private void RemontBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new RemontPage());
        }

        private void UsersBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new UsersPage());
        }

        private void StoreRegistorBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new StoreRegistorPage());
        }

        private void VihodBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new LoginPage());
        }
    }
}
