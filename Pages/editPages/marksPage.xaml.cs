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
using curs_reborn.Models;

namespace curs_reborn.Pages.editPages
{
    /// <summary>
    /// Логика взаимодействия для marksPage.xaml
    /// </summary>
    public partial class marksPage : Page
    {
        MainWindow main;
        mark cng;
        public marksPage( MainWindow _main)
        {
            InitializeComponent();
            main = _main;
            fillDG();
            IdStBox.IsReadOnly = false;
            IdSjBox.IsReadOnly = false;
            TermBox.IsReadOnly = false;
            MarkBox.IsReadOnly = false;
        }
        private void fillDG()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from g in db.marks select g).ToList();
            }

        }
        private bool checkBoxes()
        {
            var year = new Regex(@"\A[0-9]{4}/[0-9]{4}\z");
            var term = new Regex(@"\A[1-8]{1}\z");
            var id = new Regex(@"\A[0-9]{1,100}\z");
            var mark = new Regex(@"\A[2-5]{1}\z");
            if (id.IsMatch(IdStBox.Text))
            {
                if (mark.IsMatch(MarkBox.Text))
                {
                    if (term.IsMatch(TermBox.Text))
                    {
                        if (id.IsMatch(IdSjBox.Text))
                        {
                            return true;
                        }
                        else MessageBox.Show("Неподходящий номер");
                    }
                    else MessageBox.Show("Неподходящий семестр");
                }
                else MessageBox.Show("Неподходящий год");
            }
            else MessageBox.Show("Неподходящий номер");
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
                            var chk = from g in db.marks
                                      where g.student_id == cng.student_id &&
                                      g.subject_id == cng.subject_id
                                      select g;
                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                int.TryParse(MarkBox.Text, out int i);
                                chk.First().mark1 = i;
                                int.TryParse(IdStBox.Text, out i);
                                chk.First().student_id = i;
                                int.TryParse(TermBox.Text, out i);
                                chk.First().term = i;
                                int.TryParse(IdSjBox.Text, out i);
                                chk.First().subject_id = i;
                            }
                            else
                            {
                                //добавляем
                                db.marks.Add(cng);
                            }
                        }
                        else
                        {
                            //Добавление
                            int.TryParse(IdStBox.Text, out int i);
                            int.TryParse(IdStBox.Text, out int k);
                            int.TryParse(TermBox.Text, out int j);
                            int.TryParse(MarkBox.Text, out int u);
                            var st = new mark()
                            {
                                student_id = i,
                                term = j,
                                subject_id = k,
                                mark1 = u
                            };
                            db.marks.Add(st);
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обвновления базы данных: {ex.Message}");
                }
            }
            fillDG();
            IdStBox.Text = "";
            TermBox.Text = "";
            IdSjBox.Text = "";
            MarkBox.Text = "";

            IdStBox.IsReadOnly = true;
            IdSjBox.IsReadOnly = true;
            TermBox.IsReadOnly = true;
            MarkBox.IsReadOnly = true;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                //При нажатии на кнопку Сохранить
                addOrEdit();
            }
        }
        private void add(object sender, RoutedEventArgs e)
        {
            //Разрешает писать в полях и отменять выбор
            IdStBox.IsReadOnly = false;
            IdSjBox.IsReadOnly = false;
            TermBox.IsReadOnly = false;
            MarkBox.IsReadOnly = false;
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
                    cng = (mark)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        var chk = from g in db.marks
                                  where g.student_id == cng.student_id &&
                                  g.subject_id == cng.subject_id
                                  select g;
                        db.marks.Remove(chk.First());
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
                cng = (mark)DG.Items[DG.SelectedIndex];
                //Все данные записываются в блоки для редактирования
                //Пользователь может редактировать блоки
                IdStBox.Text = cng.student_id.ToString();
                IdSjBox.Text = cng.subject_id.ToString();
                TermBox.Text = cng.term.ToString();
                MarkBox.Text = cng.mark1.ToString();
                IdStBox.IsReadOnly = false;
                IdSjBox.IsReadOnly = false;
                TermBox.IsReadOnly = false;
                MarkBox.IsReadOnly = false;
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
