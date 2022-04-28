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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class usersPage : Page
    {
        MainWindow mainWindow;
        public usersPage(MainWindow _main)
        {
            InitializeComponent();
            mainWindow = _main;
            using (curs_databaseEntities db = new curs_databaseEntities())
                DG.ItemsSource = (from u in db.users select u).ToList();
            
        }

        private void addUserEvent(object sender, RoutedEventArgs e)
        {

        }
        private void removeUserEvent(object sender, RoutedEventArgs e)
        {

        }
        private void editUserEvent(object sender, RoutedEventArgs e)
        {
            DG
        }
        private void exit(object sender, RoutedEventArgs e)
        {
            mainWindow.openPage(MainWindow.pages.login);
        }
    }
}
