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
    /// Логика взаимодействия для ZPSotrudniki.xaml
    /// </summary>
    public partial class ZPSotrudniki : Page
    {
        public ZPSotrudniki()
        {
            InitializeComponent();          
            var sotrudnikiData = Class1.context.Sotrudniki.Select(x =>
            new
            {
                x.FIO,
                x.Doljnost,
                x.Oklad,
                x.Razryad,
                x.Remonts,
                NormHours = x.Remonts.Select(r => r.Work.NormHours).FirstOrDefault()
            }).ToList();

            var result = sotrudnikiData.Select(x => new
            {
                x.FIO,
                x.Doljnost,
                x.Oklad,
                x.Razryad,
                x.Remonts,
                x.NormHours,
                //Общее время ремонта 
                SummaVremeni = x.Remonts
                    .Where(r => r.EndDate.HasValue && r.StartDate.HasValue)
                    .Sum(r => (r.EndDate.Value - r.StartDate.Value).TotalHours),
                //Зарплата сотрудника 
                ZPSotrudnika = x.Remonts.Any(r => r.EndDate.HasValue && r.StartDate.HasValue) ? 
                    (decimal)x.Oklad + ((decimal)x.Oklad * (decimal)x.Razryad * 0.05m * 
                    (decimal)x.Remonts
                        .Where(r => r.EndDate.HasValue && r.StartDate.HasValue)
                        .Sum(r => (r.EndDate.Value - r.StartDate.Value).TotalHours) / 
                    (decimal)(x.NormHours == 0 ? 1 : x.NormHours))
                    : (decimal)x.Oklad + ((decimal)x.Oklad * (decimal)x.Razryad * 0.05m * 200m / 80m)
            }).ToList();

            SotrudnikiDG.ItemsSource = result;
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем данные из DataGrid
                var data = SotrudnikiDG.ItemsSource as IEnumerable<dynamic>;

                // Объявляем приложение
                Excel.Application app = new Excel.Application
                {
                    Visible = true,
                    SheetsInNewWorkbook = 1
                };

                Excel.Workbook workBook = app.Workbooks.Add(Type.Missing);
                app.DisplayAlerts = false;
                Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets.get_Item(1);
                sheet.Name = "Зарплата сотрудников";

                // Оглавление
                Range r = sheet.get_Range("A1");
                r.Font.Bold = true;
                r.VerticalAlignment = XlVAlign.xlVAlignCenter;
                r.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheet.Cells[1, 1].Value = "Отчёт по зарплате сотрудников";
                Range r1 = sheet.Cells[1, 1];
                Range r2 = sheet.Cells[1, 7];
                Range mR = sheet.get_Range(r1, r2);
                mR.Merge();

                // Названия столбцов
                Range nazv = sheet.get_Range("A3", "G3");
                nazv.Font.Bold = true;
                nazv.VerticalAlignment = XlVAlign.xlVAlignCenter;
                nazv.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                nazv.Rows.RowHeight = 25;

                sheet.Cells[3, 1].Value = "№ п/п";
                sheet.Cells[3, 2].Value = "ФИО";
                sheet.Cells[3, 3].Value = "Должность";
                sheet.Cells[3, 4].Value = "Оклад";
                sheet.Cells[3, 5].Value = "Разряд";
                sheet.Cells[3, 6].Value = "Общее время ремонта";
                sheet.Cells[3, 7].Value = "Зарплата сотрудника";

                // Заполняем данные
                int currow = 4;
                int counter = 1;

                foreach (var item in data)
                {
                    sheet.Cells[currow, 1].Value = counter++;
                    sheet.Cells[currow, 2].Value = item.FIO;
                    sheet.Cells[currow, 3].Value = item.Doljnost;

                    // Форматируем оклад с двумя знаками после запятой
                    sheet.Cells[currow, 4].Value = string.Format("{0:0.00}", item.Oklad);

                    sheet.Cells[currow, 5].Value = item.Razryad;
                    sheet.Cells[currow, 6].Value = item.SummaVremeni;

                    // Форматируем зарплату с двумя знаками после запятой
                    sheet.Cells[currow, 7].Value = string.Format("{0:0.00}", item.ZPSotrudnika);

                    // Настройки для строки с данными
                    Range dataRow = sheet.Rows[currow];
                    dataRow.RowHeight = 20;
                    dataRow.VerticalAlignment = XlVAlign.xlVAlignCenter;

                    // Отдельные настройки для ячеек
                    sheet.Cells[currow, 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Cells[currow, 4].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    sheet.Cells[currow, 5].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Cells[currow, 6].HorizontalAlignment = XlHAlign.xlHAlignRight;
                    sheet.Cells[currow, 7].HorizontalAlignment = XlHAlign.xlHAlignRight;

                    currow++;
                }

                // Форматируем столбцы с числами
                Range okladColumn = sheet.get_Range("D4", "D" + (currow - 1));
                okladColumn.NumberFormat = "0.00";

                Range zpColumn = sheet.get_Range("G4", "G" + (currow - 1));
                zpColumn.NumberFormat = "0.00";

                // Границы таблицы
                r1 = sheet.Cells[3, 1];
                r2 = sheet.Cells[currow - 1, 7];
                mR = sheet.get_Range(r1, r2);
                mR.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                // Автоподбор ширины столбцов
                sheet.Columns[1].AutoFit();
                sheet.Columns[2].AutoFit();
                sheet.Columns[3].AutoFit();
                sheet.Columns[4].AutoFit();
                sheet.Columns[5].AutoFit();
                sheet.Columns[6].AutoFit();
                sheet.Columns[7].AutoFit();

                // Итого
                Range iR = sheet.get_Range($"A{currow + 1}", $"F{currow + 1}");
                iR.Merge();
                iR.Value2 = "Общая сумма зарплат:";
                iR.Font.Bold = true;
                iR.HorizontalAlignment = XlHAlign.xlHAlignRight;
                iR.VerticalAlignment = XlVAlign.xlVAlignCenter;
                iR.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                // Счёт в итоге с двумя знаками после запятой
                Range j = sheet.get_Range($"G{currow + 1}");
                j.Font.Bold = true;
                j.Value2 = string.Format("{0:0.00}", data.Sum(d => (decimal)d.ZPSotrudnika));
                j.NumberFormat = "0.00";
                j.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчёта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }  
     
    }
}
