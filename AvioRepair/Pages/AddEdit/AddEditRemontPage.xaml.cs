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

namespace AvioRepair.Pages.AddEdit
{
    /// <summary>
    /// Логика взаимодействия для AddEditRemontPage.xaml
    /// </summary>
    public partial class AddEditRemontPage : Page
    {
        Remonts remonts;
        bool checkNew;
        public AddEditRemontPage(Remonts r)
        {
            InitializeComponent();
            EquipmentCmb.ItemsSource = Class1.context.Equipment.ToList();
            WorkCmb.ItemsSource = Class1.context.Work.ToList();
            SotrudnikiCmb.ItemsSource = Class1.context.Sotrudniki.ToList();
            ZehCmb.ItemsSource = Class1.context.Zeh.ToList();
            if (r == null)
            {
                r = new Remonts() { 
                    Equipment = Class1.context.Equipment.FirstOrDefault(),
                    Work = Class1.context.Work.FirstOrDefault(),
                    Sotrudniki = Class1.context.Sotrudniki.FirstOrDefault(),
                    Zeh = Class1.context.Zeh.FirstOrDefault() };
                checkNew = true;
            }
            else
                checkNew = false;
            DataContext = remonts = r;
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkNew)
            {
                Class1.context.Remonts.Add(remonts);
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
