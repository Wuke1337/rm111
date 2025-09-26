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
using AvioRepair.Pages.AddEdit;
using System.Diagnostics;

namespace AvioRepair.Pages
{
    /// <summary>
    /// Логика взаимодействия для ZehPage.xaml
    /// </summary>
    public partial class ZehPage : Page
    {
        public ZehPage()
        {
            InitializeComponent();
            ZehDG.ItemsSource = Class1.context.Zeh.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditZehPage(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditZehPage((sender as Button).DataContext as Zeh));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delZeh = ZehDG.SelectedItems.Cast<Zeh>().ToList();
            foreach (var delZh in delZeh) // цикл проверrи наличия в учетной таблице данных на справочной
                if (Class1.context.Zeh.Any(x => x.ID_Zeh == delZh.ID_Zeh))
                {
                    MessageBox.Show("Данные используются в таблице ремонтов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (MessageBox.Show($"Удалить {delZeh.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.Zeh.RemoveRange(delZeh);
            try
            {
                Class1.context.SaveChanges();
                ZehDG.ItemsSource = Class1.context.Zeh.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ZehDG.ItemsSource = Class1.context.Zeh.ToList();
        }

        private void PoiskTool_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        void Update()
        {
            var p = Class1.context.Zeh.ToList();
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                p = p.Where(z => 
                    z.ID_Zeh.ToString().ToLower().Contains(searchText) ||
                    z.NameZeh.ToLower().Contains(searchText) ||
                    z.PhoneZeh.ToLower().Contains(searchText) ||
                    z.Address.ToLower().Contains(searchText)
                ).ToList();
            }
            ZehDG.ItemsSource = p;
        }
    }
}
