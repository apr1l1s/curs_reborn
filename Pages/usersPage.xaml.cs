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
        List<user> users = new List<user>();

public usersPage(MainWindow _main)
        {
            InitializeComponent();
            mainWindow = _main;
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from u in db.users select u).ToList();
            }
        }
        private void removeUserEvent(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                if (DG.SelectedIndex + 1 != DG.Items.Count)
                {
                    try
                    {
                        var delUser = (user)DG.SelectedItem;
                        db.users.Remove(db.users.Find(delUser.user_id));
                        db.SaveChanges();
                        MessageBox.Show($"{delUser.user_id}");
                        DG.ItemsSource = (from u in db.users select u).ToList();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else MessageBox.Show("Вы удаляете несуществующий элемент");
            }
        }
        private void editUserEvent(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                try
                {
                    for (int i = 0; i < DG.Items.Count - 1; i++)
                    {
                        users.Add(new user());
                        users[i] = (user)DG.Items[i];
                        int j = users[i].user_id;
                        var c = (from u in db.users
                                  where u.user_id == j
                                  select u);
                        if (c.Count() > 0)
                        {
                            var user = db.users.Find(j);
                            user.login = users[i].login;
                            user.pass = users[i].pass;
                            user.access_level = users[i].access_level;
                            db.SaveChanges();

                        }
                        else
                        db.users.Add(users[i]);
                        db.SaveChanges();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void exit(object sender, RoutedEventArgs e)
        {
            mainWindow.openPage(MainWindow.pages.admin);
        }
    }
}
