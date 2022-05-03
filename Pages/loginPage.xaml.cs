
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                    try
                    {
                        var findingUser = from u in db.users
                                          where u.login == logBox.Text
                                          && u.pass == passBox.Text
                                          select u;
                        if (findingUser.Count() > 0)
                        {
                            main.CurrentUser = findingUser.First();
                            switch (main.CurrentUser.access_level)
                            {
                                case 0: main.openPage(MainWindow.pages.statement); break;
                                case 1: main.openPage(MainWindow.pages.admin); break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неправильный пароль или логин");
                            logBox.Text = "";
                            passBox.Text = "";
                        }
                    } catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
