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
    /// Логика взаимодействия для RemontPage.xaml
    /// </summary>
    public partial class RemontPage : Page
    {
        public RemontPage()
        {
            InitializeComponent();
            RemontDG.ItemsSource = Class1.context.Remonts.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditRemontPage(null));
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new RemontsZapros());
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.Navigate(new AddEditRemontPage((sender as Button).DataContext as Remonts));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delRemont = RemontDG.SelectedItems.Cast<Remonts>().ToList();
            if (MessageBox.Show($"Удалить {delRemont.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.Remonts.RemoveRange(delRemont);
            try
            {
                Class1.context.SaveChanges();
                RemontDG.ItemsSource = Class1.context.Remonts.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RemontDG.ItemsSource = Class1.context.Remonts.ToList();
        }

        private void PoiskTool_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void MAXzTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterByCost();
        }

        private void MINzTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterByCost();
        }

        private void FilterByCost()
        {
            decimal minCost;
            decimal maxCost;

            var query = Class1.context.Remonts.AsQueryable();

            // Применяем фильтр по текстовому поиску
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                query = query.Where(r => 
                    r.ID_Remont.ToString().ToLower().Contains(searchText) ||
                    r.Equipment.NameEquipment.ToLower().Contains(searchText) ||
                    r.Work.NameWork.ToLower().Contains(searchText) ||
                    r.Sotrudniki.FIO.ToLower().Contains(searchText) ||
                    r.Zeh.NameZeh.ToLower().Contains(searchText) ||
                    r.StartDate.ToString().ToLower().Contains(searchText) ||
                    r.EndDate.ToString().ToLower().Contains(searchText) ||
                    r.Status.ToLower().Contains(searchText) ||
                    r.Cost.ToString().ToLower().Contains(searchText)
                );
            }

            // Применяем фильтр по минимальной стоимости
            if (!string.IsNullOrWhiteSpace(MINzTextBox.Text) && decimal.TryParse(MINzTextBox.Text, out minCost))
            {
                query = query.Where(x => x.Cost >= minCost);
            }

            // Применяем фильтр по максимальной стоимости
            if (!string.IsNullOrWhiteSpace(MAXzTextBox.Text) && decimal.TryParse(MAXzTextBox.Text, out maxCost))
            {
                query = query.Where(x => x.Cost <= maxCost);
            }

            RemontDG.ItemsSource = query.ToList();
        }

        private void Update()
        {
            var p = Class1.context.Remonts.ToList();
            if (PoiskTool.Text.Length > 0)
            {
                string searchText = PoiskTool.Text.ToLower();
                p = p.Where(r => 
                    r.ID_Remont.ToString().ToLower().Contains(searchText) ||
                    r.Equipment.NameEquipment.ToLower().Contains(searchText) ||
                    r.Work.NameWork.ToLower().Contains(searchText) ||
                    r.Sotrudniki.FIO.ToLower().Contains(searchText) ||
                    r.Zeh.NameZeh.ToLower().Contains(searchText) ||
                    r.StartDate.ToString().ToLower().Contains(searchText) ||
                    r.EndDate.ToString().ToLower().Contains(searchText) ||
                    r.Status.ToLower().Contains(searchText) ||
                    r.Cost.ToString().ToLower().Contains(searchText)
                ).ToList();
            }
            RemontDG.ItemsSource = p;
        }
    }
}
