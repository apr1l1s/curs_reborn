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
    /// Логика взаимодействия для studPage.xaml
    /// </summary>
    public partial class studPage : Page
    {
        student cng;
        MainWindow main;
        public studPage(MainWindow _main)
        {
            InitializeComponent();
            main = _main;
            fillDG();
            SurBox.IsReadOnly = true;
            NameBox.IsReadOnly = true;
            MidBox.IsReadOnly = true;
            SBox.IsReadOnly = true;
        }
        private void fillDG()
        {
            try
            {
                using (curs_databaseEntities db = new curs_databaseEntities())
                {
                    DG.ItemsSource = (from s in db.students select s).ToList();
                }
            } catch
            {
                MessageBox.Show("Отсутствует подключение");
            }
            
          
        }
        private bool checkBoxes()
        {
            var surname = new Regex(@"\A[А-я]{1}[а-я]{1,49}\z");
            var name = new Regex(@"\A[А-я]{1}[а-я]{1,49}\z");
            var middlename = new Regex(@"\A[А-я]{1}[а-я]{1,49}\z");
            var sex = new Regex(@"\A(М|Ж)\z");
            if (surname.IsMatch(SurBox.Text))
            {
                if (name.IsMatch(NameBox.Text))
                {
                    if (middlename.IsMatch(MidBox.Text))
                    {
                        if (sex.IsMatch(SBox.Text))
                        {
                            return true;
                        }
                        else MessageBox.Show("Неподходящее отчество");
                    }
                    else MessageBox.Show("Неподходящее отчество");
                }
                else MessageBox.Show("Неподходящее имя");
            }
            else MessageBox.Show("Неподходящая фамилия");
            return false;
        }
        private void addOrEdit()
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
                            var chk = from g in db.students
                                      where g.student_id == cng.student_id
                                      select g;
                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                chk.First().sex = SBox.Text;
                                chk.First().surname = SurBox.Text;
                                chk.First().name = NameBox.Text;
                                chk.First().middlename = MidBox.Text;
                            }
                            else
                            {
                                //добавляем
                                var st = new student()
                                {
                                    surname = SurBox.Text,
                                    name = NameBox.Text,
                                    middlename = MidBox.Text,
                                    sex = SBox.Text
                                };
                                db.students.Add(st);
                            }
                        }
                        else
                        {
                            //Добавление
                            var st = new student()
                            {
                                surname = SurBox.Text,
                                name = NameBox.Text,
                                middlename = MidBox.Text,
                                sex = SBox.Text
                            };
                            db.students.Add(st);
                        }
                    }
                    db.SaveChanges();
                    fillDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обвновления базы данных: {ex.Message}");
                }
            }
            fillDG();
            SurBox.Text = "";
            NameBox.Text = "";
            MidBox.Text = "";
            SBox.Text = "";
            SurBox.IsReadOnly = true;
            NameBox.IsReadOnly = true;
            MidBox.IsReadOnly = true;
            SBox.IsReadOnly = true;

        }
        private void save(object sender, RoutedEventArgs e)
        {
            addOrEdit();
        }
        private void del(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex != -1)
            {
                try
                {
                    //Берётся элемент из грида
                    cng = (student)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        var chk = from g in db.students
                                  where g.student_id == cng.student_id
                                  select g;
                        if (chk.First() != null)
                        {
                            db.students.Remove(chk.First());
                            db.SaveChanges();
                        }
                        else MessageBox.Show("Невозможно удалить студента");
                    }
                    //Обновляется
                    fillDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }

            }
        }
        private void add(object sender, RoutedEventArgs e)
        {
            //Разрешает писать в полях и отменять выбор
            SurBox.IsReadOnly = false;
            NameBox.IsReadOnly = false;
            MidBox.IsReadOnly = false;
            SBox.IsReadOnly = false;
            cng = null;
            DG.SelectedIndex = -1;
        }
        private void edit(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex != -1)
            {
                //Берётся элемент из грида
                cng = (student)DG.Items[DG.SelectedIndex];
                //Все данные записываются в блоки для редактирования
                SurBox.Text = cng.surname;
                NameBox.Text = cng.name;
                MidBox.Text = cng.middlename;
                SBox.Text = cng.sex;
                //Пользователь может редактировать блоки
                SurBox.IsReadOnly = false;
                NameBox.IsReadOnly = false;
                MidBox.IsReadOnly = false;
                SBox.IsReadOnly = false;
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
