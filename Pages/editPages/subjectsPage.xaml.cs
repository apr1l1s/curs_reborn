using curs_reborn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace curs_reborn.Pages.editPages
{
    /// <summary>
    /// Логика взаимодействия для subjectsPage.xaml
    /// </summary>
    public partial class subjectsPage : Page
    {
        MainWindow main;
        subject cng;
        public subjectsPage(MainWindow _main)
        {
            InitializeComponent();
            main = _main;
            fillDG();
        }
        private void fillDG()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from s in db.subjects select s).ToList();
            }

        }
        private bool checkBoxes()
        {
            var title = new Regex(@"\A[А-Я,а-я,\s]{5,50}\z");
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
                            var chk = from g in db.subjects
                                      where g.subject_id == cng.subject_id
                                      select g;

                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                db.editSubject(cng.subject_id, TitleBox.Text);
                            }
                            else
                            {
                                //добавляем
                                db.addSubject(TitleBox.Text);
                            }
                        }
                        else
                        {
                            //Добавление
                            db.addSubject(TitleBox.Text);
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
                    cng = (subject)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        db.delSubject(cng.subject_id);
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
                cng = (subject)DG.Items[DG.SelectedIndex];
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
