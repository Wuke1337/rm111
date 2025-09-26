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
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            InitializeComponent();
            EquipmentDG.ItemsSource = Class1.context.Equipment.ToList();
            FiltCmbBox.ItemsSource = Class1.context.Equipment.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditEquimpentPage(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditEquimpentPage((sender as Button).DataContext as Equipment));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delEquipment = EquipmentDG.SelectedItems.Cast<Equipment>().ToList();
            foreach (var delEquipm in delEquipment) // цикл проверrи наличия в учетной таблице данных на справочной
                if (Class1.context.Equipment.Any(x => x.ID_Equipment == delEquipm.ID_Equipment))
                {
                    MessageBox.Show("Данные используются в таблице ремонтов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (MessageBox.Show($"Удалить {delEquipment.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.Sotrudniki.RemoveRange((IEnumerable<Sotrudniki>)delEquipment);
            try
            {
                Class1.context.SaveChanges();
                EquipmentDG.ItemsSource = Class1.context.Equipment.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            EquipmentDG.ItemsSource = Class1.context.Equipment.ToList();
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
            var selectedType = FiltCmbBox.SelectedItem as Equipment;

            // Выполняем фильтрацию
            var filteredData = Class1.context.Equipment.ToList();

            if (selectedType != null && !string.IsNullOrEmpty(selectedType.TypeEquipment))
            {
                filteredData = filteredData.Where(x => x.TypeEquipment == selectedType.TypeEquipment).ToList();
            }

            // Применяем поиск, если он активен
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                filteredData = filteredData.Where(s =>
                     s.ID_Equipment.ToString().ToLower().Contains(searchText) ||
                    s.Zeh.NameZeh.ToLower().Contains(searchText) ||
                    s.NameEquipment.ToLower().Contains(searchText) ||
                    s.TypeEquipment.ToLower().Contains(searchText) ||
                    s.ResoursTime.ToString().ToLower().Contains(searchText) ||
                    s.Sostoyanie.ToLower().Contains(searchText)
                ).ToList();
            }

            EquipmentDG.ItemsSource = filteredData;
        }
        void Update()
        {
            var p = Class1.context.Equipment.ToList();
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                p = p.Where(s =>
                    s.ID_Equipment.ToString().ToLower().Contains(searchText) ||
                    s.Zeh.NameZeh.ToLower().Contains(searchText) ||
                    s.NameEquipment.ToLower().Contains(searchText) ||
                    s.TypeEquipment.ToLower().Contains(searchText) ||
                    s.ResoursTime.ToString().ToLower().Contains(searchText) ||
                    s.Sostoyanie.ToLower().Contains(searchText)
                ).ToList();
            }

            // Применяем фильтр, если он выбран
            var selectedType = FiltCmbBox.SelectedItem as Equipment;
            if (selectedType != null && !string.IsNullOrEmpty(selectedType.TypeEquipment))
            {
                p = p.Where(s => s.TypeEquipment == selectedType.TypeEquipment).ToList();
            }
            EquipmentDG.ItemsSource = p;
        }
    }
}
