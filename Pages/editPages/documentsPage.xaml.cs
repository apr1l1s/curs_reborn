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
    /// Логика взаимодействия для documentsPage.xaml
    /// </summary>
    public partial class documentsPage : Page
    {
        document cng;
        MainWindow main;
        public documentsPage( MainWindow _main)
        {
            InitializeComponent();
            main = _main;
            fillDG();
        }
        private void fillDG()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from g in db.documents select g).ToList();
            }

        }
        private bool checkBoxes()
        {
            var year = new Regex(@"\A[0-9]{4}/[0-9]{4}\z");
            var term = new Regex(@"\A[1-8]{1}\z");
            var id = new Regex(@"\A[0-9]{1,100}\z");
            if (id.IsMatch(IdBox.Text))
            {
                if (year.IsMatch(YearBox.Text))
                {
                    if (term.IsMatch(TermBox.Text))
                    {
                        return true;
                    }
                }
            }
            else MessageBox.Show("Неподходящее название");
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
                            var chk = from g in db.documents
                                      where g.document_id == cng.document_id
                                      select g;
                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                chk.First().academ_year = YearBox.Text;
                                int.TryParse(IdBox.Text,out int i);
                                chk.First().student_id = i;
                                int.TryParse(TermBox.Text, out i);
                                chk.First().term = i;
                            }
                            else
                            {
                                //добавляем
                                db.documents.Add(cng);
                            }
                        }
                        else
                        {
                            //Добавление
                            int.TryParse(IdBox.Text, out int i);
                            int.TryParse(TermBox.Text, out int j);
                            var st = new document()
                            {
                                student_id = i,
                                term = j,
                                academ_year = YearBox.Text
                            };
                            db.documents.Add(st);
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
            IdBox.Text = "";
            TermBox.Text = "";
            YearBox.Text = "";

            IdBox.IsReadOnly = true;
            TermBox.IsReadOnly = true;
            YearBox.IsReadOnly = true;
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
            IdBox.IsReadOnly = false;
            TermBox.IsReadOnly = false;
            YearBox.IsReadOnly = false;
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
                    cng = (document)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        db.documents.Remove((from d in db.documents where d.document_id == cng.document_id select d).First());
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
                cng = (document)DG.Items[DG.SelectedIndex];
                //Все данные записываются в блоки для редактирования
                //Пользователь может редактировать блоки
                IdBox.IsReadOnly = false;
                TermBox.IsReadOnly = false;
                YearBox.IsReadOnly = false;
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
