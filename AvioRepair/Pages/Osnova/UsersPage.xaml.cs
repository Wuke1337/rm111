using AvioRepair.AppData;
using AvioRepair.Pages.AddEdit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            UsersDG.ItemsSource = Class1.context.Users.ToList();
            FiltCmbBox.ItemsSource = Class1.context.Users.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditUsersPage(null));
        }     

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditUsersPage((sender as Button).DataContext as Users));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delUsers = UsersDG.SelectedItems.Cast<Users>().ToList();
            foreach (var delUsrs in delUsers) // цикл проверrи наличия в учетной таблице данных на справочной
                if (Class1.context.Users.Any(x => x.ID_User == delUsrs.ID_User))
                {
                    MessageBox.Show("Данные используются в журнале регистрации входа пользователей", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (MessageBox.Show($"Удалить {delUsers.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.Users.RemoveRange(delUsers);
            try
            {
                Class1.context.SaveChanges();
                UsersDG.ItemsSource = Class1.context.Users.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsersDG.ItemsSource = Class1.context.Users.ToList();
        }

        private void PoiskTool_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        void Update()
        {
            var p = Class1.context.Users.ToList();
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                p = p.Where(u => 
                    u.ID_User.ToString().ToLower().Contains(searchText) ||
                    u.Login.ToLower().Contains(searchText) ||
                    u.Password.ToLower().Contains(searchText) ||
                    u.Rights.ToLower().Contains(searchText)
                ).ToList();
            }
            UsersDG.ItemsSource = p;
        }

        private void FiltCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update1();
        }
        void Update1()
        {
            var selectedRights = FiltCmbBox.SelectedItem as Users;

            // Выполняем фильтрацию
            var filteredData = Class1.context.Users.ToList();

            if (selectedRights != null && !string.IsNullOrEmpty(selectedRights.Rights))
            {
                filteredData = filteredData.Where(x => x.Rights == selectedRights.Rights).ToList();
            }

            // Применяем поиск, если он активен
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                filteredData = filteredData.Where(s =>
                     s.ID_User.ToString().ToLower().Contains(searchText) ||
                    s.Login.ToLower().Contains(searchText) ||
                    s.Password.ToLower().Contains(searchText) ||
                    s.Rights.ToLower().Contains(searchText)
                ).ToList();
            }

            UsersDG.ItemsSource = filteredData;
        }
    }
}
