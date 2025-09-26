using AvioRepair.AppData;
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

namespace AvioRepair.Pages.AddEdit
{
    /// <summary>
    /// Логика взаимодействия для AddEditUsersPage.xaml
    /// </summary>
    public partial class AddEditUsersPage : Page
    {
        Users users;
        bool checkNew;
        public AddEditUsersPage(Users u)
        {
            InitializeComponent();
            if (u == null)
            {
                u = new Users();
                checkNew = true;
            }
            else
                checkNew = false;
            DataContext = users = u;
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkNew)
            {
                Class1.context.Users.Add(users);
            }
            try
            {
                Class1.context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Nav.MainFrame.GoBack();
        }
    }
}
