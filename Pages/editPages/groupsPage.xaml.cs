using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using curs_reborn.Models;


namespace curs_reborn.Pages.editPages
{
    /// <summary>
    /// Логика взаимодействия для groupsPage.xaml
    /// </summary>
    public partial class groupsPage : Page
    {
        group cng;
        MainWindow main;
        public groupsPage(MainWindow _main)
        {
            InitializeComponent();
            main = _main;
            fillDG();
            TitleBox.IsReadOnly = true;
        }
        private void fillDG()
        {
            using(curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from g in db.groups select g).ToList();
            }
            
        }
        private bool checkBoxes()
        {
            var title = new Regex(@"\A[А-Я]{1,3}\-[0-9]{3}\z");
            if (title.IsMatch(TitleBox.Text))
            {
                return true;
            }
            else MessageBox.Show("Неподходящее название");
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
                        //Выбирали элемент для редактирования
                        if (cng != null)
                        {
                            //Да 
                            //Поиск уже существующих данных с этим номером
                            var chk = from g in db.groups
                                      where g.group_id == cng.group_id
                                      select g;
                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                db.editGroup(cng.group_id, TitleBox.Text);
                            }
                            else
                            {
                                //добавляем
                                db.addGroup(TitleBox.Text);
                            }
                        }
                        else 
                        {
                            //Добавление
                            db.addGroup(TitleBox.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обвновления базы данных: {ex.Message}");
                }
            }
            fillDG();
            TitleBox.Text = "";
            TitleBox.IsReadOnly = true;

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
            TitleBox.IsReadOnly = false;
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
                    cng = (group)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        db.delGroup(cng.group_id);
                        db.SaveChanges();
                    }
                    //Обновляется
                    fillDG();
                }
                catch (Exception ex)
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
                cng = (group)DG.Items[DG.SelectedIndex];
                //Все данные записываются в блоки для редактирования
                TitleBox.Text = cng.title;
                //Пользователь может редактировать блоки
                TitleBox.IsReadOnly = false;
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
