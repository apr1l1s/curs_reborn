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
using curs_reborn.Models;

namespace curs_reborn.Pages
{
    /// <summary>
    /// Логика взаимодействия для adminPage.xaml
    /// </summary>
    public partial class adminPage : Page
    {
        MainWindow mainWindow;
        public adminPage(MainWindow _main)
        {
            mainWindow = _main;
            InitializeComponent();
            fillCB();
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
            }
        }
        public void fillCB()
        {
            List<string> tables = new List<string>() { "Пользователи","Группы","Студенты","Стипендии","Оценки","История","Предметы" };
            CB.ItemsSource = tables;

        }
        private void exit(object sender, RoutedEventArgs e)
        {
            mainWindow.openPage(MainWindow.pages.login);
        }

        private void edit(object sender, RoutedEventArgs e)
        {
            if (CB.SelectedItem != null)
            {
                switch(CB.SelectedIndex)
                {
                    case 0:
                        MessageBox.Show("Пользователи");
                        break;
                }
            }
            else MessageBox.Show("Выберите таблицу");
            //mainWindow.openPage(MainWindow.pages.edit);
        }
    }
}
