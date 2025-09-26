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
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using Page = System.Windows.Controls.Page;

namespace AvioRepair.Pages.Otcheti
{
    /// <summary>
    /// Логика взаимодействия для RemontsZapros.xaml
    /// </summary>
    public partial class RemontsZapros : Page
    {
        public RemontsZapros()
        {
            InitializeComponent();
            var remontsData = Class1.context.Remonts.Select(x =>
            new
        {
            x.Equipment.NameEquipment,
            x.Work.NameWork,
            x.StartDate,
            x.EndDate,
            x.Status,
            x.Cost,
            x.Sotrudniki,
            x.Sotrudniki.Remonts,
            x.Work,
            }).ToList();

            var result = remontsData.Select(x => new
            {
                x.NameEquipment,
                x.NameWork,
                x.StartDate,
                x.EndDate,
                x.Status,
                x.Cost,
                //Общее время ремонта 
                SummaVremeni = x.Remonts
                    .Where(r => r.EndDate.HasValue && r.StartDate.HasValue)
                    .Sum(r => (r.EndDate.Value - r.StartDate.Value).TotalHours),
                //Зарплата сотрудника 
                ZPSotrudnika = x.Sotrudniki != null
                    ? (x.Remonts.Any(r => r.EndDate.HasValue && r.StartDate.HasValue)
                        ? (decimal)x.Sotrudniki.Oklad + ((decimal)x.Sotrudniki.Oklad * (decimal)x.Sotrudniki.Razryad * 0.05m *
                          (decimal)x.Remonts
                              .Where(r => r.EndDate.HasValue && r.StartDate.HasValue)
                              .Sum(r => (r.EndDate.Value - r.StartDate.Value).TotalHours) /
                          (decimal)(x.Work.NormHours == 0 ? 1 : x.Work.NormHours))
                        : (decimal)x.Sotrudniki.Oklad + ((decimal)x.Sotrudniki.Oklad * (decimal)x.Sotrudniki.Razryad * 0.05m * 200m / 80m))
                    : 0m,
                //Стоимость ремонта
                SummaRemont = (decimal)x.Cost + (x.Sotrudniki != null
            ? (x.Remonts.Any(r => r.EndDate.HasValue && r.StartDate.HasValue)
                ? (decimal)x.Sotrudniki.Oklad + ((decimal)x.Sotrudniki.Oklad * (decimal)x.Sotrudniki.Razryad * 0.05m *
                  (decimal)x.Remonts
                      .Where(r => r.EndDate.HasValue && r.StartDate.HasValue)
                      .Sum(r => (r.EndDate.Value - r.StartDate.Value).TotalHours) /
                  (decimal)(x.Work.NormHours == 0 ? 1 : x.Work.NormHours))
                : (decimal)x.Sotrudniki.Oklad + ((decimal)x.Sotrudniki.Oklad * (decimal)x.Sotrudniki.Razryad * 0.05m * 200m / 80m))
            : 0m),
                //Количество ремонтов
                KolvoRemonts = x.Remonts.Count(r => r.EndDate.HasValue && r.StartDate.HasValue),
                //Среднее время ремонта 
                SrdVremRemont = x.Remonts.Count(r => r.EndDate.HasValue && r.StartDate.HasValue) > 0
        ? x.Remonts
            .Where(r => r.EndDate.HasValue && r.StartDate.HasValue)
            .Sum(r => (r.EndDate.Value - r.StartDate.Value).TotalHours)
            / x.Remonts.Count(r => r.EndDate.HasValue && r.StartDate.HasValue)
        : 0
            }).ToList();

            RemontDG.ItemsSource = result;
        }
    

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем данные из DataGrid
                var data = RemontDG.ItemsSource as IEnumerable<dynamic>;

                // Объявляем приложение
                Excel.Application app = new Excel.Application
                {
                    Visible = true,
                    SheetsInNewWorkbook = 1
                };

                Excel.Workbook workBook = app.Workbooks.Add(Type.Missing);
                app.DisplayAlerts = false;
                Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets.get_Item(1);
                sheet.Name = "Отчёт по ремонтам";

                // Оглавление
                Range r = sheet.get_Range("A1");
                r.Font.Bold = true;
                r.VerticalAlignment = XlVAlign.xlVAlignCenter;
                r.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet.Cells[1, 1].Value = "Отчёт по ремонтам оборудования";
                Range r1 = sheet.Cells[1, 1];
                Range r2 = sheet.Cells[1, 10]; 
                Range mR = sheet.get_Range(r1, r2);
                mR.Merge();

                // Названия столбцов
                Range nazv = sheet.get_Range("A3", "J3"); 
                nazv.Font.Bold = true;
                nazv.VerticalAlignment = XlVAlign.xlVAlignCenter;
                nazv.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                nazv.Rows.RowHeight = 25;

                sheet.Cells[3, 1].Value = "№ п/п";
                sheet.Cells[3, 2].Value = "Название оборудования";
                sheet.Cells[3, 3].Value = "Название работы";
                sheet.Cells[3, 4].Value = "Дата начала";
                sheet.Cells[3, 5].Value = "Дата окончания";
                sheet.Cells[3, 6].Value = "Статус";
                sheet.Cells[3, 7].Value = "Затраты";
                sheet.Cells[3, 8].Value = "Зарплата сотрудника";
                sheet.Cells[3, 9].Value = "Среднее время ремонта";
                sheet.Cells[3, 10].Value = "Стоимость ремонта"; 

                // Заполняем данные
                int currow = 4;
                int counter = 1;

                foreach (var item in data)
                {
                    sheet.Cells[currow, 1].Value = counter++;
                    sheet.Cells[currow, 2].Value = item.NameEquipment;
                    sheet.Cells[currow, 3].Value = item.NameWork;
                    sheet.Cells[currow, 4].Value = item.StartDate?.ToString("dd.MM.yyyy");
                    sheet.Cells[currow, 5].Value = item.EndDate?.ToString("dd.MM.yyyy");
                    sheet.Cells[currow, 6].Value = item.Status;
                    sheet.Cells[currow, 7].Value = string.Format("{0:0.00}", item.Cost);
                    sheet.Cells[currow, 8].Value = string.Format("{0:0.00}", item.ZPSotrudnika);
                    sheet.Cells[currow, 9].Value = item.SrdVremRemont;
                    sheet.Cells[currow, 10].Value = string.Format("{0:0.00}", item.SummaRemont); 

                    // Настройки для строки с данными
                    Range dataRow = sheet.Rows[currow];
                    dataRow.RowHeight = 20;
                    dataRow.VerticalAlignment = XlVAlign.xlVAlignCenter;

                    // Отдельные настройки для ячеек
                    sheet.Cells[currow, 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Cells[currow, 4].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Cells[currow, 5].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Cells[currow, 7].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    sheet.Cells[currow, 8].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    sheet.Cells[currow, 9].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    sheet.Cells[currow, 10].HorizontalAlignment = XlHAlign.xlHAlignRight;

                    currow++;
                }

                // Форматируем столбцы с числами
                Range costColumn = sheet.get_Range("G4", "G" + (currow - 1));
                costColumn.NumberFormat = "0.00";

                Range zpColumn = sheet.get_Range("H4", "H" + (currow - 1));
                zpColumn.NumberFormat = "0.00";

                Range summaColumn = sheet.get_Range("I4", "I" + (currow - 1));
                summaColumn.NumberFormat = "0.00";

                Range avgTimeColumn = sheet.get_Range("J4", "J" + (currow - 1));
                avgTimeColumn.NumberFormat = "0"; 

                // Границы таблицы
                r1 = sheet.Cells[3, 1];
                r2 = sheet.Cells[currow - 1, 10];
                mR = sheet.get_Range(r1, r2);
                mR.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                // Автоподбор ширины столбцов
                for (int i = 1; i <= 10; i++)
                {
                    sheet.Columns[i].AutoFit();
                }

                // Итого (сдвигаем вправо до столбца I)
                Range iR = sheet.get_Range($"H{currow + 1}", $"I{currow + 1}");
                iR.Merge();
                iR.Value2 = "Общая стоимость ремонтов:";
                iR.Font.Bold = true;
                iR.HorizontalAlignment = XlHAlign.xlHAlignRight;
                iR.VerticalAlignment = XlVAlign.xlVAlignCenter;
                iR.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                // Счёт в итоге с двумя знаками после запятой 
                Range j = sheet.get_Range($"J{currow + 1}");
                j.Font.Bold = true;
                j.Value2 = string.Format("{0:0.00}", data.Sum(d => (decimal)d.SummaRemont));
                j.NumberFormat = "0.00";
                j.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);
                j.HorizontalAlignment = XlHAlign.xlHAlignRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчёта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }
    }
}
