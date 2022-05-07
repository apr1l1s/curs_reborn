using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using curs_reborn.Models;
using System.Text.RegularExpressions;

namespace curs_reborn.Pages.editPages
{
    /// <summary>
    /// Логика взаимодействия для usersPage.xaml
    /// </summary>
    public partial class usersPage : Page
    {
        user cng;
        MainWindow main;
        public usersPage(MainWindow _main)
        {
            InitializeComponent();
            main=_main;
            fillDG();
            LogBox.IsReadOnly = true;
            PassBox.IsReadOnly = true;
            AccBox.IsReadOnly = true;
        }
        //Обновление грида из бд
        public void fillDG()
        {
            using(curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from u in db.users select u).ToList();
            }
            
        }
        private bool checkBoxes()
        {
            var mail = new Regex(@"\A[a-z,0-9]{4,40}?@[a-z]{2,8}?\.(ru|com)\z");
            var pas = new Regex(@"\A[a-z,A-Z,0-9]{8,16}?\z");
            var acc = new Regex(@"\A[0,1]{1}\z");
            if (mail.IsMatch(LogBox.Text))
            {
                if (pas.IsMatch(PassBox.Text))
                {
                    if (acc.IsMatch(AccBox.Text))
                    {
                        return true;
                    }
                    else MessageBox.Show("Уровень доступа не подошёл");
                }
                else MessageBox.Show("Пароль не подошёл");
            }
            else MessageBox.Show("Логин не подошёл");
            return false;
        }
        private void addOrEditUser()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                try
                {
                    //Проверка
                    if (checkBoxes())
                    {
                        int.TryParse(AccBox.Text, out var i);
                        //Выбирали элемент для редактирования
                        if (cng != null)
                        {
                            //Да 
                            //Поиск уже существующих данных с этим номером
                            var chk = from u in db.users
                                      where u.user_id == cng.user_id
                                      select u;
                            
                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                db.editUser(chk.First().user_id, LogBox.Text, PassBox.Text, i);
                            }
                            else
                            {
                                //добавляем
                                db.addUser(LogBox.Text, PassBox.Text, i);
                            }
                        } else { db.addUser(LogBox.Text, PassBox.Text, i); }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обвновления базы данных: {ex.Message}");
                }
            }
            fillDG();
            LogBox.Text = "";
            PassBox.Text = "";
            AccBox.Text = "";
            LogBox.IsReadOnly = true;
            PassBox.IsReadOnly = true;
            AccBox.IsReadOnly = true;

        }
        private void save(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                //При нажатии на кнопку Сохранить
                addOrEditUser();
            }
        }
        private void add(object sender, RoutedEventArgs e)
        {
            //Разрешает писать в полях и отменять выбор
            LogBox.IsReadOnly = false;
            PassBox.IsReadOnly = false;
            AccBox.IsReadOnly = false;
            cng = null;
            DG.SelectedIndex = -1;
        }
        private void del(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex != -1)
            {
                try
                {
                    //Берётся элемент из грида
                    cng = (user)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        db.delUser(cng.user_id);
                        db.SaveChanges();
                    }
                    //Обновляется
                    fillDG();
                } catch(Exception ex)
                {
                    MessageBox.Show("Ошибка удаления" + ex.Message);
                }
                
            }
        }
        private void edit(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex != -1)
            {
                //Берётся элемент из грида
                cng = (user)DG.Items[DG.SelectedIndex];
                //Все данные записываются в блоки для редактирования
                LogBox.Text = cng.login;
                PassBox.Text = cng.pass;
                AccBox.Text = cng.access_level.ToString();
                //Пользователь может редактировать блоки
                LogBox.IsReadOnly = false;
                PassBox.IsReadOnly = false;
                AccBox.IsReadOnly = false;
            }
            else MessageBox.Show("Выберите элемент");
        }
        private void exit(object sender, RoutedEventArgs e)
        {
            //Возврат на страницу админа
            main.openPage(MainWindow.pages.admin);
        }
    }
}
