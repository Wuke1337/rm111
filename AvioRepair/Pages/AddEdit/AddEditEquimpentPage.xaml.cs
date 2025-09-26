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
    /// Логика взаимодействия для AddEditEquimpentPage.xaml
    /// </summary>
    public partial class AddEditEquimpentPage : Page
    {
        Equipment equimpent;
        bool checkNew;
        public AddEditEquimpentPage(Equipment e)
        {
            InitializeComponent();
            ZehCmb.ItemsSource = Class1.context.Zeh.ToList();
            if (e == null)
            {
                e = new Equipment() { Zeh = Class1.context.Zeh.FirstOrDefault()};
                checkNew = true;
            }
            else
                checkNew = false;
            DataContext = equimpent = e;
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkNew)
            {
                Class1.context.Equipment.Add(equimpent);
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
