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
    /// Логика взаимодействия для loginPage.xaml
    /// </summary>
    public partial class loginPage : Page
    {
        MainWindow main;
        public loginPage(MainWindow _main)
        {
            InitializeComponent();
            main = _main;
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                if (logBox.Text == "")
                {
                    MessageBox.Show("Введите логин");
                }
                else if (passBox.Text == "")
                {
                    MessageBox.Show("Введите пароль");
                }
                else
                {
                    var findingUser = from u in db.users
                                      where u.login.Contains(logBox.Text)
                                      && u.pass.Contains(passBox.Text)
                                      select u;
                    if (findingUser.Count() > 0)
                    {
                        main.CurrentUser = findingUser.First();
                        main.Title = main.titleUser;
                        MessageBox.Show("Вы вошли");
                        if (main.CurrentUser.access_level == 0)
                            main.openPage(MainWindow.pages.statement);
                        else main.openPage(MainWindow.pages.admin);
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль или логин");
                        logBox.Text = "";
                        passBox.Text = "";
                    }
                }
            }
        }
    }
}
