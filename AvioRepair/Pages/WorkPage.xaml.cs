using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AvioRepair.AppData;

namespace AvioRepair.Pages
{
    public partial class WorkPage : Window
    {
        public WorkPage()
        {
            InitializeComponent();
            
            // Инициализация DataGrid
            var works = Class1.context.Work.ToList();
            WorkDG.ItemsSource = works;

            // Инициализация ComboBox
            if (works != null && works.Any())
            {
                var uniqueTypes = works.Select(w => w.TypeWork).Distinct().ToList();
                FiltCmbBox.ItemsSource = uniqueTypes;
            }
        }

        private void FiltCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update1();
        }

        void Update1()
        {
            var p = Class1.context.Work.ToList();
            var selectedType = FiltCmbBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedType))
            {
                p = p.Where(w => w.TypeWork == selectedType).ToList();
            }

            // Применяем поиск, если он активен
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

            WorkDG.ItemsSource = p;
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
            var selectedType = FiltCmbBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedType))
            {
                p = p.Where(w => w.TypeWork == selectedType).ToList();
            }

            WorkDG.ItemsSource = p;
        }
    }
} 