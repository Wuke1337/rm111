using AvioRepair.AppData;
using AvioRepair.Pages.AddEdit;
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
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : Page
    {
        public WorkPage()
        {
            InitializeComponent();
            WorkDG.ItemsSource = Class1.context.Work.ToList();
            FiltCmbBox.ItemsSource = Class1.context.Work.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditWorkPage(null));
        }     

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditWorkPage((sender as Button).DataContext as Work));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delWork = WorkDG.SelectedItems.Cast<Work>().ToList();
            foreach (var delWrk in delWork) // цикл проверrи наличия в учетной таблице данных на справочной
                if (Class1.context.Work.Any(x => x.ID_Work == delWrk.ID_Work))
                {
                    MessageBox.Show("Данные используются в таблице ремонтов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (MessageBox.Show($"Удалить {delWork.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.Work.RemoveRange(delWork);
            try
            {
                Class1.context.SaveChanges();
                WorkDG.ItemsSource = Class1.context.Work.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WorkDG.ItemsSource = Class1.context.Work.ToList();
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
            var selectTypeWork = FiltCmbBox.SelectedItem as Work;

            // Выполняем фильтрацию
            var filteredData = Class1.context.Work.ToList();

            if (selectTypeWork != null && !string.IsNullOrEmpty(selectTypeWork.TypeWork))
            {
                filteredData = filteredData.Where(x => x.TypeWork == selectTypeWork.TypeWork).ToList();
            }

            // Применяем поиск, если он активен
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                filteredData = filteredData.Where(w => 
                    w.ID_Work.ToString().ToLower().Contains(searchText) ||
                    w.Equipment.NameEquipment.ToLower().Contains(searchText) ||
                    w.NameWork.ToLower().Contains(searchText) ||
                    w.TypeWork.ToLower().Contains(searchText) ||
                    w.NormHours.ToString().ToLower().Contains(searchText)
                ).ToList();
            }

            var vib = filteredData
                .Select(x => new
                {
                    x.ID_Work,
                    x.Equipment,
                    x.NameWork,
                    x.TypeWork,
                    x.NormHours
                })
                .ToList();

            WorkDG.ItemsSource = vib;
        }

        void Update()
        {
            var p = Class1.context.Work.ToList();
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                p = p.Where(w => 
                    w.ID_Work.ToString().ToLower().Contains(searchText) ||
                    w.Equipment.NameEquipment.ToLower().Contains(searchText) ||
                    w.NameWork.ToLower().Contains(searchText) ||
                    w.TypeWork.ToLower().Contains(searchText) ||
                    w.NormHours.ToString().ToLower().Contains(searchText)
                ).ToList();
            }

            // Применяем фильтр, если он выбран
            var selectedType = FiltCmbBox.SelectedItem as Work;
            if (selectedType != null && !string.IsNullOrEmpty(selectedType.TypeWork))
            {
                p = p.Where(w => w.TypeWork == selectedType.TypeWork).ToList();
            }

            var vib = p.Select(w => new
            {
                w.ID_Work,
                w.Equipment,
                w.NameWork,
                w.TypeWork,
                w.NormHours
            }).ToList();

            WorkDG.ItemsSource = vib;
        }
    }
}
