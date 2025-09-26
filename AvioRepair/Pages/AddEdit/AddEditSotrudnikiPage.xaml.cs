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

namespace AvioRepair.Pages.AddEdit
{
    /// <summary>
    /// Логика взаимодействия для AddEditSotrudnikiPage.xaml
    /// </summary>
    public partial class AddEditSotrudnikiPage : Page
    {
        Sotrudniki sotrudniki;
        bool checkNew;
        public AddEditSotrudnikiPage(Sotrudniki s)
        {
            InitializeComponent();
            ZehCmb.ItemsSource = Class1.context.Zeh.ToList();
            if (s == null)
            {
                s = new Sotrudniki() { Zeh = Class1.context.Zeh.FirstOrDefault() };
                checkNew = true;
            }
            else
                checkNew = false;
            DataContext = sotrudniki = s;
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            Nav.MainFrame.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkNew)
            {
                Class1.context.Sotrudniki.Add(sotrudniki);
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
