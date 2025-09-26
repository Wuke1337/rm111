using AvioRepair.AppData;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using Page = System.Windows.Controls.Page;

namespace AvioRepair.Pages
{
    /// <summary>
    /// Логика взаимодействия для StoreRegistorPage.xaml
    /// </summary>
    public partial class StoreRegistorPage : Page
    {
        public StoreRegistorPage()
        {
            InitializeComponent();
            StoreRegistorDG.ItemsSource = Class1.context.StoreRegistor.ToList();
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Объявляем приложение Excel
                Excel.Application app = new Excel.Application
                {
                    Visible = true,
                    SheetsInNewWorkbook = 1
                };

                Excel.Workbook workBook = app.Workbooks.Add(Type.Missing);
                app.DisplayAlerts = false;
                Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets.get_Item(1);
                sheet.Name = "Журнал операций";

                // Заголовок отчета
                Range r = sheet.get_Range("A1");
                r.Font.Bold = true;
                r.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[1, 1].Value = "Отчёт по журналу операций";
                Range r1 = sheet.Cells[1, 1];
                Range r2 = sheet.Cells[1, 7];
                Range mR = sheet.get_Range(r1, r2);
                mR.Merge();

                // Названия столбцов
                Range nazv = sheet.get_Range("A3", "G3");
                nazv.Font.Bold = true;
                nazv.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                nazv.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                nazv.Rows.RowHeight = 25;

                sheet.Cells[3, 1].Value = "Код входа";
                sheet.Cells[3, 2].Value = "Логин";
                sheet.Cells[3, 3].Value = "Операция";
                sheet.Cells[3, 4].Value = "Статус";
                sheet.Cells[3, 5].Value = "Имя компьютера";
                sheet.Cells[3, 6].Value = "Дата операции";

                // Получаем отфильтрованные данные
                var query = Class1.context.StoreRegistor.AsQueryable();

                if (MINDatePick.SelectedDate.HasValue)
                {
                    var startDate = MINDatePick.SelectedDate.Value.Date;
                    query = query.Where(x => x.DateOperation >= startDate);
                }

                if (MAXDatePick.SelectedDate.HasValue)
                {
                    var endDate = MAXDatePick.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.DateOperation <= endDate);
                }

                var selectData = query.ToList();

                // Заполняем данные
                int currow = 4;
                foreach (var item in selectData)
                {
                    sheet.Cells[currow, 1].Value = item.ID_Log;
                    sheet.Cells[currow, 2].Value = item.Users?.Login;
                    sheet.Cells[currow, 3].Value = item.Operation;
                    sheet.Cells[currow, 4].Value = item.StatusLog;
                    sheet.Cells[currow, 5].Value = item.NamePC;
                    sheet.Cells[currow, 6].Value = item.DateOperation;

                    // Настройки для строки с данными
                    Range dataRow = sheet.Rows[currow];
                    dataRow.RowHeight = 20;
                    dataRow.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                    // Отдельные настройки для ячеек
                    for (int i = 1; i <= 6; i++)
                    {
                        sheet.Cells[currow, i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        sheet.Cells[currow, i].WrapText = false;
                    }

                    currow++;
                }

                // Границы таблицы
                r1 = sheet.Cells[3, 1];
                r2 = sheet.Cells[currow - 1, 6];
                mR = sheet.get_Range(r1, r2);
                mR.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                // Автоподбор ширины столбцов
                for (int i = 1; i <= 6; i++)
                {
                    sheet.Columns[i].AutoFit();
                }

                // Итого
                Range iR = sheet.get_Range($"A{currow + 1}", $"E{currow + 1}");
                iR.Merge();
                iR.Value2 = "Всего записей:";
                iR.Font.Bold = true;
                iR.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                iR.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                iR.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                // Счет в итоге
                Range j = sheet.get_Range($"F{currow + 1}");
                j.Font.Bold = true;
                j.Value2 = selectData.Count;
                j.Borders.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);

                MessageBox.Show("Отчёт успешно сформирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчёта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var delStoreRegistor = StoreRegistorDG.SelectedItems.Cast<StoreRegistor>().ToList();
            if (MessageBox.Show($"Удалить {delStoreRegistor.Count} записей", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Class1.context.StoreRegistor.RemoveRange(delStoreRegistor);
            try
            {
                Class1.context.SaveChanges();
                StoreRegistorDG.ItemsSource = Class1.context.StoreRegistor.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StoreRegistorDG.ItemsSource = Class1.context.StoreRegistor.ToList();
        }

        private void MINDatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void MAXDatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            var query = Class1.context.StoreRegistor.AsQueryable();

            if (MINDatePick.SelectedDate.HasValue)
            {
                var startDate = MINDatePick.SelectedDate.Value.Date;
                query = query.Where(x => x.DateOperation >= startDate);
            }

            if (MAXDatePick.SelectedDate.HasValue)
            {
                var endDate = MAXDatePick.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.DateOperation <= endDate);
            }

            StoreRegistorDG.ItemsSource = query.ToList();
        }
    }
}
