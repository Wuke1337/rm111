using AvioRepair.AppData;
using AvioRepair.Pages.AddEdit;
using AvioRepair.Pages.Otcheti;
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
    /// Логика взаимодействия для SotrudnikiPage.xaml
    /// </summary>
    public partial class SotrudnikiPage : Page
    {
        public SotrudnikiPage()
        {
            InitializeComponent();
            SotrudnikiDG.ItemsSource = Class1.context.Sotrudniki.ToList();
            FiltCmbBox.ItemsSource = Class1.context.Sotrudniki.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditSotrudnikiPage(null));
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new ZPSotrudniki());
        }


        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditSotrudnikiPage((sender as Button).DataContext as Sotrudniki));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delSotrudniki = SotrudnikiDG.SelectedItems.Cast<Sotrudniki>().ToList();
            foreach (var delSotrud in delSotrudniki) // цикл проверrи наличия в учетной таблице данных на справочной
                if (Class1.context.Sotrudniki.Any(x => x.ID_Sotrudnik == delSotrud.ID_Sotrudnik))
                {
                    MessageBox.Show("Данные используются в таблице ремонтов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (MessageBox.Show($"Удалить {delSotrudniki.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.Sotrudniki.RemoveRange(delSotrudniki);
            try
            {
                Class1.context.SaveChanges();
                SotrudnikiDG.ItemsSource = Class1.context.Sotrudniki.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SotrudnikiDG.ItemsSource = Class1.context.Sotrudniki.ToList();
        }

        private void PoiskTool_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void FiltCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update1();
        }

        void Update1()
        {
            var selectDoljnost = FiltCmbBox.SelectedItem as Sotrudniki;

            // Выполняем фильтрацию
            var filteredData = Class1.context.Sotrudniki.ToList();

            if (selectDoljnost != null && !string.IsNullOrEmpty(selectDoljnost.Doljnost))
            {
                filteredData = filteredData.Where(x => x.Doljnost == selectDoljnost.Doljnost).ToList();
            }

            // Применяем поиск, если он активен
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                filteredData = filteredData.Where(s => 
                    s.ID_Sotrudnik.ToString().ToLower().Contains(searchText) ||
                    s.Zeh.NameZeh.ToLower().Contains(searchText) ||
                    s.FIO.ToLower().Contains(searchText) ||
                    s.Doljnost.ToLower().Contains(searchText) ||
                    s.Razryad.ToString().ToLower().Contains(searchText) ||
                    s.Oklad.ToString().ToLower().Contains(searchText) ||
                    s.PhoneSotrudnik.ToLower().Contains(searchText) ||
                    s.Data_priema.ToString().ToLower().Contains(searchText)
                ).ToList();
            }

            SotrudnikiDG.ItemsSource = filteredData;
        }

        void Update()
        {
            var p = Class1.context.Sotrudniki.ToList();
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                p = p.Where(s => 
                    s.ID_Sotrudnik.ToString().ToLower().Contains(searchText) ||
                    s.Zeh.NameZeh.ToLower().Contains(searchText) ||
                    s.FIO.ToLower().Contains(searchText) ||
                    s.Doljnost.ToLower().Contains(searchText) ||
                    s.Razryad.ToString().ToLower().Contains(searchText) ||
                    s.Oklad.ToString().ToLower().Contains(searchText) ||
                    s.PhoneSotrudnik.ToLower().Contains(searchText) ||
                    s.Data_priema.ToString().ToLower().Contains(searchText)
                ).ToList();
            }

            // Применяем фильтр, если он выбран
            var selectedDoljnost = FiltCmbBox.SelectedItem as Sotrudniki;
            if (selectedDoljnost != null && !string.IsNullOrEmpty(selectedDoljnost.Doljnost))
            {
                p = p.Where(s => s.Doljnost == selectedDoljnost.Doljnost).ToList();
            }

            SotrudnikiDG.ItemsSource = p;
        }
    }
}
